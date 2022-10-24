using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ResultPause : UI_Pause
{
    private bool isOnce = false;

    protected override void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isOnce && ResultManager.Instance.isPause)
        {
            isOnce = true;
            Pause();    // É|Å[ÉYèàóù
        }
    }
}
