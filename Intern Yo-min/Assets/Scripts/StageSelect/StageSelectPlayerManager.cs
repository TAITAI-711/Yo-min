using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectPlayerManager : PlayerManager
{
    private int PlayerLangth = 0;
    private UI_StageSelect_InPlayer[] inPlayer;

    // Start is called before the first frame update
    protected override void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        PlayerMoveObj = GetComponentsInChildren<PlayerMove>();
        UI_OseroObj = GetComponentsInChildren<UI_Osero>();
        UI_GameTimeObj = GetComponentInChildren<UI_GameTime>();
        inPlayer = gameObject.GetComponentsInChildren<UI_StageSelect_InPlayer>();
    }

    protected override void Start()
    {
        //Debug.Log(PlayerMoveObj.Length);

        // 使用プレイヤーの非表示
        for (int i = 0; i < PlayerMoveObj.Length; i++)
        {
            PlayerMoveObj[i].gameObject.SetActive(false);
        }

        // 使用オセロカラーのUI非表示
        for (int i = 0; i < UI_OseroObj.Length; i++)
        {
            UI_OseroObj[i].gameObject.SetActive(false);
        }

        // Bボタン押すUI非表示
        for (int i = 0; i < inPlayer.Length; i++)
        {
            inPlayer[i].TMPro.enabled = false;
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (!StageSelectManager.Instance.isStageSelect)
            return;

        // Bボタン押すUI表示
        for (int i = 0; i < inPlayer.Length; i++)
        {
            if (UI_StageSelectGamePadManager.Instance.GamePadList.Count <= i)
                inPlayer[i].TMPro.enabled = true;
            else
                inPlayer[i].TMPro.enabled = false;
        }

        // プレイヤーの表示
        if (GamePlayManager.Instance.Players != null && PlayerLangth != GamePlayManager.Instance.Players.Length)
        {
            PlayerLangth = GamePlayManager.Instance.Players.Length;

            // プレイヤーの色設定
            for (int i = 0; i < GamePlayManager.Instance.Players.Length; i++)
            {
                for (int j = 0; j < PlayerMoveObj.Length; j++)
                {
                    if (PlayerMoveObj[j].PlayerType == GamePlayManager.Instance.Players[i].PlayerType)
                    {
                        PlayerMoveObj[j].gameObject.SetActive(true);
                        PlayerMoveObj[j].SetPlayerOseroType(GamePlayManager.Instance.Players[i].PlayerOseroType);
                        PlayerMoveObj[j].PlayersNum = i;
                        break;
                    }
                }
            }
        }

        if (UI_StageSelectGamePadManager.Instance.GamePadList != null)
        {
            // オセロの色表示
            for (int i = 0; i < UI_StageSelectGamePadManager.Instance.GamePadList.Count; i++)
            {
                UI_OseroObj[i].gameObject.SetActive(true);
                UI_OseroObj[i].SetPlayerOseroType(UI_StageSelectGamePadManager.Instance.GamePadList[i].NowSelectPlayerOseroType);
            }
        }
    }
}
