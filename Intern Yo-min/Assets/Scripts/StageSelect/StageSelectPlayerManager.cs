using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectPlayerManager : PlayerManager
{
    private int PlayerLangth = 0;
    private UI_StageSelect_InPlayer[] inPlayer;
    private UI_StageSelect_Start Select_Start;

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
        Select_Start = gameObject.GetComponentInChildren<UI_StageSelect_Start>();
    }

    protected override void Start()
    {
        //Debug.Log(PlayerMoveObj.Length);

        // �g�p�v���C���[�̔�\��
        for (int i = 0; i < PlayerMoveObj.Length; i++)
        {
            PlayerMoveObj[i].gameObject.SetActive(false);
        }

        // �g�p�I�Z���J���[��UI��\��
        for (int i = 0; i < UI_OseroObj.Length; i++)
        {
            UI_OseroObj[i].gameObject.SetActive(false);
        }

        // B�{�^������UI��\��
        for (int i = 0; i < inPlayer.Length; i++)
        {
            inPlayer[i].TMPro.enabled = false;
        }

        Select_Start.gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (!StageSelectManager.Instance.isStageSelect)
            return;

        if (GamePlayManager.Instance.Players != null &&
            GamePlayManager.Instance.Players.Length >= 2)
        {
            Select_Start.gameObject.SetActive(true);
        }

        // B�{�^������UI�\��
        for (int i = 0; i < inPlayer.Length; i++)
        {
            if (UI_StageSelectGamePadManager.Instance.GamePadList.Count <= i)
            {
                if (!inPlayer[i].TMPro.enabled)
                    inPlayer[i].TMPro.enabled = true;
            }                
            else
            {
                if (inPlayer[i].TMPro.enabled)
                {
                    SoundManager.Instance.PlaySound("��������", false);
                    inPlayer[i].TMPro.enabled = false;
                }
            }
        }

        // �v���C���[�̕\��
        if (GamePlayManager.Instance.Players != null && PlayerLangth != GamePlayManager.Instance.Players.Length)
        {
            PlayerLangth = GamePlayManager.Instance.Players.Length;

            // �v���C���[�̐F�ݒ�
            for (int i = 0; i < GamePlayManager.Instance.Players.Length; i++)
            {
                for (int j = 0; j < PlayerMoveObj.Length; j++)
                {
                    if (PlayerMoveObj[j].PlayerType == GamePlayManager.Instance.Players[i].PlayerType)
                    {
                        if (!PlayerMoveObj[j].gameObject.activeSelf)
                        {
                            SoundManager.Instance.PlaySound("����", false);

                            PlayerMoveObj[j].gameObject.SetActive(true);
                            PlayerMoveObj[j].SetPlayerOseroType(GamePlayManager.Instance.Players[i].PlayerOseroType);
                            PlayerMoveObj[j].PlayersNum = i;
                            break;
                        }
                    }
                }
            }
        }

        if (UI_StageSelectGamePadManager.Instance.GamePadList != null)
        {
            // �I�Z���̐F�\��
            for (int i = 0; i < UI_StageSelectGamePadManager.Instance.GamePadList.Count; i++)
            {
                UI_OseroObj[i].gameObject.SetActive(true);
                UI_OseroObj[i].SetPlayerOseroType(UI_StageSelectGamePadManager.Instance.GamePadList[i].NowSelectPlayerOseroType);
            }
        }
    }
}
