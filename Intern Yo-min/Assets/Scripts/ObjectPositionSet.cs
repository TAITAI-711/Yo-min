using UnityEngine;


[SelectionBase]
public class ObjectPositionSet : MonoBehaviour
{
    // GameConfig.GridSize を1目盛りとした場合の、グリッド座標
    [SerializeField] public Vector2Int gridPos = Vector2Int.zero;
    public static readonly float GridSize = 5.0f;

    /// <summary>
    /// グリッド移動量を指定して移動する
    /// </summary>
    public void Move(Vector2Int vec2)
    {
        gridPos += vec2;
        transform.position = GetGlobalPosition(gridPos);
    }

    /// <summary>
    /// グリッド座標をGlobal座標に変換する
    /// </summary>
    public static Vector3 GetGlobalPosition(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * GridSize,
            0,
            gridPos.y * GridSize);
    }
}
