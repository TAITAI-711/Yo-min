using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banmen_Move : Banmen
{
    [Header("[ ˆÚ“®”Õ–Êİ’è ]")]

    [Tooltip("Å‰‚É‰E‚É“®‚­")]
    public bool isMoveRight = false;

    [Tooltip("ˆÚ“®ŠÔ")]
    public float MoveTime = 3.0f;

    [Tooltip("’â~ŠÔ")]
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
