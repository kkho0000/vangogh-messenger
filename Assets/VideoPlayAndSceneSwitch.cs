using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayAndSceneSwitch : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string gameSceneName;
    public Camera targetCamera;
    public Image promptText;
    public Image blackScreen;

    void Start()
    {
        // 初始时隐藏提示文字和黑屏
        promptText.enabled = false;
        blackScreen.enabled = false;

        // 设置渲染模式为相机远平面
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        // 关联目标相机
        videoPlayer.targetCamera = targetCamera;

        // 订阅视频播放结束事件
        videoPlayer.loopPointReached += EndReached;
        // 开始播放视频
        videoPlayer.Play();
    }

    void EndReached(VideoPlayer vp)
    {
        // 视频播放结束后，显示黑屏和提示文字
        blackScreen.enabled = true;
        promptText.enabled = true;
    }

    void Update()
    {
        // 检查是否有任意按键按下
        if (promptText.enabled && Input.anyKeyDown)
        {
            // 加载游戏场景
            SceneManager.LoadScene(gameSceneName);
        }
    }
}