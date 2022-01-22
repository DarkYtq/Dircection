using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTrigger : MonoBehaviour
{
    public Vector3 Direction;
    public int Power;
    public GameObject[] DirIconArray;
    public BoxCollider2D Box2d;
    Transform m_Trans;

    private void Awake()
    {
        m_Trans = transform;
    }

    public Transform GetTrans()
    {
        return m_Trans;
    }

    [ContextMenu("InitTrigger")]
    public void InitTrigger()
    {
        if(DirIconArray.Length == 0)
        {
            DirIconArray = new GameObject[4];
            DirIconArray[0] = transform.Find("Top").gameObject;
            DirIconArray[1] = transform.Find("Down").gameObject;
            DirIconArray[2] = transform.Find("Right").gameObject;
            DirIconArray[3] = transform.Find("Left").gameObject;
        }
        DirIconArray[0].SetActive((int)Direction.y > 0);
        DirIconArray[1].SetActive((int)Direction.y < 0);
        DirIconArray[2].SetActive((int)Direction.x > 0);
        DirIconArray[3].SetActive((int)Direction.x < 0);

        Box2d = GetComponent<BoxCollider2D>();
        var boxSize = Power * Direction * PlayerController.BasePowerValue;
        var boxoffset = boxSize / 2;

        if ((int)boxSize.x == 0)
        {
            boxSize.x = 60f;
        }
        if ((int)boxSize.y == 0)
        {
            boxSize.y = 60f;
        }
        Box2d.size = new Vector2(Mathf.Abs(boxSize.x), Mathf.Abs(boxSize.y));
        Box2d.offset = boxoffset;
    }

    public Vector3 GetTargetDistance()
    {
        return transform.position + Power * Direction * PlayerController.BasePowerValue;
    }

    public float GetAnimTime()
    {
        var absX = Mathf.Abs(Direction.x);
        var absY = Mathf.Abs(Direction.y);
        
        return absX> absY? absX * Power * PlayerController.BasePowerValue / PlayerController.MoveSpeed : absY * Power * PlayerController.BasePowerValue / PlayerController.MoveSpeed;
    }


}
