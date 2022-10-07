using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Yajirusi : MonoBehaviour
{
    private PlayerMove PlayerObj;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GetComponentInParent<PlayerMove>();

        //// ���W
        //Vector3 Pos = PlayerObj.transform.position + new Vector3(0.0f, 2.0f, 0.0f) * PlayerObj.transform.localScale.y;
        //float PosX = 2.0f * PlayerObj.transform.localScale.x;
        //Pos.x += PosX; // �E����ɂ��炷

        //float Angle = Calculation.UnityTwoPointAngle360(Vector2.right, PlayerObj.PlayerAngle);
        //VecQuaternion Vq = Calculation.PointRotate(PlayerObj.transform.position, Pos, Angle, Vector3.up);
        //transform.position = Vq.Pos;

        //// ��]
        //float rad = PlayerObj.ThrowingAngle * Mathf.Deg2Rad;
        //Vector3 Vec = new Vector3(PlayerObj.PlayerAngle.x, Mathf.Tan(rad), PlayerObj.PlayerAngle.y);

        //transform.rotation = Quaternion.FromToRotation(Vector3.up, Vec);
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[�̕���UI�ύX����

        // ���W
        //Vector3 Pos = PlayerObj.transform.position + new Vector3(0.0f, 2.0f, 0.0f) * PlayerObj.transform.localScale.y;
        Vector3 Pos = PlayerObj.transform.position;
        float PosX = 2.0f * PlayerObj.transform.localScale.x;
        Pos.x += PosX; // �E����ɂ��炷

        float Angle = Calculation.TwoPointAngle360(Vector2.right, PlayerObj.PlayerAngle);
        Angle -= 180.0f;
        Angle *= 2.0f;
        //Debug.Log(Angle);
        VecQuaternion Vq = Calculation.PointRotate(PlayerObj.transform.position, Pos, Angle, Vector3.up);
        transform.position = Vq.Pos;

        // ��]
        float rad = PlayerObj.ThrowingAngle * Mathf.Deg2Rad;
        //Vector3 Vec = new Vector3(PlayerObj.PlayerAngle.x, Mathf.Tan(rad), PlayerObj.PlayerAngle.y);
        Vector3 Vec = new Vector3(PlayerObj.PlayerAngle.x, 0.0f, PlayerObj.PlayerAngle.y);

        transform.rotation = Quaternion.FromToRotation(Vector3.up, Vec);
    }
}
