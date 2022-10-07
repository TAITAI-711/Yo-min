using UnityEngine;

public struct VecQuaternion
{
    public Vector3 Pos;
    public Quaternion quaternion;
}

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

    /// <summary>
    /// ある点を中心に回転
    /// </summary>
    /// <param name="OriginPos">基準点</param>
    /// <param name="TargetPos">回転したい点</param>
    /// <param name="Angle">角度</param>
    /// <param name="Axis">回転軸(Vector.right, Vector.up　Etc...)</param>
    /// <returns>回転後の座標、回転分のクオータニオン</returns>
    public static VecQuaternion PointRotate(Vector3 OriginPos, Vector3 TargetPos, float Angle, Vector3 Axis)
    {
        VecQuaternion vecQuaternion;

        Quaternion AngleAxis = Quaternion.AngleAxis(Angle, Axis); // 回転軸と角度

        Vector3 Pos = TargetPos; // 自身の座標

        Pos -= OriginPos;
        Pos = AngleAxis * Pos;
        Pos += OriginPos;

        vecQuaternion.Pos = Pos; // 現在の座標
        vecQuaternion.quaternion = AngleAxis; // 回転

        return vecQuaternion;
    }

    /// <summary>
    /// 二点間の角度を求める
    /// 戻り値：0～360
    /// </summary>
    public static float TwoPointAngle360(Vector3 origin, Vector3 target)
    {
        Vector3 dt = target - origin;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;
        if (degree < 0)//上方向を基準に時計回りに0~360の値に補正
        {
            degree += 360;
        }
        return degree;
    }

    // 二点間の角度を求める(UnityのTransform角度と一致)
    // 戻り値：0～360(Z角度)
    public static float UnityTwoPointAngle360(Vector3 origin, Vector3 target)
    {
        Vector3 dt = target - origin;
        float rad = Mathf.Atan2(dt.y, dt.x);
        float degree = rad * Mathf.Rad2Deg;
        if (degree < 0) // 右方向を基準に時計回りに0~360の値に補正
        {
            degree += 360;
        }
        return degree;
    }
}
