using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameTime : MonoBehaviour
{
    [HideInInspector] public UI_Time UI_TimeObj = null;
    [HideInInspector] public UI_StartCount UI_StartCountObj = null;
    [HideInInspector] public UI_30Sec UI_30SecObj = null; 

    private void Awake()
    {
        UI_TimeObj = GetComponentInChildren<UI_Time>();
        UI_StartCountObj = GetComponentInChildren<UI_StartCount>();
        UI_30SecObj = GetComponentInChildren<UI_30Sec>();
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
