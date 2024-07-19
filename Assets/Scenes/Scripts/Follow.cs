using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target;
    
    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate() 
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance._player.transform.position);
    }
}
