using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result_UI_Osero_Panel : MonoBehaviour
{
    private int OseroMax = 0;


    // Start is called before the first frame update
    void Start()
    {
        Result_UI_Osero[] UI_OseroObjs = ResultManager.Instance.Result_UI_Oseros;
        GamePlayManager.PlayerInfo[] PlayerObjs = GamePlayManager.Instance.Players;

        foreach (var Obj in UI_OseroObjs)
        {
            Obj.gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayerObjs.Length; i++)
        {
            UI_OseroObjs[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < PlayerObjs.Length; i++)
        {
            if (PlayerObjs[i].RankNum == 1)
            {
                OseroMax = PlayerObjs[i].OseroNum;
                break;
            }
        }
    }

    private void Update()
    {
        if (ResultManager.Instance.isCountStop && !ResultManager.Instance.isPause && FadeManager.GetNowState() == FADE_STATE.FADE_NONE)
        {
            Result_UI_Osero[] UI_OseroObjs = ResultManager.Instance.Result_UI_Oseros;
            GamePlayManager.PlayerInfo[] PlayerObjs = GamePlayManager.Instance.Players;

            bool isReady = true; 


            for (int i = 0; i < PlayerObjs.Length; i++)
            {
                if (Input.GetButtonDown(PlayerObjs[i].GamePadName_Player + "_Button_B"))
                {
                    if (!UI_OseroObjs[i].GetOK())
                    {
                        // ���艹
                        SoundManager.Instance.PlaySound("��������", false);
                        UI_OseroObjs[i].SetReadyImage();
                    }
                }

                if (!UI_OseroObjs[i].GetOK())
                    isReady = false;
            }

            if (isReady)
                ResultManager.Instance.isPause = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ResultManager.Instance.isCountStop || FadeManager.GetNowState() != FADE_STATE.FADE_NONE)
            return;

        Result_UI_Osero[] UI_OseroObjs = ResultManager.Instance.Result_UI_Oseros;
        GamePlayManager.PlayerInfo[] PlayerObjs = GamePlayManager.Instance.Players;

        // �I�Z��UI�̐��J�E���g�A�b�v����
        for (int i = 0; i < PlayerObjs.Length; i++)
        {
            int OseroNum = (int)(OseroMax * ResultManager.Instance.ResultTime.GetNowTimeRate());

            // �ő咴������ő�ɂ���
            if (OseroNum > PlayerObjs[i].OseroNum)
            {
                OseroNum = PlayerObjs[i].OseroNum;
            }

             // �I�Z���̐��Z�b�g
            if (UI_OseroObjs[i].GetText() != OseroNum.ToString())
            {
                UI_OseroObjs[i].SetText(OseroNum.ToString());
            }

            // ���ʕ\��
            if (OseroNum == PlayerObjs[i].OseroNum)
            {
                string StText = "st";
                switch (PlayerObjs[i].RankNum)
                {
                    case 1:
                        StText = "st";
                        break;
                    case 2:
                        StText = "nd";
                        break;
                    case 3:
                        StText = "rd";
                        break;
                    case 4:
                        StText = "th";
                        break;
                    default:
                        break;
                }
                UI_OseroObjs[i].SetRankText(PlayerObjs[i].RankNum.ToString() + StText);

                // �A�j���[�V�����Đ�
                if (PlayerObjs[i].RankNum == 1)
                {
                    ResultManager.Instance.ResultPlayers[i].SetThrowAnimetion();
                }
                else
                {
                    ResultManager.Instance.ResultPlayers[i].SetIdleAnimetion();
                }
            }
        }


        // �J�E���g�A�b�v�I��
        if (ResultManager.Instance.ResultTime.GetNowTimeRate() >= 1.0f)
        {
            ResultManager.Instance.isCountStop = true;
            
            // ���������{�^���\��
            for (int i = 0; i < PlayerObjs.Length; i++)
            {
                UI_OseroObjs[i].SetButtonBImage();
            }
        }
    }
}
