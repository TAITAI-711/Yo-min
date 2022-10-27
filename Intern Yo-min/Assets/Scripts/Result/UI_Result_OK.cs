using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Result_OK : MonoBehaviour
{
    [HideInInspector] public Image ImageObj;
    [SerializeField] private Sprite Sprite_Button_B;
    [SerializeField] private Sprite Sprite_Ready;

    [SerializeField] private float ButtonUpTime = 0.75f;
    private float NowTime = 0.0f;

    private void Awake()
    {
        ImageObj = gameObject.GetComponent<Image>();
        ImageObj.sprite = Sprite_Button_B;
        ImageObj.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ImageObj.enabled || ImageObj.sprite != Sprite_Button_B)
            return;


        NowTime += Time.deltaTime;

        if (NowTime > ButtonUpTime)
        {
            NowTime = 0.0f;
        }

        ImageObj.rectTransform.localScale = new Vector3(
            (1.0f - NowTime / ButtonUpTime) * 0.2f + 0.8f,
            (1.0f - NowTime / ButtonUpTime) * 0.2f + 0.8f,
            1.0f);
    }

    public void SetSpriteButtonB()
    {
        if (!ImageObj.enabled)
            ImageObj.enabled = true;

        if (ImageObj.sprite != Sprite_Button_B)
            ImageObj.sprite = Sprite_Button_B;
    }

    public void SetSpriteReady()
    {
        if (ImageObj.sprite != Sprite_Ready)
            ImageObj.sprite = Sprite_Ready;

        ImageObj.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
    }

    public bool GetNowReady()
    {
        return ImageObj.sprite == Sprite_Ready;
    }
}
