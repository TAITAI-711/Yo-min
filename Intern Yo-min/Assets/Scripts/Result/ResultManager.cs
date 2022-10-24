using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : SingletonMonoBehaviour<ResultManager>
{
    public ResultPlayer[] ResultPlayers;
    public Result_UI_Osero[] Result_UI_Oseros;
    public ResultTimeCount ResultTime;

    public bool isCountStop = false;
    public bool isPause = false;


    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        ResultPlayers = GetComponentsInChildren<ResultPlayer>();
        Result_UI_Oseros = GetComponentsInChildren<Result_UI_Osero>();
        ResultTime = GetComponentInChildren<ResultTimeCount>();

        for (int i = 0; i < GamePlayManager.Instance.Players.Length; i++)
        {
            ResultPlayers[i].SetMaterial(GamePlayManager.Instance.Players[i].PlayerOseroType.PlayerMaterial);
            Result_UI_Oseros[i].SetSprite(GamePlayManager.Instance.Players[i].PlayerOseroType.UI_OseroImage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
