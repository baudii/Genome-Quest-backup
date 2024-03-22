using TMPro.Examples;
using UnityEngine;

public class SymptomsTab : MonoBehaviour
{
    [SerializeField] GameObject shortnessOfBreathClue;
    [SerializeField] GameObject bloodSpittingClue;
    [SerializeField] GameObject chestPainClue;
    [SerializeField] Transform symptomsContainer;

    public void Init()
    {
        int i = 0;
        foreach(Transform t in symptomsContainer)
        {
            if (i >= 5)
                break;
            t.GetChild(0).gameObject.SetActive(false);
            i++;
        }
    }

    public void DisplaySymptoms(int amount)
    {
        int i = 0;
        foreach (Transform t in symptomsContainer)
        {
            if (i >= amount)
            {
                return;
            }

            t.GetChild(0).gameObject.SetActive(true);

            i++;
        }
    }
    bool chestPainEnabled;
    bool shortnessOfBreathEnabled;
    bool bloodSpittingEnabled;

    public void EnableChestPain()
    {
        if (chestPainEnabled)
            return;
        chestPainEnabled = true;
        KnowldegeController.Notify();
        chestPainClue.SetActive(true);
    }
    public void EnableShortnessOfBreath()
    {
        if (shortnessOfBreathEnabled)
            return;
        shortnessOfBreathEnabled = true;
        KnowldegeController.Notify();
        shortnessOfBreathClue.SetActive(true);
    }
    public void EnableBloodSpitting()
    {
        if (bloodSpittingEnabled)
            return;
        bloodSpittingEnabled = true;
        KnowldegeController.Notify();
        bloodSpittingClue.SetActive(true);
    }
}
