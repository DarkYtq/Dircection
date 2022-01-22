using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 Direction = new Vector3(1, 1, 0);
    public float AnimTime = 2f;
    Vector3 m_FramePower = Vector3.zero;
    Transform m_Trans;
    public GameObject[] DirIconArray;
    public static float BasePowerValue = 50f;
    public static float MoveSpeed = 100f;
    Coroutine m_MoveCoroutine;

    void Awake()
    {
        m_Trans = transform;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var trigger = other.GetComponent<MotionTrigger>();
        if (trigger != null)
        {
            var signX = Mathf.Sign(Direction.x);
            var signY = Mathf.Sign(Direction.y);
            if ((int)trigger.Direction.x != 0)
            {
                var realPower = trigger.Power * BasePowerValue - Mathf.Abs(trigger.GetTrans().localPosition.x - m_Trans.localPosition.x);
                m_FramePower.x = signX == Mathf.Sign(trigger.Direction.x) ? (int)(signX * realPower) : (int)(-signX * realPower);
            }
            if ((int)trigger.Direction.y != 0)
            {
                var realPower = trigger.Power * BasePowerValue - Mathf.Abs(trigger.GetTrans().localPosition.y - m_Trans.localPosition.y);
                m_FramePower.y = signY == Mathf.Sign(trigger.Direction.y) ? (int)(signY * realPower) : (int)(-signY * realPower);
            }
            AnimTime = trigger.GetAnimTime();
            Debug.Log(AnimTime);
        }
    }

    private void FixedUpdate()
    {
        if((int)m_FramePower.x != 0 || (int)m_FramePower.y != 0)
        {
            Debug.Log(m_FramePower);

            var targetPos = m_Trans.localPosition + m_FramePower;
            if(m_MoveCoroutine != null)
            {
                StopCoroutine(m_MoveCoroutine);
            }
            m_MoveCoroutine = StartCoroutine(MoveCoroutine(targetPos));

            m_FramePower = Vector3.zero ;

        }
    }

    IEnumerator MoveCoroutine(Vector3 targetPos)
    {
        var startPos = m_Trans.localPosition;
        float timer = 0;
        while(timer < AnimTime)
        {
            timer += Time.deltaTime;
            m_Trans.localPosition =  Vector3.Lerp(startPos, targetPos, timer / AnimTime);
            yield return null;
        }
        m_Trans.localPosition = targetPos;
    }


    [ContextMenu("InitTrigger")]
    public void InitTrigger()
    {
        DirIconArray = new GameObject[4];
        DirIconArray[0] = transform.Find("Top").gameObject;
        DirIconArray[1] = transform.Find("Down").gameObject;
        DirIconArray[2] = transform.Find("Right").gameObject;
        DirIconArray[3] = transform.Find("Left").gameObject;

        DirIconArray[0].SetActive((int)Direction.y > 0);
        DirIconArray[1].SetActive((int)Direction.y < 0);
        DirIconArray[2].SetActive((int)Direction.x > 0);
        DirIconArray[3].SetActive((int)Direction.x < 0);
    }
}
