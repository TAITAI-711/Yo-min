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
    public int Num = 0;    // ï\é¶Ç∑ÇÈÉIÉZÉçÇÃå¬êî

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

        foreach (var obj in GamePlayManager.Instance.BanmenManagerObj.BanmenObj)
        {
            num += obj.GetOseroNum(PlayerOseroType.OseroType);
        }

        if (num != Num)
        {
            Num = num;

            NumText.text = Num.ToString();
        }

        if (GamePlayManager.Instance.PlayerManagerObj.UI_GameTimeObj.UI_TimeObj.NowTime <= UI_OseroDisappearTime)
            NumText.text = "?";
    }

    public void SetPlayerOseroType(GamePlayManager.PlayerOseroTypeInfo playerOseroType)
    {
        PlayerOseroType = playerOseroType;
        this.GetComponent<Image>().sprite = PlayerOseroType.UI_OseroImage;
    }
}
