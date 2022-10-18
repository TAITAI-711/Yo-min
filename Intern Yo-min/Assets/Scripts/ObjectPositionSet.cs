using UnityEngine;


[SelectionBase]
public class ObjectPositionSet : MonoBehaviour
{
    // GameConfig.GridSize ��1�ڐ���Ƃ����ꍇ�́A�O���b�h���W
    [SerializeField] public Vector2Int gridPos = Vector2Int.zero;
    public static readonly float GridSize = 5.0f;

    /// <summary>
    /// �O���b�h�ړ��ʂ��w�肵�Ĉړ�����
    /// </summary>
    public void Move(Vector2Int vec2)
    {
        gridPos += vec2;
        Vector3 Pos = GetGlobalPosition(gridPos);
        Pos.y = transform.position.y;
        transform.position = Pos;
    }

    /// <summary>
    /// �O���b�h���W��Global���W�ɕϊ�����
    /// </summary>
    public static Vector3 GetGlobalPosition(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * GridSize,
            0,
            gridPos.y * GridSize);
    }
}