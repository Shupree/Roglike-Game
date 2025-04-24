using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/*
// MasterPiece(걸작) Manager
public class MPManager : MonoBehaviour
{
    // 걸작 효과에 화상 적용 넣을까..
    [Header ("Reference")]
    private Player _player;
    private Enemy _targetInfo;
    private List<Enemy> _EnemyInfoList;
    private Paint[] _PaintScripts;
    private PaletteManager _PaletteManager;

    [Header ("MasterPiece Data")]
    public MasterPieceData[] all_MPData;      // 모든 걸작 데이터
    //private MasterPieceData[] have_MPData = new MasterPieceData[2];    // 플레이어가 가지고 있는 모든 걸작 데이터
    public MasterPieceData use_MPData;     // 사용할 걸작 데이터

    [Header ("UI")]
    private GameObject UIObject;    // UI

    public GameObject MP_BtnUI;
    //private Image MP_BtnEmptyImg;
    private Image MP_BtnImg;

    private int finalDamage;
    private int damage;
    private int shield;
    private int heal;
    private int effect;
    
    private int addValue;

    private bool isOpend;

    public bool canUseMP;

    void Awake()
    {
        _PaletteManager = GameManager.instance._PaletteManager;

        UIObject = transform.GetChild(0).gameObject;
        UIObject.SetActive(false);

        // 테스트용 걸작 데이터 획득
        //have_MPData[0] = all_MPData[0];
        use_MPData = all_MPData[0];

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

    void LateUpdate()
    {
        if (use_MPData.conditionType == MasterPieceData.ConditionType.Cost) {   // 걸작 스택 UI 표시
            MP_BtnImg.fillAmount = GameManager.instance._PaletteManager.stack / (float)use_MPData.maximumCondition;
        }
        else {
            MP_BtnImg.fillAmount = GameManager.instance._PaletteManager.stack / (float)use_MPData.cost;
        }
    }

    public void ChangeMasterPiece(int masterPieceId)
    {
        use_MPData = all_MPData[masterPieceId];     // 걸작 교체

        // 걸작 아이콘 교체@@@
    }

    // 무작위 걸작 추첨
    public MasterPieceData PickRandomMasterPiece()
    {
        int randomNum = 0;
        MasterPieceData MPData = all_MPData[0];
        
        //int errorNum = 0;

        // 무작위 걸작 추첨
        for (int i = 0; i < 1;)
        {
            randomNum = UnityEngine.Random.Range(0, all_MPData.Length);

            // 이미 해당 걸작를 지니고 있는 경우 : 재추첨 (무한 for문 대책이 안되있음 주의 : 최소 2개 이상의 걸작 종류 필요)
            if (use_MPData == all_MPData[randomNum]) {
                //if (errorNum >= 50) {
                //    Debug.Log("오류 발생!!");
                //    break;
                //}
                //errorNum++;
                continue;
            }
            else {
                MPData = all_MPData[randomNum];
                i++;
            }
        
        }

        return MPData;
    }

    // 걸작 효과
    public void MPFunction()
    {   
        // 걸작 사용 불가능한지 판별
        if (canUseMP != true) {
            return;
        }

        // 스택 수 부족 시 return
        if (_PaletteManager.stack < use_MPData.cost) {
            return;
        }
        
        _targetInfo = GameManager.instance.targetInfo;

        switch (use_MPData.conditionType) {
            case MasterPieceData.ConditionType.None:
                _PaletteManager.stack = 0;
                addValue = 1;
                break;
            case MasterPieceData.ConditionType.Cost: 
                addValue = (_PaletteManager.stack - use_MPData.cost) / use_MPData.perCondition + 1;  // 여분 코스트 / 필요 수치 + 1
                _PaletteManager.stack = 
                    (_PaletteManager.stack - use_MPData.cost) % use_MPData.perCondition;  // 여분 반환
                break;
            case MasterPieceData.ConditionType.Health:
                if (_player.health < use_MPData.maximumCondition) {
                    return;    // HP가 부족하다면 사용 불가능
                }
                else {
                    _player.health -= use_MPData.maximumCondition;     // 필요 수치만큼 플레이어 HP 감소
                }
                _PaletteManager.stack = 0;
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
                    _PaletteManager.stack = 0;
                }
                // 조건: n만큼의 물감
                else {
                    if (_PaintScripts[i].currentNum < use_MPData.maximumCondition) {
                        Debug.Log("걸작 사용에 사용할 물감이 부족합니다!");    // 물감 부족 시 사용 불가능
                        return;
                    }
                    else {
                        _PaintScripts[i].currentNum -= use_MPData.maximumCondition;    // 물감 수 감소
                        _PaletteManager.stack = 0;
                        addValue = 1;
                    }
                }
                break;
            case MasterPieceData.ConditionType.Gold:
                if (_player.gold < use_MPData.maximumCondition) {
                    Debug.Log("걸작 사용에 사용할 골드가 부족합니다.");     // 골드 부족 시 사용 불가능
                    return;
                }
                else {
                    _player.gold -= use_MPData.maximumCondition;    // 골드 수 감소
                    _PaletteManager.stack = 0;
                    addValue = 1;
                }
                break;
            
        }

        damage = use_MPData.basicDamage + (use_MPData.perDamage * addValue);
        shield = use_MPData.baseShield + (use_MPData.perShield * addValue);
        heal = use_MPData.baseHeal + (use_MPData.perHeal * addValue);
        effect = use_MPData.basicEffect + (use_MPData.perEffect * addValue);

        // 공격 type에 따른 분류
        switch (use_MPData.attackType) {
            // 단일 공격
            case MasterPieceData.AttackType.Single:
                for (int i = 0; i < use_MPData.count; i++) {    // 타수만큼 반복
                    if (damage > 0) {
                        finalDamage = damage + _targetInfo.debuffArr[0];        // 최종 데미지 = 기본 데미지 + 적 화상 수치
                        if (_targetInfo.shield > 0) {                           // 적 실드 존재 시
                            if (_targetInfo.shield > finalDamage) {
                                _targetInfo.shield -= finalDamage;
                                finalDamage = 0;
                            }
                            else {
                                finalDamage -= _targetInfo.shield;
                            }
                        }
                        _targetInfo.health -= finalDamage;      // 데미지 누적
                    }

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
                    Debug.Log(_targetInfo.data.enemyName+"은(는) "+finalDamage+"의 데미지를 입었다.");
                }
                break;
            // 바운스
            case MasterPieceData.AttackType.Bounce:
                List<Enemy> attackedEnemyList = _EnemyInfoList.ToList();         // 적 생존 확인용 간이 리스트
                for (int i = 0; i < use_MPData.count; i++) {    // 타수만큼 반복
                    int randomNum = UnityEngine.Random.Range(0, GameManager.instance.EnemyList.Count);      // 랜덤 적 선정
                    
                    // Hp없는 적에게 공격이 튀는 것을 방지
                    if (attackedEnemyList[randomNum].health < 0) {
                        attackedEnemyList.RemoveAt(randomNum);
                        i--;
                        if (attackedEnemyList.Count <= 0) {
                            break;
                        }
                        else {
                            continue;
                        }
                    }

                    if (damage > 0) {
                        finalDamage = damage + _EnemyInfoList[randomNum].debuffArr[0];        // 최종 데미지 = 기본 데미지 + 적 화상 수치
                        if (_EnemyInfoList[randomNum].shield > 0) {             // 적 실드 존재 시
                            if (_EnemyInfoList[randomNum].shield > finalDamage) {
                                _EnemyInfoList[randomNum].shield -= finalDamage;
                                finalDamage = 0;
                            }
                            else {
                                finalDamage -= _EnemyInfoList[randomNum].shield;
                            }
                        }
                        _EnemyInfoList[randomNum].health -= finalDamage;        // 데미지 누적
                    }

                    // 적 디버프
                    if (use_MPData.effectType > 0) {
                        if (use_MPData.effectType < 20) {
                            _EnemyInfoList[randomNum].debuffArr[use_MPData.effectType - 1] += effect;
                        }
                        else if (use_MPData.effectType == 51) {     // 물감 강탈 효과
                            int n = UnityEngine.Random.Range(0, 3);
                            _PaintScripts[n].currentNum += 1;
                            if (_PaintScripts[n].currentNum > _PaintScripts[n].maxNum) {
                                _PaintScripts[n].currentNum = _PaintScripts[n].maxNum;
                            }
                        }
                    }
                    Debug.Log(_EnemyInfoList[randomNum].data.enemyName+"은(는) "+finalDamage+"의 데미지를 입었다.");
                }
                break;
            // 전체 공격
            case MasterPieceData.AttackType.Splash:
                for (int a = 0; a < use_MPData.count; a++) {    // 타수만큼 반복
                    for(int i = 0; i < GameManager.instance.EnemyList.Count; i++) {
                        if (damage > 0) {
                            finalDamage = damage + _EnemyInfoList[i].debuffArr[0];        // 최종 데미지 = 기본 데미지 + 적 화상 수치
                            if (_EnemyInfoList[i].shield > 0) {             // 적 실드 존재 시
                                if (_EnemyInfoList[i].shield > finalDamage) {
                                    _EnemyInfoList[i].shield -= finalDamage;
                                    finalDamage = 0;
                                }
                                else {
                                    finalDamage -= _EnemyInfoList[i].shield;
                                }
                            }
                            _EnemyInfoList[i].health -= finalDamage;        // 데미지 누적
                        }

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
                        Debug.Log(_EnemyInfoList[i].data.enemyName+"은(는) "+finalDamage+"의 데미지를 입었다.");
                    }
                }
                break;
        }

        // 고유 효과
        switch (use_MPData.effectType) {
            case 52:    // 만개 스택
                break;
            //case 53:    // 폭탄
                //break;

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
            GameManager.instance.targetInfo = GameManager.instance.EnemyInfoList[0];
        }

        // 플레이어의 승리 확인
        // GameManager.instance.CheckVictory();

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
*/
