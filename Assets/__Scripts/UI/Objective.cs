using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objectiveTextObject;
    string visitPatient = "visit patient";
    string goToYourDesk = "go to your desk";
    Animator animator;
    static Objective instance;
    public bool canAssignTreatment;
    public static Objective Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Objective>(true);
            return instance;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ObjectiveAssignTreatment()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("NewObjective");
        objectiveTextObject.text = "Prescribe Treatment";
    }

    public void ObjectiveGoToDesk()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("NewObjective");
        objectiveTextObject.text = goToYourDesk;
        canAssignTreatment = true;
    }

    public void ObjectiveGoToPatient()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("NewObjective");
        objectiveTextObject.text = visitPatient;
        canAssignTreatment = false;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
