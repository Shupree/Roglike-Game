using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;     // NPC 대사

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();     // 대사 생성
    }

    void GenerateData()
    {
        // string[]배열 형식으로 여러 문장을 저장

        // NPC
        talkData.Add(1001, new string[] { "Hello.", "I collect some special things.\nMay I do business with you?" });   // storeNPC

        // Interactable Object
        
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length) {     // 다음 대사 존재 X : 대화 종료
            return null;
        }
        else {      // 다음 대사 출력 : Dictionary에서 해당 문장을 찾아 리턴
            return talkData[id][talkIndex];
        }
    }
}
