using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banmen_Move : Banmen
{
    [Header("[ �ړ��Ֆʐݒ� ]")]

    [Tooltip("�ŏ��ɉE�ɓ���")]
    public bool isMoveRight = false;

    [Tooltip("�ړ�����")]
    public float MoveTime = 3.0f;

    [Tooltip("��~����")]
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
