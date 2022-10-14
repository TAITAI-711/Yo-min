using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Charge : MonoBehaviour
{
    private PlayerMove PlayerObj;

    [SerializeField] private Slider SliderObj;

    private Quaternion StartRotate;

    private void Awake()
    {
        StartRotate = transform.localRotation;
    }


    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GetComponentInParent<PlayerMove>();
        //SliderObj = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.Instance.isPause)
            return;


        Vector3 Parent = transform.parent.transform.localRotation.eulerAngles;

        // UIÇÃå¸Ç´èCê≥
        transform.rotation = StartRotate;

        SliderObj.value = PlayerObj.ChargePow;

        if (PlayerObj.NowReChargeTime <= 0.0f)
            SliderObj.gameObject.SetActive(true);
        else
            SliderObj.gameObject.SetActive(false);           
    }
}
