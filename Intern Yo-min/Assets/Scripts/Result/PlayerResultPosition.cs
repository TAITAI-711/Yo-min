using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResultPosition : MonoBehaviour
{
    [Header("テスト用のプレイヤー位置整列")]
    [SerializeField] private Vector3 PlayerPosMin;
    [SerializeField] private Vector3 PlayerPosMax;

    private ResultPlayer[] Player;

    private void OnValidate()
    {
        Player = GetComponentsInChildren<ResultPlayer>();

        if (Player.Length <= 1)
            return;

        for (int i = 0; i < Player.Length; i++)
        {
            Player[i].gameObject.transform.position = PlayerPosMin + (PlayerPosMax - PlayerPosMin) * ((float)i / (Player.Length - 1));
            //Debug.Log(PlayerPosMin + (PlayerPosMax - PlayerPosMin) * ((float)i / (Player.Length - 1)));
        }
    }


    private void Awake()
    {
        Player = GetComponentsInChildren<ResultPlayer>();

        if (Player.Length <= 1)
            return;

        switch (Player.Length)
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

        for (int i = 0; i < Player.Length; i++)
        {
            Player[i].gameObject.transform.position = PlayerPosMin + (PlayerPosMax - PlayerPosMin) * ((float)i / (Player.Length - 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
