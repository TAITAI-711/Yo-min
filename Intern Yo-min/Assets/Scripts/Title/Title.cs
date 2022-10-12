using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("éüÇÃÉVÅ[Éì");
            SetNextScene();
        }
    }

    public void SetNextScene()
    {
        SceneChangeManager.Instance.SceneChange("StageSelectScene", true);
    }
}
