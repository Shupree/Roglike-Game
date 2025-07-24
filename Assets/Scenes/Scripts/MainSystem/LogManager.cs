using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogManager : MonoBehaviour
{
    [Header("Talk Data")]
    public List<TalkData> talkDatas;        // NPC 대사 dataList
    private DialogueLine[] dialogueLines;   // 현재 대사집

    [Header("UI")]
    public GameObject mainUI;    // UI Object
    private GameObject talkPanel;    // UI Object

    private Image leftSpriteImg;            // 왼쪽 캐릭터 Image 스크립트
    private Image rightSpriteImg;           // 오른쪽 캐릭터 Image 스크립트

    private Text talkText;       // 대사 Text UI
    private Text speakerText;    // 화자 Text UI

    [Header("Others")]
    public int talkIndex;       // 현재 텍스트 번호
    bool onLog;                 // Log가 활성화됐는지

    void Awake()
    {
        // 오브젝트 및 스크립트 할당
        rightSpriteImg = mainUI.transform.GetChild(0).GetComponent<Image>();
        leftSpriteImg = mainUI.transform.GetChild(1).GetComponent<Image>();
        talkPanel = mainUI.transform.GetChild(2).gameObject;
        talkText = talkPanel.transform.GetChild(0).GetComponent<Text>();
        speakerText = talkPanel.transform.GetChild(1).GetComponent<Text>();

        Debug.Log(talkText);
        
        // 초기화
        mainUI.SetActive(false);

        talkIndex = 0;
        onLog = false;
    }

    // 대화 상대 확인 및 대사 세팅
    public void LogAction(DialogueLine[] lines)
    {
        mainUI.SetActive(true);  // UI 활성화

        dialogueLines = lines;
        talkIndex = 0;
        Talk();    // 대사 불러오기

        onLog = true;
    }

    // 다음 대사로 넘기기
    public void ShowNextLog()
    {
        if (onLog == true)
        { 
            Talk();
            talkIndex++;
        }
    }

    // 대사 출력 및 대화 종료
    void Talk()
    {
        // 대화 종료 : 초기화
        if (dialogueLines[talkIndex] == null)
        {
            /*
            if (talkObject.name == "StoreNPC")
            {    // 상점NPC의 경우 : 상점UI 출력
                GameManager.instance._StoreManager.SetUpProduct();
            }
            */
            talkIndex = 0;
            onLog = false;
            mainUI.SetActive(false);
            return;
        }

        talkText.text = dialogueLines[talkIndex].line;
        speakerText.text = dialogueLines[talkIndex].speaker;
    }

    // 대사창 왼편 혹은 오른편 스프라이트 변경
    public void ConvertSprite(Sprite sprite, bool isRight)
    {
        // 오른편 스프라이트 변경
        if (isRight)
        {
            rightSpriteImg.sprite = sprite;
        }
        // 왼편 스프라이트 변경
        else
        {
            leftSpriteImg.sprite = sprite;
        }
    }

    public DialogueLine[] GetTalk(int id, string situation = "default")
    {
        TalkSet set = talkDatas[id].talkSets.Find(s => s.situation == situation);
        return set != null ? set.lines.ToArray() : new DialogueLine[] { new DialogueLine { speaker = "Error", line = "Error Massage" } };
    }
}
