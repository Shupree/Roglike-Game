using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    [Header("Reference")]
    private TurnManager turnManager;
    private PaintManager paintManager;

    [Header("UI")]
    public Image[] skillBtnImg;       // 테마 스킬 버튼 Image

    [Header("ITheme")]
    public ThemeData[] themeDatas;       // 테마 데이터 Arr
    private ThemeData themeData;       // 테마 데이터

    [Header("Passive Effect")]
    public List<StatusEffect> passiveEffects = new List<StatusEffect>();

    [Header("Bool")]
    public bool isTrueDamage;   // 고정데미지인가?

    [Header("Target")]
    public List<ITurn> targets = new List<ITurn>();            // 공격 타겟

    public void Initialize()
    {
        turnManager = GameManager.instance.turnManager;
        paintManager = GameManager.instance.paintManager;

        //SubscribeEvent();                   // 이벤트 구독

        // 테스트용 임시 테마 적용
        ApplyTheme("Artist");           // '테마-화가' 적용
    }

    private void ApplyTheme(string themeName)
    {
        // 1. 기존 테마 패시브가 있다면 플레이어에게서 제거
        if (themeData != null && themeData.passiveData != null)
        {
            GameManager.instance.player.DecStatusEffect(themeData.passiveData, 999); // 999는 모든 스택 제거를 의미
        }

        // 2. 새로운 테마 데이터 설정
        switch (themeName)
        {
            case "Artist":
                themeData = themeDatas[0];
                break;  
        }

        // 3. 새로운 테마 패시브를 플레이어에게 직접 부여
        if (themeData != null && themeData.passiveData != null)
        {
            GameManager.instance.player.AddStatusEffect(themeData.passiveData, 0);  // 초기 스택 0으로 부여
        }

        skillBtnImg[0].sprite = themeData.skillList[0].icon;
        skillBtnImg[1].sprite = themeData.skillList[1].icon;
    }

    public ThemeData GetThemeData()
    {
        if (themeData != null)
        {
            return themeData;
        }
        else
        {
            Debug.LogError("저장 중인 ThemeData가 없습니다!");
            return null;
        }
    }

    // 걸작스킬 사용
    public void ExecuteThemeSkill(ThemeSkillData skillData)
    {
        Debug.Log($"{themeSkill}테마 스킬 발동! 타겟:{targets}");

        int addValue = 0;
        foreach (var condition in skillData.conditions)
        {
            switch (condition.type)
            {
                // 플레이어 체력 소모
                case EnhancementCondition.ConditionType.health:
                    player 
                    break;

                // 특정 물감 소모
                case EnhancementCondition.ConditionType.paintR:
                    break;
                case EnhancementCondition.ConditionType.paintB:
                    break;
                case EnhancementCondition.ConditionType.paintY:
                    break;
                case EnhancementCondition.ConditionType.paintW:
                    break;

                // 특정 상태이상 혹은 패시브 소모
                case EnhancementCondition.ConditionType.statusEffect:
                    break;
            }
        }
        if (skillData.conditions != )
                int stack = passiveEffects.Find(e => e.nameEn == themeSkill.needPassiveName).stackCount;

        // 테마스킬 조건부 효과 확인
        if (themeSkill.perNeed >= themeSkill.maxStack)
        {
            addValue = themeSkill.maxStack;
            stack -= themeSkill.perNeed * themeSkill.maxStack;
        }
        else
        {
            addValue = stack / themeSkill.perNeed;
            stack = stack % themeSkill.perNeed;
        }

        int count = 0;

        // 공격 type에 따른 분류    (targets는 turnManager로부터 받음)
        switch (themeSkill.skillType)
        {
            // 단타 공격
            case PaintSkillData.SkillType.SingleAtk:
                count = themeSkill.count + (addValue * themeSkill.perCount);
                break;
            // 전체 공격
            case PaintSkillData.SkillType.SplashAtk:
                count = themeSkill.count + (addValue * themeSkill.perCount);
                break;
            // 바운스 공격
            case PaintSkillData.SkillType.BounceAtk:
                count = 1;
                for (int i = 0; i < themeSkill.count + (addValue * themeSkill.perCount); i++)    // 타겟 재설정
                {
                    int randomNum = Random.Range(0, turnManager.enemies.Count);
                    targets.Add(turnManager.enemies[randomNum]);
                }
                break;
            // 자신 보조
            case PaintSkillData.SkillType.SingleSup:    // 자기자신 타겟 스킬
                count = themeSkill.count + (addValue * themeSkill.perCount);
                isTrueDamage = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case PaintSkillData.SkillType.SplashSup:
                count = themeSkill.count + (addValue * themeSkill.perCount);
                isTrueDamage = true;    // 아군 대상은 고정데미지
                break;
        }

        // 테마스킬의 기본 스탯 연산
        int damage = themeSkill.damage + (themeSkill.perDamage * addValue);
        int shield = themeSkill.shield + (themeSkill.perShield * addValue);
        int heal = themeSkill.heal + (themeSkill.perHeal * addValue);
        int[] effect = new int[themeSkill.effect.Length];
        for (int i = 0; i < themeSkill.effect.Length; i++)
        {
            effect[i] = themeSkill.effect[i] + (themeSkill.perEffect[i] * addValue);
        }
        
        // 데미지 연산
        GameManager.instance.player.DealDamage(damageInfo, count, shield, heal, currentSkill.effectDatas, effects);

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

                // 패시브 부여
                for (int n = 0; n < themeSkill.effectType.Length; n++)
                {
                    if (passiveEffects.Exists(e => e.nameEn == themeSkill.effectType[n]))
                    {
                        CalculatePassiveEffect(themeSkill.effectType[n], effect[n]);
                        Debug.Log($"플레이어는 {themeSkill.effectType[n]}을/를 {effect[n]}만큼 얻었다.");
                    }
                }

                // 상태이상 부여
                for (int n = 0; n < themeSkill.effectType.Length; n++)
                {
                    if (targets[c].GetStatusEffect(themeSkill.effectType[n]) > 0)
                    {
                        targets[c].AddStatusEffect(themeSkill.effectType[n], effect[n]);
                        Debug.Log($"{targets[c]}은 {themeSkill.effectType[n]}을 {effect[n]}만큼 받았다.");
                    }
                }
            }
        }

        // 유물 : 걸작 사용 시 효과
        //GameManager.instance._ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.UseMP);
    }
}
