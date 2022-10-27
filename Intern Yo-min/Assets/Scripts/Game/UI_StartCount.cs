using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartCount : MonoBehaviour
{
    private TextMeshProUGUI CountObj;

    public int CountMax = 3;
    private int NowCount = 0;
    private int OldCount = 0;
    private float NowTime = 0.0f;

    private bool isOnce = false;


    // Start is called before the first frame update
    void Start()
    {
        OldCount = NowCount = CountMax;
        isOnce = false;

        CountObj = GetComponent<TextMeshProUGUI>();
        CountObj.enabled = false;
        CountObj.text = NowCount.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FadeManager.GetNowState() != FADE_STATE.FADE_NONE)
            return;

        if (GamePlayManager.Instance.isGamePadOK && NowCount >= 0)
        {
            if (!isOnce)
            {
                isOnce = true;
                CountObj.enabled = true;
                SoundManager.Instance.PlaySound("�J�E���g�_�E��", false);
            }
            

            if (NowCount != OldCount)
            {
                OldCount = NowCount;

                if (NowCount != 0)
                {
                    SoundManager.Instance.PlaySound("�J�E���g�_�E��", false);
                    CountObj.text = NowCount.ToString();
                }
                else
                {
                    SoundManager.Instance.PlaySound("�Q�[���J�n", false);
                    CountObj.text = "START";
                    GamePlayManager.Instance.isGamePlay = true;
                }
            }

            NowTime += Time.fixedDeltaTime;

            if (NowTime >= 1.0f)
            {
                NowTime = 0.0f;
                NowCount--;
            }

            if (NowCount < 0)
            {
                CountObj.enabled = false;
                isOnce = false;
            }
        }

        if (GamePlayManager.Instance.isGameEnd)
        {
            if (!isOnce)
            {
                isOnce = true;
                CountObj.enabled = true;

                SoundManager.Instance.PlaySound("�Q�[���I��", false);

                CountObj.text = "GAMESET";
            }
        }
    }
}
