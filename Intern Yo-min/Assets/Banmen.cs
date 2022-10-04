using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banmen : MonoBehaviour
{
    [SerializeField] private GameObject MasuPrefab;

    public int Tate = 9;
    public int Yoko = 9;

    [HideInInspector] public float TateLength;
    [HideInInspector] public float YokoLength;

    private GameObject[,] Masu;
    // Start is called before the first frame update
    void Start()
    {
        Masu = new GameObject[Yoko, Tate];

        // 計算用変数

        // 開始位置(X, Z)
        Vector3 StartPos = new Vector3(gameObject.transform.position.x + -gameObject.transform.localScale.x * 0.5f, gameObject.transform.position.y + gameObject.transform.localScale.y * 0.5f, gameObject.transform.position.z + gameObject.transform.localScale.z * 0.5f);
        TateLength = gameObject.transform.localScale.z / (float)Tate;
        YokoLength = gameObject.transform.localScale.x / (float)Yoko;


        for (int i = 0; i < Yoko; i++)
        {
            for (int j = 0; j < Tate; j++)
            {
                // マス目生成
                Masu[i, j] = Instantiate(MasuPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

                // サイズ設定
                Vector3 MasuSize = Masu[i, j].transform.localScale;
                MasuSize.x = YokoLength;
                MasuSize.y = 1.0f;
                MasuSize.z = TateLength;
                Masu[i, j].transform.localScale = MasuSize;

                // 位置設定
                Vector3 MasuPos = Masu[i, j].transform.position;
                MasuPos.x = StartPos.x + (YokoLength * (i + 1) - YokoLength * 0.5f);
                MasuPos.y = StartPos.y + 0.5f;
                MasuPos.z = StartPos.z + -(TateLength * (j + 1) - TateLength * 0.5f);
                Masu[i, j].transform.position = MasuPos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetMasu(Vector3 Pos)
    {
        int X = 0;
        int Y = 0;

        for(int i = 0; i < Yoko; i++)
        {
            if (Pos.x < Masu[i, 0].transform.position.x + YokoLength)
            {
                X = i;
                break;
            }
            X = Yoko - 1;
        }

        for (int j = 0; j < Tate; j++)
        {
            if (Pos.z < Masu[0, j].transform.position.z + TateLength)
            {
                Y = j;
                break;
            }
            Y = Tate - 1;
        }

        return Masu[X, Y];
    }
}
