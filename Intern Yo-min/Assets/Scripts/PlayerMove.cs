using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Banmen BanmenObj;
    [SerializeField] private GameObject OseroPrefab;
    private Transform BanmenTf;

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
        // �͂������鏈��
        Vector3 Vel = rb.velocity; // �x���V�e�B
        float Pow = MovePow;

        if (Input.GetKey(KeyCode.W))
        {
            Vel.z += Pow;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vel.z += -Pow;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vel.x += Pow;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vel.x += -Pow;
        }


        if (Vel.x > MaxMovePow) Vel.x = MaxMovePow;
        if (Vel.x < -MaxMovePow) Vel.x = -MaxMovePow;
        if (Vel.z > MaxMovePow) Vel.z = MaxMovePow;
        if (Vel.z < -MaxMovePow) Vel.z = -MaxMovePow;

        rb.velocity = Vel;


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


        // �I�Z����΂�����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �I�Z���̐������W
            Vector3 OseroPos = Pos;
            OseroPos.y += 5.0f;

            // �I�Z������
            GameObject osero = Instantiate(OseroPrefab, OseroPos, Quaternion.identity);

            // �T�C�Y�ݒ�
            Vector3 OseroSize = osero.transform.localScale;
            OseroSize.x = BanmenObj.YokoLength - OseroScaleDown;
            OseroSize.y = 1.0f;
            OseroSize.z = BanmenObj.TateLength - OseroScaleDown;
            osero.transform.localScale = OseroSize;

            Vector3 EndPos = new Vector3(Pos.x + MaxOseroMove * BanmenObj.YokoLength, 0.0f, Pos.z);

            osero.GetComponent<Osero>().Move(BanmenObj, OseroGravity, ThrowingAngle, OseroPos, EndPos);
        }
    }
}
