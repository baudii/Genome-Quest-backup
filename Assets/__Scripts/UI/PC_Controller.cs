using UnityEngine;
using UnityEngine.UI;

public class PC_Controller : MonoBehaviour
{
    [SerializeField] Button xButton;
    [SerializeField] Button assignTreatButton;
    [SerializeField] GameObject[] tabs;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject backButton;
    [SerializeField] PlayerMovement pm;
    [SerializeField] Interactor pi;

    static PC_Controller instance;
    public static PC_Controller Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PC_Controller>(true);
            return instance;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            xButton.onClick?.Invoke();
        }
    }
    public void TurnOffPcUI()
    {
        gameObject.SetActive(false);
        pm.UnfreezeInput();
        pi.UnfreezeInput();
    }

    void OnEnable()
    {
        assignTreatButton.interactable = Objective.Instance.canAssignTreatment;
        buttons.SetActive(true);
        backButton.SetActive(false);
        foreach (GameObject go in tabs)
        {
            go.SetActive(false);
        }
    }
}
