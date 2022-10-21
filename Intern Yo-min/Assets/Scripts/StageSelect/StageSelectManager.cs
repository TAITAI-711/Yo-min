using Unity.Collections;
using UnityEngine;

public class StageSelectManager : SingletonMonoBehaviour<StageSelectManager>
{
    public bool isSelectSceneEnd = false;
    public bool isStageSelect = false;
    public string NextSceneName = "TitleScene";

    private bool OldisStageSelect = false;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }


    private void FixedUpdate()
    {
        if (!OldisStageSelect && isStageSelect)
        {
            UI_StageSelectManager.Instance.gameObject.SetActive(false);
        }
        else if (OldisStageSelect && !isStageSelect)
        {
            UI_StageSelectManager.Instance.gameObject.SetActive(true);
        }

        OldisStageSelect = isStageSelect;
    }
}
