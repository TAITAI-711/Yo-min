using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : SingletonMonoBehaviour<EventSystemManager>
{
    public EventSystem EventSystemObj = null;   // イベントシステム
    public StandaloneInputModuleButton StandaloneInputObj = null;   // スタンドアロンインプット

    private GameObject OldSelectObject = null;  // ひとつ前の選択オブジェ

    public GameObject NoSelectObj = null;   // 音がならないオブジェクト選択用

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // シーンが変わっても死なない


        EventSystemObj = gameObject.GetComponent<EventSystem>();
        StandaloneInputObj = gameObject.GetComponent<StandaloneInputModuleButton>();
    }


    private void Update()
    {
        GameObject SelectObj = EventSystemObj.currentSelectedGameObject;

        if (OldSelectObject != SelectObj &&
            SelectObj != null &&
            OldSelectObject != null)
        {
            if (NoSelectObj == null || (NoSelectObj != null && NoSelectObj != SelectObj))
                SoundManager.Instance.PlaySound("システム移動", false);
        }

        OldSelectObject = SelectObj;
    }
}
