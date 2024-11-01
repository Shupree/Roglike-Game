using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogManager : MonoBehaviour
{
    [Header ("Reference")]
    public TalkManager _TalkManager;

    [Header ("Object Reference")]
    public GameObject talkPanel;    // UI Object
    public GameObject talkObject;
    private ObjData objData;

    [Header ("Others")]
    public bool isLog;
    public TextMeshProUGUI talkText;
    public int talkIndex;

    void Awake()
    {
        _TalkManager = GetComponent<TalkManager>();

        talkPanel = transform.GetChild(0).gameObject;
        talkText = talkPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        talkPanel.SetActive(false);

        isLog = false;
        talkIndex = 0;
    }

    // 대화 상대 확인 및 대사 세팅
    public void LogAction(GameObject talkObj)
    {
        talkObject = talkObj;
        objData = talkObject.GetComponent<ObjData>();   // 대상의 ObjData 불러오기

        if (_TalkManager.GetTalk(objData.id, 0) == null) {  // 해당되는 대사 미존재 시 스킵
            return;
        }
        
        talkPanel.SetActive(true);  // UI 활성화

        Talk(objData.id, objData.isNPC);    // 대사 불러오기
        talkIndex++;
    }

    // 대사 넘기기
    public void ShowNextLog()
    {
        Talk(objData.id, objData.isNPC);
        talkIndex++;
    }

    // 대사 출력 및 대화 종료
    void Talk(int id, bool isNPC)
    {
        string text = _TalkManager.GetTalk(id, talkIndex);

        if (text == null) {     // 대화 종료 : 초기화
            if (talkObject.name == "StoreNPC") {    // 상점NPC의 경우 : 상점UI 출력
                GameManager.instance._StoreManager.SetUpProduct();
            }
            talkIndex = 0;
            isLog = false;
            talkPanel.SetActive(false);
            return;
        }

        if (isNPC) {    // NPC의 경우
            talkText.text = text;       // 대사 설정
        }
        else {          // NPC가 아닐 경우
            talkText.text = text;       // 대사 설정
        }
        isLog = true;
    }
}
