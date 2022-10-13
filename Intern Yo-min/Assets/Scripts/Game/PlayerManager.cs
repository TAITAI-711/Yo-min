using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public PlayerMove[] PlayerMoveObj;
    [HideInInspector] public UI_Osero[] UI_OseroObj;
    [HideInInspector] private UI_OseroPanel UI_OseroPanelObj;

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
        [Tooltip("プレイヤーのオセロマテリアル")]
        public Material PlayerMaterial;

        [Tooltip("プレイヤーのオセロの色")]
        public EnumOseroType OseroType;

        [Tooltip("プレイヤーのオセロのUI")]
        public Sprite UI_OseroImage;
    }

    [Header("[ プレイヤーの色設定 ]")]
    [Tooltip("プレイヤーとオセロの色設定")]
    public PlayerOseroTypeInfo[] PlayerOseroType = new PlayerOseroTypeInfo[4];


    private bool isOnce = false;

    private void Awake()
    {
        GamePlayManager.Instance.PlayerManagerObj = this;

        PlayerMoveObj = GetComponentsInChildren<PlayerMove>();
        UI_OseroObj = GetComponentsInChildren<UI_Osero>();
        UI_OseroPanelObj = GetComponentInChildren<UI_OseroPanel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(PlayerMoveObj.Length);

        // 使用プレイヤーの表示
        for (int i = 0; i < PlayerMoveObj.Length; i++)
        {
            PlayerMoveObj[i].gameObject.SetActive(false);
        }

        // 使用オセロカラーのUI表示
        for (int i = 0; i < PlayerMoveObj.Length; i++)
        {
            UI_OseroObj[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnce && GamePlayManager.Instance.isGamePadOK)
        {
            isOnce = true;

            // プレイヤーの色設定
            for (int i = 1; i <= GamePlayManager.Instance.GamePadSelectObj.PlayerNum; i++)
            {
                PlayerMoveObj[i - 1].gameObject.SetActive(true);
                PlayerMoveObj[i - 1].SetPlayerOseroType(PlayerOseroType[i - 1]);
                UI_OseroObj[i - 1].gameObject.SetActive(true);
                UI_OseroObj[i - 1].SetPlayerOseroType(PlayerOseroType[i - 1]);
            }

            UI_OseroPanelObj.SetUIPanel(GamePlayManager.Instance.GamePadSelectObj.PlayerNum);
        }
    }
}
