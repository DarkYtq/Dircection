using System.Collections.Generic;
using UnityEngine;

public class VoronoiGenerator : MonoBehaviour
{
    public int width = 512; // Voronoi 图的宽度
    public int height = 512; // Voronoi 图的高度
    public int pointCount = 10; // 随机站点的数量
    public Color[] colors; // 每个站点的颜色
    private List<Vector2> points; // 站点列表
    private List<Vector2> centers; // Voronoi 区域的中心点

    public bool IsNewMid;

    public int[] areaPointsNum;

    void Start()
    {
        GenerateVoronoi();
        OutputCenters();
    }

    void GenerateVoronoi()
    {
        // 创建一个纹理来存储 Voronoi 图
        Texture2D texture = new Texture2D(width, height);

        // 随机生成站点
        points = new List<Vector2>();
        colors = new Color[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            points.Add(new Vector2(Random.Range(0, width), Random.Range(0, height)));
            colors[i] = new Color(Random.value, Random.value, Random.value); // 随机颜色
        }

        // 初始化中心点列表
        centers = new List<Vector2>(new Vector2[pointCount]);
        areaPointsNum = new int[pointCount];

        // 遍历每个像素，计算到站点的距离
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pixel = new Vector2(x, y);
                int closestPointIndex = -1;
                float closestDistance = float.MaxValue;

                // 找到最近的站点
                for (int i = 0; i < points.Count; i++)
                {
                    float distance = Vector2.Distance(pixel, points[i]);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                // 设置像素颜色为最近站点的颜色
                texture.SetPixel(x, y, colors[closestPointIndex]);

                // 计算每个区域的中心点
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

        // 计算每个区域的平均中心点
       

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
                centers[i] /= (float)width * height; // 归一化
            }
        }


        // 应用纹理更改
        texture.Apply();

        // 在屏幕上显示 Voronoi 图
        GetComponent<Renderer>().material.mainTexture = texture;
    }

    void OutputCenters()
    {
        Debug.Log("Voronoi区域中心点：");
        for (int i = 0; i < centers.Count; i++)
        {
            Debug.Log($"区域 {i}: {centers[i]}");
        }
    }

    void OnDrawGizmos()
    {
        // 在场景视图中绘制站点
        if (points == null) return;

        Gizmos.color = Color.red;
        foreach (var point in points)
        {
            Gizmos.DrawSphere(new Vector3(point.x , 0, point.y), 0.5f);
        }
    }
}
