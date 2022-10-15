using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : SingletonMonoBehaviour<EventSystemManager>
{
    public EventSystem EventSystemObj = null;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // ƒV[ƒ“‚ª•Ï‚í‚Á‚Ä‚à€‚È‚È‚¢


        EventSystemObj = GetComponent<EventSystem>();
    }
}
