using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public enum SkillType
    {
        SingleAtk, SplashAtk, SingleSup, SplashSup
    }

    public string name;
    public string notation;
    public string icon;
    public string desc;
    public int colorType;       // 1 : 빨강, 2 : 파랑, 3 : 노랑, 4 : 하양
    public SkillType skillType;       // Single, Splash
    public int paintCondition;          // 메인 물감 수

    public int damage;
    public int count;
    public int shield;
    public int heal;
    public string effectType;
    public int effect;
}

public class CsvSkillLoader : MonoBehaviour
{
    public List<Skill> skillList = new List<Skill>();
    string path;

    void Awake()
    {
        path = "Files/SkillInfo.csv";
        ReadSkillCsv("Skills");

        foreach (var skill in skillList)
        {
            Debug.Log($"스킬 이름: {skill.name}");
        }
    }

    void ReadSkillCsv(string fileName)
    {
        if (!File.Exists(Application.dataPath + "/" + path))
        {
            Debug.LogError("csv파일을 찾을 수 없습니다.");
            return;
        }

        // csv 파일 읽기
        StreamReader reader = new StreamReader(Application.dataPath + "/" + path);    // Scv 폴더에서 파일 로드
    
        bool isFinish = false;
        bool isSkipFirstLine = false;

        while(isFinish == false)
        {
            // 한줄씩 string으로 변환
            string data = reader.ReadLine();    // 한 줄 읽기

            // data 변수가 비었는지 확인
            if(data == null) {
                // 반복문 해체
                isFinish = true;
                break;
            }

            if(!isSkipFirstLine) {
                isSkipFirstLine = true;
                continue;
            }

            // csv는 ',' 기준으로 구분 후 list에 저장
            var splitData = data.Split(',');    // ',' 단위로 분할

            // Skill 객체 선언
            Skill skill = new Skill();

            skill.name = splitData[0];
            skill.notation = splitData[1];
            skill.icon = splitData[2];
            skill.desc = splitData[3];
            skill.colorType = int.Parse(splitData[4]);
            skill.skillType = (Skill.SkillType)Enum.Parse(typeof(Skill.SkillType),splitData[5]);
            skill.paintCondition = int.Parse(splitData[6]);

            skill.damage = int.Parse(splitData[7]);
            skill.count = int.Parse(splitData[8]);
            skill.shield = int.Parse(splitData[9]);
            skill.heal = int.Parse(splitData[10]);
            skill.effectType = splitData[11];
            skill.effect = int.Parse(splitData[12]);

            skillList.Add(skill);
        }
        Debug.Log("스킬 데이터 로드 성공.");
    }
}
