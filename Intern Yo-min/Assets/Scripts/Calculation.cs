using UnityEngine;

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
}
