using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField] private GameObject Button_CreditObj;
    [SerializeField] private GameObject CreditObj;
    public GameObject FirstSelectObj;

    [HideInInspector] public bool isPressB = false;

    private bool isOnce = false;


    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        Button_CreditObj.SetActive(false);
        CreditObj.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPressB || FadeManager.GetNowState() != FADE_STATE.FADE_NONE)
            return;

        if (!isOnce)
        {
            isOnce = true;
            Button_CreditObj.SetActive(true);
            EventSystemManager.Instance.EventSystemObj.SetSelectedGameObject(FirstSelectObj);
        }

        if (Input.GetButtonDown(GamePlayManager.Instance.MenuSelectPlayerName + "_Button_Start"))
        {
            GamePlayManager.Instance.isPause = !GamePlayManager.Instance.isPause;

            if (GamePlayManager.Instance.isPause)
            {
                SoundManager.Instance.SetVolumeBGM(0.5f);
                SoundManager.Instance.PlaySound("ポップアップ", false, 0.1f);
                EventSystemManager.Instance.EventSystemObj.SetSelectedGameObject(null);
                CreditObj.SetActive(true);
            }
            else
            {
                SoundManager.Instance.SetVolumeBGM(1.0f);
                SoundManager.Instance.PlaySound("ポップアップ戻る", false, 0.1f);
                EventSystemManager.Instance.EventSystemObj.SetSelectedGameObject(FirstSelectObj);
                CreditObj.SetActive(false);
            }
        }
    }

    public void SetNextScene()
    {
        if (FadeManager.GetNowState() != FADE_STATE.FADE_NONE)
            return;

        SoundManager.Instance.PlaySound("決定", false);
        SceneChangeManager.Instance.SceneChange("StageSelectScene", true);
    }

    public void SetGameEnd()
    {
        if (FadeManager.GetNowState() != FADE_STATE.FADE_NONE)
            return;

        SoundManager.Instance.PlaySound("決定", false);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
