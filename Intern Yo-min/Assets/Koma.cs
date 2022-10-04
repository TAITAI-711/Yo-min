using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koma : MonoBehaviour
{
    private Banmen BanmenObj;
    private float FallPow;          // �I�Z���̏d��
    private Vector3 StartPos;       // �I�Z���̊J�n���W
    private Vector3 EndPos;         // �I�Z���̒��n���W
    private Vector3 MovePow;        // �ړ������(�}�X�ڊ��Z:0�`3�Ƃ�)
    private bool isMove = false;    // �ړ��J�n�t���O


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

    // �I�Z���𓮂�������(�Ֆ�, �������ɂ������, �J�n���W, ���n���W)
    public void Move(Banmen banmenObj, float fallPow, Vector3 startPos, Vector3 endPos)
    {
        BanmenObj = banmenObj;
        FallPow = fallPow;
        StartPos = startPos;
        EndPos = endPos;
        isMove = true;
    }
}
