using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Banmen BanmenObj;
    [SerializeField] private GameObject KomaPrefab;
    private Transform BanmenTf;

    
    public float MovePow = 1.0f;
    public float MaxMovePow = 10.0f;
    public float DownMovePow = 0.1f;

    public float MaxKomaMove = 0.0f;    // �I�Z���̍ő�ړ��}�X��
    public float KomaFallPow = 5.0f;    // �I�Z���̏d��

    private Rigidbody rb;

    // �I�Z���̑傫����������������p�ϐ�
    private float KomaScaleDown = 0.2f;


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


        if (Input.GetKey(KeyCode.W))
        {
            Vel.z += MovePow;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vel.z += -MovePow;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vel.x += MovePow;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vel.x += -MovePow;
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
            // �I�Z������
            GameObject Koma = Instantiate(KomaPrefab, gameObject.transform.position, Quaternion.identity);

            // �T�C�Y�ݒ�
            Vector3 KomaSize = Koma.transform.localScale;
            KomaSize.x = BanmenObj.YokoLength - KomaScaleDown;
            KomaSize.y = 1.0f;
            KomaSize.z = BanmenObj.TateLength - KomaScaleDown;
            Koma.transform.localScale = KomaSize;

            Vector3 EndPos = new Vector3(Pos.x + MaxKomaMove * BanmenObj.YokoLength, 0.0f, Pos.z + -MaxKomaMove * BanmenObj.TateLength);

            Koma.GetComponent<Koma>().Move(BanmenObj, KomaFallPow, transform.position, EndPos);
        }
    }
}
