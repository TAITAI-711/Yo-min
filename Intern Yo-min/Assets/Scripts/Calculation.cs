using UnityEngine;

static public class Calculation
{
    /// <summary>
    /// 射出速度の計算
    /// </summary>
    /// <param name="StartPos">射出開始座標</param>
    /// <param name="EndPos">着地座標</param>
    /// <param name="Angle">射出角度</param>
    /// <param name="Gravity">重力</param>
    /// <returns>射出速度</returns>
    static public Vector3 InjectionSpeed(Vector3 StartPos, Vector3 EndPos, float Angle, float Gravity)
    {
        // 射出角をラジアンに変換
        float rad = Angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(StartPos.x, StartPos.z), new Vector2(EndPos.x, EndPos.z));

        // 垂直方向の距離y
        float y = StartPos.y - EndPos.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Gravity * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(EndPos.x - StartPos.x, x * Mathf.Tan(rad), EndPos.z - StartPos.z).normalized * speed);
        }
    }
}
