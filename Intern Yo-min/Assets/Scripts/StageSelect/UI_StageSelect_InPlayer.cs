using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSelect_InPlayer : MonoBehaviour
{
    private Image ImageObj;

    [SerializeField] private float ButtonUpTime = 0.75f;
    private float NowTime = 0.0f;

    private void Awake()
    {
        ImageObj = gameObject.GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.Instance.isPause)
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

    public void SetImageActive(bool isActive)
    {
        ImageObj.enabled = isActive;
    }
    
    public bool GetImageActive()
    {
        return ImageObj.enabled;
    }
}
