using Unity.Collections;
using UnityEngine;

public class Gimmick_Wind : MonoBehaviour
{
    public enum Enum_Wind_Type
    {
        None = 0,
        Up,
        Down,
        Right,
        Left,
        Max
    }

    [System.Serializable]
    public class Wind_Info
    {
        [ReadOnly] public Enum_Wind_Type Wind_Type = Enum_Wind_Type.None;   // 風向き
        public float PlayerMovePowUp = 1.25f;// プレイヤーへの影響量(毎フレームの移動量 ×倍)
        public float PlayerMovePowDown = 0.75f;// プレイヤーへの影響量(毎フレームの移動量　×倍)
        public float OseroMovePow = 30.0f;// オセロへの影響量(毎フレームの移動量　値)
        [HideInInspector] public Vector3 WindVec = Vector3.zero;    // 風のベクトル
    }


    [SerializeField] private float WindChangeTime = 30.0f;

    //private float OseroMovePow = 2.0f;  // オセロへの影響量(マス)

    [SerializeField] public Wind_Info WindInfo;
    private float OldTime;

    private Enum_Wind_Type[] Wind_Type_Next;

    private int WindCount = 0;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        OldTime = PlayerManager.Instance.UI_GameTimeObj.UI_TimeObj.NowTime;
        WindCount = 0;

        Wind_Type_Next = new Enum_Wind_Type[(int)Enum_Wind_Type.Max - 1] 
        { Enum_Wind_Type.Up, Enum_Wind_Type.Down, Enum_Wind_Type.Right, Enum_Wind_Type.Left};

        Calculation.Shuffle(Wind_Type_Next); // シャッフル

        SetWind();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GamePlayManager.Instance.isGamePlay)
            return;


        if (OldTime - WindChangeTime >= PlayerManager.Instance.UI_GameTimeObj.UI_TimeObj.NowTime)
        {
            OldTime = PlayerManager.Instance.UI_GameTimeObj.UI_TimeObj.NowTime;
            SetWind();
        }
    }

    private void SetWind()
    {
        WindInfo.Wind_Type = Wind_Type_Next[WindCount];
        WindCount++;

        if (WindCount >= Wind_Type_Next.Length)
        {
            WindCount = 0;
            Calculation.Shuffle(Wind_Type_Next); // シャッフル
        }

        // 風方向のベクトル入力
        switch (WindInfo.Wind_Type)
        {
            case Enum_Wind_Type.Up:
                WindInfo.WindVec = Vector3.forward;
                break;
            case Enum_Wind_Type.Down:
                WindInfo.WindVec = Vector3.back;
                break;
            case Enum_Wind_Type.Right:
                WindInfo.WindVec = Vector3.right;
                break;
            case Enum_Wind_Type.Left:
                WindInfo.WindVec = Vector3.left;
                break;
            case Enum_Wind_Type.None:
            case Enum_Wind_Type.Max:
            default:
                break;
        }
    }
}
