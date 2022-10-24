using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveMax : MonoBehaviour
{
    [Header("[ プレイヤーの移動可能範囲指定 ]")]
    [Tooltip("プレイヤーの移動可能範囲")]
    public Vector3 PlayerMoveMaxScale;

    [SerializeField, Tooltip("可視化")]
    private bool isMesh = true;


    private void OnValidate()
    {
        transform.localScale = PlayerMoveMaxScale;

        Vector3 Pos = transform.localPosition;
        Pos.x = 0;
        Pos.y = PlayerMoveMaxScale.y * 0.5f;
        Pos.z = 0;
        transform.localPosition = Pos;

        GetComponent<MeshRenderer>().enabled = isMesh;
    }

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
