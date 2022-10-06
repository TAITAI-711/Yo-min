using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masu : MonoBehaviour
{
    public Osero OseroObj = null;
    public Banmen BanmenObj = null;

    public Vector2Int MasuXY = new Vector2Int(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        OseroObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOsero(Osero osero)
    {
        if (OseroObj == null)
        {
            OseroObj = osero;
            OseroObj.OseroSet(transform.position);
        }
        else
        {
            Debug.Log("入れ替え：" + osero.GetOseroType());
            OseroObj.SetOseroType(osero.GetOseroType());
            Destroy(osero.gameObject);
            //OseroObj = osero;
            //OseroObj.OseroSet(transform.position);
        }

        // オセロをひっくり返す処理
        PlayerMove.EnumOseroType MyOseroType = OseroObj.GetOseroType();

        //Debug.Log(MasuXY);

        // 右方向
        for (int i = MasuXY.x + 1; i < BanmenObj.Yoko; i++)
        {
            if (BanmenObj.Masu[i, MasuXY.y].OseroObj == null)
            {
                Debug.Log("右ない");
                break;
            }
            else if (BanmenObj.Masu[i, MasuXY.y].OseroObj.GetOseroType() == MyOseroType)
            {
                for (int Turn_i = MasuXY.x + 1; Turn_i < i; Turn_i++)
                {
                    BanmenObj.Masu[Turn_i, MasuXY.y].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }

        // 左方向
        for (int i = MasuXY.x - 1; i >= 0; i--)
        {
            if (BanmenObj.Masu[i, MasuXY.y].OseroObj == null)
            {
                Debug.Log("左ない");
                break;
            }
            else if (BanmenObj.Masu[i, MasuXY.y].OseroObj.GetOseroType() == MyOseroType)
            {
                for (int Turn_i = MasuXY.x - 1; Turn_i > i; Turn_i--)
                {
                    BanmenObj.Masu[Turn_i, MasuXY.y].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }

        // 下方向
        for (int j = MasuXY.y + 1; j < BanmenObj.Tate; j++)
        {
            if (BanmenObj.Masu[MasuXY.x, j].OseroObj == null)
            {
                Debug.Log("下ない");
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x, j].OseroObj.GetOseroType() == MyOseroType)
            {
                for (int Turn_j = MasuXY.y + 1; Turn_j < j; Turn_j++)
                {
                    BanmenObj.Masu[MasuXY.x, Turn_j].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }

        // 上方向
        for (int j = MasuXY.y - 1; j >= 0; j--)
        {
            if (BanmenObj.Masu[MasuXY.x, j].OseroObj == null)
            {
                Debug.Log("上ない");
                Debug.Log(BanmenObj.Masu[MasuXY.x, j].MasuXY);
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x, j].OseroObj.GetOseroType() == MyOseroType)
            {
                Debug.Log(BanmenObj.Masu[MasuXY.x, j].MasuXY + "入れ替え");
                for (int Turn_j = MasuXY.y - 1; Turn_j > j; Turn_j--)
                {
                    BanmenObj.Masu[MasuXY.x, Turn_j].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }


    }
}
