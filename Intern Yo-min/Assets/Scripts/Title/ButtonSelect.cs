using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    private EventSystem eventSystem;

    private Button[] ButtonObj;

    private int NowButtonNum = 0;
    private int ButtonMax = 0;

    // Start is called before the first frame update
    void Start()
    {
        ButtonObj = GetComponentsInChildren<Button>();
        ButtonMax = ButtonObj.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
