using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static GamePlayManager;

public class UI_StageSelectGamePadManager : SingletonMonoBehaviour<UI_StageSelectGamePadManager>
{
    [System.Serializable]
    public class GamePadInfo
    {
        public GamePadInfo()
        {
            PadName = "";
            isOK = false;
            isFinalyOK = false;
            isStickOnce = false;
        }
        public string PadName;
        public PlayerOseroTypeInfo NowSelectPlayerOseroType;
        public EnumPlayerType PlayerType;   // ����v���C���[�ԍ�
        public bool isOK;
        public bool isFinalyOK;
        public bool isStickOnce;
    }

    public List<GamePadInfo> GamePadList = new List<GamePadInfo>();

    private float ButtonOnceTime = 0.1f;    // �v���C���[�P���X�e�[�W�I���ƐF����𓯎��ɂ��Ȃ��p����

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        GamePlayManager.Instance.Players = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        GamePadList.Add(new GamePadInfo());
        GamePadList[0].PadName = GamePlayManager.Instance.MenuSelectPlayerName;
        GamePadList[0].NowSelectPlayerOseroType = GamePlayManager.Instance.PlayerOseroType[0];
        GamePadList[0].PlayerType = EnumPlayerType.Player1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!StageSelectManager.Instance.isStageSelect || GamePlayManager.Instance.isPause)
        {
            ButtonOnceTime = 0.1f;
            return;
        }

        if (ButtonOnceTime > 0.0f)
        {
            ButtonOnceTime -= Time.deltaTime;
            return;
        }

        // �F�ς�
        for (int i = 0; i < GamePadList.Count; i++)
        {
            // �ŏI�m�F����
            if (GamePadList[i].isOK)
            {
                if (!GamePadList[i].isFinalyOK && Input.GetButtonDown(GamePadList[i].PadName + "_Button_B"))
                {
                    GamePadList[i].isFinalyOK = true;
                }
            }
            // �F�ւ����v���C���[�̐F����
            else
            {
                string Bbutton = GamePadList[i].PadName + "_Button_B";
                string Stick_L = GamePadList[i].PadName + "_LeftAxis_X";

                if (Input.GetButtonDown(Bbutton))
                {
                    GamePadList[i].isOK = true;
                    SetGamePad();   // �Q�[���p�b�h�����}�l�[�W���[�ɓ���
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
        }

        // �v���C���[�̐l���ǉ�
        if (GamePadList.Count < 4)
        {
            // ����
            if (Input.GetButtonDown("Joystick_0_Button_B"))
            {
                for (int i = 1; i <= 12; i++)
                {
                    bool isSameName = false;

                    for (int j = 0; j < GamePadList.Count; j++)
                    {
                        if (GamePadList[j].PadName == "Joystick_" + i.ToString())
                        {
                            isSameName = true;
                            break;
                        }
                    }

                    if (isSameName)
                        continue;

                    string NowGamePadName = "Joystick_" + i.ToString() + "_Button_B";

                    if (Input.GetButtonDown(NowGamePadName))
                    {
                        AddGamePad(i);
                    }
                }
            }
        }
    }


    // �Q�[���v���C�}�l�[�W���[�ɃR���g���[���[�������
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
                    if (GamePlayManager.Instance.PlayerOseroType[j].OseroType == GamePadList[i].NowSelectPlayerOseroType.OseroType)
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

        playerInfos.Clear();
    }

    // �Q�[���p�b�h�̒ǉ�
    private void AddGamePad(int GamePadJoystickNum)
    {
        GamePadList.Add(new GamePadInfo());
        GamePadList[GamePadList.Count - 1].PlayerType = (EnumPlayerType)(GamePadList.Count - 1);
        GamePadList[GamePadList.Count - 1].PadName = "Joystick_" + GamePadJoystickNum.ToString();

        ChangeOseroType(GamePadList.Count - 1, true);
    }


    // �I�Z���̃^�C�v�ύX
    private void ChangeOseroType(int GamePadListNum, bool isFront)
    {
        int NowColorType = (int)GamePadList[GamePadListNum].NowSelectPlayerOseroType.OseroType;

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
                if (GamePlayManager.Instance.PlayerOseroType[NowColorType].OseroType == GamePadList[j].NowSelectPlayerOseroType.OseroType)
                {
                    isSameType = true;
                    break;
                }
            }
            if (isSameType)
                continue;

            GamePadList[GamePadListNum].NowSelectPlayerOseroType = GamePlayManager.Instance.PlayerOseroType[NowColorType];
            break;
        }
    }

}
