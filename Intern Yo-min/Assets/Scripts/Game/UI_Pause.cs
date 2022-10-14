using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Pause : MonoBehaviour
{
    [SerializeField] private GameObject UI_PausePanelObj;

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
        if (Input.GetButtonDown("Joystick_0_Button_Start") && GamePlayManager.Instance.isGamePlay)
        {
            GamePlayManager.Instance.isPause = !GamePlayManager.Instance.isPause;

            // É|Å[ÉYèàóù
            if (GamePlayManager.Instance.isPause)
            {
                Time.timeScale = 0.0f;
                UI_PausePanelObj.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                UI_PausePanelObj.SetActive(false);
            }
        }
    }
}
