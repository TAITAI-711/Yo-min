using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OseroPanel : MonoBehaviour
{
    public Vector2 LeftPos;
    public Vector2 RightPos;


    // オセロUIの整列処理（インスペクター上での変更時）
    private void OnValidate()
    {
        UI_Osero[] UI = GetComponentsInChildren<UI_Osero>();

        if (UI.Length > 1)
        {
            for (int i = 0; i < UI.Length; i++)
            {
                Vector3 Pos = UI[i].transform.localPosition;

                Pos.x = LeftPos.x + ((float)(RightPos.x - LeftPos.x) / (UI.Length - 1)) * i;
                Pos.y = LeftPos.y + ((float)(RightPos.y - LeftPos.y) / (UI.Length - 1)) * i;

                UI[i].transform.localPosition = Pos;
            }
        }
    }

    // オセロUIの整列処理
    public void SetUIPanel(int PlayerNum)
    {
        for (int i = 0; i < PlayerNum; i++)
        {
            Vector3 Pos = GamePlayManager.Instance.PlayerManagerObj.UI_OseroObj[i].transform.localPosition;

            Pos.x = LeftPos.x + ((float)(RightPos.x - LeftPos.x) / (PlayerNum - 1)) * i;
            Pos.y = LeftPos.y + ((float)(RightPos.y - LeftPos.y) / (PlayerNum - 1)) * i;

            GamePlayManager.Instance.PlayerManagerObj.UI_OseroObj[i].transform.localPosition = Pos;
        }
    }   
}
