using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI 버튼과 플레이어, TurnManager간 연결
public class PlayerActionHandler : MonoBehaviour
{
    TurnManager turnManager;
    PaintManager paintManager;
    StorageManager storageManager;

    public void Initialize()
    {
        turnManager = GameManager.instance.turnManager;
        paintManager = GameManager.instance.paintManager;
        storageManager = GameManager.instance.storageManager;
    }

    // '공격' 버튼에 이 메서드를 연결합니다.
    public void OnClickAttackBtn()
    {
        if (GameManager.instance.player.actionType == ActionType.masterPiece)
        {
            StartCoroutine(turnManager.ExecuteMasterPiece());
        }
        else if (turnManager.GetState() == TurnManager.TurnState.playerAct)
        {
            if (paintManager.paletteOrder == 0) return; // 물감 선택 여부 확인

            // 물감 사용 불가 처리
            paintManager.canUsePaint = false;

            //turnManager.GetState() = TurnManager.TurnState.allyTurn;                 // 캔버스 시스템 추가 시 교체해야함.

            // 공격 단계로
            StartCoroutine(turnManager.PlayerAttack());
        }
        else
        {
            return;
        }
    }

    // '지우기' 버튼에 이 메서드를 연결합니다.
    public void OnClickEraseBtn()
    {
        if (turnManager.GetState() != TurnManager.TurnState.playerAct || GameManager.instance.player.actionType == ActionType.masterPiece) return;

        for (int i = 0; i < 4; i++)
        {
            paintManager.ReturnPaint();
        }

        GameManager.instance.player.ClearActionInfo();  // 플레이어가 자신의 타겟과 스킬을 초기화
        paintManager.ClearPaint();                      // 페인트 매니저가 팔레트를 초기화
    }

    // 걸작 스킬 사용 버튼
    public void OnClickMasterPieceBtn()
    {
        // 버튼이 계속 눌리는 거 방지하기 위함
        if (turnManager.GetState() != TurnManager.TurnState.playerAct || !storageManager._MPManager.CheckCondition())
        {
            return;
        }

        OnClickEraseBtn();        // 사용 중이던 스킬 해제

        paintManager.canUsePaint = false;               // 물감 & 걸작스킬 & 테마스킬 기능 Off

        GameManager.instance.player.actionType = ActionType.masterPiece;

        paintManager.SetSkillImg(storageManager._MPManager.GetMPData().icon);
        GameManager.instance.player.SetTarget(storageManager._MPManager.GetMPData().skillType, storageManager._MPManager.GetMPData().count);     // 타겟팅 설정
    }

    // 테마 스킬 사용 버튼
    public void OnClickThemeSkillBtn(int num)
    {
        // 버튼이 계속 눌리는 거 방지하기 위함
        if (turnManager.GetState() != TurnManager.TurnState.playerAct)
        {
            return;
        }

        OnClickEraseBtn();        // 사용 중이던 스킬 해제

        GameManager.instance.player.actionType = ActionType.themeSkill;
        ThemeSkillData themeSkill = storageManager.themeManager.GetThemeData().skillList[num];
        GameManager.instance.player.themeSkill = themeSkill;                 // 사용 중인 스킬 변경

        paintManager.SetSkillImg(themeSkill.icon);      // 스킬 아이콘 변경
        GameManager.instance.player.SetTarget(themeSkill.skillType, themeSkill.count);                // 타겟팅 설정

        // 테마스킬이 요구하는 페인트 추가
        foreach (PaintManager.ColorType colorType in themeSkill.colorTypeList)
        {
            paintManager.ClickPaintBtn(paintManager.paintScArr[(int)colorType]);
        }
    }
}

