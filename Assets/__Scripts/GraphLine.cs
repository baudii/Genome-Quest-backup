using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GraphLine : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI tooltip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.transform.parent.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.transform.parent.gameObject.SetActive(false);
    }

    public void Init(string message)
    {
        tooltip.text = message;
    }
}
