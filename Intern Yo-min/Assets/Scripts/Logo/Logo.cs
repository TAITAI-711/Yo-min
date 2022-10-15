using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Joystick_0_Button_B"))
        {
            //Debug.Log("éüÇÃÉVÅ[Éì");
            SceneChangeManager.Instance.SceneChange("TitleScene", true);
        }
    }
}
