using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// �t�F�[�h�̏��
public enum FADE_STATE
{
    FADE_NONE = 0,      // �t�F�[�h���������Ă��Ȃ�
    FADE_IN,            // �t�F�[�h�C��������
    FADE_OUT,           // �t�F�[�h�A�E�g������
    //FADE_MAX            // �t�F�[�h��ԍő吔
}

// �t�F�[�h�̎��
public enum FADE_KIND
{
    FADE_SCENECHANGE = 0,      // �V�[���ύX�t�F�[�h
                               // �t�F�[�h
    //FADE_MAX                   // �t�F�[�h�̎�ލő吔
}


public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    // �ϐ�

    // �t�F�[�h�b��
    private float FadeTime = 0.4f;


    private Texture2D FadeTexture;                    // �t�F�[�h�̃e�N�X�`��
    static private FADE_STATE NowFadeState;           // ���݂̃t�F�[�h�̏��
    private FADE_STATE OldFadeState;                  // �ЂƂO�̃t�F�[�h�̏��
    private FADE_KIND NowFadeKind;                    // ���݂̃t�F�[�h�̎��
    private Color FadeColor;                          // �t�F�[�h�̃J���[

    private float NowTime;

    private string NextSceneName;

    private bool TextureFlag = false;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject); // �V�[�����ς���Ă����ȂȂ�

        // ������

        NowTime = 0.0f;

        NowFadeState = OldFadeState = FADE_STATE.FADE_NONE;
        NowFadeKind = FADE_KIND.FADE_SCENECHANGE;

        //�e�N�X�`���쐬
        //Debug.Log("FadeManager�쐬");

        StartCoroutine(FadeTextrueInit()); // �t�F�[�h�p�e�N�X�`������
    }

    // �t�F�[�h�p�e�N�X�`����������
    private IEnumerator FadeTextrueInit()
    {
        // ���ꂪ�Ȃ���ReadPixels()�ŃG���[�ɂȂ�
        yield return new WaitForEndOfFrame();

        // �t�F�[�h�p�e�N�X�`������
        FadeColor = new Color(0, 0, 0, 0);
        FadeTexture = new Texture2D(32, 32, TextureFormat.RGB24, false);
        FadeTexture.ReadPixels(new Rect(0, 0, 32, 32), 0, 0, false);
        FadeTexture.SetPixel(0, 0, Color.white);
        FadeTexture.Apply();
        TextureFlag = true;
    }

    private void OnGUI()
    {
        if (NowFadeState == FADE_STATE.FADE_NONE)
            return;

        //�����x���X�V���ăe�N�X�`����`��
        if (TextureFlag)
        {
            GUI.color = FadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);
        }        
    }

    //private void Start() 
    //{

    //}

    private void Update()
    {
        // �t�F�[�h������
        if (NowFadeState != FADE_STATE.FADE_NONE)
        {
            if (NowFadeState == FADE_STATE.FADE_OUT)
            {// �t�F�[�h�A�E�g����
                switch (NowFadeKind)
                {
                    case FADE_KIND.FADE_SCENECHANGE:
                        FadeColor.a = NowTime / FadeTime;
                        break;
                    default:
                        break;
                }
                
                NowTime += Time.unscaledDeltaTime; // ���ԉ��Z
                //NowTime += Time.deltaTime; // ���ԉ��Z(����������_��)

                if (FadeColor.a >= 1.0f)
                {
                    // �t�F�[�h�C�������ɐ؂�ւ�
                    FadeColor.a = 1.0f;
                    NowTime = 0.0f;
                    NowFadeState = FADE_STATE.FADE_IN;
                }
            }
            else if (NowFadeState == FADE_STATE.FADE_IN)
            {// �t�F�[�h�C������
                switch (NowFadeKind)// ���l�����Z���ĉ�ʂ𕂂��オ�点��
                {
                    case FADE_KIND.FADE_SCENECHANGE:
                        FadeColor.a = 1.0f - NowTime / FadeTime;
                        break;
                    default:
                        break;
                }

                NowTime += Time.unscaledDeltaTime; // ���ԉ��Z
                //NowTime += Time.deltaTime; // ���ԉ��Z(����������_��)

                if (FadeColor.a <= 0.0f)
                {
                    // �t�F�[�h�����I��
                    FadeColor.a = 0.0f;
                    NowTime = 0.0f;
                    NowFadeState = FADE_STATE.FADE_NONE;
                }
            }
        }

        // �t�F�[�h�C�����̏���
        if (OldFadeState == FADE_STATE.FADE_OUT && NowFadeState == FADE_STATE.FADE_IN)
        {
            //�V�[���I���Ȃ̂Ŏ��Ԃ����Ƃɖ߂�
            //Time.timeScale = 1.0f;

            // �V�[���̐؂�ւ�菈���Ȃǁ�����
            SceneChangeManager.Instance.SceneChange(NextSceneName, false);
        }

        OldFadeState = NowFadeState; // �ЂƂO�̃t�F�[�h�Ɍ��݂̃t�F�[�h����
    }


    // �t�F�[�h�ύX
    //
    // �����P�F���̃V�[���̖��O
    // �����Q�F�t�F�[�h�̎��
    public void FadeStart(string nextSceneName, FADE_KIND FadeKind)
    {
        if (NowFadeState == FADE_STATE.FADE_NONE)
        {
            NextSceneName = nextSceneName;

            NowFadeState = FADE_STATE.FADE_OUT;
            NowFadeKind = FadeKind;

            NowTime = 0.0f;
        }
    }

    public void FadeLogoStart()
    {
        if (NowFadeState == FADE_STATE.FADE_NONE)
        {
            NextSceneName = "LogoScene";

            NowFadeState = FADE_STATE.FADE_IN;
            NowFadeKind = FADE_KIND.FADE_SCENECHANGE;

            FadeColor.a = 1.0f;

            NowTime = 0.0f;
        }
    }


    static public FADE_STATE GetNowState()
    {
        return NowFadeState;
    }
}
