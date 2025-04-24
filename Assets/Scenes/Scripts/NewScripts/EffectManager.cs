using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public class StatusEffectLoader
    {
        public static List<StatusEffect> LoadStatusEffects(string filePath)     // (static : 객체 생성없이 함수 이용 가능, 메모리 상시 차지)
        {
            string json = File.ReadAllText(filePath);       // json 파일 불러오기
            return JsonUtility.FromJson<StatusEffectList>(json).Effects;    // JsonUtility로 데이터 변환
        }
    }

    [System.Serializable]
    public class StatusEffectList
    {
        public List<StatusEffect> Effects;
    }
}