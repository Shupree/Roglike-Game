using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// MasterPiece(걸작) Manager
public class MPManager : MonoBehaviour
{
    // 걸작 효과에 화상 적용 넣을까..
    public MasterPieceData[] all_MPData;      // 모든 걸작 데이터
    private MasterPieceData[] have_MPData = new MasterPieceData[2];    // 플레이어가 가지고 있는 모든 걸작 데이터
    public MasterPieceData use_MPData;     // 사용할 걸작 데이터

    private GameObject UIObject;

    private Player _player;
    private Enemy _targetInfo;
    private List<Enemy> _EnemyInfoList;
    private Paint[] _PaintScripts;

    public GameObject MP_BtnUI;
    //private Image MP_BtnEmptyImg;
    //private Image MP_BtnImg;
    private Image MP_BtnImg;

    private int damage;
    private int shield;
    private int heal;
    private int effect;
    
    private int addValue;

    private bool isOpend;

    void Awake()
    {
        UIObject = transform.GetChild(0).gameObject;
        UIObject.SetActive(false);

        // 테스트용 걸작 데이터 획득
        have_MPData[0] = all_MPData[0];
        use_MPData = have_MPData[0];

        // 걸작 세팅
        GameManager.instance.MP_Data = use_MPData;

        _player = GameManager.instance._player;
        _EnemyInfoList = GameManager.instance.EnemyInfoList;
        _PaintScripts = GameManager.instance._PaintScripts;

        // 걸작 사용 버튼 세팅
        //MP_BtnEmptyImg = MP_BtnUI.transform.GetChild(0).GetComponent<Image>();
        //MP_BtnImg = MP_BtnUI.transform.GetChild(1).GetComponent<Image>();
        MP_BtnImg = MP_BtnUI.transform.GetChild(1).GetComponent<Image>();

        damage = 0;
        shield = 0;
        heal = 0;
        effect = 0;
    }

    public void LateUpdate()
    {
        if (use_MPData.conditionType == MasterPieceData.ConditionType.Cost) {
            MP_BtnImg.fillAmount = GameManager.instance._PaintManager.stack / (float)use_MPData.maximumCondition;
        }
        else {
            MP_BtnImg.fillAmount = GameManager.instance._PaintManager.stack / (float)use_MPData.cost;
        }
    }

