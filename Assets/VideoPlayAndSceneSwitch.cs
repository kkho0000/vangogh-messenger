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
        // ��ʼʱ������ʾ���ֺͺ���
        promptText.enabled = false;
        blackScreen.enabled = false;

        // ������ȾģʽΪ���Զƽ��
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        // ����Ŀ�����
        videoPlayer.targetCamera = targetCamera;

        // ������Ƶ���Ž����¼�
        videoPlayer.loopPointReached += EndReached;
        // ��ʼ������Ƶ
        videoPlayer.Play();
    }

    void EndReached(VideoPlayer vp)
    {
        // ��Ƶ���Ž�������ʾ��������ʾ����
        blackScreen.enabled = true;
        promptText.enabled = true;
    }

    void Update()
    {
        // ����Ƿ������ⰴ������
        if (promptText.enabled && Input.anyKeyDown)
        {
            // ������Ϸ����
            SceneManager.LoadScene(gameSceneName);
        }
    }
}