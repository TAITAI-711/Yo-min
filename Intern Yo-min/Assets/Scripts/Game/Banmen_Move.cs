using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banmen_Move : Banmen
{
    [Header("[ 移動盤面設定 ]")]

    [Tooltip("最初に右に動く")]
    public bool isMoveRight = false;

    [Tooltip("移動時間")]
    public float MoveTime = 3.0f;

    [Tooltip("停止時間")]
    public float StopTime = 5.0f;

    private float NowStopTime = 0.0f;

    public GameObject MovePointObj = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (NowStopTime <= 0.0f)
        {
            if (!isMoveRight)
            {

            }
        }
        else
        {
            NowStopTime -= Time.fixedDeltaTime;
        }
    }
}
