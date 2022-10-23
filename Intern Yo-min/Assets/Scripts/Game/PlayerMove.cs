using Unity.Collections;
using UnityEngine;
using static GamePlayManager;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private GameObject OseroPrefab;

    [Header("[ �v���C���[�ݒ� ]")]
    [Tooltip("�v���C���[�̃R���g���[���[�ԍ�")]
    [SerializeField]
    EnumPlayerType PlayerType = EnumPlayerType.Player1;

    private GamePlayManager.PlayerOseroTypeInfo PlayerOseroType;

    [Tooltip("�v���C���[�̈ړ���(���b�ōő呬�x�ɒB���邩)"), Range(0.05F, 3.0F)]
    public float MovePow = 0.2f;
    [Tooltip("�v���C���[�̍ő�ړ���(1�b�Ԃɓ�������)"), Range(10.0F, 150.0F)]
    public float MaxMovePow = 50.0f;
    [Tooltip("�v���C���[�̈ړ��ʌ����l(���͂��Ă��Ȃ����̒�R�l)"), Range(0.0001F, 0.2F)]
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

    [SerializeField, Tooltip("�v���C���[���m�̏Փˎ��̒��˕Ԃ��"), Range(0.0F, 800.0F)]
    private float PlayerCrashPow = 200.0f;

    [SerializeField, Tooltip("�v���C���[���ړ������ɍ��킹�ĉ�]���邩")]
    private bool isPlayerRotate = true;


    [Header("[ �I�Z���ݒ� ]")]
    [Tooltip("�I�Z���̍ő�򋗗��}�X��"), Range(1F, 9F)] 
    public float MaxOseroMove = 5.0f;    // �I�Z���̍ő�򋗗��}�X��
    [Tooltip("�I�Z���̏d��"), Range(1F, 200F)] 
    public float OseroGravity = 100.0f;    // �I�Z���̏d��

    [SerializeField, Range(5F, 85F), Tooltip("�ˏo����p�x")]
    public float ThrowingAngle = 45.0f;

    [SerializeField, Tooltip("�I�Z���̉�]��(1.0f��1�b�Ԃ�1��])"), Range(0.0F, 10.0F)]
    private float OseroRotate = 6.0f;


    private Rigidbody rb;
    private Collider cd;

    // �v���C���[�̌���
    [HideInInspector] public Vector2 ShootAngle;   // ���������

    // �`���[�W�p�ϐ�
    [HideInInspector] public float ChargePow;
    private float NowChargeTime;
    [HideInInspector] public float NowReChargeTime;

    // ���̓L�[�̈�񔻒�p
    bool isPress = false;           // ���݉�����Ă��邩
    private bool isOldPress = false; // �O�̃t���[���ŉ�������

    private MeshRenderer Mesh = null; // �v���C���[�̃��b�V�������_���[
    private SkinnedMeshRenderer SMesh = null; // �v���C���[�̃X�L�����b�V�������_���[

    private Animator AnimatorObj = null; // �v���C���[�̃A�j���[�^�[

    // �X�M�~�b�N�p
    private float StartDownMovePow; // �����ړ�������


    // �v���C���[�̓��͂̂�
    //private Vector2 PlayerMoveAngleVec = Vector2.zero;
    //private Vector2 PlayerShootAngleVec = Vector2.zero;


    private void Awake()
    {
        // ������
        rb = gameObject.GetComponent<Rigidbody>();
        cd = gameObject.GetComponent<Collider>();
        ChargePow = 0.0f;
        NowChargeTime = 0.0f;
        NowReChargeTime = 0.0f;

        AnimatorObj = GetComponent<Animator>();


        // �����̃v���C���[�̌���
        ShootAngle.x = 0.0f - transform.position.x;
        ShootAngle.y = 0.0f - transform.position.z;
        ShootAngle = ShootAngle.normalized;

        PlayerRotate();

        // �����ړ����x������
        StartDownMovePow = DownMovePow;

        // �Փ˔������
        //if (PlayerCrashPow <= 0.0f)
        //{
        //    gameObject.layer = LayerMask.NameToLayer("Player_NoHit");
        //}

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

    // �ړ�
    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GamePlayManager.Instance.isGamePlay)
        {
            // �v���C���[�̈ړ�����
            PlayerMovePow();

            // �v���C���[�̈ړ����W�̌Œ菈��
            PlayerMoveMax();

            // �v���C���[���I�Z�����΂������̏������v���C���[�̌���
            PlayerShootAngle();
            
            // �v���C���[�̉�]����
            PlayerRotate();

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


    // �v���C���[�̉�]����
    private void PlayerRotate()
    {
        if (isPlayerRotate)
        {
            // ��]
            Vector3 Vec = new Vector3(ShootAngle.x, 0.0f, ShootAngle.y);
            transform.rotation = Quaternion.LookRotation(Vec);
        }
    }

    // �I�u�W�F�N�g�Ƃ̏Փ�
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[���m�̏Փˏ���
            if (PlayerCrashPow > 0.0f)
            {
                Vector3 vec;
                vec.x = transform.position.x - collision.gameObject.transform.position.x;
                vec.y = 0.0f;
                vec.z = transform.position.z - collision.gameObject.transform.position.z;

                vec.Normalize();

                rb.velocity = Vector3.zero;
                rb.AddForce(vec * PlayerCrashPow, ForceMode.VelocityChange);
            }
        }
    }

    // �M�~�b�N�Փ�

    private void OnTriggerEnter(Collider other)
    {
        HitGimmick_Ice(other);
    }
    private void OnTriggerStay(Collider other)
    {
        HitGimmick_Ice(other);
    }
    // �X����
    private void HitGimmick_Ice(Collider other)
    {
        if (other.gameObject.CompareTag("Gimmick_Ice"))
        {
            if (transform.position.x <= other.gameObject.transform.position.x + other.gameObject.transform.localScale.x &&
                transform.position.x >= other.gameObject.transform.position.x - other.gameObject.transform.localScale.x &&
                transform.position.z <= other.gameObject.transform.position.z + other.gameObject.transform.localScale.z &&
                transform.position.z >= other.gameObject.transform.position.z - other.gameObject.transform.localScale.z)
            {
                float Pow = (1.0f - other.GetComponent<Gimmick_Ice>().IcePow) * StartDownMovePow;

                DownMovePow = Pow;

                //if (PlayerType == EnumPlayerType.Player2)
                //{
                //    Debug.Log("�������Ă�");
                //    Debug.Log(DownMovePow);
                //} 
            }
        }
    }



    // �v���C���[���I�Z�����΂������̏���
    private void PlayerShootAngle()
    {
        Vector2 Vec = new Vector2(0, 0);

        // �I�Z���̓�����
        switch (OseroShootType)
        {
            case EnumOseroShootType.Type1:

                Vec.x = Input.GetAxis(GamePlayManager.Instance.Players[(int)PlayerType].GamePadName_Player + "_RightAxis_X");
                Vec.y = Input.GetAxis(GamePlayManager.Instance.Players[(int)PlayerType].GamePadName_Player + "_RightAxis_Y");
                break;
            case EnumOseroShootType.Type2:
            case EnumOseroShootType.Type3:
                Vec.x = Input.GetAxis(GamePlayManager.Instance.Players[(int)PlayerType].GamePadName_Player + "_LeftAxis_X");
                Vec.y = Input.GetAxis(GamePlayManager.Instance.Players[(int)PlayerType].GamePadName_Player + "_LeftAxis_Y");
                break;
            default:
                break;
        }

        //UnityEngine.Debug.Log(Vec);

        //�X�e�B�b�N�̓��͂����ȏ�Ȃ��ꍇ�͔��f����Ȃ��@���@�����ĂȂ�
        if (Mathf.Abs(Vec.magnitude) > 0.6f && !AnimatorObj.GetBool("isThrow"))
        {
            //UnityEngine.Debug.Log(Vec.normalized);
            ShootAngle = Vec.normalized;
        }
    }


    // �I�Z����΂�����
    private void PlayerShootOsero()
    {
        isPress = false;

        // ���`���[�W���ԍX�V
        if (NowReChargeTime > 0.0f)
            NowReChargeTime -= Time.fixedDeltaTime;

        // R�g���K�[����
        if (Input.GetAxis(GamePlayManager.Instance.Players[(int)PlayerType].GamePadName_Player + "_Button_L2_R2") > 0)
        {
            isPress = true;
        }

        // ���͂Ȃ�
        if (!isPress)
        {
            // 1��O�����Ă�
            if (isOldPress)
            {
                // ������
                if (AnimatorObj != null)
                    AnimatorObj.SetBool("isThrow", true);
            }

            isOldPress = false;
        }
        // �����Ă�@���@���`���[�W����
        else if (NowReChargeTime <= 0.0f)
        {
            // �����r���Ȃ�`���[�W���Ȃ�
            if (AnimatorObj != null && AnimatorObj.GetBool("isThrow"))
                return;


            isOldPress = true;

            // �`���[�W����
            NowChargeTime += Time.fixedDeltaTime;

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

    public void OseroThrow()
    {
        // �I�Z���̐������W
        Vector3 OseroPos = transform.position;
        OseroPos.y += 18.0f;

        // �I�Z������
        GameObject osero = Instantiate(OseroPrefab, OseroPos, Quaternion.identity);
        Osero OseroObj = osero.GetComponent<Osero>();
        FloorManager.Instance.FloorObj.SetFieldOsero(OseroObj);

        // �F�ݒ�
        OseroObj.SetOseroType(PlayerOseroType);

        // �T�C�Y�ݒ�
        //Vector3 OseroSize = osero.transform.localScale;
        //OseroSize.x = BanmenObj.YokoLength - OseroScaleDown;
        //OseroSize.y = 1.0f;
        //OseroSize.z = BanmenObj.TateLength - OseroScaleDown;
        //osero.transform.localScale = OseroSize;

        // ���n���W
        Vector3 EndPos = new Vector3
            (
                OseroPos.x + ShootAngle.x * MaxOseroMove * GamePlayManager.MasuScaleXZ * ChargePow,
                GamePlayManager.MasuScaleY,
                OseroPos.z + ShootAngle.y * MaxOseroMove * GamePlayManager.MasuScaleXZ * ChargePow
            );

        // �I�Z����΂�
        OseroObj.Move(OseroGravity, ThrowingAngle, OseroPos, EndPos, OseroRotate);

        // �`���[�W�I��
        NowChargeTime = 0.0f;
        ChargePow = 0.0f;
        NowReChargeTime = ReChargeTime;

        if (AnimatorObj != null)
            AnimatorObj.SetBool("isThrow", false);
    }

    // �v���C���[�̈ړ����W�̌Œ菈��
    private void PlayerMoveMax()
    {
        // �ړ�����
        Vector3 Pos = gameObject.transform.position; // �ʒu

        Vector3 MaxPos = FloorManager.Instance.PlayerMoveMaxObj.PlayerMoveMaxScale * 0.5f;

        MaxPos.x -= cd.bounds.size.x * 0.5f;
        MaxPos.y -= cd.bounds.size.y * 0.5f;
        MaxPos.z -= cd.bounds.size.z * 0.5f;

        Pos.x = Mathf.Clamp(Pos.x, -MaxPos.x, MaxPos.x);
        Pos.y = Mathf.Clamp(Pos.y, -MaxPos.y, MaxPos.y);
        Pos.z = Mathf.Clamp(Pos.z, -MaxPos.z, MaxPos.z);

        gameObject.transform.position = Pos;
    }

    // �v���C���[�̈ړ�����
    private void PlayerMovePow()
    {
        // �͂������鏈��
        Vector3 Vel = rb.velocity;          // �x���V�e�B
        float Pow = MaxMovePow / MovePow;   // 1�b�Ԃŉ������
        Vector3 MaxMove = new Vector3(MaxMovePow, 0.0f, MaxMovePow);

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

                Vec.x = Input.GetAxis(GamePlayManager.Instance.Players[(int)PlayerType].GamePadName_Player + "_LeftAxis_X");
                Vec.y = Input.GetAxis(GamePlayManager.Instance.Players[(int)PlayerType].GamePadName_Player + "_LeftAxis_Y");
                break;
        }

        //Debug.Log("X:" + Input.GetAxis("Joystick_1_LeftAxis_X"));
        //Debug.Log("Y:" + -Input.GetAxis("Joystick_1_LeftAxis_Y"));

        //�X�e�B�b�N�̓��͂����ȏ�Ȃ��ꍇ�͔��f����Ȃ��@���@�Ȃ��ĂȂ�
        if (Mathf.Abs(Vec.magnitude) > 0.6f && !AnimatorObj.GetBool("isThrow"))
        {
            Vec.Normalize();

            // �ړ��ʉ�����
            Vector3 MoveVel = new Vector3(Vec.x, 0.0f, Vec.y) * Pow * Time.fixedDeltaTime;

            // ���Ȃ�
            if (BanmenManager.Instance.Gimmick_WindObj == null || BanmenManager.Instance.Gimmick_WindObj.WindInfo.Wind_Type == Gimmick_Wind.Enum_Wind_Type.None)
            {
                // �ړ��ʑ���
                Vel += MoveVel;

                // �ő�ړ��ʒ�������߂�
                if (Vel.x > MaxMovePow) Vel.x = MaxMovePow;
                if (Vel.x < -MaxMovePow) Vel.x = -MaxMovePow;
                if (Vel.z > MaxMovePow) Vel.z = MaxMovePow;
                if (Vel.z < -MaxMovePow) Vel.z = -MaxMovePow;
            }
            // ������
            else
            {
                //+�������ɗ͉�����

                Gimmick_Wind.Wind_Info WindInfo = BanmenManager.Instance.Gimmick_WindObj.WindInfo;

                // ���ړ�������
                if (!Mathf.Approximately(WindInfo.WindVec.x, 0))
                {
                    // X
                    if (Mathf.Approximately(Mathf.Sign(WindInfo.WindVec.x), Mathf.Sign(MoveVel.x)))
                    {
                        MoveVel.x = MoveVel.x * WindInfo.PlayerMovePowUp;
                    }
                    else
                    {
                        MoveVel.x = MoveVel.x * WindInfo.PlayerMovePowDown;
                    }
                }
                if (!Mathf.Approximately(WindInfo.WindVec.z, 0))
                {
                    // Z
                    if (Mathf.Approximately(Mathf.Sign(WindInfo.WindVec.z), Mathf.Sign(MoveVel.z)))
                    {
                        MoveVel.z = MoveVel.z * WindInfo.PlayerMovePowUp;
                    }
                    else
                    {
                        MoveVel.z = MoveVel.z * WindInfo.PlayerMovePowDown;
                    }
                }

                // �ړ��ʑ���
                Vel += MoveVel;

                // ���ő�ړ���
                if (!Mathf.Approximately(WindInfo.WindVec.x, 0))
                {
                    // X
                    if (Mathf.Approximately(Mathf.Sign(WindInfo.WindVec.x), Mathf.Sign(Vel.x)))
                    {
                        MaxMove.x = MaxMove.x * WindInfo.PlayerMovePowUp;
                    }
                    else
                    {
                        MaxMove.x = MaxMove.x * WindInfo.PlayerMovePowDown;
                    }
                }
                if (!Mathf.Approximately(WindInfo.WindVec.z, 0))
                {
                    // Z
                    if (Mathf.Approximately(Mathf.Sign(WindInfo.WindVec.z), Mathf.Sign(Vel.z)))
                    {
                        MaxMove.z = MaxMove.z * WindInfo.PlayerMovePowUp;
                    }
                    else
                    {
                        MaxMove.z = MaxMove.z * WindInfo.PlayerMovePowDown;
                    }
                }

                // �ő�ړ��ʒ�������߂�
                if (Vel.x > MaxMove.x) Vel.x = MaxMove.x;
                if (Vel.x < -MaxMove.x) Vel.x = -MaxMove.x;
                if (Vel.z > MaxMove.z) Vel.z = MaxMove.z;
                if (Vel.z < -MaxMove.z) Vel.z = -MaxMove.z;
            }

            rb.velocity = Vel; // �ړ��ʕύX
        }
        // ���͂��Ă��Ȃ�������
        else
        {
            PlayerMoveDown();
        }
    }

    // �v���C���[�̈ړ��ʌ�������
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

    //// �M�~�b�N�̗͂ɂ��v���C���[�̈ړ�
    //private void WindPlayerMove()
    //{

    //}


    // �v���C���[�̃I�Z���̃^�C�v�Z�b�g
    public void SetPlayerOseroType(GamePlayManager.PlayerOseroTypeInfo playerOseroType)
    {
        PlayerOseroType = playerOseroType;

        if ((Mesh = GetComponent<MeshRenderer>()) != null)
        {
            Mesh.material = PlayerOseroType.PlayerMaterial;
        }

        if ((SMesh = GetComponentInChildren<SkinnedMeshRenderer>()) != null)
        {
            SMesh.material = PlayerOseroType.PlayerMaterial;
        }
    }
}
