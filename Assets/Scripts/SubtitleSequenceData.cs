using UnityEngine;

[CreateAssetMenu(fileName = "SubtitleSequence", menuName = "Subtitles/Sequence")]
public class SubtitleSequenceData : ScriptableObject
{
    [System.Serializable]
    public class SubtitleEntry
    {
        public string text;         // ��Ļ�ı�
        public float duration;      // ������ʾʱ�����룩
        public float delayBefore;   // ����ǰ�ӳ٣��룩
    }

    public SubtitleEntry[] entries; // ��Ļ��Ŀ�б�
}