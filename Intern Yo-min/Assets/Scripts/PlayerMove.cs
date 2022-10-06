using System.Diagnostics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Banmen BanmenObj;
    [SerializeField] private GameObject OseroPrefab;
    private Transform BanmenTf;
    [SerializeField] private Material[] OseroMaterials = new Material[4];

    public enum EnumPlayerType
    {
        Player1,
        Player2,
        Player3,
        Player4
    }
    [Header("[ �v���C���[�ݒ� ]")]
    [Tooltip("�v���C���[�̃R���g���[���[�ԍ�")]
    [SerializeField]
    EnumPlayerType PlayerType = EnumPlayerType.Player1;

    public enum EnumOseroType
    {
        White,
        Black,
        Blue,
        Red
    }
    [Tooltip("�v���C���[�̃I�Z���̐F")]
    [SerializeField]
    EnumOseroType OseroType = EnumOseroType.White;

    [Tooltip("�v���C���[�̈ړ���"), Range(1F, 30F)]
    public float MovePow = 1.0f;
    [Tooltip("�v���C���[�̍ő�ړ���"), Range(10F, 100F)]
    public float MaxMovePow = 10.0f;
    [Tooltip("�v���C���[�̈ړ��ʌ����l(��C��R�l)"), Range(0.1F, 20.0F)]
    public float DownMovePow = 0.1f;


    [Header("[ �I�Z���ݒ� ]")]
    [Tooltip("�I�Z���̍ő�򋗗��}�X��"), Range(1F, 9F)] 
    public float MaxOseroMove = 3.0f;    // �I�Z���̍ő�򋗗��}�X��
    [Tooltip("�I�Z���̏d��"), Range(1F, 100F)] 
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
        BanmenTf = BanmenObj.GetComponent<Transform>();

        // �ő�ړ�����
        MaxMovePosX = BanmenTf.position.x + BanmenTf.localScale.x * 0.5f + 
            gameObject.transform.localScale.x * 0.5f + 0.1f; // 0.1f���]�T��������

        MaxMovePosZ = BanmenTf.position.z + BanmenTf.localScale.z * 0.5f + 
            gameObject.transform.localScale.z * 0.5f + 0.1f; // 0.1f���]�T��������

        switch (OseroType)
        {
            case EnumOseroType.White:
                gameObject.GetComponent<MeshRenderer>().material = OseroMaterials[0];
                break;
            case EnumOseroType.Black:
                gameObject.GetComponent<MeshRenderer>().material = OseroMaterials[1];
                break;
            case EnumOseroType.Blue:
                gameObject.GetComponent<MeshRenderer>().material = OseroMaterials[2];
                break;
            case EnumOseroType.Red:
                gameObject.GetComponent<MeshRenderer>().material = OseroMaterials[3];
                break;
            default:
                break;
        }

        // �����̃v���C���[�̌���
        PlayerAngle = BanmenObj.transform.position - transform.position;
        PlayerAngle = PlayerAngle.normalized;
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
        Vector2 Vec = new Vector2(0, 0);

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

        //UnityEngine.Debug.Log(Vec);

        //�X�e�B�b�N�̓��͂����ȏ�Ȃ��ꍇ�͔��f����Ȃ�
        if (Mathf.Abs(Vec.magnitude) > 0.7f)
        {
            //UnityEngine.Debug.Log(Vec.normalized);
            PlayerAngle = Vec.normalized;
        }
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

            // �F�ݒ�
            osero.GetComponent<Osero>().SetOseroType(OseroType);

            // �T�C�Y�ݒ�
            Vector3 OseroSize = osero.transform.localScale;
            OseroSize.x = BanmenObj.YokoLength - OseroScaleDown;
            OseroSize.y = 1.0f;
            OseroSize.z = BanmenObj.TateLength - OseroScaleDown;
            osero.transform.localScale = OseroSize;

            // ���n���W
            Vector3 EndPos = new Vector3(OseroPos.x + PlayerAngle.x * MaxOseroMove * BanmenObj.YokoLength, 0.0f, OseroPos.z + PlayerAngle.y * MaxOseroMove * BanmenObj.TateLength);

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
