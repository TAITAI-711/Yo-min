using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GamePlayManager : SingletonMonoBehaviour<GamePlayManager>
{
    [ReadOnly] public bool isGamePlay = false;
    [ReadOnly] public bool isGamePadOK = false;
    [ReadOnly] public bool isStartCount = false;
    [ReadOnly] public bool isGameEnd = false;
    [ReadOnly] public bool isPause = false;
    [ReadOnly] public static readonly float MasuScaleY = 4.6f;
    [ReadOnly] public static readonly float MasuScaleXZ = 10.0f;

    [ReadOnly] public string OldGameStageName = "GameScene";

    public enum EnumPlayerType
    {
        Player1 = 0,
        Player2,
        Player3,
        Player4
    }

    [System.Serializable]
    public enum EnumOseroType
    {
        Red = 0,
        Blue,
        Yellow,
        Purple,
        Black,
        White,
        Green
    }

    [System.Serializable]
    public struct PlayerOseroTypeInfo
    {
        [Tooltip("�v���C���[�̃}�e���A��")]
        public Material PlayerMaterial;


        [Tooltip("�v���C���[�̃I�Z���}�e���A��")]
        public Material OseroMaterial;


        [Tooltip("�v���C���[�̃I�Z���̐F")]
        public EnumOseroType OseroType;


        [Tooltip("�v���C���[�̃I�Z����UI")]
        public Sprite UI_OseroImage;
    }

    [Header("[ �v���C���[�̐F�ݒ� ]")]
    [Tooltip("�v���C���[�ƃI�Z���̐F�ݒ�")]
    [ReadOnly] public PlayerOseroTypeInfo[] PlayerOseroType = new PlayerOseroTypeInfo[4];


    [System.Serializable]
    public struct PlayerInfo
    {
        public int OseroNum;          // �v���C���[�̃I�Z���̖����ۑ��p  
        public PlayerOseroTypeInfo PlayerOseroType; // �v���C���[�̃I�Z���̎��
        public string GamePadName_Player;   // �v���C���[�̃Q�[���p�b�h��
        public int RankNum; // ����
        public EnumPlayerType PlayerType;   // ����v���C���[�ԍ�
    }
    public PlayerInfo[] Players;

    public string MenuSelectPlayerName = "Joystick_0";


    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // �V�[�����ς���Ă����ȂȂ�

        GameReset();

        //============
        // �f�o�b�O�p
        //============
        Players = new PlayerInfo[0];
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].OseroNum = 20 * (i + 1);
            Players[i].RankNum = Players.Length - i;

            Players[i].PlayerOseroType = PlayerOseroType[i];

            Players[i].GamePadName_Player = "Joystick_0";
        }

        MenuSelectPlayerName = "Joystick_0";
        OldGameStageName = "GameScene";
        //============
        // �����܂�
        //============
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }

    public void GameReset()
    {
        isGamePlay = false;
        //isGamePadOK = false;
        isStartCount = false;
        isGameEnd = false;
        isPause = false;
    }

    public void AllReset()
    {
        GameReset();

        isGamePadOK = false;
        Players = null;
    }

    // ���U���g�O�Ƀv���C���[�̃I�Z���̐��ۑ�����
    public void PlayerOseroNumSet()
    {
        // �I�Z���̐��ۑ�
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].OseroNum = PlayerManager.Instance.UI_OseroObj[i].Num;
        }

        // ���ʕt��
        List<int> Ranks = new List<int>();

        for (int i = 0; i < Players.Length; i++)
        {
            Ranks.Add(Players[i].OseroNum);
        }
        //Ranks.Sort((a, b) => b - a);
        Ranks.Sort();

        for (int i = 0; i < Ranks.Count; i++)
        {
            for (int j = 0; j < Players.Length; j++)
            {
                if (Ranks[i] == Players[j].OseroNum)
                {
                    Players[j].RankNum = Ranks.Count - i;
                }
            }
        }
    }
}
