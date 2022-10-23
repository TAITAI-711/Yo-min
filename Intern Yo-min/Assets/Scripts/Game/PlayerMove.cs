using Unity.Collections;
using UnityEngine;
using static GamePlayManager;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private GameObject OseroPrefab;

    [Header("[ プレイヤー設定 ]")]
    [Tooltip("プレイヤーのコントローラー番号")]
    [SerializeField]
    EnumPlayerType PlayerType = EnumPlayerType.Player1;

    private GamePlayManager.PlayerOseroTypeInfo PlayerOseroType;

    [Tooltip("プレイヤーの移動量(何秒で最大速度に達するか)"), Range(0.05F, 3.0F)]
    public float MovePow = 0.2f;
    [Tooltip("プレイヤーの最大移動量(1秒間に動く距離)"), Range(10.0F, 150.0F)]
    public float MaxMovePow = 50.0f;
    [Tooltip("プレイヤーの移動量減少値(入力していない時の抵抗値)"), Range(0.0001F, 0.2F)]
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

    [SerializeField, Tooltip("プレイヤー同士の衝突時の跳ね返り量"), Range(0.0F, 800.0F)]
    private float PlayerCrashPow = 200.0f;

    [SerializeField, Tooltip("プレイヤーが移動方向に合わせて回転するか")]
    private bool isPlayerRotate = true;


    [Header("[ オセロ設定 ]")]
    [Tooltip("オセロの最大飛距離マス数"), Range(1F, 9F)] 
    public float MaxOseroMove = 5.0f;    // オセロの最大飛距離マス数
    [Tooltip("オセロの重力"), Range(1F, 200F)] 
    public float OseroGravity = 100.0f;    // オセロの重力

    [SerializeField, Range(5F, 85F), Tooltip("射出する角度")]
    public float ThrowingAngle = 45.0f;

    [SerializeField, Tooltip("オセロの回転量(1.0fで1秒間に1回転)"), Range(0.0F, 10.0F)]
    private float OseroRotate = 6.0f;


    private Rigidbody rb;
    private Collider cd;

    // プレイヤーの向き
    [HideInInspector] public Vector2 ShootAngle;   // 投げる向き

    // チャージ用変数
    [HideInInspector] public float ChargePow;
    private float NowChargeTime;
    [HideInInspector] public float NowReChargeTime;

    // 入力キーの一回判定用
    bool isPress = false;           // 現在押されているか
    private bool isOldPress = false; // 前のフレームで押したか

    private MeshRenderer Mesh = null; // プレイヤーのメッシュレンダラー
    private SkinnedMeshRenderer SMesh = null; // プレイヤーのスキンメッシュレンダラー

    private Animator AnimatorObj = null; // プレイヤーのアニメーター

    // 氷ギミック用
    private float StartDownMovePow; // 初期移動減少量


    // プレイヤーの入力のみ
    //private Vector2 PlayerMoveAngleVec = Vector2.zero;
    //private Vector2 PlayerShootAngleVec = Vector2.zero;


    private void Awake()
    {
        // 初期化
        rb = gameObject.GetComponent<Rigidbody>();
        cd = gameObject.GetComponent<Collider>();
        ChargePow = 0.0f;
        NowChargeTime = 0.0f;
        NowReChargeTime = 0.0f;

        AnimatorObj = GetComponent<Animator>();


        // 初期のプレイヤーの向き
        ShootAngle.x = 0.0f - transform.position.x;
        ShootAngle.y = 0.0f - transform.position.z;
        ShootAngle = ShootAngle.normalized;

        PlayerRotate();

        // 初期移動速度減少量
        StartDownMovePow = DownMovePow;

        // 衝突判定消す
        //if (PlayerCrashPow <= 0.0f)
        //{
        //    gameObject.layer = LayerMask.NameToLayer("Player_NoHit");
        //}

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

    // 移動
    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GamePlayManager.Instance.isGamePlay)
        {
            // プレイヤーの移動処理
            PlayerMovePow();

            // プレイヤーの移動座標の固定処理
            PlayerMoveMax();

            // プレイヤーがオセロを飛ばす向きの処理＆プレイヤーの向き
            PlayerShootAngle();
            
            // プレイヤーの回転処理
            PlayerRotate();

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


    // プレイヤーの回転処理
    private void PlayerRotate()
    {
        if (isPlayerRotate)
        {
            // 回転
            Vector3 Vec = new Vector3(ShootAngle.x, 0.0f, ShootAngle.y);
            transform.rotation = Quaternion.LookRotation(Vec);
        }
    }

    // オブジェクトとの衝突
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

    // ギミック衝突

    private void OnTriggerEnter(Collider other)
    {
        HitGimmick_Ice(other);
    }
    private void OnTriggerStay(Collider other)
    {
        HitGimmick_Ice(other);
    }
    // 氷処理
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
                //    Debug.Log("当たってる");
                //    Debug.Log(DownMovePow);
                //} 
            }
        }
    }



    // プレイヤーがオセロを飛ばす向きの処理
    private void PlayerShootAngle()
    {
        Vector2 Vec = new Vector2(0, 0);

        // オセロの投げ方
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

        //スティックの入力が一定以上ない場合は反映されない　＆　投げてない
        if (Mathf.Abs(Vec.magnitude) > 0.6f && !AnimatorObj.GetBool("isThrow"))
        {
            //UnityEngine.Debug.Log(Vec.normalized);
            ShootAngle = Vec.normalized;
        }
    }


    // オセロ飛ばす処理
    private void PlayerShootOsero()
    {
        isPress = false;

        // リチャージ時間更新
        if (NowReChargeTime > 0.0f)
            NowReChargeTime -= Time.fixedDeltaTime;

        // Rトリガー入力
        if (Input.GetAxis(GamePlayManager.Instance.Players[(int)PlayerType].GamePadName_Player + "_Button_L2_R2") > 0)
        {
            isPress = true;
        }

        // 入力ない
        if (!isPress)
        {
            // 1回前おしてた
            if (isOldPress)
            {
                // 投げる
                if (AnimatorObj != null)
                    AnimatorObj.SetBool("isThrow", true);
            }

            isOldPress = false;
        }
        // 押してる　＆　リチャージ完了
        else if (NowReChargeTime <= 0.0f)
        {
            // 投げ途中ならチャージしない
            if (AnimatorObj != null && AnimatorObj.GetBool("isThrow"))
                return;


            isOldPress = true;

            // チャージ処理
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
        // オセロの生成座標
        Vector3 OseroPos = transform.position;
        OseroPos.y += 18.0f;

        // オセロ生成
        GameObject osero = Instantiate(OseroPrefab, OseroPos, Quaternion.identity);
        Osero OseroObj = osero.GetComponent<Osero>();
        FloorManager.Instance.FloorObj.SetFieldOsero(OseroObj);

        // 色設定
        OseroObj.SetOseroType(PlayerOseroType);

        // サイズ設定
        //Vector3 OseroSize = osero.transform.localScale;
        //OseroSize.x = BanmenObj.YokoLength - OseroScaleDown;
        //OseroSize.y = 1.0f;
        //OseroSize.z = BanmenObj.TateLength - OseroScaleDown;
        //osero.transform.localScale = OseroSize;

        // 着地座標
        Vector3 EndPos = new Vector3
            (
                OseroPos.x + ShootAngle.x * MaxOseroMove * GamePlayManager.MasuScaleXZ * ChargePow,
                GamePlayManager.MasuScaleY,
                OseroPos.z + ShootAngle.y * MaxOseroMove * GamePlayManager.MasuScaleXZ * ChargePow
            );

        // オセロ飛ばす
        OseroObj.Move(OseroGravity, ThrowingAngle, OseroPos, EndPos, OseroRotate);

        // チャージ終了
        NowChargeTime = 0.0f;
        ChargePow = 0.0f;
        NowReChargeTime = ReChargeTime;

        if (AnimatorObj != null)
            AnimatorObj.SetBool("isThrow", false);
    }

    // プレイヤーの移動座標の固定処理
    private void PlayerMoveMax()
    {
        // 移動制限
        Vector3 Pos = gameObject.transform.position; // 位置

        Vector3 MaxPos = FloorManager.Instance.PlayerMoveMaxObj.PlayerMoveMaxScale * 0.5f;

        MaxPos.x -= cd.bounds.size.x * 0.5f;
        MaxPos.y -= cd.bounds.size.y * 0.5f;
        MaxPos.z -= cd.bounds.size.z * 0.5f;

        Pos.x = Mathf.Clamp(Pos.x, -MaxPos.x, MaxPos.x);
        Pos.y = Mathf.Clamp(Pos.y, -MaxPos.y, MaxPos.y);
        Pos.z = Mathf.Clamp(Pos.z, -MaxPos.z, MaxPos.z);

        gameObject.transform.position = Pos;
    }

    // プレイヤーの移動処理
    private void PlayerMovePow()
    {
        // 力を加える処理
        Vector3 Vel = rb.velocity;          // ベロシティ
        float Pow = MaxMovePow / MovePow;   // 1秒間で加える量
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

        //スティックの入力が一定以上ない場合は反映されない　＆　なげてない
        if (Mathf.Abs(Vec.magnitude) > 0.6f && !AnimatorObj.GetBool("isThrow"))
        {
            Vec.Normalize();

            // 移動量加える
            Vector3 MoveVel = new Vector3(Vec.x, 0.0f, Vec.y) * Pow * Time.fixedDeltaTime;

            // 風なし
            if (BanmenManager.Instance.Gimmick_WindObj == null || BanmenManager.Instance.Gimmick_WindObj.WindInfo.Wind_Type == Gimmick_Wind.Enum_Wind_Type.None)
            {
                // 移動量増加
                Vel += MoveVel;

                // 最大移動量超えたら戻す
                if (Vel.x > MaxMovePow) Vel.x = MaxMovePow;
                if (Vel.x < -MaxMovePow) Vel.x = -MaxMovePow;
                if (Vel.z > MaxMovePow) Vel.z = MaxMovePow;
                if (Vel.z < -MaxMovePow) Vel.z = -MaxMovePow;
            }
            // 風あり
            else
            {
                //+風方向に力加える

                Gimmick_Wind.Wind_Info WindInfo = BanmenManager.Instance.Gimmick_WindObj.WindInfo;

                // 風移動増加量
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

                // 移動量増加
                Vel += MoveVel;

                // 風最大移動量
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

                // 最大移動量超えたら戻す
                if (Vel.x > MaxMove.x) Vel.x = MaxMove.x;
                if (Vel.x < -MaxMove.x) Vel.x = -MaxMove.x;
                if (Vel.z > MaxMove.z) Vel.z = MaxMove.z;
                if (Vel.z < -MaxMove.z) Vel.z = -MaxMove.z;
            }

            rb.velocity = Vel; // 移動量変更
        }
        // 入力していなかったら
        else
        {
            PlayerMoveDown();
        }
    }

    // プレイヤーの移動量減少処理
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

    //// ギミックの力によるプレイヤーの移動
    //private void WindPlayerMove()
    //{

    //}


    // プレイヤーのオセロのタイプセット
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
