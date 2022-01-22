using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChooseItem : MonoBehaviour
{
    public GameObject Prefab;

    public List<Button> BtnList;
    public List<Vector3> BtnDirectionList = new List<Vector3>();

    void Awake()
    {
        foreach(var btn in BtnList)
        {
            btn.onClick.AddListener(() => OnBtnClick(btn.gameObject));
        }

    }

    void OnBtnClick(GameObject button)
    {
        for(int i = 0; i < BtnList.Count; i++)
        {
            if(button == BtnList[i].gameObject)
            {
                MapManager.Instance.InstiateTirgger(BtnDirectionList[i]);
            }
        }

    }


    [ContextMenu("InitBtn")]
    public void InitBtn()
    {
        for (int y = 1; y >= -1; y--)
        {
            for (int x = -1; x <= 1; x++)
            {
                var direction = new Vector3(x, y, 0);
                BtnDirectionList.Add(direction);
                var btnRoot = Instantiate(Prefab, transform);
                btnRoot.name = "BtnDirection" + BtnDirectionList.Count;
                var btn = btnRoot.GetComponentInChildren<Button>();
                BtnList.Add(btn);
                GameObject[] dirIconArray = new GameObject[4];
                dirIconArray[0] = btn.transform.Find("Top").gameObject;
                dirIconArray[1] = btn.transform.Find("Down").gameObject;
                dirIconArray[2] = btn.transform.Find("Right").gameObject;
                dirIconArray[3] = btn.transform.Find("Left").gameObject;
                dirIconArray[0].SetActive((int)direction.y > 0);
                dirIconArray[1].SetActive((int)direction.y < 0);
                dirIconArray[2].SetActive((int)direction.x > 0);
                dirIconArray[3].SetActive((int)direction.x < 0);

            }
        }

    }
}
