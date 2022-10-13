using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Osero : MonoBehaviour
{
    private MeshRenderer OseroMesh = null;

    private Vector3 Gravity;        // オセロの重力
    private Vector3 StartPos;       // オセロの開始座標
    private Vector3 EndPos;         // オセロの着地座標
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
                SetDestroy();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Banmen"))
        {
            //Debug.Log("盤面と当たった");

            // 盤面より高いなら
            //if (transform.position.y >= collision.gameObject.transform.position.y - GamePlayManager.MasuScaleY * 0.5f)

            // 盤面の内側なら
            if (transform.position.x <= collision.gameObject.transform.position.x + collision.gameObject.transform.localScale.x * 0.5f &&
                transform.position.x >= collision.gameObject.transform.position.x - collision.gameObject.transform.localScale.x * 0.5f &&
                transform.position.z <= collision.gameObject.transform.position.z + collision.gameObject.transform.localScale.z * 0.5f &&
                transform.position.z >= collision.gameObject.transform.position.z - collision.gameObject.transform.localScale.z * 0.5f)
            {
                Masu masu = collision.gameObject.GetComponent<Banmen>().GetMasu(transform.position);
                masu.SetOsero(this);
            }

            //Destroy(gameObject);
        }
    }

    public void SetDestroy()
    {
        if (!isOseroSet)
            Destroy(gameObject);
    }

    //private void OnTriggerExit(Collider collider)
    //{
    //    if (collider.CompareTag("Player"))
    //    {
    //        Debug.Log("プレイヤーからオセロが離れた");
    //        Cd.isTrigger = false;
    //    }
    //}

    // オセロを動かす処理(重力, 射出角度, 開始座標, 着地座標, 回転量(1秒に1回転で1))
    public void Move(float gravity, float throwingAngle, Vector3 startPos, Vector3 endPos, float Rotate)
    {
        Gravity.y = -gravity;
        Angle = throwingAngle;
        StartPos = startPos;
        EndPos = endPos;

        Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;

        Cd = gameObject.GetComponent<Collider>();
        Cd.isTrigger = false;

        // 回転速度の上限廃止
        Rb.maxAngularVelocity = 100;

        // レイヤー変更
        //gameObject.layer = LayerMask.NameToLayer("HitOsero");

        // 射出速度計算
        Vector3 Vel = Calculation.InjectionSpeed(StartPos, EndPos, Angle, Gravity.y);

        // オセロ射出
        Rb.AddForce(Vel * Rb.mass, ForceMode.Impulse);

        // オセロ回転処理
        Vector3 vec;
        vec.x = (EndPos.x - StartPos.x);
        vec.y = 0.0f;
        vec.z = (EndPos.z - StartPos.z);

        Vector2 Rotatevec;
        Rotatevec.x = vec.x * -Mathf.Cos(Mathf.PI * -0.5f) - vec.z * Mathf.Sin(Mathf.PI * -0.5f);
        Rotatevec.y = vec.x * Mathf.Sin(Mathf.PI * -0.5f) + vec.z * Mathf.Cos(Mathf.PI * -0.5f);

        vec.x = Rotatevec.x;
        vec.z = Rotatevec.y;
        vec.Normalize();

        Rb.AddRelativeTorque(vec * Mathf.PI * 2.0f * Rotate, ForceMode.VelocityChange);
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
