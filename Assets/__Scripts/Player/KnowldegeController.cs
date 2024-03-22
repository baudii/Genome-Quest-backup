using System.Collections.Generic;
using UnityEngine;

public class KnowldegeController : MonoBehaviour
{
    public int KnownSymptoms = 0;
    public int KnownVitals = 0;
    public int KnownDiagnoses = 0;
    public void SetSymptoms(int amount)
    {
        KnownSymptoms = amount;
        NotificationUI.Instance.Notify("Knowledge obtained");
    }

    public void AddSymptoms(int amount)
    {
        SetSymptoms(KnownSymptoms + amount);
    }

    public void SetVitals(int amount)
    {
        KnownVitals = amount;
    }

    public void SetDiagnoses(int amount)
    {
        KnownDiagnoses = amount;
        NotificationUI.Instance.Notify("Knowledge obtained");
    }

    public static void Notify()
    {
        NotificationUI.Instance.Notify("Knowledge obtained");
    }
}
