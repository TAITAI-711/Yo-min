using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ResultTimeCount : MonoBehaviour
{
    [SerializeField] private float CountUpTime = 4.7f;    // カウントアップする秒数
    [ReadOnly] private float NowTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        NowTime = CountUpTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FadeManager.GetNowState() != FADE_STATE.FADE_NONE)
            return;

        // 最初の一回
        if (NowTime >= CountUpTime)
        {
            SoundManager.Instance.PlaySound("リザルトカウントアップ", false);
        }

        if (NowTime > 0.0f)
        {
            NowTime -= Time.fixedDeltaTime;

            if (NowTime <= 0.0f)
            {
                NowTime = 0.0f;
                SoundManager.Instance.PlaySound("リザルト順位決定", false);
            }
        }
    }

    public float GetNowTimeRate()
    {
        return 1.0f - (NowTime / CountUpTime);
    }
}
