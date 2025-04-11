using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;
    public TMP_Text subtitleText; // �󶨵�UI�ı����
    private bool isPlaying = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �糡������
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ������Ļ����
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
        subtitleText.gameObject.SetActive(true); // ȷ����Ļ�ı����ʼ�մ��ڻ״̬

        foreach (var entry in sequenceData.entries)
        {
            yield return new WaitForSeconds(entry.delayBefore);
            subtitleText.text = entry.text;
            yield return new WaitForSeconds(entry.duration);
        }

        subtitleText.gameObject.SetActive(false); // ��������Ŀ������Ϻ�������Ļ�ı����
        isPlaying = false;
    }

}