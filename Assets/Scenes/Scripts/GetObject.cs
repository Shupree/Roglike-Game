using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
public class GetObject : MonoBehaviour
{
    private RaycastHit2D hit;

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
                        GameManager.instance.target = hit.collider.gameObject;      // 타겟팅
                        GameManager.instance.targetInfo = hit.collider.gameObject.GetComponent<Enemy>();
                        break;
                    case "Chest":
                        Debug.Log(hit.collider.gameObject.name);
                        hit.collider.gameObject.GetComponent<ChestScript>().OpenChest();
                        break;
                }
            }
        }
    }
}
*/
