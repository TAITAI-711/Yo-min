using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] PlayerMove[] PlayerMoveObj;
    [HideInInspector] UI_Osero[] OseroUIObj;

    [System.Serializable]
    public enum EnumOseroType
    {
        Red = 0,
        Blue,
        Yellow,
        Purple
    }

    [System.Serializable]
    public struct PlayerOseroTypeInfo
    {
        [Tooltip("�v���C���[�̃I�Z���}�e���A��")]
        public Material PlayerMaterial;

        [Tooltip("�v���C���[�̃I�Z���̐F")]
        public EnumOseroType OseroType;
    }

    [Header("[ �v���C���[�̐F�ݒ� ]")]
    [Tooltip("�v���C���[�ƃI�Z���̐F�ݒ�")]
    public PlayerOseroTypeInfo[] PlayerOseroType = new PlayerOseroTypeInfo[4];


    private bool isOnce = false;

    private void Awake()
    {
        GamePlayManager.Instance.PlayerManagerObj = this;

        PlayerMoveObj = GetComponentsInChildren<PlayerMove>();
        OseroUIObj = GetComponentsInChildren<UI_Osero>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(PlayerMoveObj.Length);

        // �g�p�v���C���[�̕\��
        for (int i = 0; i < PlayerMoveObj.Length; i++)
        {
            PlayerMoveObj[i].gameObject.SetActive(false);
        }

        // �g�p�I�Z���J���[��UI�\��
        for (int i = 0; i < PlayerMoveObj.Length; i++)
        {
            OseroUIObj[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnce && GamePlayManager.Instance.isGamePadOK)
        {
            isOnce = true;

            // �v���C���[�̐F�ݒ�
            for (int i = 1; i <= GamePlayManager.Instance.GamePadSelectObj.PlayerNum; i++)
            {
                PlayerMoveObj[i - 1].gameObject.SetActive(true);
                PlayerMoveObj[i - 1].SetPlayerOseroType(PlayerOseroType[i - 1]);
                OseroUIObj[i - 1].gameObject.SetActive(true);
                OseroUIObj[i - 1].SetPlayerOseroType(PlayerOseroType[i - 1]);
            }
        }
    }
}
