using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class Banmen : MonoBehaviour
{
    [SerializeField] private GameObject MasuPrefab;

    [Range(1, 20)] public int Tate = 9;
    [Range(1, 20)] public int Yoko = 9;

    [HideInInspector] public float TateLength;
    [HideInInspector] public float YokoLength;

    public Masu[,] Masu;

    protected virtual void OnValidate()
    {
        gameObject.transform.localScale = new Vector3(
            Yoko * GamePlayManager.MasuScaleXZ, 
            GamePlayManager.MasuScaleY, 
            Tate * GamePlayManager.MasuScaleXZ);

        Vector3 Pos = transform.position;
        Pos.y = GamePlayManager.MasuScaleY * 0.5f;
        gameObject.transform.position = Pos;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Yoko = Tate;

        Masu = new Masu[Yoko, Tate];

        // 計算用変数

        // 開始位置(X, Z)
        Vector3 StartPos = new Vector3(
            gameObject.transform.position.x + -gameObject.transform.localScale.x * 0.5f, 
            gameObject.transform.position.y + -GamePlayManager.MasuScaleY * 0.5f, 
            gameObject.transform.position.z + gameObject.transform.localScale.z * 0.5f);
        TateLength = gameObject.transform.localScale.z / (float)Tate;
        YokoLength = gameObject.transform.localScale.x / (float)Yoko;


        for (int i = 0; i < Yoko; i++)
        {
            for (int j = 0; j < Tate; j++)
            {
                // マス目生成
                GameObject Obj = Instantiate(MasuPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                Masu[i, j] = Obj.GetComponent<Masu>();
                Masu[i, j].BanmenObj = this;
                Masu[i, j].MasuXY = new Vector2Int(i, j);
                Masu[i, j].SetParent(this.transform);

                // サイズ設定
                //Vector3 MasuSize = Masu[i, j].transform.localScale;
                //MasuSize.x = 1.0f;
                //MasuSize.y = 1.0f;
                //MasuSize.z = 1.0f;
                //Masu[i, j].transform.localScale = MasuSize;

                // 位置設定
                Vector3 MasuPos = Masu[i, j].transform.position;
                MasuPos.x = StartPos.x + (YokoLength * (i + 1) - YokoLength * 0.5f);
                MasuPos.y = StartPos.y;
                MasuPos.z = StartPos.z + -(TateLength * (j + 1) - TateLength * 0.5f);
                Masu[i, j].transform.position = MasuPos;
            }
        }


        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Masu GetMasu(Vector3 Pos)
    {
        int X = 0;
        int Y = 0;

        for(int i = 0; i < Yoko; i++)
        {
            if (Pos.x < Masu[i, 0].transform.position.x + YokoLength * 0.5f)
            {
                X = i;
                break;
            }
            X = Yoko - 1;
        }

        for (int j = 0; j < Tate; j++)
        {
            if (Pos.z > Masu[0, j].transform.position.z - TateLength * 0.5f)
            {
                Y = j;
                break;
            }
            Y = Tate - 1;
        }

        return Masu[X, Y];
    }

    public int GetOseroNum(GamePlayManager.EnumOseroType oseroType)
    {
        int Num = 0;
        foreach (var obj in Masu)
        {   
            if(obj.OseroObj != null)
            {
                if (obj.OseroObj.GetOseroType().OseroType == oseroType)
                {
                    Num++;
                }
            }           
        }
        return Num;
    }
}
