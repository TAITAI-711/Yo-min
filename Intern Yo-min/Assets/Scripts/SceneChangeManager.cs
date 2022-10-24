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

        DontDestroyOnLoad(this.gameObject); // �V�[�����ς���Ă����ȂȂ�
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �V�[���̕ύX
    // ����1�F���̃V�[���̖��O
    // ����2�F�t�F�[�h���邩
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
                    SoundManager.Instance.PlaySound("BGM_�^�C�g��", true);
                    break;
                case "StageSelectScene":
                    SoundManager.Instance.PlaySound("BGM_���C�����j���[", true);
                    break;
                case "GameScene":
                    SoundManager.Instance.PlaySound("BGM_�x�[�V�b�N", true);
                    break;
                case "Stage_2":
                    SoundManager.Instance.PlaySound("BGM_�c���", true);
                    break;
                case "Stage_3":
                    SoundManager.Instance.PlaySound("BGM_����", true);
                    break;
                case "Stage_4":
                    SoundManager.Instance.PlaySound("BGM_�W�����O��", true);
                    break;
                case "Stage_5":
                    SoundManager.Instance.PlaySound("BGM_�X", true);
                    break;
                case "Stage_6":
                    SoundManager.Instance.PlaySound("BGM_����", true);
                    break;
                case "ResultScene":
                    //SoundManager.Instance.PlaySound("BGM_���U���g", true);
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
