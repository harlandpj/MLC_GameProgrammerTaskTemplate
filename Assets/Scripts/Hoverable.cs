using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<GameObject> OnHoverEnter = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> OnHoverExit = new UnityEvent<GameObject>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter?.Invoke(eventData.selectedObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit?.Invoke(eventData.selectedObject);
    }
}
