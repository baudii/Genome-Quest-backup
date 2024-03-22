using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VitalsUI : MonoBehaviour
{
    [SerializeField] Button xButton;
    [SerializeField, Range(0,1)] float[] initialValues;
    [SerializeField] Color critical;
    [SerializeField] Color unstable;
    [SerializeField] Color stable;
    [SerializeField] KnowldegeController knowldegeController;

    public enum Vitals
    {
        heartRate,
        temperature,
        respiratoryRate,
        bloodPressure,
        glucose,
        saturation,
        nutrtions
    }

    List<Transform> vitalsTransforms;

    private void OnEnable()
    {
        EnableSliders(knowldegeController.KnownVitals);
    }

    private void Awake()
    {
        Day0OnStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            xButton.onClick?.Invoke();
        }
    }

    public void EnableSliders(int currentKnowledge)
    {
        for (int i = 0; i < currentKnowledge; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            SetSliderValue(i, initialValues[i]);
        }
    }

    void Day0OnStart()
    {
        vitalsTransforms = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            vitalsTransforms.Add(transform.GetChild(i));
            if (i >= 2)
                vitalsTransforms[i].gameObject.SetActive(false);
        }
        SetSliderValue((int)Vitals.heartRate, initialValues[(int)Vitals.heartRate]);
        SetSliderValue((int)Vitals.temperature, initialValues[(int)Vitals.temperature]);
    }

    void SetSliderValue(int position, float value)
    {
        var slider = vitalsTransforms[position].GetChild(0).GetComponent<Slider>();
        slider.value = value;
        SetSliderColor(slider.GetComponent<Image>(), value);
    }
    
    void SetSliderColor(Image image, float value)
    {
        if (value >= 0.7f || value <= 0.3f)
        {
            image.color = critical;
            image.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "critical";
        }
        else if (value >= 0.6f || value <= 0.4f)
        {
            image.color = unstable;
            image.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "unstable";
        }
        else
        {
            image.color = stable;
            image.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "stable";
        }
    }

    void SetActiveSlider(int position, bool active)
    {
        vitalsTransforms[position].gameObject.SetActive(active);
    }
}
