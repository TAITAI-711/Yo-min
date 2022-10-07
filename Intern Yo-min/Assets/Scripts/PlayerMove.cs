using System.Diagnostics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Banmen BanmenObj;
    [SerializeField] private GameObject OseroPrefab;
    private Transform BanmenTf;
    [SerializeField] private Material[] OseroMaterials = new Material[4];
    [SerializeField] private UI_Osero UIOseroColorObj;

    public enum EnumPlayerType
    {
        Player1,
        Player2,
        Player3,
        Player4,
        Player5
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

    [Tooltip("�v���C���[�̈ړ���(���b�ōő呬�x�ɒB���邩)"), Range(0.05F, 3.0F)]
    public float MovePow = 0.2f;
    [Tooltip("�v���C���[�̍ő�ړ���(1�b�Ԃɓ�������)"), Range(10.0F, 150.0F)]
    public float MaxMovePow = 10.0f;
    [Tooltip("�v���C���[�̈ړ��ʌ����l(���͂��Ă��Ȃ����̒�R�l)"), Range(0.0001F, 0.05F)]
    public float DownMovePow = 0.05f;
    [Tooltip("�v���C���[�̃I�Z�����΂��`���[�W���x(�b)"), Range(0.1F, 3F)]
    public float ChargeSpeed = 0.5f;
    [Tooltip("�I�Z����΂��̍Ďg�p����(�b)"), Range(0.0F, 3.0F)]
    public float ReChargeTime = 0.5f;
    public enum EnumOseroShootType
    {
        Type1,
        Type2,
        Type3
    }
    [Tooltip("�v���C���[�̃I�Z���̔�΂���")]
    public EnumOseroShootType OseroShootType = EnumOseroShootType.Type1;


    [Header("[ �I�Z���ݒ� ]")]
    [Tooltip("�I�Z���̍ő�򋗗��}�X��"), Range(1F, 9F)] 
    public float MaxOseroMove = 3.0f;    // �I�Z���̍ő�򋗗��}�X��
    [Tooltip("�I�Z���̏d��"), Range(1F, 200F)] 
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

    // �`���[�W�p�ϐ�
    private float ChargePow;
    private float NowChargeTime;
    private float NowReChargeTime;

    // ���̓L�[�̈�񔻒�p
    bool isPress = false;           // ���݉�����Ă��邩
    private bool isOldPress = false; // �O�̃t���[���ŉ�������

    // Start is called before the first frame update
    void Start()
    {
        // ������
        rb = gameObject.GetComponent<Rigidbody>();
        ChargePow = 0.0f;
        NowChargeTime = 0.0f;
        NowReChargeTime = 0.0f;

        // �Ֆʎ擾
        BanmenTf = BanmenObj.GetComponent<Transform>();

        // �ő�ړ�����
        MaxMovePosX = BanmenTf.position.x + BanmenTf.localScale.x * 0.5f + 
            gameObject.transform.localScale.x * 0.5f + gameObject.transform.localScale.x * 2.0f
            + 0.1f; // 0.1f���]�T��������

        MaxMovePosZ = BanmenTf.position.z + BanmenTf.localScale.z * 0.5f + 
            gameObject.transform.localScale.z * 0.5f + gameObject.transform.localScale.z * 2.0f
            + 0.1f; // 0.1f���]�T��������

        // �I�Z���̎��
        MeshRenderer Mr = gameObject.GetComponent<MeshRenderer>();

        switch (OseroType)
        {
            case EnumOseroType.White:
                Mr.material = OseroMaterials[0];
                break;
            case EnumOseroType.Black:
                Mr.material = OseroMaterials[1];
                break;
            case EnumOseroType.Blue:
                Mr.material = OseroMaterials[2];
                break;
            case EnumOseroType.Red:
                Mr.material = OseroMaterials[3];
                break;
            default:
                break;
        }
        UIOseroColorObj.UIOseroColorSet(Mr.material.color, OseroType);

        // �����̃v���C���[�̌���
        PlayerAngle = BanmenObj.transform.position - transform.position;
        PlayerAngle = PlayerAngle.normalized;

        // �v���C���[���擾
        //string[] cName = Input.GetJoystickNames();
        //var currentConnectionCount = 0;
        //for (int i = 0; i < cName.Length; i++)
        //{
        //    if (cName[i] != "")
        //    {
        //        currentConnectionCount++;
        //    }
        //}
        //UnityEngine.Debug.Log(currentConnectionCount);
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

        switch (OseroShootType)
        {
            case EnumOseroShootType.Type1:
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
                    case EnumPlayerType.Player5:
                        Vec.x = Input.GetAxis("Joystick_5_RightAxis_X");
                        Vec.y = -Input.GetAxis("Joystick_5_RightAxis_Y");
                        break;
                    default:
                        break;
                }
                break;
            case EnumOseroShootType.Type2:
                if (isPress)
                {
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
                        case EnumPlayerType.Player5:
                            Vec.x = Input.GetAxis("Joystick_5_LeftAxis_X");
                            Vec.y = -Input.GetAxis("Joystick_5_LeftAxis_Y");
                            break;
                        default:
                            break;
                    }
                }
                break;
            case EnumOseroShootType.Type3:
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
                        case EnumPlayerType.Player5:
                            Vec.x = Input.GetAxis("Joystick_5_LeftAxis_X");
                            Vec.y = -Input.GetAxis("Joystick_5_LeftAxis_Y");
                            break;
                        default:
                            break;
                    }
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
        isPress = false;

        // �Ďg�p���ԍX�V
        if (NowReChargeTime > 0.0f)
            NowReChargeTime -= Time.deltaTime;

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
            case EnumPlayerType.Player5:
                if (Input.GetAxis("Joystick_5_Button_L2_R2") > 0)
                    isPress = true;
                break;
            default:
                break;
        }

        if (!isPress)
        {
            if (isOldPress)
            {
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
                Vector3 EndPos = new Vector3
                    (
                        OseroPos.x + PlayerAngle.x * MaxOseroMove * BanmenObj.YokoLength * ChargePow,
                        BanmenObj.transform.position.y + 0.5f, 
                        OseroPos.z + PlayerAngle.y * MaxOseroMove * BanmenObj.TateLength * ChargePow
                    );

                osero.GetComponent<Osero>().Move(BanmenObj, OseroGravity, ThrowingAngle, OseroPos, EndPos);

                // �`���[�W�I��
                NowChargeTime = 0.0f;
                ChargePow = 0.0f;
                NowReChargeTime = ReChargeTime;
            }

            isOldPress = false;
        }
        else if (NowReChargeTime <= 0.0f)
        {
            isOldPress = true;

            // �`���[�W����
            NowChargeTime += Time.deltaTime;

            if (NowChargeTime > ChargeSpeed)
            {
                ChargePow = 1.0f;
            }
            else
            {
                ChargePow = NowChargeTime / ChargeSpeed;
            }
            UnityEngine.Debug.Log(ChargePow);
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
        Vector3 Vel = rb.velocity;          // �x���V�e�B
        float Pow = MaxMovePow / MovePow;   // 1�b�Ԃŉ������

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

        switch (OseroShootType)
        {
            case EnumOseroShootType.Type1:
            case EnumOseroShootType.Type2:
            case EnumOseroShootType.Type3:
                if (OseroShootType == EnumOseroShootType.Type2 && isPress) 
                    break;

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
                    case EnumPlayerType.Player5:
                        Vec.x = Input.GetAxis("Joystick_5_LeftAxis_X");
                        Vec.y = -Input.GetAxis("Joystick_5_LeftAxis_Y");
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        //Debug.Log("X:" + Input.GetAxis("Joystick_1_LeftAxis_X"));
        //Debug.Log("Y:" + -Input.GetAxis("Joystick_1_LeftAxis_Y"));


        // ���͂��Ă�����
        if (Vec.x != 0 || Vec.y != 0)
        {
            Vec.Normalize();

            // �ړ��ʉ�����
            Vel.x += Pow * Vec.x * Time.deltaTime;
            Vel.z += Pow * Vec.y * Time.deltaTime;

            // �ő�ړ��ʒ�������߂�
            if (Vel.x > MaxMovePow) Vel.x = MaxMovePow;
            if (Vel.x < -MaxMovePow) Vel.x = -MaxMovePow;
            if (Vel.z > MaxMovePow) Vel.z = MaxMovePow;
            if (Vel.z < -MaxMovePow) Vel.z = -MaxMovePow;
        }
        // ���͂��Ă��Ȃ�������
        else
        {
            if (Vel.magnitude < 0.01f) // ���ʈȉ��͎~�܂�
            {
                Vel = Vector3.zero;
            }
            else
            {
                Vel *= 1.0f - DownMovePow;
            }
        }

        rb.velocity = Vel; // �ړ��ʕύX
    }
}
