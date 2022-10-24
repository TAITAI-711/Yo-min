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
        [ReadOnly] public Enum_Wind_Type Wind_Type = Enum_Wind_Type.None;   // ������
        public float PlayerMovePowUp = 1.25f;// �v���C���[�ւ̉e����(���t���[���̈ړ��� �~�{)
        public float PlayerMovePowDown = 0.75f;// �v���C���[�ւ̉e����(���t���[���̈ړ��ʁ@�~�{)
        public float OseroMovePow = 30.0f;// �I�Z���ւ̉e����(���t���[���̈ړ��ʁ@�l)
        [HideInInspector] public Vector3 WindVec = Vector3.zero;    // ���̃x�N�g��
    }


    [SerializeField] private float WindChangeTime = 30.0f;

    //private float OseroMovePow = 2.0f;  // �I�Z���ւ̉e����(�}�X)

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

        Calculation.Shuffle(Wind_Type_Next); // �V���b�t��

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
            Calculation.Shuffle(Wind_Type_Next); // �V���b�t��
        }

        // �������̃x�N�g������
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
