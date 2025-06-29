using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetPattern : MonoBehaviour
{
    /*
    [Header ("reference")]
    private Enemy _Enemy;

    [Header ("SkillData")]
    public MonsterSkillData[] basicSkillArr;    // 조건X 스킬 Array
    public MonsterSkillData[] conditionalSkillArr;  // 조건부 스킬 Array

    [Header ("Skill To Use")]
    [HideInInspector] public MonsterSkillData skillData;    // 사용할 스킬

    [Header ("Order")]
    private int order = 0;  // 스킬 사용 순서
    private int[] conditionalNumArr;  // 조건부 스킬 사용 횟수 Array

    void Awake()
    {
        _Enemy = transform.gameObject.GetComponent<Enemy>();    // Enemy스크립트 가져오기

        conditionalNumArr = new int[conditionalSkillArr.Length];    // 사용 횟수 초기화
    }

    public MonsterSkillData SetSkill()  // 사용할 스킬 확정 후 return
    {
        skillData = null;

        // 조건부 스킬의 조건 확인
        for (int i = 0; i < conditionalSkillArr.Length; i++) {
            switch (conditionalSkillArr[i].conditionType) {
                case MonsterSkillData.ConditionType.HP:
                    // 스킬 사용 횟수가 남아있고 HP조건을 달성했을 때 실행
                    if (conditionalSkillArr[i].availableNum > conditionalNumArr[i] && conditionalSkillArr[i].conditionNum > _Enemy.health) {
                        skillData = conditionalSkillArr[i];     // 해당 스킬 사용 확정
                        conditionalNumArr[i]++;                 // 스킬 사용 횟수 증가
                    }
                    break;
                }

            // 사용할 스킬이 확정될 시 break
            if (skillData != null) {
                break;
            }
        }

        // 일반 스킬의 사용 순서 확인
        if (skillData == null) {
            skillData = basicSkillArr[order];   // 해당 스킬 사용 확정
            
            // 일반 스킬의 순서 정렬
            if (order >= basicSkillArr.Length) {
                order = 0;      // 순서가 끝까지 도달 시 초기화
            }
            else {
                order++;        // 순서+
            }
        }

        return skillData;
    }
    */
}
