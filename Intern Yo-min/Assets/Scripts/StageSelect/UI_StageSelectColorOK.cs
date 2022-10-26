using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSelectColorOK : MonoBehaviour
{
    [HideInInspector] public Image ImageObj;
    [SerializeField] private Sprite ColorOKSprite;


    private void Awake()
    {
        ImageObj = gameObject.GetComponent<Image>();
        ImageObj.sprite = ColorOKSprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
