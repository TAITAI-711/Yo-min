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
    [Header("[ プレイヤー設定 ]")]
    [Tooltip("プレイヤーのコントローラー番号")]
    [SerializeField]
    EnumPlayerType PlayerType = EnumPlayerType.Player1;

    public enum EnumOseroType
    {
        White,
        Black,
        Blue,
        Red
    }
    [Tooltip("プレイヤーのオセロの色")]
    [SerializeField]
    EnumOseroType OseroType = EnumOseroType.White;

    [Tooltip("プレイヤーの移動量"), Range(1F, 30F)]
    public float MovePow = 1.0f;
    [Tooltip("プレイヤーの最大移動量"), Range(10F, 100F)]
    public float MaxMovePow = 10.0f;
    [Tooltip("プレイヤーの移動量減少値(空気抵抗値)"), Range(0.1F, 20.0F)]
    public float DownMovePow = 0.1f;


    [Header("[ オセロ設定 ]")]
    [Tooltip("オセロの最大飛距離マス数"), Range(1F, 9F)] 
    public float MaxOseroMove = 3.0f;    // オセロの最大飛距離マス数
    [Tooltip("オセロの重力"), Range(1F, 100F)] 
    public float OseroGravity = 9.8f;    // オセロの重力

    [SerializeField, Range(5F, 85F), Tooltip("射出する角度")]
    private float ThrowingAngle;

    private Rigidbody rb;

    // オセロの大きさ少し小さくする用変数
    private float OseroScaleDown = 0.2f;


    // 最大移動範囲用変数
    private float MaxMovePosX = 0;
    private float MaxMovePosZ = 0;

    // プレイヤーの向き
    private Vector2 PlayerAngle;

    // 入力キーの一回判定用
    private bool isKeyDown = false;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = DownMovePow;

        // 盤面取得
        BanmenTf = BanmenObj.GetComponent<Transform>();

        // 最大移動距離
        MaxMovePosX = BanmenTf.position.x + BanmenTf.localScale.x * 0.5f + 
            gameObject.transform.localScale.x * 0.5f + 0.1f; // 0.1f分余裕もたせる

        MaxMovePosZ = BanmenTf.position.z + BanmenTf.localScale.z * 0.5f + 
            gameObject.transform.localScale.z * 0.5f + 0.1f; // 0.1f分余裕もたせる

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

        // 初期のプレイヤーの向き
        PlayerAngle = BanmenObj.transform.position - transform.position;
        PlayerAngle = PlayerAngle.normalized;
    }

    // Update is called once per frame
    void Update()
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

    // プレイヤーがオセロを飛ばす向きの処理
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

            // オセロの生成座標
            Vector3 OseroPos = transform.position;
            OseroPos.y += 5.0f;

            // オセロ生成
            GameObject osero = Instantiate(OseroPrefab, OseroPos, Quaternion.identity);

            // 色設定
            osero.GetComponent<Osero>().SetOseroType(OseroType);

            // サイズ設定
            Vector3 OseroSize = osero.transform.localScale;
            OseroSize.x = BanmenObj.YokoLength - OseroScaleDown;
            OseroSize.y = 1.0f;
            OseroSize.z = BanmenObj.TateLength - OseroScaleDown;
            osero.transform.localScale = OseroSize;

            // 着地座標
            Vector3 EndPos = new Vector3(OseroPos.x + PlayerAngle.x * MaxOseroMove * BanmenObj.YokoLength, 0.0f, OseroPos.z + PlayerAngle.y * MaxOseroMove * BanmenObj.TateLength);

            osero.GetComponent<Osero>().Move(BanmenObj, OseroGravity, ThrowingAngle, OseroPos, EndPos);
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
        Vector3 Vel = rb.velocity; // ベロシティ
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

        // 移動量加える
        Vel.x += Pow * Vec.x;
        Vel.z += Pow * Vec.y;

        // 最大移動量超えたら戻す
        if (Vel.x > MaxMovePow) Vel.x = MaxMovePow;
        if (Vel.x < -MaxMovePow) Vel.x = -MaxMovePow;
        if (Vel.z > MaxMovePow) Vel.z = MaxMovePow;
        if (Vel.z < -MaxMovePow) Vel.z = -MaxMovePow;

        rb.velocity = Vel; // 移動量変更
    }
}
