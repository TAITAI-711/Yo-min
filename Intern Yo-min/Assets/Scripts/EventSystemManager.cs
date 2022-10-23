using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : SingletonMonoBehaviour<EventSystemManager>
{
    public EventSystem EventSystemObj = null;
    public StandaloneInputModuleButton StandaloneInputObj = null;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // �V�[�����ς���Ă����ȂȂ�


        EventSystemObj = gameObject.GetComponent<EventSystem>();
        StandaloneInputObj = gameObject.GetComponent<StandaloneInputModuleButton>();
    }
}
