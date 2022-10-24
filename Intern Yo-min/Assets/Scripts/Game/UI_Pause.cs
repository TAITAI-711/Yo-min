using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Pause : MonoBehaviour
{
    [SerializeField] private GameObject UI_PausePanelObj;
    [SerializeField] private GameObject FirstSelectObj;

    protected void Awake()
    {
        if (UI_PausePanelObj == null)
            UI_PausePanelObj = GetComponentInChildren<GameObject>();

        UI_PausePanelObj.SetActive(false);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetButtonDown(GamePlayManager.Instance.MenuSelectPlayerName + "_Button_Select") && (GamePlayManager.Instance.isGamePlay || GamePlayManager.Instance.isGameEnd))
        {
            Pause();    // ポーズ処理
        }
    }


    // ポーズ処理
    protected void Pause()
    {
        //Debug.Log("ぽーずよばれた");

        SoundManager.Instance.PlaySound("ポップアップ", false, 0.1f);

        GamePlayManager.Instance.isPause = !GamePlayManager.Instance.isPause;

        if (GamePlayManager.Instance.isPause)
        {
            Time.timeScale = 0.0f;
            UI_PausePanelObj.SetActive(true);
            EventSystemManager.Instance.EventSystemObj.SetSelectedGameObject(FirstSelectObj);
        }
        else
        {
            Time.timeScale = 1.0f;
            EventSystemManager.Instance.EventSystemObj.SetSelectedGameObject(null);
            UI_PausePanelObj.SetActive(false);
        }
    }


    public void Button_UnPause()
    {
        Pause(); // ポーズ処理
    }

    public void Button_StageSelect()
    {
        SoundManager.Instance.PlaySound("決定", false);
        SceneChangeManager.Instance.SceneChange("StageSelectScene", true);
    }

    public void Button_Title()
    {
        SoundManager.Instance.PlaySound("決定", false);
        SceneChangeManager.Instance.SceneChange("TitleScene", true);
    }

    public void Button_GamePlayAgain()
    {
        SoundManager.Instance.PlaySound("決定", false);
        SceneChangeManager.Instance.SceneChange(GamePlayManager.Instance.OldGameStageName, true);
    }
}
