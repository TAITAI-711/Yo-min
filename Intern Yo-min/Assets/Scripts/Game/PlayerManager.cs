using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public PlayerMove[] PlayerMoveObj;
    [HideInInspector] public UI_Osero[] UI_OseroObj;
    //[HideInInspector] private UI_OseroPanel UI_OseroPanelObj;
    [HideInInspector] public UI_GameTime UI_GameTimeObj;

    private bool isOnce = false;

    private void Awake()
    {
        GamePlayManager.Instance.PlayerManagerObj = this;

        isOnce = false;

        PlayerMoveObj = GetComponentsInChildren<PlayerMove>();
        UI_OseroObj = GetComponentsInChildren<UI_Osero>();
        //UI_OseroPanelObj = GetComponentInChildren<UI_OseroPanel>();
        UI_GameTimeObj = GetComponentInChildren<UI_GameTime>();
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
    void FixedUpdate()
    {
        if (!isOnce && GamePlayManager.Instance.isGamePadOK)
        {
            isOnce = true;

            // プレイヤーの色設定
            for (int i = 1; i <= GamePlayManager.Instance.Players.Length; i++)
            {
                PlayerMoveObj[i - 1].gameObject.SetActive(true);
                PlayerMoveObj[i - 1].SetPlayerOseroType(GamePlayManager.Instance.Players[i - 1].PlayerOseroType);
                UI_OseroObj[i - 1].gameObject.SetActive(true);
                UI_OseroObj[i - 1].SetPlayerOseroType(GamePlayManager.Instance.Players[i - 1].PlayerOseroType);
            }

            //if (UI_OseroPanelObj != null)
            //    UI_OseroPanelObj.SetUIPanel(GamePlayManager.Instance.Players.Length);
        }
    }
}
