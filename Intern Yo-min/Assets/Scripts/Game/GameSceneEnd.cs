using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneEnd : MonoBehaviour
{
    private bool isOnce = false;

    [SerializeField] private float NextSceneTime = 3.0f;
    private float NowTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        NowTime = NextSceneTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GamePlayManager.Instance.isGameEnd)
        {
            NowTime -= Time.fixedDeltaTime;

            if (!isOnce && NowTime <= 0.0f)
            {
                isOnce = true;

                GamePlayManager.Instance.PlayerOseroNumSet();

                SceneChangeManager.Instance.SceneChange("ResultScene", true);
            }
        }
    }
}
