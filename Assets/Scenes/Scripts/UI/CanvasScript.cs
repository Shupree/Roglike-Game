using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    private SkillData skillData;
    public int level;
    public Sprite basicSprite;

    Image icon;
    Text explanation;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];

        Text[] texts = GetComponentsInChildren<Text>();
        explanation = texts[0];
    }

    void LateUpdate() 
    {
        explanation.text = "Enforced." + (level + 1);
    }

    public void ConvertSprite(SkillData data)
    {
        skillData = data;
        icon.sprite = data.skillIcon;
    }

    public void ClearSprite()
    {
        icon.sprite = basicSprite;
    }

    public void OnClick()
    {
        
    }
}
