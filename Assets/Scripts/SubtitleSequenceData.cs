using UnityEngine;

[CreateAssetMenu(fileName = "SubtitleSequence", menuName = "Subtitles/Sequence")]
public class SubtitleSequenceData : ScriptableObject
{
    [Header("全局音频")]
    public AudioClip sequenceAudio; // 整个字幕序列的音频

    [Header("字幕条目")]
    public SubtitleEntry[] entries;

    [System.Serializable]
    public class SubtitleEntry
    {
        [TextArea] public string text; // 字幕文本
        public float startTime;        // 字幕开始时间（相对于音频开始的时间，单位：秒）
        public float duration;         // 字幕显示时长（秒）
    }
}