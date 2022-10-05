using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Banmen BanmenObj;
    [SerializeField] private GameObject OseroPrefab;
    private Transform BanmenTf;

    public enum EnumPlayerType
    {
        Player1,
        Player2,
        Player3,
        Player4
    }
    [Tooltip("�v���C���[�̃R���g���[���[�ԍ�")]
    [SerializeField]
    EnumPlayerType PlayerType = EnumPlayerType.Player1;

    [Tooltip("�v���C���[�̈ړ���")]
    public float MovePow = 1.0f;
    [Tooltip("�v���C���[�̍ő�ړ���")]
    public float MaxMovePow = 10.0f;
    [Tooltip("�v���C���[�̈ړ��ʌ����l(��C��R�l)")]
    public float DownMovePow = 0.1f;

    [Tooltip("�I�Z���̍ő�򋗗��}�X��")] 
    public float MaxOseroMove = 3.0f;    // �I�Z���̍ő�򋗗��}�X��
    [Tooltip("�I�Z���̏d��")] 
    public float OseroGravity = 9.8f;    // �I�Z���̏d��

    [SerializeField, Range(5F, 85F), Tooltip("�ˏo����p�x")]
    private float ThrowingAngle;

    private Rigidbody rb;

    // �I�Z���̑傫����������������p�ϐ�
    private float OseroScaleDown = 0.2f;


    // �ő�ړ��͈͗p�ϐ�
    private float MaxMovePosX = 0;
    private float MaxMovePosZ = 0;

    // �v���C���[�̌���
    private Vector2 PlayerAngle;

    // ���̓L�[�̈�񔻒�p
    private bool isKeyDown = false;

    // Start is called before the first frame update
    void Start()
    {
        // ������
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = DownMovePow;

        // �Ֆʎ擾
        if (BanmenObj != null)
        {
            BanmenTf = BanmenObj.GetComponent<Transform>();
        }

        // �ő�ړ�����
        MaxMovePosX = BanmenTf.position.x + BanmenTf.localScale.x * 0.5f + 
            gameObject.transform.localScale.x * 0.5f + 0.1f; // 0.1f���]�T��������

        MaxMovePosZ = BanmenTf.position.z + BanmenTf.localScale.z * 0.5f + 
            gameObject.transform.localScale.z * 0.5f + 0.1f; // 0.1f���]�T��������
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[�̈ړ�����
        PlayerMovePow();

        // �v���C���[�̈ړ����W�̌Œ菈��
        PlayerMoveMax();

        // �v���C���[���I�Z�����΂������̏���
        PlayerShootAngle();

        // �I�Z����΂�����
        PlayerShootOsero();        
    }

    // �v���C���[���I�Z�����΂������̏���
    private void PlayerShootAngle()
    {
        Vector2 Vec = new Vector2();

        switch (PlayerType)
        {
            case EnumPlayerType.Player1:
                Vec.x = Input.GetAxis("Joystick_1_RightAxis_X");
                Vec.y = -Input.GetAxis("Joystick_1_RightAxis_Y");
                break;
            case EnumPlayerType.Player2:
                Vec.x = Input.GetAxis("Joystick_2_RightAxis_X");
                Vec.y = -Input.GetAxis("Joystick_2_RightAxis_Y");
                break;
            case EnumPlayerType.Player3:
                Vec.x = Input.GetAxis("Joystick_3_RightAxis_X");
                Vec.y = -Input.GetAxis("Joystick_3_RightAxis_Y");
                break;
            case EnumPlayerType.Player4:
                Vec.x = Input.GetAxis("Joystick_4_RightAxis_X");
                Vec.y = -Input.GetAxis("Joystick_4_RightAxis_Y");
                break;
            default:
                break;
        }

        PlayerAngle = Vec.normalized;
    }

    // �I�Z����΂�����
    private void PlayerShootOsero()
    {
        bool isPress = false;

        switch (PlayerType)
        {
            case EnumPlayerType.Player1:
                if (Input.GetAxis("Joystick_1_Button_L2_R2") > 0)
                    isPress = true;
                break;
            case EnumPlayerType.Player2:
                if (Input.GetAxis("Joystick_2_Button_L2_R2") > 0)
                    isPress = true;
                break;
            case EnumPlayerType.Player3:
                if (Input.GetAxis("Joystick_3_Button_L2_R2") > 0)
                    isPress = true;
                break;
            case EnumPlayerType.Player4:
                if (Input.GetAxis("Joystick_4_Button_L2_R2") > 0)
                    isPress = true;
                break;
            default:
                break;
        }

        if (!isPress)
        {
            isKeyDown = false;
        }
        else if (!isKeyDown)
        {
            isKeyDown = true;

            // �I�Z���̐������W
            Vector3 OseroPos = transform.position;
            OseroPos.y += 5.0f;

            // �I�Z������
            GameObject osero = Instantiate(OseroPrefab, OseroPos, Quaternion.identity);

            // �T�C�Y�ݒ�
            Vector3 OseroSize = osero.transform.localScale;
            OseroSize.x = BanmenObj.YokoLength - OseroScaleDown;
            OseroSize.y = 1.0f;
            OseroSize.z = BanmenObj.TateLength - OseroScaleDown;
            osero.transform.localScale = OseroSize;

            // ���n���W
            //new Vector3()
            Vector3 EndPos = new Vector3(OseroPos.x + MaxOseroMove * BanmenObj.YokoLength, 0.0f, OseroPos.z);

            osero.GetComponent<Osero>().Move(BanmenObj, OseroGravity, ThrowingAngle, OseroPos, EndPos);
        }
    }

    // �v���C���[�̈ړ����W�̌Œ菈��
    private void PlayerMoveMax()
    {
        // �ړ�����
        Vector3 Pos = gameObject.transform.position; // �ʒu


        if (Pos.x > MaxMovePosX)
            Pos.x = MaxMovePosX;

        if (Pos.x < -MaxMovePosX)
            Pos.x = -MaxMovePosX;

        if (Pos.z > MaxMovePosZ)
            Pos.z = MaxMovePosZ;

        if (Pos.z < -MaxMovePosZ)
            Pos.z = -MaxMovePosZ;

        gameObject.transform.position = Pos;
    }

    // �v���C���[�̈ړ�����
    private void PlayerMovePow()
    {
        // �͂������鏈��
        Vector3 Vel = rb.velocity; // �x���V�e�B
        float Pow = MovePow;

        //if (Input.GetKey(KeyCode.W))
        //{
        //    Vel.z += Pow;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    Vel.z += -Pow;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    Vel.x += Pow;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    Vel.x += -Pow;
        //}

        Vector2 Vec = new Vector2();
        switch (PlayerType)
        {
            case EnumPlayerType.Player1:
                Vec.x = Input.GetAxis("Joystick_1_LeftAxis_X");
                Vec.y = -Input.GetAxis("Joystick_1_LeftAxis_Y");
                break;
            case EnumPlayerType.Player2:
                Vec.x = Input.GetAxis("Joystick_2_LeftAxis_X");
                Vec.y = -Input.GetAxis("Joystick_2_LeftAxis_Y");
                break;
            case EnumPlayerType.Player3:
                Vec.x = Input.GetAxis("Joystick_3_LeftAxis_X");
                Vec.y = -Input.GetAxis("Joystick_3_LeftAxis_Y");
                break;
            case EnumPlayerType.Player4:
                Vec.x = Input.GetAxis("Joystick_4_LeftAxis_X");
                Vec.y = -Input.GetAxis("Joystick_4_LeftAxis_Y");
                break;
            default:
                break;
        }

        //Debug.Log("X:" + Input.GetAxis("Joystick_1_LeftAxis_X"));
        //Debug.Log("Y:" + -Input.GetAxis("Joystick_1_LeftAxis_Y"));

        Vec.Normalize();

        // �ړ��ʉ�����
        Vel.x += Pow * Vec.x;
        Vel.z += Pow * Vec.y;

        // �ő�ړ��ʒ�������߂�
        if (Vel.x > MaxMovePow) Vel.x = MaxMovePow;
        if (Vel.x < -MaxMovePow) Vel.x = -MaxMovePow;
        if (Vel.z > MaxMovePow) Vel.z = MaxMovePow;
        if (Vel.z < -MaxMovePow) Vel.z = -MaxMovePow;

        rb.velocity = Vel; // �ړ��ʕύX
    }
}
