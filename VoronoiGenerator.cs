using System.Collections.Generic;
using UnityEngine;

public class VoronoiGenerator : MonoBehaviour
{
    public int width = 512; // Voronoi ͼ�Ŀ��
    public int height = 512; // Voronoi ͼ�ĸ߶�
    public int pointCount = 10; // ���վ�������
    public Color[] colors; // ÿ��վ�����ɫ
    private List<Vector2> points; // վ���б�
    private List<Vector2> centers; // Voronoi ��������ĵ�

    public bool IsNewMid;

    public int[] areaPointsNum;

    void Start()
    {
        GenerateVoronoi();
        OutputCenters();
    }

    void GenerateVoronoi()
    {
        // ����һ���������洢 Voronoi ͼ
        Texture2D texture = new Texture2D(width, height);

        // �������վ��
        points = new List<Vector2>();
        colors = new Color[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            points.Add(new Vector2(Random.Range(0, width), Random.Range(0, height)));
            colors[i] = new Color(Random.value, Random.value, Random.value); // �����ɫ
        }

        // ��ʼ�����ĵ��б�
        centers = new List<Vector2>(new Vector2[pointCount]);
        areaPointsNum = new int[pointCount];

        // ����ÿ�����أ����㵽վ��ľ���
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pixel = new Vector2(x, y);
                int closestPointIndex = -1;
                float closestDistance = float.MaxValue;

                // �ҵ������վ��
                for (int i = 0; i < points.Count; i++)
                {
                    float distance = Vector2.Distance(pixel, points[i]);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                // ����������ɫΪ���վ�����ɫ
                texture.SetPixel(x, y, colors[closestPointIndex]);

                // ����ÿ����������ĵ�
                //if (centers[closestPointIndex] == Vector2.zero)
                //{
                //    centers[closestPointIndex] = pixel;
                //       areaPointsNum[closestPointIndex]++;
                //}
                //else
                {
                    centers[closestPointIndex] += pixel;
                    areaPointsNum[closestPointIndex]++;
                }
            }
        }

        // ����ÿ�������ƽ�����ĵ�
       

        if(IsNewMid)
        {
            for (int i = 0; i < centers.Count; i++)
            {
                centers[i] = centers[i] / areaPointsNum[i];
            }
        }
        else
        {
            for (int i = 0; i < centers.Count; i++)
            {
                centers[i] /= (float)width * height; // ��һ��
            }
        }


        // Ӧ���������
        texture.Apply();

        // ����Ļ����ʾ Voronoi ͼ
        GetComponent<Renderer>().material.mainTexture = texture;
    }

    void OutputCenters()
    {
        Debug.Log("Voronoi�������ĵ㣺");
        for (int i = 0; i < centers.Count; i++)
        {
            Debug.Log($"���� {i}: {centers[i]}");
        }
    }

    void OnDrawGizmos()
    {
        // �ڳ�����ͼ�л���վ��
        if (points == null) return;

        Gizmos.color = Color.red;
        foreach (var point in points)
        {
            Gizmos.DrawSphere(new Vector3(point.x , 0, point.y), 0.5f);
        }
    }
}
