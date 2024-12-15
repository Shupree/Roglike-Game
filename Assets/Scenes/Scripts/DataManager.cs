using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

// 플레이어 정보
public class PlayerData
{
    public int gold;
    public int hp;
}

public class DataManager : MonoBehaviour
{
    // 싱글톤
    public static DataManager instance;

    // 데이터 생성
    PlayerData nowPlayer = new PlayerData();

    string path;
    string filename = "save";

    private void Awake()
    {
        // 인스턴스화
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        // 오브젝트 파괴 방지
        DontDestroyOnLoad(this.gameObject);

        path = Application.persistentDataPath + "/";
    }

    void Start()
    {

    }

    public void SaveData()
    {
        // Json으로 변환 (string 형식)
        string data = JsonUtility.ToJson(nowPlayer);
        
        File.WriteAllText(path + filename, data);
    }

    public void LoadData()
    {
        // Data로 변환
        string data = File.ReadAllText(path + filename);
        nowPlayer = JsonUtility.FromJson<PlayerData>(data);
    }
}
