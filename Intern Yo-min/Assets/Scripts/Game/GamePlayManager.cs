using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerManager;

public class GamePlayManager : SingletonMonoBehaviour<GamePlayManager>
{
    [ReadOnly] public bool isGamePlay = false;
    [ReadOnly] public bool isGamePadOK = false;
    [ReadOnly] public bool isStartCount = false;
    [ReadOnly] public bool isGameEnd = false;
    [ReadOnly] public bool isPause = false;
    [ReadOnly] public BanmenManager BanmenManagerObj = null;
    [ReadOnly] public UI_GamePadSelect GamePadSelectObj = null;
    [ReadOnly] public PlayerManager PlayerManagerObj = null;
    [ReadOnly] public FloorManager FloorManagerObj = null;
    [ReadOnly] public Gimmick_Wind Gimmick_WindObj = null;
    [ReadOnly] public static readonly float MasuScaleY = 4.6f;
    [ReadOnly] public static readonly float MasuScaleXZ = 10.0f;


    public struct PlayerInfo
    {
        public int OseroNum;          // プレイヤーのオセロの枚数保存用
        public EnumOseroType OseroType;    // プレイヤーのオセロの種類
    }
    public PlayerInfo[] Players;


    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // シーンが変わっても死なない

        GameReset();
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
        isGamePadOK = false;
        isStartCount = false;
        isGameEnd = false;
        isPause = false;
    }

    public void AllReset()
    {
        GameReset();

        Players = null;
    }

    // リザルト前にプレイヤーのオセロの数保存処理
    public void PlayerOseroNumSet()
    {
        for (int i = 0; i < PlayerManagerObj.UI_OseroObj.Length; i++)
        {
            for (int j = 0; j < Players.Length; j++)
            {
                if (PlayerManagerObj.UI_OseroObj[i].PlayerOseroType.OseroType == Players[j].OseroType)
                {
                    Players[i].OseroNum = PlayerManagerObj.UI_OseroObj[i].Num;
                }
            }
        }
    }
}
