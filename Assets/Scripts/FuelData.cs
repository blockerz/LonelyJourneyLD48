using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelData : MonoBehaviour
{
    SpaceProbe probe;
    Slider FuelCapacitySlider;
    Slider FuelGenerationSlider;
    Text FuelChange;
    Image FuelBarFill;
    Text SpeedValue;
    Text TrajectoryValue;

    void Start()
    {
        probe = GameManager.Instance.Probe;
        FuelCapacitySlider = GameObject.Find("FuelBar").GetComponent<Slider>();
        FuelGenerationSlider = GameObject.Find("FuelGeneration").GetComponent<Slider>();
        FuelChange = GameObject.Find("FuelChange").GetComponent<Text>();
        SpeedValue = GameObject.Find("SpeedValue").GetComponent<Text>();
        TrajectoryValue = GameObject.Find("TrajectoryValue").GetComponent<Text>();
        FuelBarFill = GameObject.Find("FuelBarFill").GetComponent<Image>();

        FuelGenerationSlider.value = 1;
    }


    void Update()
    {
        FuelCapacitySlider.value = probe.FuelCurrent / probe.FuelMax;
        FuelChange.text = probe.FuelCurrent + " (" + Mathf.RoundToInt(probe.FuelGenerationRate).ToString() + ")";

        if ((probe.FuelCurrent / probe.FuelMax) > 0.3f )
        {
            FuelBarFill.color = Color.green;
        }
        else
        {
            FuelBarFill.color = Color.red;
        }

        SpeedValue.text = probe.CurrentSpeed + " kph";
        if (Mathf.Abs(probe.CurrentSpeed - probe.DesiredSpeed) > 0.001)
        {
            if (probe.CurrentSpeed - probe.DesiredSpeed < 0)
                SpeedValue.color = Color.yellow;
            else
                SpeedValue.color = Color.red;
        }
        else
        {
            SpeedValue.color = Color.green;
        }

        TrajectoryValue.text = probe.CurrentTrajectory + " (" + (probe.CurrentTrajectory - probe.DesiredTrajectory) + ")";
        if(Mathf.Abs(probe.CurrentTrajectory - probe.DesiredTrajectory) > 0.001)
        {
            TrajectoryValue.color = Color.red;
        }
        else
        {
            TrajectoryValue.color = Color.green;
        }

        probe.FuelGenerationRate = FuelGenerationSlider.value;
    }
}
