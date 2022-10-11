using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Osero : MonoBehaviour
{
    [SerializeField] private GameObject OseroNum;
    //[SerializeField] private BanmenManager BanmenManagerObj;
    private TextMeshProUGUI NumText;

    [HideInInspector] public PlayerManager.PlayerOseroTypeInfo PlayerOseroType;
    private int Num = 0;    // �\������I�Z���̌�

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
        foreach (var obj in GamePlayManager.Instance.BanmenManagerObj.BanmenObj)
        {
            num += obj.GetOseroNum(PlayerOseroType.OseroType);
        }

        if (num != Num)
        {
            Num = num;
            NumText.text = Num.ToString();
        }
    }

    public void UIOseroColorSet(PlayerManager.EnumOseroType oseroType)
    {
        PlayerOseroType.OseroType = oseroType;
        this.GetComponent<Image>().color = PlayerOseroType.PlayerMaterial.color;
    }
}