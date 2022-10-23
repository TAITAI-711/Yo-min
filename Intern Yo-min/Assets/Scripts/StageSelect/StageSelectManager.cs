using Unity.Collections;
using UnityEngine;

public class StageSelectManager : SingletonMonoBehaviour<StageSelectManager>
{
    public bool isSelectSceneEnd = false;
    public bool isStageSelect = false;
    public string NextSceneName = "TitleScene";

    private bool OldisStageSelect = false;

    private bool isOnce = false;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }


    private void Update()
    {
        if (GamePlayManager.Instance.isPause)
            return;

        if (!isOnce && GamePlayManager.Instance.Players != null && 
            GamePlayManager.Instance.Players.Length >= 2 && 
            Input.GetButtonDown(GamePlayManager.Instance.Players[0].GamePadName_Player + "_Button_Start"))
        {
            isOnce = true;

            GamePlayManager.Instance.isGamePadOK = true;

            SceneChangeManager.Instance.SceneChange(NextSceneName, true);
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
