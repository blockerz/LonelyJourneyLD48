using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceProbe : MonoBehaviour
{
    public float VisibleSpeed = 20; 
    public float VisibleMaxSpeed = 30;
    public float PowerMaxCapacity = 1000;
    public float PowerCurrentCapacity = 500;
    public float PowerDraw;
    public float PowerGeneration;
    public float PowerChange;
    public float MaxSpeed = 100000;
    public float CurrentSpeed;
    public float DesiredSpeed;
    public float DesiredTrajectory = 180f;
    public float CurrentTrajectory = 180f;
    public float FuelMax, FuelCurrent, FuelGenerationRate;
    public float Tick = 0f;
    public float missionTime;

    public int thrustCost, fuelGenerationCost;
    private bool thrustActive = false;

    private float deltaV = 0;

    public List<Device> Devices;

    public float DataCollected = 0; 
    public float DistanceTraveled = 0;

    public string OSVersion;

    public Mission CurrentMission;
    public bool AISleep = false;

    void Start()
    {
        missionTime = 0;
        CurrentSpeed = DesiredSpeed = 18000;
        MaxSpeed = 100000;
        PowerGeneration = 100;
        FuelGenerationRate = 1;
        FuelMax = 100;
        FuelCurrent = FuelMax;
        thrustCost = -10;
        fuelGenerationCost = -25;
        OSVersion = "Journey_2 v1.0.1";

        VisibleSpeed = (CurrentSpeed / MaxSpeed) * VisibleMaxSpeed;

        Devices = new List<Device>();
        Devices.Add(new Device("Magnetometer", -10, 1));
        Devices.Add(new Device("Plasma Analyser", -5, 1));
        Devices.Add(new Device("Particle Detector", -15, 1));
        Devices.Add(new Device("Geiger Tube", -5, 1));
        Devices.Add(new Device("Cosmic Ray Scope", -25, 1));
        Devices.Add(new Device("Radiation Detector", -15, 1));

    }

    void Update()
    {
        Tick -= Time.deltaTime;
        missionTime += Time.deltaTime;

        if (Tick <= 0)
        {
            Tick = 2f;
            VisibleSpeed = (CurrentSpeed / MaxSpeed) * VisibleMaxSpeed;
            VisibleSpeed = Mathf.Clamp(VisibleSpeed, 0, VisibleMaxSpeed);

            DistanceTraveled += Mathf.RoundToInt((Tick / 60f / 60f) * CurrentSpeed);
            PowerDraw = 0;

            Devices.ForEach(device =>
            {
                if(device.Active && device.PowerConsumption + PowerCurrentCapacity > 0)
                {
                    DataCollected += Mathf.RoundToInt(device.DataCollected * Tick);
                    PowerCurrentCapacity += device.PowerConsumption;
                    PowerDraw += device.PowerConsumption;
                }
            });

            for (int r = 0; r < FuelGenerationRate; r++)
            {
                if (FuelCurrent < FuelMax && fuelGenerationCost + PowerCurrentCapacity > 0)
                {
                    FuelCurrent += 1;
                    FuelCurrent = Mathf.Clamp(FuelCurrent, 0, FuelMax);
                    PowerCurrentCapacity += fuelGenerationCost;
                    PowerDraw += fuelGenerationCost;
                }
            }

            PowerChange = PowerGeneration + PowerDraw;
            
            PowerCurrentCapacity += PowerGeneration;
            PowerCurrentCapacity = Mathf.Clamp(PowerCurrentCapacity, 0, PowerMaxCapacity);

            if(deltaV > 0)
            {
                StartCoroutine(ApplyDeltaV(deltaV));
                deltaV = 0;
            }
        }
    }

    public void StartMission(Mission mission)
    {
        CurrentMission = mission;

        if (mission.GoalHeading >= 0)
        {
            DesiredTrajectory = mission.GoalHeading;
        }
        if (mission.GoalSpeed >= 0)
        {
            DesiredSpeed = mission.GoalSpeed;
        }
        if (mission.PowerGenerationChange != 0)
        {
            PowerGeneration += mission.PowerGenerationChange;
        }
        if(mission.OSVersionUpgrade?.Length > 0)
        {
            OSVersion = mission.OSVersionUpgrade;
        }
        if(mission.DeltaV > 0)
        {
            deltaV = mission.DeltaV;
        }
        GameManager.Instance.Messages.Add(mission.MissionMessage);
    }

    public void CompleteMission()
    {
        if(DesiredSpeed == CurrentSpeed && 
            DesiredTrajectory == CurrentTrajectory &&
            DevicesActive())
        {
            AISleep = true;
        }
        else
        {
            GameManager.Instance.Messages.Add(
                new Message(Director.SYSTEM, "To activate Sleep, set the velocity and speed to match misson parameters. " +
                "Then activate all instruments."));
        }
    }

    private bool DevicesActive()
    {
        var device = Devices.Find(device => device.Active == false);
        if (device != null)
            return false;
        return true;
    }

    public void ToggleDevice(string name)
    {
        //Debug.Log(name);
        var device = Devices.Find(device => device.Name.Equals(name));
        if (device != null)
            device.Active = !device.Active;
        //Debug.Log(device.Active);
    }

    public void ApplyForwardThrust()
    {
        StartCoroutine(ChangeSpeed(-100));
    }
    
    public void ApplyRearThrust()
    {
        StartCoroutine(ChangeSpeed(100));
    }

    private IEnumerator ChangeSpeed(int amount)
    {
        if (FuelCurrent + thrustCost < 0 || thrustActive)
            yield break;

        thrustActive = true;
        int iterations = 10;
        int change = amount / iterations;
        int fuelChange = thrustCost / iterations;

        while (iterations > 0)
        {
            iterations--;
            if (FuelCurrent > fuelChange)
            {
                FuelCurrent += fuelChange;
                CurrentSpeed += change;
                CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, MaxSpeed);
            }
            yield return new WaitForSeconds(0.1f);
        }
        thrustActive = false;
    }

    private IEnumerator ApplyDeltaV(float amount)
    {                
        int iterations = 100;
        float change = amount / iterations;        

        while (iterations > 0)
        {
            iterations--;
            
            CurrentSpeed += change;
            CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, MaxSpeed);
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ApplyLeftThrust()
    {
        StartCoroutine(ChangeTrajectory(-1));
    }

    public void ApplyRightThrust()
    {
        StartCoroutine(ChangeTrajectory(1));
    }

    private IEnumerator ChangeTrajectory(float amount)
    {
        if (FuelCurrent + thrustCost < 0 || thrustActive)
            yield break;

        thrustActive = true;
        int iterations = 10;
        float change = amount / iterations;
        int fuelChange = thrustCost / iterations;

        while (iterations > 0)
        {
            iterations--;

            FuelCurrent += fuelChange;
            CurrentTrajectory += change;
            if (CurrentTrajectory < 0)
                CurrentTrajectory += 360;
            if (CurrentTrajectory > 360)
                CurrentTrajectory -= 360;
            CurrentTrajectory = Mathf.Clamp(CurrentTrajectory, 0, 360);

            yield return new WaitForSeconds(0.1f);
        }

        CurrentTrajectory = Mathf.RoundToInt(CurrentTrajectory);
        thrustActive = false;
    }
}
