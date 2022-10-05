using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Osero : MonoBehaviour
{
    private Banmen BanmenObj;       // 盤面オブジェクト
    private Vector3 Gravity;        // オセロの重力
    private Vector3 StartPos;       // オセロの開始座標
    private Vector3 EndPos;         // オセロの着地座標
    private Vector3 MovePow;        // 移動する量(マス目換算:0～3とか)
    private float Angle;            // 射出角度

    private Rigidbody Rb;
    private Collider Cd;
    //private bool isMove = false;    // 移動開始フラグ


    // Start is called before the first frame update
    void Start()
    {
        Gravity.y = -9.8f;
        Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;

        Cd = gameObject.GetComponent<Collider>();
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rb.AddForce(Gravity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Banmen"))
        {
            //Debug.Log("盤面と当たった");
            BanmenObj.GetMasu(transform.position);

            Destroy(gameObject);
        }
    }

    //private void OnTriggerExit(Collider collider)
    //{
    //    if (collider.CompareTag("Player"))
    //    {
    //        Debug.Log("プレイヤーからオセロが離れた");
    //        Cd.isTrigger = false;
    //    }
    //}

    // オセロを動かす処理(盤面, 重力, 射出角度, 開始座標, 着地座標)
    public void Move(Banmen banmenObj, float gravity, float throwingAngle, Vector3 startPos, Vector3 endPos)
    {
        BanmenObj = banmenObj;
        Gravity.y = -gravity;
        Angle = throwingAngle;
        StartPos = startPos;
        EndPos = endPos;

        Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;

        Cd = gameObject.GetComponent<Collider>();
        Cd.isTrigger = false;

        // レイヤー変更
        //gameObject.layer = LayerMask.NameToLayer("HitOsero");

        // 射出速度計算
        Vector3 Vel = Calculation.InjectionSpeed(StartPos, EndPos, Angle, Gravity.y);

        // オセロ射出
        Rb.AddForce(Vel * Rb.mass, ForceMode.Impulse);
    }
}
