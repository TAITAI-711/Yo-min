using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Result_OK : MonoBehaviour
{
    [HideInInspector] public Image ImageObj;
    [SerializeField] private Sprite Sprite_Button_B;
    [SerializeField] private Sprite Sprite_Ready;

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
    }

    public bool GetNowReady()
    {
        return ImageObj.sprite == Sprite_Ready;
    }
}
