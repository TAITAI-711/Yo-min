using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UI_Charge : MonoBehaviour
{
    private Quaternion StartRotate;
    private Slider SliderObj;

    private PlayerMove PlayerObj;

    //public RectTransform Rt;
    //private RectTransform MyRt;

    private void Awake()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        //MyRt = GetComponent<RectTransform>();
        PlayerObj = GetComponentInParent<PlayerMove>();
        SliderObj = GetComponentInChildren<Slider>();
        StartRotate = transform.localRotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // UIÇÃå¸Ç´èCê≥
        transform.rotation = StartRotate;

        SliderObj.value = PlayerObj.ChargePow;

        if (PlayerObj.NowReChargeTime <= 0.0f)
            SliderObj.gameObject.SetActive(true);
        else
            SliderObj.gameObject.SetActive(false);


        //Vector3 PlayerPos = PlayerObj.transform.position;
        ////PlayerPos.y += 20.0f;

        //if (Rt != null)
        //{
        //    var pos = Vector2.zero;
        //    var uiCamera = Camera.main;
        //    var worldCamera = Camera.main;
        //    var canvasRect = MyRt;

        //    var screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, PlayerPos);
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out pos);
        //    Rt.localPosition = pos;
        //}
    }
}
