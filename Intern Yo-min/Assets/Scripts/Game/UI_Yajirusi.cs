using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Yajirusi : MonoBehaviour
{
    private PlayerMove PlayerObj;

    private static float YajirusiLengthOfsetX = 8.0f;
    private static float YajirusiLengthOfsetY = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GetComponentInParent<PlayerMove>();

        //// 座標
        //Vector3 Pos = PlayerObj.transform.position + new Vector3(0.0f, 2.0f, 0.0f) * PlayerObj.transform.localScale.y;
        //float PosX = 2.0f * PlayerObj.transform.localScale.x;
        //Pos.x += PosX; // 右を基準にずらす

        //float Angle = Calculation.UnityTwoPointAngle360(Vector2.right, PlayerObj.PlayerAngle);
        //VecQuaternion Vq = Calculation.PointRotate(PlayerObj.transform.position, Pos, Angle, Vector3.up);
        //transform.position = Vq.Pos;

        //// 回転
        //float rad = PlayerObj.ThrowingAngle * Mathf.Deg2Rad;
        //Vector3 Vec = new Vector3(PlayerObj.PlayerAngle.x, Mathf.Tan(rad), PlayerObj.PlayerAngle.y);

        //transform.rotation = Quaternion.FromToRotation(Vector3.up, Vec);
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.Instance.isPause)
            return;

        // プレイヤーの方向UI変更処理

        // 座標
        //Vector3 Pos = PlayerObj.transform.position + new Vector3(0.0f, 2.0f, 0.0f) * PlayerObj.transform.localScale.y;


        Vector3 OriginPos = PlayerObj.transform.position + new Vector3(0, YajirusiLengthOfsetY, 0); // 上に基準をずらす
        Vector3 TargetPos = OriginPos + new Vector3(YajirusiLengthOfsetX, 0, 0); // 右にずらす

        float Angle = Calculation.TwoPointAngle360(Vector2.right, PlayerObj.ShootAngle);
        Angle -= 180.0f;
        Angle *= 2.0f;

        //Debug.Log(Angle);

        VecQuaternion Vq = Calculation.PointRotate(OriginPos, TargetPos, Angle, Vector3.up);
        transform.position = Vq.Pos;

        // 回転
        //float rad = PlayerObj.ThrowingAngle * Mathf.Deg2Rad;
        //Vector3 Vec = new Vector3(PlayerObj.PlayerAngle.x, Mathf.Tan(rad), PlayerObj.PlayerAngle.y);


        Vector3 Vec = new Vector3(PlayerObj.ShootAngle.x, 0.0f, PlayerObj.ShootAngle.y);
        transform.rotation = Quaternion.LookRotation(Vector3.up, Vec);
    }
}
