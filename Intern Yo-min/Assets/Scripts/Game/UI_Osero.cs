using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Osero : MonoBehaviour
{
    [SerializeField] private GameObject OseroNum;
    //[SerializeField] private BanmenManager BanmenManagerObj;
    private TextMeshProUGUI NumText;

    [HideInInspector] public GamePlayManager.PlayerOseroTypeInfo PlayerOseroType;
    public int Num = 0;    // 表示するオセロの個数

    private static float UI_OseroDisappearTime = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        NumText = OseroNum.GetComponent<TextMeshProUGUI>();
        NumText.text = "0";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int num = 0;

        foreach (var obj in BanmenManager.Instance.BanmenObj)
        {
            num += obj.GetOseroNum(PlayerOseroType.OseroType);
        }

        if (num != Num)
        {
            Num = num;

            NumText.text = Num.ToString();
        }

        if (PlayerManager.Instance.UI_GameTimeObj != null)
        {
            if (PlayerManager.Instance.UI_GameTimeObj.UI_TimeObj.NowTime <= UI_OseroDisappearTime)
                NumText.text = "?";
        }
    }

    public void SetPlayerOseroType(GamePlayManager.PlayerOseroTypeInfo playerOseroType)
    {
        PlayerOseroType = playerOseroType;
        this.GetComponent<Image>().sprite = PlayerOseroType.UI_OseroImage;
    }
}
