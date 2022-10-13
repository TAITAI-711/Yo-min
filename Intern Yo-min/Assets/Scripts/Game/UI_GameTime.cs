using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameTime : MonoBehaviour
{
    public UI_Time UI_TimeObj = null;
    public UI_StartCount UI_StartCountObj = null;

    private void Awake()
    {
        UI_TimeObj = GetComponentInChildren<UI_Time>();
        UI_StartCountObj = GetComponentInChildren<UI_StartCount>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
