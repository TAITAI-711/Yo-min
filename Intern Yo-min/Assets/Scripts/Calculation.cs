using UnityEngine;

public struct VecQuaternion
{
    public Vector3 Pos;
    public Quaternion quaternion;
}

static public class Calculation
{
    /// <summary>
    /// �ˏo���x�̌v�Z
    /// </summary>
    /// <param name="StartPos">�ˏo�J�n���W</param>
    /// <param name="EndPos">���n���W</param>
    /// <param name="Angle">�ˏo�p�x</param>
    /// <param name="Gravity">�d��</param>
    /// <returns>�ˏo���x</returns>
    static public Vector3 InjectionSpeed(Vector3 StartPos, Vector3 EndPos, float Angle, float Gravity)
    {
        // �ˏo�p�����W�A���ɕϊ�
        float rad = Angle * Mathf.PI / 180;

        // ���������̋���x
        float x = Vector2.Distance(new Vector2(StartPos.x, StartPos.z), new Vector2(EndPos.x, EndPos.z));

        // ���������̋���y
        float y = StartPos.y - EndPos.y;

        // �Ε����˂̌����������x�ɂ��ĉ���
        float speed = Mathf.Sqrt(-Gravity * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // �����𖞂����������Z�o�ł��Ȃ����Vector3.zero��Ԃ�
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(EndPos.x - StartPos.x, x * Mathf.Tan(rad), EndPos.z - StartPos.z).normalized * speed);
        }
    }

    /// <summary>
    /// ����_�𒆐S�ɉ�]
    /// </summary>
    /// <param name="OriginPos">��_</param>
    /// <param name="TargetPos">��]�������_</param>
    /// <param name="Angle">�p�x</param>
    /// <param name="Axis">��]��(Vector.right, Vector.up�@Etc...)</param>
    /// <returns>��]��̍��W�A��]���̃N�I�[�^�j�I��</returns>
    public static VecQuaternion PointRotate(Vector3 OriginPos, Vector3 TargetPos, float Angle, Vector3 Axis)
    {
        VecQuaternion vecQuaternion;

        Quaternion AngleAxis = Quaternion.AngleAxis(Angle, Axis); // ��]���Ɗp�x

        Vector3 Pos = TargetPos; // ���g�̍��W

        Pos -= OriginPos;
        Pos = AngleAxis * Pos;
        Pos += OriginPos;

        vecQuaternion.Pos = Pos; // ���݂̍��W
        vecQuaternion.quaternion = AngleAxis; // ��]

        return vecQuaternion;
    }

    /// <summary>
    /// ��_�Ԃ̊p�x�����߂�
    /// �߂�l�F0�`360
    /// </summary>
    public static float TwoPointAngle360(Vector3 origin, Vector3 target)
    {
        Vector3 dt = target - origin;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;
        if (degree < 0)//���������Ɏ��v����0~360�̒l�ɕ␳
        {
            degree += 360;
        }
        return degree;
    }

    // ��_�Ԃ̊p�x�����߂�(Unity��Transform�p�x�ƈ�v)
    // �߂�l�F0�`360(Z�p�x)
    public static float UnityTwoPointAngle360(Vector3 origin, Vector3 target)
    {
        Vector3 dt = target - origin;
        float rad = Mathf.Atan2(dt.y, dt.x);
        float degree = rad * Mathf.Rad2Deg;
        if (degree < 0) // �E��������Ɏ��v����0~360�̒l�ɕ␳
        {
            degree += 360;
        }
        return degree;
    }
}
