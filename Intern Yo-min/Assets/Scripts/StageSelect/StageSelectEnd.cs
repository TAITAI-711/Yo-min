using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectEnd : MonoBehaviour
{
    private bool isOnce = false;

    private void FixedUpdate()
    {
        if (!isOnce && StageSelectManager.Instance.isSelectSceneEnd)
        {
            isOnce = true;

            SceneChangeManager.Instance.SceneChange(StageSelectManager.Instance.NextSceneName, true);
        }
    }
}
