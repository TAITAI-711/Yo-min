using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// フェードの状態
public enum FADE_STATE
{
    FADE_NONE = 0,      // フェード処理をしていない
    FADE_IN,            // フェードイン処理中
    FADE_OUT,           // フェードアウト処理中
    //FADE_MAX            // フェード状態最大数
}

// フェードの種類
public enum FADE_KIND
{
    FADE_SCENECHANGE = 0,      // シーン変更フェード
                               // フェード
    //FADE_MAX                   // フェードの種類最大数
}


public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    // 変数

    // フェード秒数
    private float FadeTime = 0.4f;


    private Texture2D FadeTexture;                    // フェードのテクスチャ
    static private FADE_STATE NowFadeState;           // 現在のフェードの状態
    private FADE_STATE OldFadeState;                  // ひとつ前のフェードの状態
    private FADE_KIND NowFadeKind;                    // 現在のフェードの種類
    private Color FadeColor;                          // フェードのカラー

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

        DontDestroyOnLoad(this.gameObject); // シーンが変わっても死なない

        // 初期化

        NowTime = 0.0f;

        NowFadeState = OldFadeState = FADE_STATE.FADE_NONE;
        NowFadeKind = FADE_KIND.FADE_SCENECHANGE;

        //テクスチャ作成
        //Debug.Log("FadeManager作成");

        StartCoroutine(FadeTextrueInit()); // フェード用テクスチャ生成
    }

    // フェード用テクスチャ生成処理
    private IEnumerator FadeTextrueInit()
    {
        // これがないとReadPixels()でエラーになる
        yield return new WaitForEndOfFrame();

        // フェード用テクスチャ生成
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

        //透明度を更新してテクスチャを描画
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
        // フェード処理中
        if (NowFadeState != FADE_STATE.FADE_NONE)
        {
            if (NowFadeState == FADE_STATE.FADE_OUT)
            {// フェードアウト処理
                switch (NowFadeKind)
                {
                    case FADE_KIND.FADE_SCENECHANGE:
                        FadeColor.a = NowTime / FadeTime;
                        break;
                    default:
                        break;
                }
                
                NowTime += Time.unscaledDeltaTime; // 時間加算
                //NowTime += Time.deltaTime; // 時間加算(こっちじゃダメ)

                if (FadeColor.a >= 1.0f)
                {
                    // フェードイン処理に切り替え
                    FadeColor.a = 1.0f;
                    NowTime = 0.0f;
                    NowFadeState = FADE_STATE.FADE_IN;
                }
            }
            else if (NowFadeState == FADE_STATE.FADE_IN)
            {// フェードイン処理
                switch (NowFadeKind)// α値を減算して画面を浮き上がらせる
                {
                    case FADE_KIND.FADE_SCENECHANGE:
                        FadeColor.a = 1.0f - NowTime / FadeTime;
                        break;
                    default:
                        break;
                }

                NowTime += Time.unscaledDeltaTime; // 時間加算
                //NowTime += Time.deltaTime; // 時間加算(こっちじゃダメ)

                if (FadeColor.a <= 0.0f)
                {
                    // フェード処理終了
                    FadeColor.a = 0.0f;
                    NowTime = 0.0f;
                    NowFadeState = FADE_STATE.FADE_NONE;
                }
            }
        }

        // フェードイン時の処理
        if (OldFadeState == FADE_STATE.FADE_OUT && NowFadeState == FADE_STATE.FADE_IN)
        {
            //シーン終了なので時間をもとに戻す
            //Time.timeScale = 1.0f;

            // シーンの切り替わり処理など↓↓↓
            SceneChangeManager.Instance.SceneChange(NextSceneName, false);
        }

        OldFadeState = NowFadeState; // ひとつ前のフェードに現在のフェード入力
    }


    // フェード変更
    //
    // 引数１：次のシーンの名前
    // 引数２：フェードの種類
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
