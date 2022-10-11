using System.Diagnostics;
using Unity.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Banmen BanmenObj;
    [SerializeField] private GameObject OseroPrefab;
    private Transform BanmenTf;
    //[SerializeField] private UI_Osero UIOseroColorObj;

    public enum EnumPlayerType
    {
        Player1 = 0,
        Player2,
        Player3,
        Player4
    }
    [Header("[ �v���C���[�ݒ� ]")]
    [Tooltip("�v���C���[�̃R���g���[���[�ԍ�")]
    [SerializeField]
    EnumPlayerType PlayerType = EnumPlayerType.Player1;

    private PlayerManager.PlayerOseroTypeInfo PlayerOseroType;

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
        Type1 = 0,
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
    public float ThrowingAngle;

    private Rigidbody rb;

    // �I�Z���̑傫����������������p�ϐ�
    private float OseroScaleDown = 0.2f;


    // �ő�ړ��͈͗p�ϐ�
    private float MaxMovePosX = 0;
    private float MaxMovePosZ = 0;

    // �v���C���[�̌���
    [HideInInspector] public Vector2 PlayerAngle;

    // �`���[�W�p�ϐ�
    [HideInInspector] public float ChargePow;
    private float NowChargeTime;
    [HideInInspector] public float NowReChargeTime;

    // ���̓L�[�̈�񔻒�p
    bool isPress = false;           // ���݉�����Ă��邩
    private bool isOldPress = false; // �O�̃t���[���ŉ�������

    private MeshRenderer Mesh = null;

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

        // �����̃v���C���[�̌���
        PlayerAngle.x = 0.0f - transform.position.x;
        PlayerAngle.y = 0.0f - transform.position.z;
        PlayerAngle = PlayerAngle.normalized;

        //UnityEngine.Debug.Log(PlayerAngle);

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
        if (GamePlayManager.Instance.isGamePlay)
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
        else
        {
            // �v���C���[�̈ړ��ʌ�������
            PlayerMoveDown();

            // �v���C���[�̈ړ����W�̌Œ菈��
            PlayerMoveMax();
        }
    }

    // �v���C���[���I�Z�����΂������̏���
    private void PlayerShootAngle()
    {
        Vector2 Vec = new Vector2(0, 0);

        switch (OseroShootType)
        {
            case EnumOseroShootType.Type1:

                Vec.x = Input.GetAxis(GamePlayManager.Instance.GamePadSelectObj.GamePadName_Player[(int)PlayerType] + "_RightAxis_X");
                Vec.y = -Input.GetAxis(GamePlayManager.Instance.GamePadSelectObj.GamePadName_Player[(int)PlayerType] + "_RightAxis_Y");
                break;
            case EnumOseroShootType.Type2:
                if (isPress)
                {
                    Vec.x = Input.GetAxis(GamePlayManager.Instance.GamePadSelectObj.GamePadName_Player[(int)PlayerType] + "_LeftAxis_X");
                    Vec.y = -Input.GetAxis(GamePlayManager.Instance.GamePadSelectObj.GamePadName_Player[(int)PlayerType] + "_LeftAxis_Y");
                }
                break;
            case EnumOseroShootType.Type3:
                Vec.x = Input.GetAxis(GamePlayManager.Instance.GamePadSelectObj.GamePadName_Player[(int)PlayerType] + "_LeftAxis_X");
                Vec.y = -Input.GetAxis(GamePlayManager.Instance.GamePadSelectObj.GamePadName_Player[(int)PlayerType] + "_LeftAxis_Y");
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

        if (Input.GetAxis(GamePlayManager.Instance.GamePadSelectObj.GamePadName_Player[(int)PlayerType] + "_Button_L2_R2") > 0)
        {
            isPress = true;
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
                osero.GetComponent<Osero>().SetOseroType(PlayerOseroType);

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
            //UnityEngine.Debug.Log(ChargePow);
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

        Vector2 Vec = new Vector2(0, 0);

        switch (OseroShootType)
        {
            case EnumOseroShootType.Type1:
            case EnumOseroShootType.Type2:
            case EnumOseroShootType.Type3:
                if (OseroShootType == EnumOseroShootType.Type2 && isPress) 
                    break;

                Vec.x = Input.GetAxis(GamePlayManager.Instance.GamePadSelectObj.GamePadName_Player[(int)PlayerType] + "_LeftAxis_X");
                Vec.y = -Input.GetAxis(GamePlayManager.Instance.GamePadSelectObj.GamePadName_Player[(int)PlayerType] + "_LeftAxis_Y");
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

            rb.velocity = Vel; // �ړ��ʕύX
        }
        // ���͂��Ă��Ȃ�������
        else
        {
            PlayerMoveDown();
        }
    }

    private void PlayerMoveDown()
    {
        Vector3 Vel = rb.velocity; // �x���V�e�B

        if (Vel.magnitude < 0.01f) // ���ʈȉ��͎~�܂�
        {
            Vel = Vector3.zero;
        }
        else
        {
            Vel *= 1.0f - DownMovePow;
        }

        rb.velocity = Vel; // �ړ��ʕύX
    }

    public void SetPlayerOseroType(PlayerManager.PlayerOseroTypeInfo playerOseroType)
    {
        PlayerOseroType = playerOseroType;

        if (Mesh == null)
        {
            Mesh = GetComponent<MeshRenderer>();
        }

        Mesh.material = PlayerOseroType.PlayerMaterial;
    }
}
