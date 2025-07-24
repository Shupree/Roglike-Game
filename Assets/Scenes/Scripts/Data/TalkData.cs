using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkData", menuName = "Scriptable Object/Talk Data")]
public class TalkData : ScriptableObject
{
    public int npcId;               // NPC ID
    public List<TalkSet> talkSets;  // 대사집
}

[System.Serializable]
public class TalkSet
{
    public string situation;        // 상황 : "default", "questComplete", "lowHP", ...
    public List<DialogueLine> lines;      // 대사
}

[System.Serializable]
public class DialogueLine
{
    public string speaker;  // NPC 이름
    [TextArea(2, 4)]
    public string line;     // 실제 대사
}
