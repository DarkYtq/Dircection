using System.Collections.Generic;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    public int gridWidth = 10; // 网格宽度（六边形的数量）
    public int gridHeight = 10; // 网格高度（六边形的数量）
    public float hexRadius = 1f; // 六边形的半径
    public GameObject hexPrefab; // 六边形单元格的预制体

    private float hexWidth; // 六边形的宽度
    private float hexHeight; // 六边形的高度
    private float hexHorizontalSpacing; // 六边形水平间距
    private float hexVerticalSpacing; // 六边形垂直间距

    void Start()
    {
        // 计算六边形的尺寸
        hexWidth = 2f * hexRadius; // 宽度
        hexHeight = Mathf.Sqrt(3f) * hexRadius; // 高度
        hexHorizontalSpacing = hexWidth * 0.75f; // 水平间距（重叠部分）
        hexVerticalSpacing = hexHeight; // 垂直间距

        // 生成蜂窝图
        GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // 计算六边形的偏移位置
                float xOffset = x * hexHorizontalSpacing;
                float yOffset = y * hexVerticalSpacing;

                // 偶数列向下偏移半个六边形高度
                if (x % 2 == 1)
                {
                    yOffset += hexHeight * 0.5f;
                }

                // 计算六边形的世界坐标
                Vector3 hexPosition = new Vector3(xOffset, 0f, yOffset);

                // 实例化六边形单元格
                GameObject hex = Instantiate(hexPrefab, hexPosition, Quaternion.identity, transform);
                hex.name = $"Hex ({x}, {y})";
                hex.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360),0 );
                hex.transform.localScale = Vector3.one *Random.Range(.5f,1.5f);
            }
        }
    }

    void OnDrawGizmos()
    {
        // 可视化六边形网格（仅在编辑器中显示）
        Gizmos.color = Color.yellow;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                float xOffset = x * hexHorizontalSpacing;
                float yOffset = y * hexVerticalSpacing;

                if (x % 2 == 1)
                {
                    yOffset += hexHeight * 0.5f;
                }

                Vector3 hexPosition = new Vector3(xOffset, 0f, yOffset);
                DrawHexGizmo(hexPosition);
            }
        }
    }

    void DrawHexGizmo(Vector3 position)
    {
        // 绘制六边形的边界
        float angleDeg = 60f;
        Vector3 prevPoint = Vector3.zero;

        for (int i = 0; i <= 6; i++)
        {
            float angleRad = Mathf.Deg2Rad * (angleDeg * i);
            Vector3 point = new Vector3(
                position.x + hexRadius * Mathf.Cos(angleRad),
                position.y,
                position.z + hexRadius * Mathf.Sin(angleRad)
            );

            if (i > 0)
            {
                Gizmos.DrawLine(prevPoint, point);
            }

            prevPoint = point;
        }
    }
}
