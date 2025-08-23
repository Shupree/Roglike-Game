using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    [Header("Reference")]
    private Player player;
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
    public bool isIgnoreShield;   // 고정데미지인가?

    [Header("Target")]
    public List<ITurn> targets = new List<ITurn>();            // 공격 타겟

    public void Initialize()
    {
        player = GameManager.instance.player;
        turnManager = GameManager.instance.turnManager;
        paintManager = GameManager.instance.paintManager;

        // 테스트용 임시 테마 적용
        ApplyTheme("Artist");           // '테마-화가' 적용
    }

    private void ApplyTheme(string themeName)
    {
        // 1. 기존 테마 패시브가 있다면 플레이어에게서 제거
        if (themeData != null && themeData.passiveData != null)
        {
            player.DecStatusEffect(themeData.passiveData, 999); // 999는 모든 스택 제거를 의미
        }

        // 2. 새로운 테마 데이터 설정
        switch (themeName)
        {
            case "Artist":
                themeData = themeDatas[0];
                break;
            case "Gardener":
                themeData = themeDatas[1]; // 예시: 정원사 테마 데이터
                break;
        }

        // 3. 새로운 테마 패시브가 있다면 플레이어에게 직접 부여
        if (themeData.passiveData != null)
        {
            player.AddStatusEffect(themeData.passiveData, 1);  // 패시브는 보통 1스택으로 시작
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

    /// <summary>
    /// 테마 스킬을 실행하는 범용 메서드
    /// </summary>
    public void CalculateSkillAbility(ThemeSkillData skillData)
    {
        Debug.Log($"{skillData.skillName}테마 스킬 발동! 타겟:{targets}");

        List<ActionInfo> actionInfos = new List<ActionInfo>();     // 유닛별 조건 충족 수치
        List<ITurn> conditionTargets = new List<ITurn>();       // 조건별 적용 대상
        int addValue = 0;       // 개별 적용값

        // 조건 확인 코드
        switch (skillData.conditionType)
        {
            // 조건 없음
            case ConditionType.none:
                for (int i = 0; i < targets.Count; i++)
                {
                    actionInfos.Add(CalculateActionInfo(skillData, 0));     // 수치 저장
                }
                break;
            // 조건 : 플레이어의 HP 감소
            case ConditionType.playerHealth:
                // 가능한 높은 중첩값 산정
                addValue = Mathf.Min(
                    player.GetStatus("HP") / skillData.condition_HP.valuePerApply,
                    skillData.condition_HP.maxApplyCount
                    );
                // HP가 0이 되는 경우 방지 
                if (player.GetStatus("HP") % skillData.condition_HP.valuePerApply <= 0)
                {
                    addValue--;
                }

                if (addValue > 0)
                {
                    DamageInfo selfDamageInfo = new DamageInfo { amount = addValue * skillData.condition_HP.valuePerApply, isIgnoreShield = true };
                    player.TakeDamage(selfDamageInfo, false);
                }

                for (int i = 0; i < targets.Count; i++)
                {
                    actionInfos.Add(CalculateActionInfo(skillData, addValue));       // 수치 저장
                }
                break;

            // 조건 : (플레이어 / 적)이 특정 상태이상을 n만큼 보유
            case ConditionType.StatusEffect:
                // 대상이 플레이어인지 적인지 판별
                if (skillData.condition_Effect.isTargetEnemy)
                {
                    conditionTargets = targets;
                    foreach (var target in conditionTargets)
                    {
                        addValue = Mathf.Min(
                            target.GetStatusEffectStack(skillData.condition_Effect.statusEffectData) / skillData.condition_Effect.valuePerApply,
                            skillData.condition_Effect.maxApplyCount);
                        actionInfos.Add(CalculateActionInfo(skillData, addValue));
                        if (skillData.condition_Effect.consumeStack && addValue > 0)
                        {
                            target.DecStatusEffect(skillData.condition_Effect.statusEffectData, addValue * skillData.condition_Effect.valuePerApply);
                        }
                    }
                }
                else
                {
                    addValue = Mathf.Min(
                        player.GetStatusEffectStack(skillData.condition_Effect.statusEffectData) / skillData.condition_Effect.valuePerApply,
                        skillData.condition_Effect.maxApplyCount);
                    if (skillData.condition_Effect.consumeStack && addValue > 0)
                    {
                        player.DecStatusEffect(skillData.condition_Effect.statusEffectData, addValue * skillData.condition_Effect.valuePerApply);
                    }
                    for (int i = 0; i < targets.Count; i++)
                    {
                        actionInfos.Add(CalculateActionInfo(skillData, addValue));
                    }
                }
                break;

            // 조건 : (플레이어 / 적)이 특정 종류의 상태이상을 n만큼 보유
            case ConditionType.StatusEffectType:
                // 대상이 플레이어인지 적인지 판별
                if (skillData.condition_EffectType.isTargetEnemy)
                {
                    conditionTargets = targets;
                    foreach (var target in conditionTargets)
                    {
                        addValue = StatusEffectManager.GetEffectTypeStack(target, skillData.condition_EffectType.effectType);
                        addValue = Mathf.Min(
                            addValue / skillData.condition_EffectType.valuePerApply,
                            skillData.condition_EffectType.maxApplyCount);
                        actionInfos.Add(CalculateActionInfo(skillData, addValue));
                    }
                }
                else
                {
                    addValue = StatusEffectManager.GetEffectTypeStack(player, skillData.condition_EffectType.effectType);
                    addValue = Mathf.Min(
                        addValue / skillData.condition_EffectType.valuePerApply,
                        skillData.condition_EffectType.maxApplyCount);
                    for (int i = 0; i < targets.Count; i++)
                    {
                        actionInfos.Add(CalculateActionInfo(skillData, addValue));
                    }
                }
                break;
        }

        int count = 0;
        isIgnoreShield = false;

        // 공격 type에 따른 분류    (targets는 turnManager로부터 받음)
        switch (skillData.skillType)
        {
            // 단타 공격
            case PaintSkillData.SkillType.SingleAtk:
                count = skillData.count + (addValue * skillData.perCount);
                break;
            // 전체 공격
            case PaintSkillData.SkillType.SplashAtk:
                count = skillData.count + (addValue * skillData.perCount);
                break;
            // 바운스 공격 (바운스의 공격은 StatusEffect와 EffectType의 isTargetEnemy == True인 경우 정상적으로 발동 X)
            case PaintSkillData.SkillType.BounceAtk:
                count = 1;
                ActionInfo bounceActionInfo;
                if (actionInfos.Count > 0)
                {
                    bounceActionInfo = actionInfos[0];
                }
                else
                {
                    bounceActionInfo = CalculateActionInfo(skillData, addValue);
                }

                targets.Clear();
                actionInfos.Clear();

                int bounceCount = skillData.count + (addValue * skillData.perCount);
                for (int i = 0; i < bounceCount; i++)    // 타겟 재설정
                {
                    int randomNum = Random.Range(0, turnManager.enemies.Count);
                    targets.Add(turnManager.enemies[randomNum]);
                    actionInfos.Add(bounceActionInfo);
                }
                break;
            // 자신 보조
            case PaintSkillData.SkillType.SingleSup:    // 자기자신 타겟 스킬
                count = skillData.count + (addValue * skillData.perCount);
                isIgnoreShield = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case PaintSkillData.SkillType.SplashSup:
                count = skillData.count + (addValue * skillData.perCount);
                isIgnoreShield = true;    // 아군 대상은 고정데미지
                break;
        }

        // 공격 / 상태이상 부여
        for (int i = 0; i < count; i++)   // 타수만큼 반복
        {
            for (int a = 0; a < targets.Count; a++)     // 모든 타겟 공격
            {
                // 전투 로직
                BattleLogic.ActionLogic(player, targets[a], actionInfos[a]);
            }
        }
        // 유물 : 걸작 사용 시 효과
        //GameManager.instance._ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.UseMP);
    }

    private ActionInfo CalculateActionInfo(ThemeSkillData skillData, int addValue)
    {
        DamageInfo damageInfo = new DamageInfo { amount = skillData.damage + (skillData.perDamage * addValue), isIgnoreShield = false };
        int shield = skillData.shield + (skillData.perShield * addValue);
        int heal = skillData.heal + (skillData.perHeal * addValue);
        List<int> effects = new List<int>();
        for (int e = 0; e < skillData.effects.Count; e++)
        {
            effects.Add(skillData.effects[e] + (skillData.perEffect[e] * addValue));
        }
        StatusEffectInfo statusEffectInfo = new StatusEffectInfo { effectDatas = skillData.effectDatas, effects = effects };
        
        ActionInfo actionInfo = new ActionInfo { 
            damageInfo = damageInfo,
            statusEffectInfo = statusEffectInfo,
            heal = heal,
            shield = shield
        };

        return actionInfo;
    }
}