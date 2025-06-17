using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetObject : MonoBehaviour
{
    private TurnManager turnManager;
    private StorageManager storageManager;
    private Player player;

    private RaycastHit2D hit;

    public void Initialize()
    {
        turnManager = GameManager.instance.turnManager;
        storageManager = GameManager.instance.storageManager;
        player = GameManager.instance.player;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            // 마우스 포지션에서 레이를 던져서 잡힌 오브젝트를 hit에 저장
            if(hit.collider != null)
            {
                switch(hit.collider.tag){
                    case "Enemy":
                        Debug.Log(hit.collider.gameObject.name);
                        if (player.mainSkill != null &&
                            player.mainSkill.skillType == Skill.SkillType.SingleAtk &&
                            turnManager.GetState() == TurnManager.State.playerAct)      // 단일 스킬일 경우
                        {
                            turnManager.targets.Clear();
                            turnManager.targets.Add(hit.collider.gameObject.GetComponent<Enemy>());      // 타겟팅
                        }
                        else if (storageManager._MPManager.GetMPData() != null &&
                                storageManager._MPManager.GetMPData().skillType == Skill.SkillType.SingleAtk &&
                                turnManager.GetState() == TurnManager.State.useMP)      // 걸작 스킬이 단일 스킬일 경우
                        { 
                            turnManager.targets.Clear();
                            turnManager.targets.Add(hit.collider.gameObject.GetComponent<Enemy>());      // 타겟팅
                        }
                        break;
                    case "Chest":
                        Debug.Log(hit.collider.gameObject.name);
                        // hit.collider.gameObject.GetComponent<ChestScript>().OpenChest();
                        break;
                }
            }
        }
    }
}

