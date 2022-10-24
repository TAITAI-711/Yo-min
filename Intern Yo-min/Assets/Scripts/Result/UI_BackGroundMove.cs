using UnityEngine;
using UnityEngine.UI;

public class UI_BackGroundMove : MonoBehaviour
{
    RectTransform Rt;

    Vector3 StartPos;

    float MoveTime = 10.0f;
    float NowTime = 0.0f;

    private void Start()
    {
        Rt = gameObject.GetComponent<RectTransform>();

        StartPos = Rt.localPosition;
    }

    void FixedUpdate()
    {
        Vector3 Pos = Rt.localPosition;

        Pos.x = StartPos.x + 1920.0f * NowTime / MoveTime;
        Pos.y = StartPos.y + -1080.0f * NowTime / MoveTime;

        Rt.localPosition = Pos;

        NowTime += Time.fixedDeltaTime;

        if (NowTime >= MoveTime)
        {
            NowTime = 0.0f;

            Rt.localPosition = StartPos;
        }
    }
}
