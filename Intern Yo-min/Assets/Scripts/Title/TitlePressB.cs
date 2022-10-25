using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitlePressB : MonoBehaviour
{
    [SerializeField] private GameObject ButtonsObj;
    private TextMeshProUGUI TMPro;
    private StandaloneInputModuleButton StandaloneObj;

    [SerializeField] private float AlphaTime = 0.8f;
    private float NowAlphaTime = 0.0f;
    private bool isCountUp = false;

    [SerializeField] private float PressAlphaTime = 0.25f;


    [SerializeField] private float PressBTime = 2.0f;
    private float NowTime = 0.0f;
    private bool isPress = false;

    // Start is called before the first frame update
    void Awake()
    {
        ButtonsObj.SetActive(false);
        TMPro = GetComponent<TextMeshProUGUI>();
        NowTime = PressBTime;
        NowAlphaTime = AlphaTime;
    }

    private void Start()
    {
        StandaloneObj = EventSystemManager.Instance.StandaloneInputObj;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPress)
        {
            NowTime -= Time.deltaTime;

            if (NowTime < 0.0f)
            {
                ButtonsObj.SetActive(true);
                gameObject.SetActive(false);
            }

            if (isCountUp)
            {
                NowAlphaTime += Time.deltaTime;

                if (NowAlphaTime >= PressAlphaTime)
                {
                    NowAlphaTime = PressAlphaTime;
                    isCountUp = !isCountUp;
                }
            }
            else
            {
                NowAlphaTime -= Time.deltaTime;

                if (NowAlphaTime <= 0.0f)
                {
                    NowAlphaTime = 0.0f;
                    isCountUp = !isCountUp;
                }
            }
            TMPro.alpha = (NowAlphaTime / PressAlphaTime);
        }
        else
        {
            if (Input.GetButtonDown("Joystick_0_Button_B"))
            {
                for (int i = 1; i <= 10; i++)
                {
                    string ButtonName = "Joystick_" + i.ToString() + "_Button_B";

                    if (Input.GetButtonDown(ButtonName))
                    {
                        isPress = true;

                        // ‰¹
                        SoundManager.Instance.PlaySound("Œˆ’è", false);


                        GamePlayManager.Instance.MenuSelectPlayerName = "Joystick_" + i.ToString();

                        StandaloneObj.verticalAxis = "Joystick_" + i.ToString() + "_LeftAxis_Y";
                        StandaloneObj.horizontalAxis = "Joystick_" + i.ToString() + "_LeftAxis_X";
                        StandaloneObj.submitButton = ButtonName;

                        NowAlphaTime = 0.0f;

                        break;
                    }
                }
            }

            if (isCountUp)
            {
                NowAlphaTime += Time.deltaTime;

                if (NowAlphaTime >= AlphaTime)
                {
                    NowAlphaTime = AlphaTime;
                    isCountUp = !isCountUp;
                }
            }
            else
            {
                NowAlphaTime -= Time.deltaTime;

                if (NowAlphaTime <= 0.0f)
                {
                    NowAlphaTime = 0.0f;
                    isCountUp = !isCountUp;
                }    
            }
            TMPro.alpha = (NowAlphaTime / AlphaTime);
        }
    }
}
