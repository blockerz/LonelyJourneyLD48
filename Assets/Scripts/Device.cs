using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Device 
{
    public string Name;
    public int PowerConsumption;
    public bool Active;
    public int DataCollected;

    public Device(string name, int power, int data, bool active = true)
    {
        Name = name;
        PowerConsumption = power;
        Active = active;
        DataCollected = data;
    }
}
