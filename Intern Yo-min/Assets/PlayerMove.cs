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

    public float MaxKomaMove = 0.0f;    // オセロの最大移動マス数
    public float KomaFallPow = 5.0f;    // オセロの重力

    private Rigidbody rb;

    // オセロの大きさ少し小さくする用変数
    private float KomaScaleDown = 0.2f;


    // 最大移動範囲用変数
    private float MaxMovePosX = 0;
    private float MaxMovePosZ = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = DownMovePow;

        // 盤面取得
        if (BanmenObj != null)
        {
            BanmenTf = BanmenObj.GetComponent<Transform>();
        }

        // 最大移動距離
        MaxMovePosX = BanmenTf.position.x + BanmenTf.localScale.x * 0.5f + 
            gameObject.transform.localScale.x * 0.5f + 0.1f; // 0.1f分余裕もたせる

        MaxMovePosZ = BanmenTf.position.z + BanmenTf.localScale.z * 0.5f + 
            gameObject.transform.localScale.z * 0.5f + 0.1f; // 0.1f分余裕もたせる
    }

    // Update is called once per frame
    void Update()
    {
        // 力を加える処理
        Vector3 Vel = rb.velocity; // ベロシティ


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


        // 移動制限
        Vector3 Pos = gameObject.transform.position; // 位置


        if (Pos.x > MaxMovePosX)
            Pos.x = MaxMovePosX;

        if (Pos.x < -MaxMovePosX)
            Pos.x = -MaxMovePosX;

        if (Pos.z > MaxMovePosZ)
            Pos.z = MaxMovePosZ;

        if (Pos.z < -MaxMovePosZ)
            Pos.z = -MaxMovePosZ;

        gameObject.transform.position = Pos;


        // オセロ飛ばす処理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // オセロ生成
            GameObject Koma = Instantiate(KomaPrefab, gameObject.transform.position, Quaternion.identity);

            // サイズ設定
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
