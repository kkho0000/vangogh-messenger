using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;
    public TMP_Text subtitleText;
    public AudioSource audioSource;

    private SubtitleSequenceData currentSequence;
    private List<SubtitleSequenceData.SubtitleEntry> entries;
    private float audioStartTime;
    private int currentEntryIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySequence(SubtitleSequenceData sequenceData)
    {
        if (sequenceData == null) return;
        if (!subtitleText.gameObject.activeSelf)
        {
            subtitleText.gameObject.SetActive(true);
        }

        currentSequence = sequenceData;
        entries = new List<SubtitleSequenceData.SubtitleEntry>(sequenceData.entries);
        currentEntryIndex = 0;

        // 播放音频
        audioSource.clip = sequenceData.sequenceAudio;
        audioSource.Play();
        audioStartTime = Time.time;

        // 开始检查字幕时间轴
        StartCoroutine(CheckSubtitleTimeline());
    }

    private IEnumerator CheckSubtitleTimeline()
    {
        while (audioSource.isPlaying || currentEntryIndex < entries.Count)
        {
            float currentAudioTime = Time.time - audioStartTime;

            // 检查当前是否需要显示字幕
            if (currentEntryIndex < entries.Count)
            {
                var entry = entries[currentEntryIndex];
                if (currentAudioTime >= entry.startTime)
                {
                    ShowSubtitle(entry.text);
                    yield return new WaitForSeconds(entry.duration);
                    currentEntryIndex++;
                }
            }

            yield return null; // 每帧检查一次
        }

        // 音频播放完毕，隐藏字幕
        subtitleText.gameObject.SetActive(false);
    }

    private void ShowSubtitle(string text)
    {
        subtitleText.text = text;
        subtitleText.gameObject.SetActive(true);
    }

    /*private void ScheduleHideSubtitle(float delay)
    {
        StartCoroutine(HideSubtitleAfterDelay(delay));
    }

    private IEnumerator HideSubtitleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        subtitleText.gameObject.SetActive(false);
    }*/
}/*using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;
    public TMP_Text subtitleText;

    private Queue<SubtitleSequenceData> subtitleQueue = new Queue<SubtitleSequenceData>();
    private bool isPlaying = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySequence(SubtitleSequenceData sequenceData)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        subtitleQueue.Enqueue(sequenceData);
        if (!isPlaying) StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isPlaying = true;
        while (subtitleQueue.Count > 0)
        {
            var sequence = subtitleQueue.Dequeue();
            yield return StartCoroutine(PlaySingleSequence(sequence));
        }
        isPlaying = false;
    }

    private IEnumerator PlaySingleSequence(SubtitleSequenceData sequence)
    {
        subtitleText.gameObject.SetActive(true); 
        foreach (var entry in sequence.entries)
        {
            yield return new WaitForSeconds(entry.delayBefore);
            subtitleText.text = entry.text;
            yield return new WaitForSeconds(entry.duration);
        }
        subtitleText.gameObject.SetActive(false);
        isPlaying = false;
    }
}*/