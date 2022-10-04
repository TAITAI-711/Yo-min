using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koma : MonoBehaviour
{
    private Banmen BanmenObj;
    private float FallPow;          // オセロの重力
    private Vector3 StartPos;       // オセロの開始座標
    private Vector3 EndPos;         // オセロの着地座標
    private Vector3 MovePow;        // 移動する量(マス目換算:0～3とか)
    private bool isMove = false;    // 移動開始フラグ


    // Start is called before the first frame update
    void Start()
    {
        isMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMove)
        {
            Vector3 Pos = BanmenObj.GetMasu(MovePow).transform.position;
            gameObject.transform.position = Pos;
            isMove = false;
        }
    }

    // オセロを動かす処理(盤面, 下方向にかかる力, 開始座標, 着地座標)
    public void Move(Banmen banmenObj, float fallPow, Vector3 startPos, Vector3 endPos)
    {
        BanmenObj = banmenObj;
        FallPow = fallPow;
        StartPos = startPos;
        EndPos = endPos;
        isMove = true;
    }
}
