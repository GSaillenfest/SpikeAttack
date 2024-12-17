using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickDebogger : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem; void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerEventData = new PointerEventData(eventSystem); 
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);
            foreach (RaycastResult result in results)
            {
                Debug.Log("UI Object Clicked: " + result.gameObject.name);
            }
        }
    }
}
