using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public SkillData data;
    public int level;

    Image icon;
    Text explanation;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.skillIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        explanation = texts[0];
    }

    void LateUpdate() 
    {
        explanation.text = "Enforced." + (level + 1);
    }

    public void OnClick()
    {
        
    }
}
