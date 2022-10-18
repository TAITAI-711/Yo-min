using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Pause : MonoBehaviour
{
    [SerializeField] private GameObject UI_PausePanelObj;
    [SerializeField] private GameObject FirstSelectObj;

    private void Awake()
    {
        if (UI_PausePanelObj == null)
            UI_PausePanelObj = GetComponentInChildren<GameObject>();

        UI_PausePanelObj.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Joystick_0_Button_Start") && (GamePlayManager.Instance.isGamePlay || GamePlayManager.Instance.isGameEnd))
        {
            Pause();    // �|�[�Y����
        }
    }


    // �|�[�Y����
    private void Pause()
    {
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
        Pause(); // �|�[�Y����
    }

    public void Button_StageSelect()
    {
        SceneChangeManager.Instance.SceneChange("StageSelectScene", true);
    }

    public void Button_Title()
    {
        SceneChangeManager.Instance.SceneChange("TitleScene", true);
    }
}