using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Osero : MonoBehaviour
{
    [SerializeField] private GameObject OseroNum;
    [SerializeField] private BanmenManager BanmenManagerObj;
    private TextMeshProUGUI NumText;

    private PlayerMove.EnumOseroType OseroType = PlayerMove.EnumOseroType.White;
    private int Num = 0;    // ï\é¶Ç∑ÇÈÉIÉZÉçÇÃå¬êî

    // Start is called before the first frame update
    void Start()
    {
        NumText = OseroNum.GetComponent<TextMeshProUGUI>();
        NumText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        int num = 0;
        foreach (var obj in BanmenManagerObj.BanmenObj)
        {
            num += obj.GetOseroNum(OseroType);
        }

        if (num != Num)
        {
            Num = num;
            NumText.text = Num.ToString();
        }
    }

    public void UIOseroColorSet(Color color, PlayerMove.EnumOseroType oseroType)
    {
        this.GetComponent<Image>().color = color;
        OseroType = oseroType;
    }
}
