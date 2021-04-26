using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerData : MonoBehaviour
{
    SpaceProbe probe; 
    Slider PowerCapacitySlider;
    Text PowerChange;
    Text DataCollected;
    Text DistanceTraveled;
    Image PowerBarFill; 

    void Start()
    {
        probe = GameManager.Instance.Probe;
        PowerCapacitySlider = GetComponentInChildren<Slider>();
        PowerChange = GameObject.Find("PowerChange").GetComponent<Text>();
        DataCollected = GameObject.Find("DataValue").GetComponent<Text>();
        DistanceTraveled = GameObject.Find("DistanceValue").GetComponent<Text>();
        PowerBarFill = GameObject.Find("PowerBarFill").GetComponent<Image>();
    }

    
    void Update()
    {
        PowerCapacitySlider.value = probe.PowerCurrentCapacity / probe.PowerMaxCapacity;
        PowerChange.text = probe.PowerCurrentCapacity + " (" + Mathf.RoundToInt(probe.PowerChange).ToString() + ")";

        if (probe.PowerChange >= 0)
        {
            PowerBarFill.color = Color.green;
        }
        else
        {
            PowerBarFill.color = Color.red;
        }

        DataCollected.text = probe.DataCollected.ToString("N0") + " KB";
        DistanceTraveled.text = probe.DistanceTraveled.ToString("N0") + " KM";
    }
}
