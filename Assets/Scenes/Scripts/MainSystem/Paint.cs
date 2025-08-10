using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Paint : MonoBehaviour
{
    public PaintManager.ColorType colorType;
    public Image image;

    public int maxPaint;
    public int paint;

    void Awake()
    {
        // image = gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();

        // 물감 최대치로 회복
        paint = maxPaint;
    }

    private void FixedUpdate() 
    {
        // 슬라이더UI 변경
        if (paint > maxPaint) {
            paint = maxPaint;
        }
        else if (paint < 0) {
            paint = 0;
        }
        
        image.fillAmount = paint / (float)maxPaint;
    }

    // 최대 페인트 수 확인
    public int GetMaxNum()
    {
        return maxPaint;
    }

    // 현재 페인트 수 확인
    public int GetNum()
    {
        return paint;
    }

    // 페인트 수 보충
    public void FillNum(int num)
    {
        paint += num;
        
        // 오류 대비 (페인트 초과 보충 오류)
        if (paint > maxPaint) {
            paint = maxPaint;
        }
    }

    // 페인트 수 완충
    public void FillUp()
    {
        paint = maxPaint;
    }

}
