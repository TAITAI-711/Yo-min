using Unity.Collections;
using UnityEngine;

public class StageSelectManager : SingletonMonoBehaviour<StageSelectManager>
{
    public bool isStageSelectEnd = false;
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

    private void Start()
    {
        
    }


    private void Update()
    {
        if (GamePlayManager.Instance.isPause || !isStageSelectEnd)
            return;

        if (!isOnce && Input.GetButtonDown(GamePlayManager.Instance.Players[0].GamePadName_Player + "_Button_Start"))
        {
            isOnce = true;

            GamePlayManager.Instance.isGamePadOK = true;

            SoundManager.Instance.PlaySound("Œˆ’è", false);

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
