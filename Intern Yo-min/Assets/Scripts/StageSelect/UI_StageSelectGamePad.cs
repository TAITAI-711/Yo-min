using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static GamePlayManager;

public class UI_StageSelectGamePad : UI_GamePadSelect
{
    private class GamePadInfo
    {
        public GamePadInfo()
        {
            PadName = "";
            NowSelectOseroType = EnumOseroType.Red;
            isOK = false;
            isStickOnce = false;
        }
        public string PadName;
        public EnumOseroType NowSelectOseroType;
        public EnumPlayerType PlayerType;   // 操作プレイヤー番号
        public bool isOK;
        public bool isStickOnce;
    }

    private List<GamePadInfo> GamePadList = new List<GamePadInfo>();

    // Start is called before the first frame update
    void Start()
    {
        GamePadList.Add(new GamePadInfo());
        GamePadList[0].PadName = GamePlayManager.Instance.MenuSelectPlayerName;
        GamePadList[0].NowSelectOseroType = EnumOseroType.Red;
        GamePadList[0].PlayerType = EnumPlayerType.Player1;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!StageSelectManager.Instance.isStageSelect)
            return;

        // 色変え
        for (int i = 0; i < GamePadList.Count; i++)
        {
            if (GamePadList[i].isOK)
                continue;

            string Bbutton = GamePadList[i].PadName + "_Button_B";
            string Stick_L = GamePadList[i].PadName + "_LeftAxis_X";

            if (Input.GetButtonDown(Bbutton))
            {
                GamePadList[i].isOK = true;
                SetGamePad();   // ゲームパッド情報をマネージャーに入力
            }

            if (Input.GetAxis(Stick_L) != 0.0f)
            {
                if (!GamePadList[i].isStickOnce)
                {
                    GamePadList[i].isStickOnce = true;

                    if (Input.GetAxis(Stick_L) > 0.0f)
                    {
                        ChangeOseroType(i, true);
                    }
                    else
                    {
                        ChangeOseroType(i, false);
                    }
                }
            }
            else
            {
                GamePadList[i].isStickOnce = false;
            }
        }

        // プレイヤーの人数追加
        if (GamePadList.Count < 4)
        {
            // 決定
            if (Input.GetButtonDown("Joystick_0_Button_B"))
            {
                for (int i = 1; i <= 10; i++)
                {
                    for (int j = 0; j < GamePadList.Count; j++)
                    {
                        if (GamePadList[j].PadName == "Joystick_" + i.ToString())
                            continue;
                    }

                    string NowGamePadName = "Joystick_" + i.ToString() + "_Button_B";

                    if (Input.GetButtonDown(NowGamePadName))
                    {
                        AddGamePad(i);

                        break;
                    }
                }
            }
        }
    }


    // ゲームプレイマネージャーにコントローラー情報を入力
    private void SetGamePad()
    {
        List<PlayerInfo> playerInfos = new List<PlayerInfo>();
        int OK_Num = 0;

        for (int i = 0; i < GamePadList.Count; i++)
        {
            if (GamePadList[i].isOK)
            {
                PlayerInfo PInfo = new PlayerInfo();
                PInfo.OseroNum = 0;
                PInfo.RankNum = 1;
                PInfo.GamePadName_Player = GamePadList[i].PadName;
                PInfo.PlayerType = GamePadList[i].PlayerType;

                for (int j = 0; j < GamePlayManager.Instance.PlayerOseroType.Length; j++)
                {
                    if (GamePlayManager.Instance.PlayerOseroType[j].OseroType == GamePadList[i].NowSelectOseroType)
                    {
                        PInfo.PlayerOseroType = GamePlayManager.Instance.PlayerOseroType[j];
                        break;
                    }
                }

                playerInfos.Add(PInfo);
                OK_Num++;
            }
        }

        GamePlayManager.Instance.Players = new PlayerInfo[OK_Num];

        for (int i = 0; i < GamePlayManager.Instance.Players.Length; i++)
        {
            GamePlayManager.Instance.Players[i] = playerInfos[i];
        }
    }

    // ゲームパッドの追加
    private void AddGamePad(int GamePadJoystickNum)
    {
        GamePadList.Add(new GamePadInfo());
        GamePadList[GamePadList.Count - 1].PlayerType = (EnumPlayerType)(GamePadList.Count - 1);
        GamePadList[GamePadList.Count - 1].PadName = "Joystick_" + GamePadJoystickNum.ToString();

        ChangeOseroType(GamePadList.Count - 1, true);
    }


    // オセロのタイプ変更
    private void ChangeOseroType(int GamePadListNum, bool isFront)
    {
        int NowColorType = (int)GamePadList[GamePadListNum].NowSelectOseroType;

        for (int i = 0; i < GamePlayManager.Instance.PlayerOseroType.Length; i++)
        {
            bool isSameType = false;

            if (isFront)
                NowColorType++;
            else
                NowColorType--;

            if (NowColorType >= GamePlayManager.Instance.PlayerOseroType.Length)
                NowColorType = 0;

            if (NowColorType < 0)
                NowColorType = GamePlayManager.Instance.PlayerOseroType.Length - 1;

            for (int j = 0; j < GamePadList.Count; j++)
            {
                if (GamePlayManager.Instance.PlayerOseroType[NowColorType].OseroType == GamePadList[j].NowSelectOseroType)
                {
                    isSameType = true;
                    break;
                }
            }
            if (isSameType)
                continue;

            GamePadList[GamePadListNum].NowSelectOseroType = GamePlayManager.Instance.PlayerOseroType[NowColorType].OseroType;
        }
    }

}
