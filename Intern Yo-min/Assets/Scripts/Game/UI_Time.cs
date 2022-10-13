using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class UI_Time : MonoBehaviour
{
    private TextMeshProUGUI TimeObj;

    [Header("[ �Q�[�����Ԑݒ� ]")]
    [Tooltip("1�Q�[���̎���(�b)"), Range(30.0F, 180.0F)]
    public float GameTime = 120.0f;

    private float NowTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        NowTime = GameTime;

        TimeObj = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.Instance.isGamePlay)
        {
            if (Input.GetKeyDown(KeyCode.W))
                NowTime -= 10.0f;
            if (NowTime > 0.0f)
            {
                // �Q�[���̎��ԕ\��
                if (((int)(NowTime % 60.0f)) < 10)
                {
                    TimeObj.text = ((int)(NowTime / 60)).ToString() + ":" + "0" + ((int)(NowTime % 60.0f));
                }
                else
                {
                    TimeObj.text = ((int)(NowTime / 60)).ToString() + ":" + ((int)(NowTime % 60.0f));
                }

                // ���ԍX�V
                NowTime -= Time.deltaTime;
            }

            if (GamePlayManager.Instance.isGamePlay && NowTime <= 0.0f)
            {
                GamePlayManager.Instance.isGamePlay = false;
                GamePlayManager.Instance.isGameEnd = true;
                NowTime = 0.0f;

                // �Q�[���̎��ԕ\��
                //TimeObj.text = ((int)(NowTime / 60)).ToString() + ":" + ((int)(NowTime % 60.0f));
                TimeObj.fontSize = 90;
                TimeObj.text = "GameSet";
            }
        }
    }

    public float GetNowTime()
    {
        return NowTime;
    }
}