    public void MPFunction()
    {   
        // 스택 수 부족 시 return
        if(GameManager.instance._PaintManager.stack < use_MPData.cost) {
            return;
        }
        
        _targetInfo = GameManager.instance.targetInfo;

        switch (use_MPData.conditionType) {
            case MasterPieceData.ConditionType.None:
                GameManager.instance._PaintManager.stack = 0;
                addValue = 1;
                break;
            case MasterPieceData.ConditionType.Cost: 
                addValue = (GameManager.instance._PaintManager.stack - use_MPData.cost) / use_MPData.perCondition;  // 여분 코스트 / 필요 수치
                GameManager.instance._PaintManager.stack = 
                    (GameManager.instance._PaintManager.stack - use_MPData.cost) % use_MPData.perCondition;  // 여분 반환
                break;
            case MasterPieceData.ConditionType.Health:
                if (_player.health < use_MPData.perCondition) {
                    return;    // HP가 부족하다면 사용 불가능
                }
                else {
                    _player.health -= use_MPData.perCondition;     // 필요 수치만큼 플레이어 HP 감소
                }
                GameManager.instance._PaintManager.stack = 0;
                addValue = 1;   // 횟수는 1회로 한정
                break;
            case MasterPieceData.ConditionType.Paint:  // 물감
                int i = 0;
                if (use_MPData.conditionColor == MasterPieceData.PaintType.B) {
                    i = 1;
                }
                else if (use_MPData.conditionColor == MasterPieceData.PaintType.Y) {
                    i = 2;
                }
                else if (use_MPData.conditionColor == MasterPieceData.PaintType.W) {
                    i = 3;
                }

                // 제한 없음.   (물감 1당 1스택)
                if (use_MPData.maximumCondition == -1) {
                    addValue = _PaintScripts[i].currentNum;
                    _PaintScripts[i].currentNum = 0;   // 물감 전부 제거
                    GameManager.instance._PaintManager.stack = 0;
                }
                // 조건: n만큼의 물감
                else {
                    if (_PaintScripts[i].currentNum < use_MPData.perCondition) {
                        Debug.Log("걸작 사용에 물감이 부족합니다!");    // 물감 부족 시 사용 불가능
                        return;
                    }
                    else {
                        _PaintScripts[i].currentNum -= use_MPData.perCondition;    // 물감 수 감소
                        GameManager.instance._PaintManager.stack = 0;
                        addValue = 1;
                    }
                }
                break;
            case MasterPieceData.ConditionType.Gold:
                //
                // 골드 및 상점 시스템부터 제작 후 제작할 것!
                //
                //
                break;
            
        }

        damage = use_MPData.basicDamage + (use_MPData.perDamage * addValue);
        shield = use_MPData.baseShield + (use_MPData.perShield * addValue);
        heal = use_MPData.baseHeal + (use_MPData.perHeal * addValue);
        effect = use_MPData.basicEffect + (use_MPData.perEffect * addValue);

        // 공격 type에 따른 분류
        switch (use_MPData.attackType) {
            // 단타 공격
            case MasterPieceData.AttackType.Single:
                _targetInfo.health -= 
                    damage + _targetInfo.debuffArr[0];

                // 적 디버프
                if (use_MPData.effectType > 0) {
                    if (use_MPData.effectType < 20) {
                        _targetInfo.debuffArr[use_MPData.effectType - 1] += effect;
                    }
                    else if (use_MPData.effectType == 51) {     // 물감 강탈 효과
                        int n = UnityEngine.Random.Range(0, 3);
                        _PaintScripts[n].currentNum += 1;
                        if (_PaintScripts[n].currentNum > _PaintScripts[n].maxNum) {
                            _PaintScripts[n].currentNum = _PaintScripts[n].maxNum;
                        }
                    }
                }
                Debug.Log("적은 "+damage+"의 데미지를 입었다.");
                break;
            // 다단 히트
            case MasterPieceData.AttackType.Multiple:
                for (int i = 0; i < use_MPData.count; i++) {    // 타수 증가
                    _targetInfo.health -= 
                        damage + _targetInfo.debuffArr[0];

                    // 적 디버프
                    if (use_MPData.effectType > 0) {
                        if (use_MPData.effectType < 20) {
                            _targetInfo.debuffArr[use_MPData.effectType - 1] += effect;
                        }
                        else if (use_MPData.effectType == 51) {     // 물감 강탈 효과
                            int n = UnityEngine.Random.Range(0, 3);
                            _PaintScripts[n].currentNum += 1;
                            if (_PaintScripts[n].currentNum > _PaintScripts[n].maxNum) {
                                _PaintScripts[n].currentNum = _PaintScripts[n].maxNum;
                            }
                        }
                    }
                    Debug.Log("적은 "+damage+"의 데미지를 입었다.");
                }
                break;
            // 전체 공격
            case MasterPieceData.AttackType.Splash:
                for(int i = 0; i < GameManager.instance.EnemyList.Count; i++) {
                    _EnemyInfoList[i].health -=
                        damage + _EnemyInfoList[i].debuffArr[0];

                    // 적 디버프
                    if (use_MPData.effectType > 0) {
                        if (use_MPData.effectType < 20) {
                            _EnemyInfoList[i].debuffArr[use_MPData.effectType - 1] += effect;
                        }
                        else if (use_MPData.effectType == 51) {     // 물감 강탈 효과
                            int n = UnityEngine.Random.Range(0, 3);
                            _PaintScripts[n].currentNum += 1;
                            if (_PaintScripts[n].currentNum > _PaintScripts[n].maxNum) {
                                _PaintScripts[n].currentNum = _PaintScripts[n].maxNum;
                            }
                        }
                    }
                }
                Debug.Log("적들은 "+damage+"의 데미지를 입었다.");
                break;
        }

        // 고유 효과
        switch (use_MPData.effectType) {
            case 52:    // 만개 스택
                break;
            case 53:    // 폭탄
                break;

        }

        // 자기 대상 효과
        if (use_MPData.self_EffectType != 0) {
            // 해로운 효과
            if (use_MPData.self_EffectType < 20) {
                _player.debuffArr[use_MPData.effectType - 1] += effect;
            }
            // 이로운 효과
            else {
                _player.buffArr[use_MPData.self_EffectType - 21] += use_MPData.self_Effect;
            }
        }

        // 플레이어 보호막 획득
        _player.shield += shield;

        // 플레이어 회복
        _player.health += heal;
        if (_player.health > _player.maxHealth) {
            _player.health = _player.maxHealth;
        }

        //
        // 추가 효과(Extra Effect)는 사용될 시 제작하겠음.
        //
        //

        // 적 디버프 연산
        for(int a = 0; a < _EnemyInfoList.Count; a++) {
            // 감전 효과 연산
            _EnemyInfoList[a].ElectricShock();
            // 추위 효과 연산
            _EnemyInfoList[a].Coldness();
        }
        
        // 플레이어 빙결 효과
        _player.Coldness();
        // 플레이어 감전 효과
        _player.ElectricShock();

        // 타겟 사망 시 타겟팅 재설정
        if (!GameManager.instance.target) {
            GameManager.instance.target = GameManager.instance.EnemyList[0];
            _targetInfo = _EnemyInfoList[0];
        }

        GameManager.instance.CheckVictory();

        // 유물 : 걸작 사용 시 효과
        GameManager.instance._ArtifactManager.ArtifactFunction(ArtifactData.TriggerSituation.UseMP);
    }

    public void BagBtn()
    {
        if (isOpend == false) {
            UIObject.SetActive(true);
            isOpend = true;
        }
        else {
            UIObject.SetActive(false);
            isOpend = false;
        }
    }
}
