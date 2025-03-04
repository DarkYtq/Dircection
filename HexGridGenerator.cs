using System.Collections.Generic;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    public int gridWidth = 10; // �����ȣ������ε�������
    public int gridHeight = 10; // ����߶ȣ������ε�������
    public float hexRadius = 1f; // �����εİ뾶
    public GameObject hexPrefab; // �����ε�Ԫ���Ԥ����

    private float hexWidth; // �����εĿ��
    private float hexHeight; // �����εĸ߶�
    private float hexHorizontalSpacing; // ������ˮƽ���
    private float hexVerticalSpacing; // �����δ�ֱ���

    void Start()
    {
        // ���������εĳߴ�
        hexWidth = 2f * hexRadius; // ���
        hexHeight = Mathf.Sqrt(3f) * hexRadius; // �߶�
        hexHorizontalSpacing = hexWidth * 0.75f; // ˮƽ��ࣨ�ص����֣�
        hexVerticalSpacing = hexHeight; // ��ֱ���

        // ���ɷ���ͼ
        GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // ���������ε�ƫ��λ��
                float xOffset = x * hexHorizontalSpacing;
                float yOffset = y * hexVerticalSpacing;

                // ż��������ƫ�ư�������θ߶�
                if (x % 2 == 1)
                {
                    yOffset += hexHeight * 0.5f;
                }

                // ���������ε���������
                Vector3 hexPosition = new Vector3(xOffset, 0f, yOffset);

                // ʵ���������ε�Ԫ��
                GameObject hex = Instantiate(hexPrefab, hexPosition, Quaternion.identity, transform);
                hex.name = $"Hex ({x}, {y})";
                hex.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360),0 );
                hex.transform.localScale = Vector3.one *Random.Range(.5f,1.5f);
            }
        }
    }

    void OnDrawGizmos()
    {
        // ���ӻ����������񣨽��ڱ༭������ʾ��
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
        // ���������εı߽�
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
