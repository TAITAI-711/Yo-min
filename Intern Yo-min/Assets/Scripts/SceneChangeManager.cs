using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : SingletonMonoBehaviour<SceneChangeManager>
{
    private string NextSceneName;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject); // シーンが変わっても死なない
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // シーンの変更
    // 引数1：次のシーンの名前
    // 引数2：フェードするか
    public void SceneChange(string nextSceneName, bool isFade)
    {
        NextSceneName = nextSceneName;

        if (isFade)
        {
            FadeManager.Instance.FadeStart(NextSceneName, FADE_KIND.FADE_SCENECHANGE);
        }            
        else
        {
            SceneManager.LoadScene(NextSceneName);

            Time.timeScale = 1.0f;

            if (GamePlayManager.Instance != null)
                GamePlayManager.Instance.GameReset();
        }
    }
}
