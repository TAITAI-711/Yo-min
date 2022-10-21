using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : SingletonMonoBehaviour<StageSelectManager>
{
    public bool isSelectSceneEnd = false;
    public bool isStageSelect = false;
    public string NextSceneName = "TitleScene";

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
