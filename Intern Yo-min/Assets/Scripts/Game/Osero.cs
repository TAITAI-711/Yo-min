using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Osero : MonoBehaviour
{
    private MeshRenderer OseroMesh = null;
    private Banmen BanmenObj;       // 盤面オブジェクト

    private Vector3 Gravity;        // オセロの重力
    private Vector3 StartPos;       // オセロの開始座標
    private Vector3 EndPos;         // オセロの着地座標
    //private Vector3 MovePow;        // 移動する量(マス目換算:0～3とか)
    private float Angle;            // 射出角度

    [HideInInspector] public PlayerManager.PlayerOseroTypeInfo PlayerOseroType;

    private Rigidbody Rb;
    private Collider Cd;



    private bool isOseroSet = false;        // 盤上にオセロ固定フラグ

    //private bool isMove = false;    // 移動開始フラグ


    // Start is called before the first frame update
    void Start()
    {
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
        if (!isOseroSet)
        {
            Rb.AddForce(Gravity, ForceMode.Acceleration);

            if (transform.position.y < -50.0f)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Banmen"))
        {
            //Debug.Log("盤面と当たった");
            Masu masu = BanmenObj.GetMasu(transform.position);
            masu.SetOsero(this);

            //Destroy(gameObject);
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

    public void OseroSet(Vector3 OseroSetPosition)
    {
        isOseroSet = true;
        Rb.isKinematic = true;
        Cd.isTrigger = true;
        transform.position = OseroSetPosition;
        transform.rotation = Quaternion.identity;
    }

    public void SetOseroType(PlayerManager.PlayerOseroTypeInfo oseroType)
    {
        PlayerOseroType = oseroType;

        if (OseroMesh == null)
        {
            OseroMesh = GetComponent<MeshRenderer>();
        }

        OseroMesh.material = PlayerOseroType.PlayerMaterial;
    }

    public PlayerManager.PlayerOseroTypeInfo GetOseroType()
    {
        return PlayerOseroType;
    }
}
