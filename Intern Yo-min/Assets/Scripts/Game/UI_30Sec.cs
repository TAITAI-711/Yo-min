using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_30Sec : MonoBehaviour
{
    [SerializeField] private GameObject EndPosObject;
    private RectTransform MyRt;
    private RectTransform EndPosObjectRt;

    private Vector3 StartPos;
    private Vector3 EndPos;

    public float MoveTime = 3.0f;
    private float NowTime = 0.0f;

    private bool isMove = false;


    private void Awake()
    {
        MyRt = gameObject.GetComponent<RectTransform>();
        EndPosObjectRt = EndPosObject.GetComponent<RectTransform>();
        NowTime = MoveTime;
        StartPos = MyRt.localPosition;
        EndPos = EndPosObjectRt.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.Instance.isPause || !isMove || NowTime <= 0.0f)
            return;

        MyRt.localPosition = StartPos + (EndPos - StartPos) * (1.0f - NowTime / MoveTime);

        NowTime -= Time.deltaTime;

        if (NowTime <= 0.0f)
        {
            NowTime = 0.0f;
            MyRt.localPosition = EndPos;
            gameObject.SetActive(false);
        }  
    }

    public void SetMove()
    {
        isMove = true;

        // Žc‚è30•b‚Ì‰¹–Â‚ç‚·
    }
}
