using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicsGraphController : MonoBehaviour
{
    string tooltipText;

    public void SetToolTipText(List<string> selectedNames)
    {
        string text = "";
        foreach (string name in selectedNames)
        {
            print(name);
            text += name + "\n";
        }

        tooltipText = text;
    }
    static DynamicsGraphController instance;
    public static DynamicsGraphController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DynamicsGraphController>(true);
            return instance;
        }
    }

    int previousDynamicsValue = 0;

    [SerializeField] RectTransform linePrefab;
    [SerializeField] Button dynamicsButton;
    /*void Awake()
    {
        foreach (Transform t in transform)
        {
            if (t.name != "Day0")
            {
                print(t.name);
                t.gameObject.SetActive(false);
            }
        }
    }*/

    public void EnableDynamics()
    {
        dynamicsButton.interactable = true;
    }

    public void SetDynamics(int day, int guessedMedicines)
    {
        var x = transform.GetChild(day);
        x.gameObject.SetActive(true);
        for (int i = 0; i < x.childCount; i++)
        {
            if (i == (3 - guessedMedicines))
                x.GetChild(i).gameObject.SetActive(true);
        }
        DrawLine(day - 1, previousDynamicsValue, guessedMedicines);
        previousDynamicsValue = guessedMedicines;
    }

    [ContextMenu("Test")]
    public void Test()
    {
        DrawLine(0, 0, 3);
    }

    public void DrawLine(int previousDay, int valueFrom, int valueTo)
    {
        RectTransform fromParent = transform.GetChild(previousDay).GetComponent<RectTransform>();
        RectTransform toParent = transform.GetChild(previousDay + 1).GetComponent<RectTransform>();
        RectTransform from = transform.GetChild(previousDay).GetChild(3 - valueFrom).GetComponent<RectTransform>();
        RectTransform to = transform.GetChild(previousDay+1).GetChild(3 - valueTo).GetComponent<RectTransform>();
        var line = Instantiate(linePrefab, from);
        line.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tooltipText;
        line.anchoredPosition = Vector2.zero;
        float zRotation = Mathf.Rad2Deg * Mathf.Atan2(to.position.y - from.position.y, to.position.x - from.position.x);
        line.eulerAngles = new Vector3(0, 0, zRotation);
        line.GetChild(0).eulerAngles = new Vector3(0, 0, 0);

        //calc length

        float xDelta = toParent.anchoredPosition.x - fromParent.anchoredPosition.x;
        float yDelta = to.anchoredPosition.y - from.anchoredPosition.y;
        float hypothenus = Mathf.Sqrt(xDelta * xDelta + yDelta * yDelta);
        line.sizeDelta = new Vector2(hypothenus, 15);
    }
}
