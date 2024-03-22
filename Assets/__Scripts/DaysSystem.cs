using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DaysSystem : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    [SerializeField] Animator blackScreenAnimator;
    [SerializeField] public UnityEvent OnDay1Start;
    [SerializeField] public UnityEvent OnDay1End;
    [SerializeField] public UnityEvent OnDay2Start;
    [SerializeField] public UnityEvent OnDay2End;
    [SerializeField] public UnityEvent OnDay3Start;
    [SerializeField] public UnityEvent OnDay3End;

    //currentday, currentguesses, previousguesses
    public UnityEvent<int,int,int> OnDayStart;
    public UnityEvent OnDayEnd;

    public static DaysSystem Instance;
    public int currentDay = 0;
    int previousDayCorrectGuesses;
    int currentDayCorrectGuesses;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        var dialogues = FindObjectsOfType<CharacterDialogueComponent>(true);
        foreach (var dialogue in dialogues)
        {
            dialogue.Init();
        }
    }

    private void Start()
    {
        OnDayStart?.Invoke(0, 0, 0);
    }


    public void StartNextDay(int correctGuesses)
    {
        if (currentDay >= 3)
        {
            GameEnd.Instance.EndGame(correctGuesses);
            return;
        }
        OnDayEnd?.Invoke();
        currentDay++;
        previousDayCorrectGuesses = currentDayCorrectGuesses;
        currentDayCorrectGuesses = correctGuesses;
        DynamicsGraphController.Instance.SetDynamics(currentDay, correctGuesses);
        StartCoroutine(TransitionToNextDay());
    }

    IEnumerator TransitionToNextDay()
    {
        blackScreenAnimator.gameObject.SetActive(true);
        blackScreenAnimator.Play("BlackScreenFadeIn");
        yield return new WaitForSeconds(2f);
        var daysLeftText = blackScreenAnimator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (4 - currentDay == 1)
            daysLeftText.text = "1 day left";
        else
            daysLeftText.text = $"{4-currentDay} days left";
        daysLeftText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        InvokeEvent();
        pm.SetInitialPosition();
        PC_Controller.Instance.TurnOffPcUI();
        Interactor.toggled = false;
        Objective.Instance.ObjectiveGoToPatient();

        blackScreenAnimator.Play("BlackScreenFadeOut");
        daysLeftText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        blackScreenAnimator.gameObject.SetActive(false);
    }

    void InvokeEvent()
    {
        switch (currentDay)
        {
            case 1:
                OnDay1Start?.Invoke();
                break;
            case 2: 
                OnDay1End?.Invoke();
                OnDay2Start?.Invoke();
                break;
            case 3:
                OnDay2End?.Invoke();
                OnDay3Start?.Invoke();
                break;
            case 4:
                OnDay3End?.Invoke();
                break;
        }
        OnDayStart?.Invoke(currentDay, currentDayCorrectGuesses, previousDayCorrectGuesses);
    }
}
