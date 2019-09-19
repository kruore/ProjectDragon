using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    public GameObject stick;
    [SerializeField]
    private Vector3 stickFirstPos;
    private Vector3 joyVec;
    public float radius;
    public UIEventTrigger stickEventTrigger;
    // Start is called before the first frame update
    void Start()
    {
        stickEventTrigger = gameObject.GetComponent<UIEventTrigger>();
        radius = gameObject.GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        stickFirstPos = gameObject.transform.position;

      //  float phoneWindowSize = transform.parent.GetComponent<UIRoot>().fitWidth;
        //radius *= phoneWindowSize;
    }
    public void DragingJoyStick(BaseEventData _data)
    {
        PointerEventData data = _data as PointerEventData;
        Vector3 pos = data.position;

        joyVec = (pos - stickFirstPos).normalized;
        float distance = Vector3.Distance(pos, stickFirstPos);

        if (distance < radius)
            stick.transform.position = stickFirstPos + joyVec * distance;
        else
            stick.transform.position = stickFirstPos + joyVec * radius;
    }

    public void DragEndJoyStick()
    {
        stick.transform.position = stickFirstPos;
        joyVec = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
