using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �X�e�[�W�I���ŕ��򂳂���
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Joystick_0_Button_B"))
        {
            // SceneChangeManager.Instance.SceneChange("Stage_1", true);
            // SceneChangeManager.Instance.SceneChange("Stage_2", true);
            //Debug.Log("���̃V�[��");
            SceneChangeManager.Instance.SceneChange("GameScene", true);
        }
    }
}
