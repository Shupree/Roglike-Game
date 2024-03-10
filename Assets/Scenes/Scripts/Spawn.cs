using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] Position;
    public GameObject Dummy_Monster;
    public GameObject Monster_01;
    public GameObject Monster_02;
    public void ClickToSpawn()
    {
        Monster_01 = Instantiate(Dummy_Monster, Position[0].transform.position, Quaternion.Euler(0, 0, 0));
        Monster_01.transform.parent = Position[0].transform;
        GameManager.instance.EnemyArr.Add(Monster_01);
        //GameManager.instance.EnemyNum++;

        Monster_02 = Instantiate(Dummy_Monster, Position[1].transform.position, Quaternion.Euler(0, 0, 0));
        Monster_02.transform.parent = Position[1].transform;
        GameManager.instance.EnemyArr.Add(Monster_02);
        //GameManager.instance.EnemyNum++;
    }
}
