using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DummyMP : MonoBehaviour
{
    // inspecter의 MP_Data는 해당 script 생성 시 start()에 생성할 때 사용한 스크립트의 inspecter의 MP_Data를 가져오면 될 듯.
    // GameManager의 MP_Data도 할당하는 거 잊지 말고!
    public MasterPieceData MP_Data;
    public int cost;    // 비용

    private int num;
    private Paint _Paint;
    private Image icon;

    private void Start() 
    {
        _Paint = GameManager.instance._PaintUI[1].GetComponent<Paint>();    // 파란 페인트 script

        icon = transform.GetChild(1).gameObject.GetComponent<Image>();
        icon.sprite = MP_Data.MP_Sprite;    // 이미지 변경

        cost = MP_Data.cost;    // 코스트값 불러오기
    }

    private void FixedUpdate() 
    {
        icon.fillAmount = GameManager.instance._PaintManager.stack / (float)cost;   // 계속 다른 스크립트꺼 불러써서 무리가 조금 갈 수도 있음.
    }
    
    // 효과 : 모든 파란색 물감 제거 후 (제거 수 + 1) 만큼 모든 적 추위 +
    public void Function() 
    {  
        // 실행
        if (cost <= GameManager.instance._PaintManager.stack && GameManager.instance.state == GameManager.State.playerTurn) {
            num = 1;
            num += _Paint.currentNum;
            _Paint.currentNum = 0;

            for(int i = 0; i < GameManager.instance.EnemyArr.Count; i++) {
                GameManager.instance.EnemyArr[i].GetComponent<Enemy>().debuffArr[3] += num;
                GameManager.instance.EnemyArr[i].GetComponent<Enemy>().Coldness();
                //이번에도 후순위 친구가 상태이상에 안 걸리는 버그 발생 수정할 것
            }

            GameManager.instance._PaintManager.stack = 0;
        }
        else {
            Debug.Log("스택 수 부족");
        }
    }
}
