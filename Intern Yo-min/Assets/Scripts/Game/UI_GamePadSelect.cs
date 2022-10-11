using TMPro;
using Unity.Collections;
using UnityEngine;

public class UI_GamePadSelect : MonoBehaviour
{
    private TextMeshProUGUI GamePadSelectUI;

    private bool isPress = false;


    [ReadOnly] public int PlayerNum = 2;
    private int NowPlayerNum = 0;

    [ReadOnly] public string[] GamePadName_Player = new string[4];


    private void Awake()
    {
        GamePlayManager.Instance.GamePadSelectObj = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GamePadSelectUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GamePlayManager.Instance.isGamePadOK)
        {
            // �v���C���[�̐l������
            if (NowPlayerNum <= 0)
            {

                // �l���̓��͎�t
                if (Input.GetAxis("Joystick_0_LeftAxis_X") != 0)
                {
                    if (!isPress)
                    {
                        if (Input.GetAxis("Joystick_0_LeftAxis_X") > 0)
                        {
                            if (PlayerNum < 4)
                            {
                                PlayerNum++;
                            }
                        }
                        else
                        {
                            if (PlayerNum > 2)
                            {
                                PlayerNum--;
                            }
                        }
                    }

                    isPress = true;
                }
                else
                {
                    isPress = false;
                }

                // ����
                if (Input.GetButtonDown("Joystick_0_Button_B"))
                {
                    isPress = false;
                    NowPlayerNum = 1;
                }

                // �l���̕\��
                GamePadSelectUI.text = "ALLPlayerNum:" + PlayerNum + "\n\nSelect  GamePad  LeftStick" + "\n\nSet  GamePad  B  Button";

            }
            else
            {
                // R�g���K�[���͑҂�
                for (int i = 1; i <= PlayerNum; i++)
                {
                    if (NowPlayerNum == i)
                    {
                        if (Input.GetButtonDown("Joystick_0_Button_B"))
                        {
                            for (int j = 1; j <= 7; j++)
                            {
                                string NowGamePadName = "Joystick_" + j.ToString() + "_Button_B";

                                if (Input.GetButtonDown(NowGamePadName))
                                {
                                    GamePadName_Player[i - 1] = "Joystick_" + j.ToString();
                                    i = PlayerNum + 1;
                                    NowPlayerNum++;

                                    if (NowPlayerNum > PlayerNum)
                                    {
                                        GamePlayManager.Instance.isGamePadOK = true;
                                        GamePlayManager.Instance.isGamePlay = true;

                                        gameObject.SetActive(false);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                // �{�^�����͕\��
                GamePadSelectUI.text = "Player  " + NowPlayerNum.ToString() + "\nPress  GamePad  B  Button";
            }
        }
    }
}
