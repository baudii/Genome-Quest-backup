using Newtonsoft.Json.Serialization;
using OpenAI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrescribeTreatment : MonoBehaviour
{
    [SerializeField] DiagnoseUI diagnoseUI;
    [SerializeField] TextMeshProUGUI choose3Text;
    [SerializeField] Button endDayButton;
    [SerializeField] Toggle[] toggles;
    [SerializeField] int[][] s;
    [SerializeField] Treatments[] treatments;

    int correct;
    int selected;

    private void Start()
    {
        OnDayStart(0);
        selectedNames = new List<string>();
    }

    private void OnEnable()
    {
        SetToggles();
    }

    void SetToggles()
    {
        if (selected >= 3)
            return;
        foreach (var treatment in treatments)
        {
            bool toEnable = false;
            foreach (var disease in treatment.diseases)
            {
                if (diagnoseUI.IsEnabled(disease))
                {
                    toEnable = true;
                }
            }
            foreach (var toggle in treatment.adjasentToggles)
            {
                toggles[toggle].interactable = toEnable;
                if (!toEnable)
                    toggles[toggle].isOn = false;
            }
        }
    }

    public void OnDayStart(int day)
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((isOn) => OnToggleSwitch(isOn, toggle));
        }
    }

    List<string> selectedNames;

    public void OnToggleSwitch(bool isOn, Toggle currentToggle)
    {
        if (isOn)
        {
            selected++;
            selectedNames.Add(currentToggle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
        }
        else
        {
            selected--;
            selectedNames.Remove(currentToggle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
        } 

        if (currentToggle.name == "+")
        {
            if (isOn)
                correct++;
            else
                correct--;
        }

        choose3Text.text = "Choose at least 2";
        if (selected >= 2)
        {
            choose3Text.gameObject.SetActive(false);
            endDayButton.interactable = true;
        }
        else
        {
            choose3Text.gameObject.SetActive(true);
            endDayButton.interactable = false;
        }
        if (selected >= 3)
        {
            foreach (Toggle toggle in toggles)
            {
                if (toggle.isOn == false)
                {
                    toggle.interactable = false;
                }
            }
        }
        else if (selected < 3)
        {
            SetToggles();
        }
    }

    public void SubmitChoice()
    {
        DynamicsGraphController.Instance.SetToolTipText(selectedNames);
        DaysSystem.Instance.StartNextDay(correct);
    }

    private void OnDestroy()
    {
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }
}
[Serializable]
public class Treatments
{
    public enum Disease
    {
        DAH,
        Acute,
        Flu,
        Pneumonia
    }

    public Disease[] diseases;
    public int[] adjasentToggles;
}
