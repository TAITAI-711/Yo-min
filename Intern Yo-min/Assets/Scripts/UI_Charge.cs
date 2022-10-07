using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Charge : MonoBehaviour
{
    private PlayerMove PlayerObj;

    [SerializeField] private Slider SliderObj;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GetComponentInParent<PlayerMove>();
        //SliderObj = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        SliderObj.value = PlayerObj.ChargePow;

        if (PlayerObj.NowReChargeTime <= 0.0f)
            SliderObj.gameObject.SetActive(true);
        else
            SliderObj.gameObject.SetActive(false);           
    }
}
