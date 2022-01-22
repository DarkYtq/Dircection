using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    Dictionary<Transform, MotionTrigger> m_MotionTriggerDcit = new Dictionary<Transform, MotionTrigger>();
    public int[] MapValueArray;
    public Vector2 MapSize = new Vector2(1600, 900);
    public Vector2 MapItemSize = new Vector2(50, 50);
    public Vector2 MapStartPos = new Vector2(-800, -450);
    public Button[] ItemCreateBtnArray;
    public Transform CurrentClickBtn;

    public GameObject Canvas;
    public Transform TriggerRoot;
    public UIChooseItem ChooseItemCtrl;

    [Header("OnlyInitByCode")]
    public int yLen;
    public int xLen;

    private void Awake()
    {
        Instance = this;
        foreach(var btn in ItemCreateBtnArray)
        {
            btn.onClick.AddListener(() => OnBtnCreateClick(btn.transform));
        }
    }

    [ContextMenu("InitMapGrid")]
    public void InitMapGrid()
    {
        yLen = (int)(MapSize.y / MapItemSize.y);
        xLen = (int)(MapSize.x / MapItemSize.x);
        MapValueArray = new int[yLen * xLen];

        var items = Canvas.GetComponentsInChildren<BoxCollider2D>();
        foreach(var item in items)
        {
            if(item.tag == "MotionTrigger")
            {
                ResetUIPos(item.transform, 1);
            }
        }

        ItemCreateBtnArray = Canvas.GetComponentsInChildren<Button>();
        foreach (var item in ItemCreateBtnArray)
        {
            ResetUIPos(item.transform, 2);
        }
    }

    void ResetUIPos(Transform item, int mapValue)
    {
        var worldPos = item.localPosition;
        var posX = (int)((worldPos.x - MapStartPos.x) / MapItemSize.x);
        var posY = (int)((worldPos.y - MapStartPos.y) / MapItemSize.y);
        if (posX > xLen - 1)
        {
            posX = xLen - 1;
        }

        if (posY > yLen - 1)
        {
            posY = yLen - 1;
        }
        item.localPosition = new Vector3(MapStartPos.x + MapItemSize.x * posX, MapStartPos.y + MapItemSize.y * posY, 0);
        MapValueArray[posY * xLen + posX] = mapValue;
    }


    [ContextMenu("Test")]
    private void Test()
    {
        var prefafb = Resources.Load<GameObject>("MapNull");


        for (int y = 0; y < yLen; y++)
        {
            for (int x = 0; x < xLen; x++)
            {
                if (MapValueArray[y * xLen + x] == 0)
                {
                    var obj = Instantiate(prefafb, Canvas.transform);
                    obj.transform.localPosition = new Vector3(MapStartPos.x + MapItemSize.x * x, MapStartPos.y + MapItemSize.y * y, 0);
                }
            }
        }

    }

    public MotionTrigger GetMotionTrigger(Transform btn)
    {
        MotionTrigger result = null;
        m_MotionTriggerDcit.TryGetValue(btn, out result);
        return result;
    }

    public void InstiateTirgger(Vector3 dirction)
    {
        ChooseItemCtrl.gameObject.SetActive(false);

        var trigger = GetMotionTrigger(CurrentClickBtn);
        if (trigger == null)
        {
            var triggerPrefab = Resources.Load<MotionTrigger>("MotionTrigger");
            trigger = Instantiate(triggerPrefab, TriggerRoot);
        }
        trigger.Direction = dirction;
        trigger.transform.localPosition = CurrentClickBtn.localPosition;
        trigger.InitTrigger();
    }

    public void OnBtnCreateClick(Transform btn)
    {
        CurrentClickBtn = btn;
        ChooseItemCtrl.gameObject.SetActive(true);
        ChooseItemCtrl.transform.localPosition = btn.localPosition + new Vector3(200, 200, 0);
    }

}
