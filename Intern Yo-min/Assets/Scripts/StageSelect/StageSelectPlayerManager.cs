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
        if (PlayerLangth == GamePlayManager.Instance.Players.Length)
            return;


        PlayerLangth = GamePlayManager.Instance.Players.Length;

        // プレイヤーの色設定
        for (int i = 1; i <= GamePlayManager.Instance.Players.Length; i++)
        {
            PlayerMoveObj[i - 1].gameObject.SetActive(true);
            PlayerMoveObj[i - 1].SetPlayerOseroType(GamePlayManager.Instance.Players[i - 1].PlayerOseroType);
            UI_OseroObj[i - 1].gameObject.SetActive(true);
            UI_OseroObj[i - 1].SetPlayerOseroType(GamePlayManager.Instance.Players[i - 1].PlayerOseroType);
        }
    }
}
