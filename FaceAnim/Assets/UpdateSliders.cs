using UnityEngine;
using UnityEngine.UI;

public class UpdateSliders : MonoBehaviour {

    public EmotionControl control;
    public Slider[] sliders;

    public void UpdateUI()
    {
        sliders[0].value = control.ecstasy;
        sliders[1].value = control.admiration;
        sliders[2].value = control.terror;
        sliders[3].value = control.amazement;
        sliders[4].value = control.grief;
        sliders[5].value = control.loathing;
        sliders[6].value = control.rage;
        sliders[7].value = control.vigilance;
    }

    public void EcstasyUpdate(float value)
    {
        control.ecstasy = value;
    }

    public void AdmirationUpdate(float value)
    {
        control.admiration = value;
    }

    public void TerrorUpdate(float value)
    {
        control.terror = value;
    }

    public void AmazementUpdate(float value)
    {
        control.amazement = value;
    }

    public void GriefUpdate(float value)
    {
        control.grief = value;
    }

    public void LoathingUpdate(float value)
    {
        control.loathing = value;
    }

    public void RageUpdate(float value)
    {
        control.rage = value;
    }

    public void VigilanceUpdate(float value)
    {
        control.vigilance = value;
    }


}
