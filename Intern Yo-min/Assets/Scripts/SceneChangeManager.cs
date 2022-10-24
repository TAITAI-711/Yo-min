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
            SoundManager.Instance.StopSoundAll();

            SceneManager.LoadScene(NextSceneName);

            switch (NextSceneName)
            {
                case "TitleScene":
                    SoundManager.Instance.PlaySoundBGMDelay("BGM_タイトル", 0.5f, 0.8f);
                    break;
                case "StageSelectScene":
                    SoundManager.Instance.PlaySoundBGMDelay("BGM_メインメニュー", 0.5f);
                    break;
                case "GameScene":
                    SoundManager.Instance.PlaySoundBGMDelay("BGM_ベーシック", 0.5f);
                    break;
                case "Stage_2":
                    SoundManager.Instance.PlaySoundBGMDelay("BGM_田んぼ", 0.5f);
                    break;
                case "Stage_3":
                    SoundManager.Instance.PlaySoundBGMDelay("BGM_浮島", 0.5f);
                    break;
                case "Stage_4":
                    SoundManager.Instance.PlaySoundBGMDelay("BGM_ジャングル", 0.5f);
                    break;
                case "Stage_5":
                    SoundManager.Instance.PlaySoundBGMDelay("BGM_氷", 0.5f);
                    break;
                case "Stage_6":
                    SoundManager.Instance.PlaySoundBGMDelay("BGM_草原", 0.5f);
                    break;
                case "ResultScene":
                    //SoundManager.Instance.PlaySound("BGM_リザルト", true);
                    break;
                default:
                    break;
            }


            Time.timeScale = 1.0f;

            if (GamePlayManager.Instance != null)
                GamePlayManager.Instance.GameReset();
        }
    }
}
