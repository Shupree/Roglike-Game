using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactManager : MonoBehaviour
{
    public ArtifactData[] all_ArtifactData;   // 모든 유물 데이터
    private List<ArtifactData> have_ArtifactData;  // 플레이어가 지니고 있는 모든 유물 데이터
    // 리스트이지만 최대 수는 6개이기 때문에 유의해서 코딩해야함.

    private GameObject slotParent;
    private Image[] slotImgArr = new Image[6];

    private Player _player;
    private Enemy _targetInfo;
    private List<Enemy> targetInfoList;
    private Paint[] _PaintScripts;
    private bool canUseArtifact;

    void Awake()
    {
        slotParent = transform.GetChild(0).GetChild(0).gameObject;

        for (int i = 0; i < 6; i++) 
        {
            slotImgArr[i] = slotParent.transform.GetChild(i).GetComponent<Image>();
        }

        _player = GameManager.instance._player;
        _PaintScripts = GameManager.instance._PaintScripts;
    }

    // Data.WhenToTrigger에 따른 함수 작동
    public void ArtifactFunction(string triggerName)
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
                case "None":
                    break;
                case "PlayerEffect":
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
                case "EnemyEffect":
                    // 적중 시 적 인원 수 확인
                    if (have_ArtifactData[i].whenToTrigger == "OnHit") {
                        if (GameManager.instance.usingSkill.attackType != "Multiple") {
                            targetInfoList = GameManager.instance.EnemyInfoList;
                        }
                        else {
                            targetInfoList[0] = _targetInfo;
                        }
                    }
                    // 턴 시작 시 모든 적 인원 수 확인
                    if (have_ArtifactData[i].whenToTrigger == "StartTurn") {
                        targetInfoList = GameManager.instance.EnemyInfoList;
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
                case "Health":
                    if (_player.health < have_ArtifactData[i].conditionNum) {
                        canUseArtifact = false;     // HP가 부족하면 사용 불가능
                    }
                    else {
                        _player.health -= have_ArtifactData[i].conditionNum;
                    }
                    break;
                case "Paint":
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
                case "Gold":
                    //
                    // 골드 및 상점 시스템부터 제작 후 제작할 것!
                    //
                    //
                    break;
            }
            if (canUseArtifact == false) {
                continue;
            }

            if (targetInfoList.Count <= 0) {
                switch (have_ArtifactData[i].target) {
                    case "Target":
                        targetInfoList[0] = _targetInfo;
                        break;
                    case "All":
                        targetInfoList = GameManager.instance.EnemyInfoList;
                        break;
                }
            }

            // 마저 만들 것!
            //
            //
            //
        }
    }
}
