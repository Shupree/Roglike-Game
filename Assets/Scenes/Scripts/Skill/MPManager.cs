using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPManager : MonoBehaviour
{
    [Header("Reference")]
    private TurnManager turnManager;
    private PaintManager paintManager;

    [Header("Slider")]
    public Image slider;

    [Header("MasterPiece Data")]
    public MasterPieceData[] MPDataArr;     // 모든 걸작스킬 저장소
    private MasterPieceData MPData;         // 현재 사용 중인 걸작

    [Header("Target")]
    public List<ITurn> targets = new List<ITurn>();            // 공격 타겟

    [Header("Figure")]
    public int stack;           // 걸작 사용을 위한 스택
    private int maxStack;       // 최대 스택 수

    private int addValue;       // MP 스킬 발동 중 중첩 값

    [Header("Bool")]
    public bool isTrueDamage;   // 고정데미지인가?

    public void Initialize()
    {
        turnManager = GameManager.instance.turnManager;
        paintManager = GameManager.instance.paintManager;

        // 테스트용 임시 스킬
        MPData = MPDataArr[0];

        maxStack = MPData.maxCost;
        slider.fillAmount = stack / (float)maxStack;    // 걸작스킬 UI Bar 업데이트
    }

    // 현재 사용 중인 걸작 스킬 Data 반환
    public MasterPieceData GetMPData()
    {
        return MPData;
    }

    // 걸작스킬 변경
    void ConvertMasterPiece()
    {

    }

    // 걸작스킬 조건 확인
    public bool CheckCondition()
    {
        // 걸작 사용 불가능한지 판별
        if (paintManager.canUsePaint != true)
        {
            return false;
        }

        // 스택 수 부족 시 return
        if (stack < MPData.cost)
        {
            return false;
        }

        switch (MPData.conditionType)
        {
            case MasterPieceData.ConditionType.None:
                ClearStack();
                addValue = 1;
                break;

            case MasterPieceData.ConditionType.OverCost:
                addValue = (stack - MPData.cost) / MPData.perCondition; // 여분 코스트 / 필요 수치
                ClearStack();                           // 스택 초기화
                AddStack(stack % MPData.perCondition);  // 여분 반환
                break;

            case MasterPieceData.ConditionType.Health:
                addValue = turnManager.allies[0].GetStatus("HP") / MPData.perCondition;     // 중첩 값 연산
                if (turnManager.allies[0].GetStatus("HP") % MPData.perCondition <= 0)
                {
                    addValue--;     // HP가 0이 되는 경우 방지
                }

                if (addValue > MPData.maxCondition)
                {
                    addValue = MPData.maxCondition;     // 상한치를 넘는 경우 방지
                }
                else if (addValue <= 0)
                {
                    Debug.Log("걸작 사용에 사용할 HP가 부족합니다!");
                    return false;                             // HP가 부족하다면 사용 불가능
                }
                turnManager.allies[0].TakeDamage(MPData.perCondition * addValue, true);      // 필요 수치만큼 플레이어 HP 감소 (allies[0] = player)
                ClearStack();
                break;

            case MasterPieceData.ConditionType.Paint:   // 물감
                int colorType = 0;
                switch (MPData.conditionDetail)
                {
                    case "Red":
                        colorType = 1;
                        break;
                    case "Blue":
                        colorType = 2;
                        break;
                    case "Yellow":
                        colorType = 3;
                        break;
                    case "White":
                        colorType = 4;
                        break;
                }

                addValue = paintManager.GetPaintInfo(colorType) / MPData.perCondition;        // 중첩 값 연산
                if (addValue > MPData.maxCondition)
                {
                    addValue = MPData.maxCondition;
                }
                else if (addValue <= 0)
                {
                    Debug.Log("걸작 사용에 사용할 물감이 부족합니다!");
                    return false;                             // 물감 부족 시 사용 불가능
                }
                paintManager.ReducePaint(colorType, MPData.perCondition * addValue);      // 물감 수 감소
                ClearStack();
                break;

            case MasterPieceData.ConditionType.Gold:
                addValue = turnManager.player.gold / MPData.perCondition;
                if (addValue > MPData.maxCondition)
                {
                    addValue = MPData.maxCondition;
                }
                else if (addValue <= 0)
                {
                    Debug.Log("걸작 사용에 사용할 골드가 부족합니다.");     // 골드 부족 시 사용 불가능
                    return false;
                }
                turnManager.player.gold -= MPData.perCondition * addValue;    // 골드 수 감소
                ClearStack();
                break;
        }
        return true;
    }

    // 걸작스킬 사용
    public void ExecuteMPSkill()
    {
        Debug.Log($"{MPData.MP_Name}걸작 발동! / 중첩값:{addValue} / 타겟:{targets}");

        int count = 0;

        // 공격 type에 따른 분류    (targets는 turnManager로부터 받음)
        switch (MPData.skillType)
        {
            // 단타 공격
            case Skill.SkillType.SingleAtk:
                count = MPData.count + (addValue * MPData.perCount);
                break;
            // 전체 공격
            case Skill.SkillType.SplashAtk:
                count = MPData.count + (addValue * MPData.perCount);
                break;
            // 바운스 공격
            case Skill.SkillType.BounceAtk:
                count = 1;
                for (int i = 0; i < MPData.count + (addValue * MPData.perCount); i++)    // 타겟 재설정
                {
                    int randomNum = Random.Range(0, turnManager.enemies.Count);
                    targets.Add(turnManager.enemies[randomNum]);
                }
                break;
            // 자신 보조
            case Skill.SkillType.SingleSup:    // 자기자신 타겟 스킬
                count = MPData.count + (addValue * MPData.perCount);
                isTrueDamage = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case Skill.SkillType.SplashSup:
                count = MPData.count + (addValue * MPData.perCount);
                isTrueDamage = true;    // 아군 대상은 고정데미지
                break;
        }

        // 걸작스킬의 기본 스탯 연산
        int damage = MPData.damage + (MPData.perDamage * addValue);
        int shield = MPData.shield + (MPData.perShield * addValue);
        int heal = MPData.heal + (MPData.perHeal * addValue);
        int[] effect = new int[MPData.effect.Length];
        for (int i = 0; i < MPData.effect.Length; i++)
        {
            effect[i] = MPData.effect[0] + (MPData.perEffect[0] * addValue);
        }

        // 데미지 연산
        for (int c = 0; c < targets.Count; c++)
        {
            for (int i = 0; i < count; i++)   // 타수만큼 반복
            {
                // 데미지
                if (damage > 0)    // 기본 데미지가 0일 시 스킵
                {
                    if (!isTrueDamage)
                    {
                        damage += targets[c].GetStatusEffect("Burn");   // 화상 데미지
                    }
                    targets[c].TakeDamage(damage, false);      // 공격
                    Debug.Log($"{targets[c]}은 {damage} 의 데미지를 입었다.");
                }

                // 회복량
                if (heal > 0)
                {
                    targets[c].TakeHeal(heal);
                    Debug.Log($"{targets[c]}은 {heal} 만큼 체력을 회복했다.");
                }

                // 상태이상 부여
                for (int n = 0; n < MPData.effectType.Length; n++)
                {
                    targets[c].AddStatusEffect(MPData.effectType[n], effect[n]);
                    Debug.Log($"{targets[c]}은 {MPData.effectType[n]}을 {effect[n]}만큼 받었다.");
                }
            }
        }

        // 유물 : 걸작 사용 시 효과
        //GameManager.instance._ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.UseMP);
    }

    // 스택 적립
    public void AddStack(int num)
    {
        if (maxStack <= stack + num)
        {
            stack = maxStack;
            slider.fillAmount = stack / (float)maxStack;
        }
        else
        {
            stack += num;
            slider.fillAmount = stack / (float)maxStack;
        }
    }

    // 스택 초기화
    public void ClearStack()
    {
        stack = 0;
        slider.fillAmount = stack / (float)maxStack;
    }
}
