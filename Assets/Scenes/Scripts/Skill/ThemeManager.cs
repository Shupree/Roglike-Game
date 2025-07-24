using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    [Header("Reference")]
    private TurnManager turnManager;
    private PaintManager paintManager;

    [Header("ImageSc")]
    public Image[] skillBtnImg;       // 테마 스킬 버튼 Image

    [Header("ITheme")]
    private IThemePassive iThemePassive;        // 현재 사용 중인 테마 패시브
    public ThemeData[] themeDatas;       // 테마 데이터 Arr
    private ThemeData themeData;       // 테마 데이터

    [Header("Target")]
    public List<ITurn> targets = new List<ITurn>();            // 공격 타겟

    [Header("Passive Effect")]
    public List<StatusEffect> passiveEffects = new List<StatusEffect>();

    [Header("Bool")]
    public bool isTrueDamage;   // 고정데미지인가?

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
        switch (themeName)
        {
            case "Artist":
                themeData = themeDatas[0];
                iThemePassive = new ArtistPassive();
                break;  
        }

        LoadPassiveEffect(themeData.fillName);      // 패시브 데이터 로드

        iThemePassive.ApplyITheme(this);

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

    // JSON 데이터 로드 (테마 패시브)
    private void LoadPassiveEffect(string fileName)
    {
        StatusEffectLoader loader = new StatusEffectLoader();
        passiveEffects = loader.LoadStatusEffects(fileName);     // 파일명에서 확장자 제외
    }

    // 패시브 스택을 증감시키는 함수
    public void CalculatePassiveEffect(string effectName, int num)
    {
        StatusEffect passiveEffect = passiveEffects.Find(e => e.nameEn == effectName);

        if (passiveEffect.maxStack <= passiveEffect.stackCount + num)
        {
            passiveEffect.stackCount = passiveEffect.maxStack;
        }
        else
        { 
            passiveEffect.stackCount += num;
        }
    }

    // 걸작스킬 사용
    public void ExecuteThemeSkill(ThemeSkillData themeSkill)
    {
        Debug.Log($"{themeSkill}테마 스킬 발동! 타겟:{targets}");

        int addValue = 0;
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
            case Skill.SkillType.SingleAtk:
                count = themeSkill.count + (addValue * themeSkill.perCount);
                break;
            // 전체 공격
            case Skill.SkillType.SplashAtk:
                count = themeSkill.count + (addValue * themeSkill.perCount);
                break;
            // 바운스 공격
            case Skill.SkillType.BounceAtk:
                count = 1;
                for (int i = 0; i < themeSkill.count + (addValue * themeSkill.perCount); i++)    // 타겟 재설정
                {
                    int randomNum = Random.Range(0, turnManager.enemies.Count);
                    targets.Add(turnManager.enemies[randomNum]);
                }
                break;
            // 자신 보조
            case Skill.SkillType.SingleSup:    // 자기자신 타겟 스킬
                count = themeSkill.count + (addValue * themeSkill.perCount);
                isTrueDamage = true;    // 자신 대상은 고정데미지
                break;

            // 전체 아군 보조
            case Skill.SkillType.SplashSup:
                count = themeSkill.count + (addValue * themeSkill.perCount);
                isTrueDamage = true;    // 아군 대상은 고정데미지
                break;
        }

        // 걸작스킬의 기본 스탯 연산
        int damage = themeSkill.damage + (themeSkill.perDamage * addValue);
        int shield = themeSkill.shield + (themeSkill.perShield * addValue);
        int heal = themeSkill.heal + (themeSkill.perHeal * addValue);
        int[] effect = new int[themeSkill.effect.Length];
        for (int i = 0; i < themeSkill.effect.Length; i++)
        {
            effect[i] = themeSkill.effect[i] + (themeSkill.perEffect[i] * addValue);
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
