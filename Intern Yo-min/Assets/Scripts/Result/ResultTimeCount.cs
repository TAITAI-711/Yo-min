using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ResultTimeCount : MonoBehaviour
{
    [SerializeField] private float CountUpTime = 5.0f;    // カウントアップする秒数
    [ReadOnly] private float NowTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        NowTime = CountUpTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (NowTime > 0.0f)
        {
            NowTime -= Time.fixedDeltaTime;

            if (NowTime <= 0.0f)
            {
                NowTime = 0.0f;
            }
        }
    }

    public float GetNowTimeRate()
    {
        return 1.0f - (NowTime / CountUpTime);
    }
}
