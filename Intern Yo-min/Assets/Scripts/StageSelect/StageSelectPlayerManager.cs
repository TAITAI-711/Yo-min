using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectPlayerManager : PlayerManager
{
    private int PlayerLangth = 0;

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
    }

    protected override void Start()
    {
        //Debug.Log(PlayerMoveObj.Length);

        // 使用プレイヤーの表示
        for (int i = 0; i < PlayerMoveObj.Length; i++)
        {
            PlayerMoveObj[i].gameObject.SetActive(false);
        }

        // 使用オセロカラーのUI表示
        for (int i = 0; i < UI_OseroObj.Length; i++)
        {
            UI_OseroObj[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (!StageSelectManager.Instance.isStageSelect)
            return;

        if (GamePlayManager.Instance.Players != null && PlayerLangth != GamePlayManager.Instance.Players.Length)
        {
            PlayerLangth = GamePlayManager.Instance.Players.Length;

            // プレイヤーの色設定
            for (int i = 0; i < GamePlayManager.Instance.Players.Length; i++)
            {
                PlayerMoveObj[i].gameObject.SetActive(true);
                PlayerMoveObj[i].SetPlayerOseroType(GamePlayManager.Instance.Players[i].PlayerOseroType);
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
