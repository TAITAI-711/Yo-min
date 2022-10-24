using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result_UI_Osero : MonoBehaviour
{
    private float OffsetPosY = -1.8f;
    private float OffsetRankPosY = 15.0f;
    private float OffsetOKPosY = -4.8f;

    private TextMeshProUGUI[] TextMP = null;

    private void Awake()
    {
        TextMP = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }

    public void SetPosition(Vector3 Pos)
    {
        Vector3 Position = Pos;

        RectTransform Rt = GetComponent<RectTransform>();

        // ÉIÉZÉçUI
        Vector3 UIPos = Position;
        UIPos.y += OffsetPosY;

        // èáà UI
        Vector3 UIRankPos = Position;
        UIRankPos.y += OffsetRankPosY;

        // OKÇÃUI
        Vector3 UIOKPos = Position;
        UIOKPos.y += OffsetOKPosY;

        Rt.position = RectTransformUtility.WorldToScreenPoint(Camera.main, UIPos);

        if (TextMP == null)
            TextMP = GetComponentsInChildren<TextMeshProUGUI>();

        TextMP[1].rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, UIRankPos);
        TextMP[2].rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, UIOKPos);
    }

    public void SetText(string Text)
    {
        if (TextMP == null)
            TextMP = GetComponentsInChildren<TextMeshProUGUI>();

        TextMP[0].text = Text;
    }

    public string GetText()
    {
        if (TextMP == null)
            TextMP = GetComponentsInChildren<TextMeshProUGUI>();

        return TextMP[0].text;
    }

    
    public void SetRankText(string Text)
    {
        if (TextMP == null)
            TextMP = GetComponentsInChildren<TextMeshProUGUI>();

        TextMP[1].text = Text;
    }

    public string GetRankText()
    {
        if (TextMP == null)
            TextMP = GetComponentsInChildren<TextMeshProUGUI>();

        return TextMP[1].text;
    }

    public void SetOKText(string Text)
    {
        if (TextMP == null)
            TextMP = GetComponentsInChildren<TextMeshProUGUI>();

        TextMP[2].text = Text;
    }

    public string GetOKText()
    {
        if (TextMP == null)
            TextMP = GetComponentsInChildren<TextMeshProUGUI>();

        return TextMP[2].text;
    }
}
