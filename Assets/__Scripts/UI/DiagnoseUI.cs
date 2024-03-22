using UnityEngine;

public class DiagnoseUI : MonoBehaviour
{
    public bool dahEnabled;
    public bool acuteEnabled;
    public bool fluEnabled;
    public bool pneumoniaEnabled;

    public void ToggleDAH(bool isOn)
    {
        dahEnabled = isOn;
    }
    public void ToggleAcute(bool isOn)
    {
        acuteEnabled = isOn;
    }
    public void ToggleFlu(bool isOn)
    {
        fluEnabled = isOn;
    }
    public void TogglePneumonia(bool isOn)
    {
        pneumoniaEnabled = isOn;
    }

    public bool IsEnabled(Treatments.Disease disease)
    {
        switch (disease)
        {
            case Treatments.Disease.DAH: return dahEnabled;
            case Treatments.Disease.Acute: return acuteEnabled;
            case Treatments.Disease.Flu: return fluEnabled;
            case Treatments.Disease.Pneumonia: return pneumoniaEnabled;
        }
        return false;
    }
}
