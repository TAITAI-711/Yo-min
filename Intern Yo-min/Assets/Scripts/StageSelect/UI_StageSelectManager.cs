using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StageSelectManager : SingletonMonoBehaviour<UI_StageSelectManager>
{
    private UI_Stage[] UI_StageObj;
    private Button[] UI_ButtonObj;

    private int NowSelectNum = 0;
    private int OldSelectNum = 0;

    private float CountTime = 0.1f;
    private float NowTime = 0.0f;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        UI_StageObj = gameObject.GetComponentsInChildren<UI_Stage>();
        UI_ButtonObj = gameObject.GetComponentsInChildren<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < UI_StageObj.Length; i++)
        {
            if (i == 0)
                UI_StageObj[i].gameObject.transform.localPosition = Vector3.zero;
            else
                UI_StageObj[i].gameObject.transform.localPosition = new Vector3(2000, 0, 0);
        }

        // 真ん中のボタンを選択
        EventSystemManager.Instance.EventSystemObj.SetSelectedGameObject(UI_ButtonObj[2].gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (NowTime <= 0.0f)
        {
            // 右を選択
            if (EventSystemManager.Instance.EventSystemObj.currentSelectedGameObject == UI_ButtonObj[0].gameObject)
            {
                if (NowSelectNum < UI_StageObj.Length - 1)
                    NowSelectNum++;

                NowTime = CountTime;
            }

            // 左を選択
            if (EventSystemManager.Instance.EventSystemObj.currentSelectedGameObject == UI_ButtonObj[1].gameObject)
            {
                if (NowSelectNum > 0)
                    NowSelectNum--;

                NowTime = CountTime;
            }
        }
        else
        {
            NowTime -= Time.deltaTime;

            if (NowTime <= 0.0f)
            {
                // 真ん中のボタンを選択
                EventSystemManager.Instance.EventSystemObj.SetSelectedGameObject(UI_ButtonObj[2].gameObject);
            }
        }
        

        // 選択中のステージ情報に切り替わる
        if (NowSelectNum != OldSelectNum)
        {
            UI_StageObj[NowSelectNum].gameObject.transform.localPosition = Vector3.zero;
            UI_StageObj[OldSelectNum].gameObject.transform.localPosition = new Vector3(2000, 0, 0);

            OldSelectNum = NowSelectNum;
        }

        // ステージ選択
        if (Input.GetButtonDown(GamePlayManager.Instance.MenuSelectPlayerName + "_Button_B"))
        {
            StageSelectManager.Instance.NextSceneName = UI_StageObj[NowSelectNum].StageSceneName;

            GamePlayManager.Instance.isGamePlay = true;

            StageSelectManager.Instance.isStageSelect = true;
        }
    }
}
