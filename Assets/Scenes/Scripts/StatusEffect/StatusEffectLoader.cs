using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Status_Effects.json 로드 후 구조체로 구성    (파일 위치 : Assets/Scenes/Scripts/Resources)
public class StatusEffectLoader
{
    public List<StatusEffect> LoadStatusEffects(string fileName)
    {
        // Resources 폴더에서 파일 로드 (확장자 제외)
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

        if (jsonFile == null)
        {
            Debug.LogError($"JSON 파일 {fileName}을(를) 찾을 수 없습니다. Resources 폴더에 파일이 있는지 확인하세요.");
            return new List<StatusEffect>();
        }

        string json = jsonFile.text;

        // JSON 데이터 역직렬화
        StatusEffectContainer container = JsonUtility.FromJson<StatusEffectContainer>(json);

        if (container == null || container.StatusEffects == null)
        {
            Debug.LogError("JSON 데이터를 역직렬화하는 데 실패했습니다. 파일 형식을 확인하세요.");
            return new List<StatusEffect>();
        }

        Debug.Log($"로드된 상태이상 효과 개수: {container.StatusEffects.Count}");

        // 오류 확인
        foreach (var status in container.StatusEffects)
        {
            if (!Enum.IsDefined(typeof(EffectInfo), status.effectInfo.ToString()))
            {
                Debug.LogWarning($"정의되지 않은 effectInfo 값: {status.effectInfo}");
            }
        }
        
        return container.StatusEffects;
    }
}
