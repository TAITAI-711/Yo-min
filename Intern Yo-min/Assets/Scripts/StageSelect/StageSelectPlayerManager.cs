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
    private UI_StageSelect_Ready[] Select_Ready;
    private UI_StageSelectColorOK[] Select_ColorOK;
    private UI_StageSelect_GamePlaying Select_GamePlaying;

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
        Select_Ready = gameObject.GetComponentsInChildren<UI_StageSelect_Ready>();
        Select_ColorOK = gameObject.GetComponentsInChildren<UI_StageSelectColorOK>();
        Select_GamePlaying = gameObject.GetComponentInChildren<UI_StageSelect_GamePlaying>();
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
            inPlayer[i].SetImageActive(false);
        }

        // ���f�B�{�^��
        for (int i = 0; i < Select_Ready.Length; i++)
        {
            Select_Ready[i].ImageObj.enabled = false;
        }
        
        // �J���[�̏�������
        for (int i = 0; i < Select_ColorOK.Length; i++)
        {
            Select_ColorOK[i].ImageObj.enabled = false;
        }

        // �������
        Select_GamePlaying.gameObject.SetActive(false);

        Select_Start.gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (!StageSelectManager.Instance.isStageSelect)
            return;

        // �������UI
        if (!Select_GamePlaying.gameObject.activeSelf)
        {
            Select_GamePlaying.gameObject.SetActive(true);
        }


        // ���V�[���֐i�ޗp��UI
        if (StageSelectManager.Instance.isStageSelectEnd)
        {
            if (!Select_Start.gameObject.activeSelf)
                Select_Start.gameObject.SetActive(true);
        }
        else
        {
            if (Select_Start.gameObject.activeSelf)
                Select_Start.gameObject.SetActive(true);
        }

        // B�{�^������UI�\��
        for (int i = 0; i < inPlayer.Length; i++)
        {
            if (UI_StageSelectGamePadManager.Instance.GamePadList.Count <= i)
            {
                if (!inPlayer[i].GetImageActive())
                    inPlayer[i].SetImageActive(true);
            }                
            else
            {
                if (inPlayer[i].GetImageActive())
                {
                    SoundManager.Instance.PlaySound("��������", false);
                    inPlayer[i].SetImageActive(false);
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
                            SoundManager.Instance.PlaySound("��������", false);

                            PlayerMoveObj[j].gameObject.SetActive(true);
                            PlayerMoveObj[j].SetPlayerOseroType(GamePlayManager.Instance.Players[i].PlayerOseroType);
                            EffectManager.Instance.SetEffect("PlayerGo", PlayerMoveObj[j].gameObject.transform.position + new Vector3(0.0f, 5.0f, 0.0f), Quaternion.identity, 8.0f);
                            break;
                        }
                    }
                }
            }


            // �Q�[���v���C�}�l�[�W���[�̃v���C���[�ƕR�Â��ԍ��̎w��X�V
            for (int i = 0; i < PlayerMoveObj.Length; i++)
            {
                if (PlayerMoveObj[i].gameObject.activeSelf)
                    PlayerMoveObj[i].SetPlayersNum();
            }
        }

        if (UI_StageSelectGamePadManager.Instance.GamePadList != null)
        {
            bool isStageSelectEnd = true;

            if (UI_StageSelectGamePadManager.Instance.GamePadList.Count < 2)
                isStageSelectEnd = false;

            // ���݂̎Q�����v���C���[���X�g�̐�������
            for (int i = 0; i < UI_StageSelectGamePadManager.Instance.GamePadList.Count; i++)
            {
                // �I�Z���̐F�\��
                if (!UI_OseroObj[i].gameObject.activeSelf)
                    UI_OseroObj[i].gameObject.SetActive(true);

                if (UI_OseroObj[i].PlayerOseroType.OseroType != UI_StageSelectGamePadManager.Instance.GamePadList[i].NowSelectPlayerOseroType.OseroType)
                {
                    SoundManager.Instance.PlaySound("�V�X�e���ړ�", false);
                    UI_OseroObj[i].SetPlayerOseroType(UI_StageSelectGamePadManager.Instance.GamePadList[i].NowSelectPlayerOseroType);
                }


                // �F����\��
                if (UI_StageSelectGamePadManager.Instance.GamePadList[i].isOK)
                {
                    if (Select_ColorOK[i].ImageObj.enabled)
                        Select_ColorOK[i].ImageObj.enabled = false;
                }
                else
                {
                    if (!Select_ColorOK[i].ImageObj.enabled)
                        Select_ColorOK[i].ImageObj.enabled = true;
                }


                // �ŏI�m�F�I�u�W�F�N�g�\��
                if (UI_StageSelectGamePadManager.Instance.GamePadList[i].isOK && !UI_StageSelectGamePadManager.Instance.GamePadList[i].isFinalyOK)
                {
                    if (!Select_Ready[i].ImageObj.enabled)
                        Select_Ready[i].ImageObj.enabled = true;

                    Select_Ready[i].SetSpriteWait();
                }
                else
                {
                    if (UI_StageSelectGamePadManager.Instance.GamePadList[i].isFinalyOK)
                    {
                        if (!Select_Ready[i].ImageObj.enabled)
                            Select_Ready[i].ImageObj.enabled = true;

                        Select_Ready[i].SetSpriteReady();
                    }
                    else
                    {
                        if (Select_Ready[i].ImageObj.enabled)
                            Select_Ready[i].ImageObj.enabled = false;
                    }
                }

                // ��ł��p�b�h�̏������������ĂȂ�
                if (!UI_StageSelectGamePadManager.Instance.GamePadList[i].isFinalyOK)
                    isStageSelectEnd = false;
            }

            // ���̃V�[���֐i�߂邩
            if (isStageSelectEnd)
                StageSelectManager.Instance.isStageSelectEnd = true;
            else
                StageSelectManager.Instance.isStageSelectEnd = false;
        }
    }
}
