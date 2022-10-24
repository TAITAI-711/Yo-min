using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    [SerializeField] private Image BlackImage;

    private float LogoTime = 3.0f;
    private float NowTime = 0.0f;


    //private bool isFade = true;
    //[SerializeField] private float FadeTime = 0.4f;
    //private float NowFadeTime = 0.0f;


    private float WaitTime = 0.5f;


    // Start is called before the first frame update
    void Start()
    {        
        NowTime = LogoTime;
        //NowFadeTime = FadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(WaitTime > 0.0f)
        {
            WaitTime -= Time.deltaTime;

            if (WaitTime < 0.0f)
            {
                BlackImage.enabled = false;
                FadeManager.Instance.FadeLogoStart();
            }
            return;
        }




        // フェード中
        if (FadeManager.GetNowState() != FADE_STATE.FADE_NONE)
            return;


        //if (NowFadeTime > 0.0f)
        //{
        //    NowFadeTime -= Time.deltaTime;

        //    if (NowFadeTime <= 0.0f)
        //    {
        //        NowFadeTime = 0.0f;
        //        isFade = false;
        //    }

        //    Color color = BlackImage.color;
        //    color.a = (NowFadeTime / FadeTime);
        //    BlackImage.color = color;
        //}


        //// 最初のフェード中
        //if (isFade)
        //    return;





        NowTime -= Time.deltaTime;

        if (NowTime < 0.0f)
        {
            SceneChangeManager.Instance.SceneChange("TitleScene", true);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Joystick_0_Button_B"))
        {
            //Debug.Log("次のシーン");
            SceneChangeManager.Instance.SceneChange("TitleScene", true);
        }
    }
}
