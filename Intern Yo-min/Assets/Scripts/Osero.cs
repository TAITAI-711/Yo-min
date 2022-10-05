using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osero : MonoBehaviour
{
    private Banmen BanmenObj;       // �ՖʃI�u�W�F�N�g
    private Vector3 Gravity;          // �I�Z���̏d��
    private Vector3 StartPos;       // �I�Z���̊J�n���W
    private Vector3 EndPos;         // �I�Z���̒��n���W
    private Vector3 MovePow;        // �ړ������(�}�X�ڊ��Z:0�`3�Ƃ�)
    private float Angle;            // �ˏo�p�x

    private Rigidbody Rb;
    //private bool isMove = false;    // �ړ��J�n�t���O


    // Start is called before the first frame update
    void Start()
    {
        Gravity.y = -9.8f;
        Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rb.AddForce(Gravity, ForceMode.Acceleration);
    }

    // �I�Z���𓮂�������(�Ֆ�, �d��, �ˏo�p�x, �J�n���W, ���n���W)
    public void Move(Banmen banmenObj, float gravity, float throwingAngle, Vector3 startPos, Vector3 endPos)
    {
        BanmenObj = banmenObj;
        Gravity.y = -gravity;
        Angle = throwingAngle;
        StartPos = startPos;
        EndPos = endPos;

        Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;

        // �ˏo���x�v�Z
        Vector3 Vel = Calculation.InjectionSpeed(StartPos, EndPos, Angle, Gravity.y);

        // �I�Z���ˏo
        Rb.AddForce(Vel * Rb.mass, ForceMode.Impulse);
    }
}
