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
            OseroObj.OseroSet(transform.position + new Vector3(0, GamePlayManager.MasuScaleY + OseroObj.transform.localScale.y, 0));
        }
        else
        {
            //Debug.Log("����ւ��F" + osero.GetOseroType());
            OseroObj.SetOseroType(osero.GetOseroType());
            Destroy(osero.gameObject);
            //OseroObj = osero;
            //OseroObj.OseroSet(transform.position);
        }

        // �I�Z�����Ђ�����Ԃ�����
        PlayerManager.PlayerOseroTypeInfo MyOseroType = OseroObj.GetOseroType();

        //Debug.Log(MasuXY);

        // �E����
        for (int i = MasuXY.x + 1; i < BanmenObj.Yoko; i++)
        {
            if (BanmenObj.Masu[i, MasuXY.y].OseroObj == null)
            {
                //Debug.Log("�E�Ȃ�");
                break;
            }
            else if (BanmenObj.Masu[i, MasuXY.y].OseroObj.GetOseroType().OseroType == MyOseroType.OseroType)
            {
                for (int Turn_i = MasuXY.x + 1; Turn_i < i; Turn_i++)
                {
                    BanmenObj.Masu[Turn_i, MasuXY.y].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }

        // ������
        for (int i = MasuXY.x - 1; i >= 0; i--)
        {
            if (BanmenObj.Masu[i, MasuXY.y].OseroObj == null)
            {
                //Debug.Log("���Ȃ�");
                break;
            }
            else if (BanmenObj.Masu[i, MasuXY.y].OseroObj.GetOseroType().OseroType == MyOseroType.OseroType)
            {
                for (int Turn_i = MasuXY.x - 1; Turn_i > i; Turn_i--)
                {
                    BanmenObj.Masu[Turn_i, MasuXY.y].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }

        // ������
        for (int j = MasuXY.y + 1; j < BanmenObj.Tate; j++)
        {
            if (BanmenObj.Masu[MasuXY.x, j].OseroObj == null)
            {
                //Debug.Log("���Ȃ�");
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x, j].OseroObj.GetOseroType().OseroType == MyOseroType.OseroType)
            {
                for (int Turn_j = MasuXY.y + 1; Turn_j < j; Turn_j++)
                {
                    BanmenObj.Masu[MasuXY.x, Turn_j].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }

        // �����
        for (int j = MasuXY.y - 1; j >= 0; j--)
        {
            if (BanmenObj.Masu[MasuXY.x, j].OseroObj == null)
            {
                //Debug.Log("��Ȃ�");
                //Debug.Log(BanmenObj.Masu[MasuXY.x, j].MasuXY);
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x, j].OseroObj.GetOseroType().OseroType == MyOseroType.OseroType)
            {
                //Debug.Log(BanmenObj.Masu[MasuXY.x, j].MasuXY + "����ւ�");
                for (int Turn_j = MasuXY.y - 1; Turn_j > j; Turn_j--)
                {
                    BanmenObj.Masu[MasuXY.x, Turn_j].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }

        // �΂߉E�����
        for (int i = 1; ; i++)
        {
            //Debug.Log("�J��Ԃ����E��");
            if (MasuXY.x + i > BanmenObj.Yoko - 1 || MasuXY.y - i < 0)
            {
                //Debug.Log("�������͈͊O");
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x + i, MasuXY.y - i].OseroObj == null)
            {
                //Debug.Log("������null");
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x + i, MasuXY.y - i].OseroObj.GetOseroType().OseroType == MyOseroType.OseroType)
            {
                for (int j = 1; j < i; j++)
                {
                    BanmenObj.Masu[MasuXY.x + j, MasuXY.y - j].OseroObj.SetOseroType(MyOseroType);
                }
                //Debug.Log("�����������R�}");
                break;
            }
        }
        //Debug.Log("�J��Ԃ��I���");

        // �΂߉E������
        for (int i = 1; ; i++)
        {
            if (MasuXY.x + i > BanmenObj.Yoko - 1 || MasuXY.y + i > BanmenObj.Tate - 1)
            {
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x + i, MasuXY.y + i].OseroObj == null)
            {
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x + i, MasuXY.y + i].OseroObj.GetOseroType().OseroType == MyOseroType.OseroType)
            {
                for (int j = 1; j < i; j++)
                {
                    BanmenObj.Masu[MasuXY.x + j, MasuXY.y + j].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }

        // �΂ߍ������
        for (int i = 1; ; i++)
        {
            if (MasuXY.x - i < 0 || MasuXY.y - i < 0)
            {
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x - i, MasuXY.y - i].OseroObj == null)
            {
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x - i, MasuXY.y - i].OseroObj.GetOseroType().OseroType == MyOseroType.OseroType)
            {
                for (int j = 1; j < i; j++)
                {
                    BanmenObj.Masu[MasuXY.x - j, MasuXY.y - j].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }

        // �΂ߍ�������
        for (int i = 1; ; i++)
        {
            if (MasuXY.x - i < 0 || MasuXY.y + i > BanmenObj.Tate - 1)
            {
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x - i, MasuXY.y + i].OseroObj == null)
            {
                break;
            }
            else if (BanmenObj.Masu[MasuXY.x - i, MasuXY.y + i].OseroObj.GetOseroType().OseroType == MyOseroType.OseroType)
            {
                for (int j = 1; j < i; j++)
                {
                    BanmenObj.Masu[MasuXY.x - j, MasuXY.y + j].OseroObj.SetOseroType(MyOseroType);
                }
                break;
            }
        }
    }
}
