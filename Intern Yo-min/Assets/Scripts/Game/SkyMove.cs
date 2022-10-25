using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMove : MonoBehaviour
{
    private RectTransform Rt;

    Vector3 StartPos;

    public float MoveTime = 30.0f;
    float NowTime = 0.0f;

    public float Width = 300.0f;

    private void Awake()
    {
        Rt = gameObject.GetComponent<RectTransform>();

        StartPos = Rt.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 Pos = Rt.localPosition;

        //Pos.x = StartPos.x + Width * NowTime / MoveTime;
        Pos.y = StartPos.y + -Width * NowTime / MoveTime;

        Rt.localPosition = Pos;

        NowTime += Time.fixedDeltaTime;

        if (NowTime >= MoveTime)
        {
            NowTime = 0.0f;

            Rt.localPosition = StartPos;
        }
    }
}
