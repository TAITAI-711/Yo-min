using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSelect_Ready : MonoBehaviour
{
    [HideInInspector] public Image ImageObj;
    [SerializeField] private Sprite WaitSprite;
    [SerializeField] private Sprite ReadySprite;

    private void Awake()
    {
        ImageObj = gameObject.GetComponent<Image>();
        ImageObj.sprite = WaitSprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetWaitSprite()
    {
        if (ImageObj.sprite != WaitSprite)
            ImageObj.sprite = WaitSprite;
    }

    public void SetReadySprite()
    {
        if (ImageObj.sprite != ReadySprite)
            ImageObj.sprite = ReadySprite;
    }
}
