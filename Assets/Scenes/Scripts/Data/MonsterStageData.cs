using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSet", menuName = "Scriptble Object/MonsterStageData")]
public class MonsterStageData : ScriptableObject
{
    // 스킬 테이블
    public enum StageType { Normal, Elite, Boss }

    [Header("# Main Info")]
    public StageType stageType;
    // 적용 스테이지 : 1S, 2S, 3S, 4S   (0S는 테스트 맵)
    public string stageStr;
    // 해당 스테이지의 세팅 넘버 : 1S - "1"
    public int setNum;
    // 포지션별 몬스터 배치 (int EnemyID)
    public int monster01;
    public int monster02;
    public int monster03;
    public int monster04;
}
