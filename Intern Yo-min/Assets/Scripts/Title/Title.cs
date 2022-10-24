using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject FirstSelectObj;

    // Start is called before the first frame update
    void Start()
    {
        EventSystemManager.Instance.EventSystemObj.SetSelectedGameObject(FirstSelectObj);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    //Debug.Log("次のシーン");
        //    SetNextScene();
        //}
    }

    public void SetNextScene()
    {
        SoundManager.Instance.PlaySound("決定", false);
        SceneChangeManager.Instance.SceneChange("StageSelectScene", true);
    }

    public void SetGameEnd()
    {
        SoundManager.Instance.PlaySound("決定", false);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
