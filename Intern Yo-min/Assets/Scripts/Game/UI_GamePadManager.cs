using TMPro;
using Unity.Collections;
using UnityEngine;
using static GamePlayManager;

public class UI_GamePadManager : SingletonMonoBehaviour<UI_GamePadManager>
{
    private TextMeshProUGUI GamePadSelectUI;

    private bool isPress = false;


    private int PlayerNum = 2;
    private int NowPlayerNum = 0;

    private string[] GamePadName_Player = new string[4];


    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        GamePadSelectUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (GamePlayManager.Instance.isGamePadOK)
            GamePadSelectUI.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GamePlayManager.Instance.isGamePadOK)
        {
            // プレイヤーの人数決め
            if (NowPlayerNum <= 0)
            {
                // 人数の入力受付
                if (Input.GetAxis("Joystick_0_LeftAxis_X") != 0)
                {
                    if (!isPress)
                    {
                        if (Input.GetAxis("Joystick_0_LeftAxis_X") > 0)
                        {
                            if (PlayerNum < 4)
                            {
                                PlayerNum++;
                            }
                        }
                        else
                        {
                            if (PlayerNum > 2)
                            {
                                PlayerNum--;
                            }
                        }
                    }

                    isPress = true;
                }
                else
                {
                    isPress = false;
                }

                // 決定
                if (Input.GetButtonDown("Joystick_0_Button_B"))
                {
                    isPress = false;
                    NowPlayerNum = 1;
                }

                // 人数の表示
                GamePadSelectUI.text = "ALLPlayerNum:" + PlayerNum + "\n\nSelect  GamePad  LeftStick" + "\n\nSet  GamePad  B  Button";

            }
            else
            {
                // Rトリガー入力待ち
                for (int i = 1; i <= PlayerNum; i++)
                {
                    if (NowPlayerNum == i)
                    {
                        if (Input.GetButtonDown("Joystick_0_Button_B"))
                        {
                            for (int j = 1; j <= 12; j++)
                            {
                                string NowGamePadName = "Joystick_" + j.ToString() + "_Button_B";

                                if (Input.GetButtonDown(NowGamePadName))
                                {
                                    GamePadName_Player[i - 1] = "Joystick_" + j.ToString();
                                    i = PlayerNum + 1;
                                    NowPlayerNum++;

                                    if (NowPlayerNum > PlayerNum)
                                    {
                                        GamePlayManager.Instance.isGamePadOK = true;
                                        //GamePlayManager.Instance.isGamePlay = true;

                                        GamePlayManager.Instance.Players = new GamePlayManager.PlayerInfo[PlayerNum];
                                        for (int k = 0; k < PlayerNum; k++)
                                        {
                                            GamePlayManager.Instance.Players[k].OseroNum = 0;
                                            GamePlayManager.Instance.Players[k].GamePadName_Player = GamePadName_Player[k];
                                            GamePlayManager.Instance.Players[k].PlayerType = (EnumPlayerType)k;
                                            //GamePlayManager.Instance.Players[k].MaterialInfo.OseroType = (PlayerManager.EnumOseroType)k;

                                            for (int OseroTypeCnt = 0; OseroTypeCnt < GamePlayManager.Instance.PlayerOseroType.Length; OseroTypeCnt++)
                                            {
                                                if (GamePlayManager.Instance.PlayerOseroType[OseroTypeCnt].OseroType == (GamePlayManager.EnumOseroType)k)
                                                {
                                                    GamePlayManager.Instance.Players[k].PlayerOseroType = GamePlayManager.Instance.PlayerOseroType[OseroTypeCnt];
                                                }
                                            }
                                        }

                                        gameObject.SetActive(false);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                // ボタン入力表示
                GamePadSelectUI.text = "Player  " + NowPlayerNum.ToString() + "\nPress  GamePad  B  Button";
            }
        }
    }
}
