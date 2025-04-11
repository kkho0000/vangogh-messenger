using UnityEngine;

[CreateAssetMenu(fileName = "SubtitleSequence", menuName = "Subtitles/Sequence")]
public class SubtitleSequenceData : ScriptableObject
{
    [System.Serializable]
    public class SubtitleEntry
    {
        public string text;         // 字幕文本
        public float duration;      // 单句显示时长（秒）
        public float delayBefore;   // 播放前延迟（秒）
    }

    public SubtitleEntry[] entries; // 字幕条目列表
}