using UnityEngine;

[CreateAssetMenu(fileName = "SubtitleSequence", menuName = "Subtitles/Sequence")]
public class SubtitleSequenceData : ScriptableObject
{
    [Header("ȫ����Ƶ")]
    public AudioClip sequenceAudio; // ������Ļ���е���Ƶ

    [Header("��Ļ��Ŀ")]
    public SubtitleEntry[] entries;

    [System.Serializable]
    public class SubtitleEntry
    {
        [TextArea] public string text; // ��Ļ�ı�
        public float startTime;        // ��Ļ��ʼʱ�䣨�������Ƶ��ʼ��ʱ�䣬��λ���룩
        public float duration;         // ��Ļ��ʾʱ�����룩
    }
}