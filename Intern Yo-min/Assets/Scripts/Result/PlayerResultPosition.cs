using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResultPosition : MonoBehaviour
{
    private Vector3 PlayerPosMin;
    private Vector3 PlayerPosMax;

    //private void OnValidate()
    //{
    //    Player = GetComponentsInChildren<ResultPlayer>();

    //    if (Player.Length <= 1)
    //        return;

    //    for (int i = 0; i < Player.Length; i++)
    //    {
    //        Player[i].gameObject.transform.position = PlayerPosMin + (PlayerPosMax - PlayerPosMin) * ((float)i / (Player.Length - 1));
    //        //Debug.Log(PlayerPosMin + (PlayerPosMax - PlayerPosMin) * ((float)i / (Player.Length - 1)));
    //    }
    //}


    private void Start()
    {
        ResultPlayer[] Players = ResultManager.Instance.ResultPlayers;

        foreach (var Obj in Players)
        {
            Obj.gameObject.SetActive(false);
        }

        for (int i = 0; i < GamePlayManager.Instance.Players.Length; i++)
        {
            Players[i].gameObject.SetActive(true);
        }


        if (GamePlayManager.Instance.Players.Length <= 1)
            return;

        switch (GamePlayManager.Instance.Players.Length)
        {
            case 2:
                PlayerPosMin = new Vector3(-8, 0, 0);
                PlayerPosMax = new Vector3(8, 0, 0);
                break;
            case 3:
                PlayerPosMin = new Vector3(-10, 0, 0);
                PlayerPosMax = new Vector3(10, 0, 0);
                break;
            case 4:
                PlayerPosMin = new Vector3(-12, 0, 0);
                PlayerPosMax = new Vector3(12, 0, 0);
                break;
            default:
                break;
        }

        for (int i = 0; i < GamePlayManager.Instance.Players.Length; i++)
        {
            Players[i].gameObject.transform.position = PlayerPosMin + (PlayerPosMax - PlayerPosMin) * ((float)i / (GamePlayManager.Instance.Players.Length - 1));
            ResultManager.Instance.Result_UI_Oseros[i].SetPosition(Players[i].gameObject.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
