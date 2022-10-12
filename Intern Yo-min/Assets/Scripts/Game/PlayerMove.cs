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
    [Header("[ プレイヤー設定 ]")]
    [Tooltip("プレイヤーのコントローラー番号")]
    [SerializeField]
    EnumPlayerType PlayerType = EnumPlayerType.Player1;

    private PlayerManager.PlayerOseroTypeInfo PlayerOseroType;

    [Tooltip("プレイヤーの移動量(何秒で最大速度に達するか)"), Range(0.05F, 3.0F)]
    public float MovePow = 0.2f;
    [Tooltip("プレイヤーの最大移動量(1秒間に動く距離)"), Range(10.0F, 150.0F)]
    public float MaxMovePow = 10.0f;
    [Tooltip("プレイヤーの移動量減少値(入力していない時の抵抗値)"), Range(0.0001F, 0.05F)]
    public float DownMovePow = 0.05f;
    [Tooltip("プレイヤーのオセロを飛ばすチャージ速度(秒)"), Range(0.1F, 3F)]
    public float ChargeSpeed = 0.5f;
    [Tooltip("オセロ飛ばしの再使用時間(秒)"), Range(0.0F, 3.0F)]
    public float ReChargeTime = 0.5f;
    public enum EnumOseroShootType
    {
        Type1 = 0,
        Type2,
        Type3
    }
    [Tooltip("プレイヤーのオセロの飛ばし方")]
    public EnumOseroShootType OseroShootType = EnumOseroShootType.Type1;

    [Tooltip("プレイヤー同士の衝突時の跳ね返り量"), Range(0.0F, 800.0F)]
    public float PlayerCrashPow = 0.0f;


    [Header("[ オセロ設定 ]")]
    [Tooltip("オセロの最大飛距離マス数"), Range(1F, 9F)] 
    public float MaxOseroMove = 3.0f;    // オセロの最大飛距離マス数
    [Tooltip("オセロの重力"), Range(1F, 200F)] 
    public float OseroGravity = 9.8f;    // オセロの重力

    [SerializeField, Range(5F, 85F), Tooltip("射出する角度")]
    public float ThrowingAngle;

    private Rigidbody rb;

    // オセロの大きさ少し小さくする用変数
    private float OseroScaleDown = 0.2f;


    // 最大移動範囲用変数
    private float MaxMovePosX = 0;
    private float MaxMovePosZ = 0;

    // プレイヤーの向き
    [HideInInspector] public Vector2 PlayerAngle;

    // チャージ用変数
    [HideInInspector] public float ChargePow;
    private float NowChargeTime;
    [HideInInspector] public float NowReChargeTime;

    // 入力キーの一回判定用
    bool isPress = false;           // 現在押されているか
    private bool isOldPress = false; // 前のフレームで押したか

    private MeshRenderer Mesh = null; // プレイヤーのメッシュレンダラー

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        rb = gameObject.GetComponent<Rigidbody>();
        ChargePow = 0.0f;
        NowChargeTime = 0.0f;
        NowReChargeTime = 0.0f;

        // 盤面取得
        BanmenTf = BanmenObj.GetComponent<Transform>();

        // 最大移動距離
        MaxMovePosX = BanmenTf.position.x + BanmenTf.localScale.x * 0.5f + 
            gameObject.transform.localScale.x * 0.5f + gameObject.transform.localScale.x * 2.0f
            + 0.1f; // 0.1f分余裕もたせる

        MaxMovePosZ = BanmenTf.position.z + BanmenTf.localScale.z * 0.5f + 
            gameObject.transform.localScale.z * 0.5f + gameObject.transform.localScale.z * 2.0f
            + 0.1f; // 0.1f分余裕もたせる

        // オセロの種類
        MeshRenderer Mr = gameObject.GetComponent<MeshRenderer>();

        // 初期のプレイヤーの向き
        PlayerAngle.x = 0.0f - transform.position.x;
        PlayerAngle.y = 0.0f - transform.position.z;
        PlayerAngle = PlayerAngle.normalized;

        //UnityEngine.Debug.Log(PlayerAngle);

        // プレイヤー数取得
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
            // プレイヤーの移動処理
            PlayerMovePow();

            // プレイヤーの移動座標の固定処理
            PlayerMoveMax();

            // プレイヤーがオセロを飛ばす向きの処理
            PlayerShootAngle();

            // オセロ飛ばす処理
            PlayerShootOsero();
        }
        else
        {
            // プレイヤーの移動量減少処理
            PlayerMoveDown();

            // プレイヤーの移動座標の固定処理
            PlayerMoveMax();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤー同士の衝突処理
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




    // プレイヤーがオセロを飛ばす向きの処理
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

        //スティックの入力が一定以上ない場合は反映されない
        if (Mathf.Abs(Vec.magnitude) > 0.7f)
        {
            //UnityEngine.Debug.Log(Vec.normalized);
            PlayerAngle = Vec.normalized;
        }
    }


    // オセロ飛ばす処理
    private void PlayerShootOsero()
    {
        isPress = false;

        // 再使用時間更新
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
                // オセロの生成座標
                Vector3 OseroPos = transform.position;
                OseroPos.y += 5.0f;

                // オセロ生成
                GameObject osero = Instantiate(OseroPrefab, OseroPos, Quaternion.identity);

                // 色設定
                osero.GetComponent<Osero>().SetOseroType(PlayerOseroType);

                // サイズ設定
                Vector3 OseroSize = osero.transform.localScale;
                OseroSize.x = BanmenObj.YokoLength - OseroScaleDown;
                OseroSize.y = 1.0f;
                OseroSize.z = BanmenObj.TateLength - OseroScaleDown;
                osero.transform.localScale = OseroSize;

                // 着地座標
                Vector3 EndPos = new Vector3
                    (
                        OseroPos.x + PlayerAngle.x * MaxOseroMove * BanmenObj.YokoLength * ChargePow,
                        BanmenObj.transform.position.y + 0.5f, 
                        OseroPos.z + PlayerAngle.y * MaxOseroMove * BanmenObj.TateLength * ChargePow
                    );

                osero.GetComponent<Osero>().Move(BanmenObj, OseroGravity, ThrowingAngle, OseroPos, EndPos);

                // チャージ終了
                NowChargeTime = 0.0f;
                ChargePow = 0.0f;
                NowReChargeTime = ReChargeTime;
            }

            isOldPress = false;
        }
        else if (NowReChargeTime <= 0.0f)
        {
            isOldPress = true;

            // チャージ処理
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

    // プレイヤーの移動座標の固定処理
    private void PlayerMoveMax()
    {
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
    }

    // プレイヤーの移動処理
    private void PlayerMovePow()
    {
        // 力を加える処理
        Vector3 Vel = rb.velocity;          // ベロシティ
        float Pow = MaxMovePow / MovePow;   // 1秒間で加える量

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


        // 入力していたら
        if (Vec.x != 0 || Vec.y != 0)
        {
            Vec.Normalize();

            // 移動量加える
            Vel.x += Pow * Vec.x * Time.deltaTime;
            Vel.z += Pow * Vec.y * Time.deltaTime;

            // 最大移動量超えたら戻す
            if (Vel.x > MaxMovePow) Vel.x = MaxMovePow;
            if (Vel.x < -MaxMovePow) Vel.x = -MaxMovePow;
            if (Vel.z > MaxMovePow) Vel.z = MaxMovePow;
            if (Vel.z < -MaxMovePow) Vel.z = -MaxMovePow;

            rb.velocity = Vel; // 移動量変更
        }
        // 入力していなかったら
        else
        {
            PlayerMoveDown();
        }
    }

    private void PlayerMoveDown()
    {
        Vector3 Vel = rb.velocity; // ベロシティ

        if (Vel.magnitude < 0.01f) // 一定量以下は止まる
        {
            Vel = Vector3.zero;
        }
        else
        {
            Vel *= 1.0f - DownMovePow;
        }

        rb.velocity = Vel; // 移動量変更
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
