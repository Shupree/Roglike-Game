using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactManager : MonoBehaviour
{
    public ArtifactData[] all_ArtifactData;   // 모든 유물 데이터
    private List<ArtifactData> have_ArtifactData = new List<ArtifactData>();  // 플레이어가 지니고 있는 모든 유물 데이터 리스트
    // 리스트이지만 최대 수는 6개이기 때문에 유의해서 코딩해야함.

    private GameObject slotParent;
    private Image[] slotImgArr = new Image[6];

    private Player _player;
    private Enemy _targetInfo;
    private List<Enemy> _EnemyInfoList;
    private Paint[] _PaintScripts;

    private List<Enemy> targetInfoList = new List<Enemy>();
    private bool canUseArtifact;

    void Awake()
    {
        slotParent = transform.GetChild(0).GetChild(0).gameObject;

        for (int i = 0; i < 6; i++) 
        {
            slotImgArr[i] = slotParent.transform.GetChild(i).GetComponent<Image>();
        }

        _player = GameManager.instance._player;
        _EnemyInfoList = GameManager.instance.EnemyInfoList;
        _PaintScripts = GameManager.instance._PaintScripts;

        // 임시 이미지 장착
        // 본래는 유물 획득/제거/교체 시에 유물 이미지를 변경하면 됨.
        AddArtifact(3);
    }

    public void AddArtifact(int artifactId) 
    {   
        // 아티팩트가 최대(6개) 개수 미만일 때
        if (have_ArtifactData.Count < 6) {
            have_ArtifactData.Add(all_ArtifactData[artifactId]);
            slotImgArr[have_ArtifactData.Count - 1].sprite = have_ArtifactData[have_ArtifactData.Count - 1].sprite;
        }
        // 아티팩트가 최대(6개) 개수를 넘었을 때
        else {
            // 아티팩트 교환 UI 출력해야 함.
        }
    }

    public List<ArtifactData> PickRandomArtifact(int num)     // num 수만큼 뽑기
    {
        int randomNum = 0;
        List<ArtifactData> artifactList = new List<ArtifactData>();

        //int errorNum = 0;

        // 무작위 장신구 추첨
        for (int i = 0; i < num;)
        {
            randomNum = UnityEngine.Random.Range(0, all_ArtifactData.Length);

            /*for (int a = 0; a < pickedSkillId.Count; a++)
            {
                // 이미 뽑았던 장신구는 재추점 1회 방지 (새로운 스킬 추첨률 증가)
                if (randomNum == pickedSkillId[a]) {
                    i = 0;
                    pickedSkillId.Remove(randomNum);
                }
            }*/

            // 이미 해당 장신구를 지니고 있는 경우 : 재추첨 (무한 for문 대책이 안되있음 주의 : 최소 10개 이상의 장신구 종류 필요)
            if (have_ArtifactData.Contains(all_ArtifactData[randomNum])) {
                //errorNum++;
                continue;
            }
            // 중복된 장신구가 있는 경우 : 재추첨
            else if (artifactList.Contains(all_ArtifactData[randomNum])) {
                //errorNum++;
                continue;
            }
            else {
                artifactList.Add(all_ArtifactData[randomNum]);
                i++;
            }
        }

        return artifactList;
    }

    // Data.WhenToTrigger에 따른 함수 작동
    public void ArtifactFunction(ArtifactData.TriggerSituation triggerName)
    {
        for (int i = 0; i < have_ArtifactData.Count; i++)
        {
            // 트리거 확인
            if (triggerName != have_ArtifactData[i].whenToTrigger) {
                continue;
            }

            // 유물 효과
            targetInfoList.Clear();     // 유물의 타겟 초기화
            canUseArtifact = true;      // 유물 사용가능하도록 설정
            _targetInfo = GameManager.instance.targetInfo;      // 스킬의 타겟 초기화

            switch (have_ArtifactData[i].conditionType) {
                case ArtifactData.ConditionType.None:
                    break;
                case ArtifactData.ConditionType.PlayerEffect:
                    // 디버프일 시
                    if (have_ArtifactData[i].conditionEffect < 20) {
                        if (_player.debuffArr[have_ArtifactData[i].conditionEffect - 1] > have_ArtifactData[i].conditionNum) {
                            canUseArtifact = false;
                        }
                    }
                    // 버프일 시
                    else if (have_ArtifactData[i].conditionEffect < 30) {
                        if (_player.buffArr[have_ArtifactData[i].conditionEffect - 21] < have_ArtifactData[i].conditionNum) {
                            canUseArtifact = false;
                        }
                    }
                    break;
                case ArtifactData.ConditionType.EnemyEffect:
                    // 적중 시 적 인원 수 확인
                    if (have_ArtifactData[i].whenToTrigger == ArtifactData.TriggerSituation.OnHit) {
                        if (GameManager.instance.usingSkill.attackType == SkillData.AttackType.Splash) {
                            for (int a = 0; a < GameManager.instance.EnemyInfoList.Count; a++) {
                                targetInfoList.Add(GameManager.instance.EnemyInfoList[a]);
                            }
                        }
                        else {
                            targetInfoList[0] = _targetInfo;
                        }
                    }
                    // 턴 시작 시 모든 적 인원 수 확인
                    else if (have_ArtifactData[i].whenToTrigger == ArtifactData.TriggerSituation.StartTurn) {
                        for (int a = 0; a < GameManager.instance.EnemyInfoList.Count; a++) {
                            targetInfoList.Add(GameManager.instance.EnemyInfoList[a]);
                        }
                    }
                    for(int a = targetInfoList.Count; i >= 0; a--)
                    {
                        // 디버프일 시
                        if (have_ArtifactData[i].conditionEffect < 20) {
                            if (targetInfoList[a].debuffArr[have_ArtifactData[i].conditionEffect - 1] < have_ArtifactData[i].conditionNum) {
                                targetInfoList.Remove(targetInfoList[a]);
                            }
                        }
                        // 버프일 시
                        else if (have_ArtifactData[i].conditionEffect < 30) {
                            if (targetInfoList[a].buffArr[have_ArtifactData[i].conditionEffect - 21] < have_ArtifactData[i].conditionNum) {
                                targetInfoList.Remove(targetInfoList[a]);
                            }
                        }
                    }
                    if (targetInfoList.Count <= 0) {
                        canUseArtifact = false;
                    }
                    break;
                case ArtifactData.ConditionType.Health:
                    if (_player.health < have_ArtifactData[i].conditionNum) {
                        canUseArtifact = false;     // HP가 부족하면 사용 불가능
                    }
                    else {
                        _player.health -= have_ArtifactData[i].conditionNum;
                    }
                    break;
                case ArtifactData.ConditionType.Paint:
                    int x = 0;
                    if (have_ArtifactData[i].conditionColor == "B") {
                        x = 1;
                    }
                    else if (have_ArtifactData[i].conditionColor == "Y") {
                        x = 2;
                    }
                    else if (have_ArtifactData[i].conditionColor == "W") {
                        x = 3;
                    }
                    if (_PaintScripts[x].currentNum < have_ArtifactData[i].conditionNum) {
                        canUseArtifact = false;         // 물감 부족 시 사용 불가능
                    }
                    else {
                        _PaintScripts[x].currentNum -= have_ArtifactData[i].conditionNum;    // 물감 수 감소
                    }
                    break;
                case ArtifactData.ConditionType.Gold:
                    if (_player.gold < have_ArtifactData[i].conditionNum) {
                        canUseArtifact = false;     // HP가 부족하면 사용 불가능
                    }
                    else {
                        _player.gold -= have_ArtifactData[i].conditionNum;
                    }
                    break;
            }
            
            // 아티팩트 조건 부적합 시 사용 실패
            if (canUseArtifact == false) {
                continue;
            }

            // 만약 지정된 대상이 없을 경우
            if (targetInfoList.Count <= 0) {
                switch (have_ArtifactData[i].targetType) {
                    // 대상 : 플레이어
                    case ArtifactData.TargetType.Player:
                        targetInfoList.Clear();
                        break;
                    // 대상 : 현재 타겟
                    case ArtifactData.TargetType.Target:
                        targetInfoList[0] = _targetInfo;
                        break;
                    // 대상 : 적 전체
                    case ArtifactData.TargetType.All:
                        for (int a = 0; a < _EnemyInfoList.Count; a++) {
                            targetInfoList.Add(_EnemyInfoList[a]);
                        }
                        break;
                    // 대상 : 랜덤
                    case ArtifactData.TargetType.Random:
                        int randomNum = UnityEngine.Random.Range(0, _EnemyInfoList.Count);
                        targetInfoList.Add(_EnemyInfoList[randomNum]);
                        break;
                }
            }

            // 적 대상 효과
            if (targetInfoList.Count > 0) {
                for (int a = 0; a < targetInfoList.Count; a++) {
                    // 공격
                    targetInfoList[a].health -= have_ArtifactData[i].damage;
                    // 버프/디버프 부여
                    if (have_ArtifactData[i].effectType > 0) {
                        // 디버프
                        if (have_ArtifactData[i].effectType < 20) {
                            targetInfoList[a].debuffArr[have_ArtifactData[i].effectType - 1] += have_ArtifactData[i].effect;
                        }
                        // 버프
                        else if (have_ArtifactData[i].effectType < 30) {
                            targetInfoList[a].buffArr[have_ArtifactData[i].effectType - 21] += have_ArtifactData[i].effect;
                        }
                    }
                    // 버프/디버프 제거
                    if (have_ArtifactData[i].eraseEffectType > 0) {
                        // 디버프
                        if (have_ArtifactData[i].eraseEffectType < 20) {
                            targetInfoList[a].debuffArr[have_ArtifactData[i].eraseEffectType - 1] -= have_ArtifactData[i].eraseEffect;
                            // 음수 차단
                            if (targetInfoList[a].debuffArr[have_ArtifactData[i].eraseEffectType - 1] < 0) {
                                targetInfoList[a].debuffArr[have_ArtifactData[i].eraseEffectType - 1] = 0;
                            }
                        }
                        // 버프
                        else if (have_ArtifactData[i].eraseEffectType < 30) {
                            targetInfoList[a].buffArr[have_ArtifactData[i].eraseEffectType - 21] -= have_ArtifactData[i].eraseEffect;
                            // 음수 차단
                            if (targetInfoList[a].buffArr[have_ArtifactData[i].eraseEffectType - 21] < 0) {
                                targetInfoList[a].buffArr[have_ArtifactData[i].eraseEffectType - 21] = 0;
                            }
                        }
                    }
                }
            }
            // 플레이어 대상 효과
            else {
                // 버프/디버프 부여
                if (have_ArtifactData[i].effectType > 0) {
                    // 디버프
                    if (have_ArtifactData[i].effectType < 20) {
                        _player.debuffArr[have_ArtifactData[i].effectType - 1] += have_ArtifactData[i].effect;
                    }
                    // 버프
                    else if (have_ArtifactData[i].effectType < 30) {
                        _player.buffArr[have_ArtifactData[i].effectType - 21] += have_ArtifactData[i].effect;
                    }
                }
                // 버프/디버프 제거
                if (have_ArtifactData[i].eraseEffectType > 0) {
                    // 디버프
                    if (have_ArtifactData[i].eraseEffectType < 20) {
                        _player.debuffArr[have_ArtifactData[i].eraseEffectType - 1] -= have_ArtifactData[i].eraseEffect;
                        // 음수 차단
                        if (_player.debuffArr[have_ArtifactData[i].eraseEffectType - 1] < 0) {
                            _player.debuffArr[have_ArtifactData[i].eraseEffectType - 1] = 0;
                        }
                    }
                    // 버프
                    else if (have_ArtifactData[i].eraseEffectType < 30) {
                        _player.buffArr[have_ArtifactData[i].eraseEffectType - 21] -= have_ArtifactData[i].eraseEffect;
                        // 음수 차단
                        if (_player.buffArr[have_ArtifactData[i].eraseEffectType - 21] < 0) {
                            _player.buffArr[have_ArtifactData[i].eraseEffectType - 21] = 0;
                        }
                    }
                }
            }
            // 플레이어 보호막 획득
            _player.shield += have_ArtifactData[i].shield;

            // 플레이어 회복
            _player.health += have_ArtifactData[i].heal;
            if (_player.health > _player.maxHealth) {
                _player.health = _player.maxHealth;
            }

            Debug.Log(have_ArtifactData[i].ArtifactName + "의 효과 발동!");

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
        }
    }
}
