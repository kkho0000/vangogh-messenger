using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab; // 目标预制体
    public float spawnInterval = 2f; // 生成间隔时间
    public float spawnRadius = 20f; // 生成半径
    public int maxTargets = 5; // 最大目标数量
    private List<GameObject> targets = new List<GameObject>(); // 存储生成的目标

    // 协程生成目标
    private IEnumerator SpawnTargets()
    {
        while (true) // 无限循环
        {
            if (targets.Count < maxTargets) // 如果当前目标数量小于最大目标数量
            {
                // 在局部坐标系中生成随机位置
                Vector3 localSpawnPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
                
                // 将局部坐标转换为世界坐标
                Vector3 spawnPosition = transform.TransformPoint(localSpawnPosition);

                // 实例化目标
                GameObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
                
                // 设置目标的父物体为当前物体
                target.transform.SetParent(this.transform);

                // 启用目标的碰撞体
                target.GetComponent<SphereCollider>().enabled = true;

                // 添加到目标列表
                targets.Add(target);
            }
            yield return new WaitForSeconds(spawnInterval); // 等待指定时间
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTargets()); // 启动生成目标的协程
    }
}