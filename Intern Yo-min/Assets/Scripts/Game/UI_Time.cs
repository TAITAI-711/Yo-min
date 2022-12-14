using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class UI_Time : MonoBehaviour
{
    private TextMeshProUGUI TimeObj;

    [Header("[ ゲーム時間設定 ]")]
    [Tooltip("1ゲームの時間(秒)"), Range(30.0F, 180.0F)]
    public float GameTime = 120.0f;

    public float NowTime = 0.0f;

    private bool isOnce_UI_30Sec = false;

    // Start is called before the first frame update
    void Awake()
    {
        NowTime = GameTime;

        TimeObj = GetComponent<TextMeshProUGUI>();
    }


    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
            NowTime -= 10.0f;
#endif
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (GamePlayManager.Instance.isGamePlay)
        {
            if (NowTime > 0.0f)
            {
                // ゲームの時間表示
                if (((int)(NowTime % 60.0f)) < 10)
                {
                    TimeObj.text = ((int)(NowTime / 60)).ToString() + ":" + "0" + ((int)(NowTime % 60.0f));
                }
                else
                {
                    TimeObj.text = ((int)(NowTime / 60)).ToString() + ":" + ((int)(NowTime % 60.0f));
                }

                // 時間更新
                NowTime -= Time.fixedDeltaTime;
            }

            if (GamePlayManager.Instance.isGamePlay && NowTime <= 0.0f)
            {
                GamePlayManager.Instance.isGamePlay = false;
                GamePlayManager.Instance.isGameEnd = true;
                NowTime = 0.0f;

                // ゲームの時間表示
                //TimeObj.text = ((int)(NowTime / 60)).ToString() + ":" + ((int)(NowTime % 60.0f));
                TimeObj.fontSize = 90;
                TimeObj.text = "GameSet";
            }
        }


        // 残り30秒の表示
        if (!isOnce_UI_30Sec && NowTime <= 31.0f)
        {
            isOnce_UI_30Sec = true;

            if (PlayerManager.Instance.UI_GameTimeObj.UI_30SecObj != null)
                PlayerManager.Instance.UI_GameTimeObj.UI_30SecObj.SetMove();
            TimeObj.color = new Color(1.0f, 0.2f, 0.2f, 1.0f);
        }
    }

    public float GetNowTime()
    {
        return NowTime;
    }
}
