using UnityEngine;
using UnityEngine.UI;

public class DeskUI : MonoBehaviour
{
    [SerializeField] KnowldegeController knowledge;
    [SerializeField] SymptomsTab symptomsTab;
    [SerializeField] Button[] buttons;
    [SerializeField] Button backButton;
    [SerializeField] RectTransform[] subMenus;
    public static DeskUI Instance;
    GameObject buttonsParent;
    [SerializeField] GameObject deskMainParent;

    int currentDay;

    private void Awake()
    {
        buttonsParent = buttons[0].transform.parent.gameObject;
        backButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() =>
            {
                backButton.gameObject.SetActive(true);
                buttonsParent.SetActive(false);
                EnableSubMenu(button.gameObject.name);
            });
        }
        backButton.onClick.AddListener(() =>
        {
            buttonsParent.SetActive(true);
            foreach (var subMenu in subMenus)
                subMenu.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
        });
    }
    void EnableSubMenu(string name)
    {
        foreach (var subMenu in subMenus)
        {
            if (subMenu.gameObject.name == name)
            {
                subMenu.gameObject.SetActive(true);
            }
        }
    }

    public void StartPC()
    {
        deskMainParent.SetActive(true);
        symptomsTab.Init();

        foreach (var b in buttons)
        {
            if (b.gameObject.name == "Symptoms")
            {
                if (knowledge.KnownSymptoms > 0)
                    b.interactable = true;
                symptomsTab.DisplaySymptoms(knowledge.KnownSymptoms);
            }
        }
    }

}
