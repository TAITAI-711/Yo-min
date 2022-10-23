using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    private float LogoTime = 2.5f;
    private float NowTime = 0.0f;


    private void Awake()
    {
        FadeManager.Instance.FadeLogoStart();
    }
    // Start is called before the first frame update
    void Start()
    {
        NowTime = LogoTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeManager.GetNowState() != FADE_STATE.FADE_NONE)
            return;

        NowTime -= Time.deltaTime;

        if (NowTime < 0.0f)
        {
            SceneChangeManager.Instance.SceneChange("TitleScene", true);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Joystick_0_Button_B"))
        {
            //Debug.Log("ŽŸ‚ÌƒV[ƒ“");
            SceneChangeManager.Instance.SceneChange("TitleScene", true);
        }
    }
}
