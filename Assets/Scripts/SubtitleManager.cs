using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;
    public TMP_Text subtitleText; // 绑定到UI文本组件
    private bool isPlaying = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景保留
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 播放字幕序列
    public void PlaySequence(SubtitleSequenceData sequenceData)
    {
        if (isPlaying || sequenceData == null) return;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        isPlaying = true;
        StartCoroutine(PlaySubtitles(sequenceData));
    }

    private IEnumerator PlaySubtitles(SubtitleSequenceData sequenceData)
    {
        subtitleText.gameObject.SetActive(true); // 确保字幕文本组件始终处于活动状态

        foreach (var entry in sequenceData.entries)
        {
            yield return new WaitForSeconds(entry.delayBefore);
            subtitleText.text = entry.text;
            yield return new WaitForSeconds(entry.duration);
        }

        subtitleText.gameObject.SetActive(false); // 在所有条目播放完毕后隐藏字幕文本组件
        isPlaying = false;
    }

}