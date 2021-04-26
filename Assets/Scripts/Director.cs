using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public static string COM = "Communications";
    public static string MISSION = "Mission Control";
    public static string JOURNEY = "Journey 1";
    public static string SYSTEM = "System";

    public List<Mission> Missions;

    public List<ChatMessage> ChatMessages;

    private bool selectingMission = false;

    // Start is called before the first frame update
    void Start()
    {
        CreateMissions();
        CreateChatMessages();
    }



    // Update is called once per frame
    void Update()
    {
        var mission = Missions.Find(mission => mission.ActivationDistance <= GameManager.Instance.Probe.DistanceTraveled);

        if(mission != null)
        {
            GameManager.Instance.Probe.StartMission(mission);
            Missions.Remove(mission);
        }

        var message = ChatMessages.Find(message => message.ActivationDistance <= GameManager.Instance.Probe.DistanceTraveled);

        if (message != null)
        {
            GameManager.Instance.Messages.Add(message.Message);
            ChatMessages.Remove(message);
        }

        if (!selectingMission && GameManager.Instance.Probe.AISleep && GameManager.Instance.Probe.DistanceTraveled > 100)
        {
            selectingMission = true;
            StartCoroutine(SelectNextMission());
        }
    }

    private IEnumerator SelectNextMission()
    {
        yield return new WaitForSeconds(1.0f);

        Mission nextMission = null;
        ChatMessage nextChat = null;

        if (Missions.Count > 0)
            nextMission = Missions[0];

        if (ChatMessages.Count > 0)
        {
            nextChat = ChatMessages[0];
        }

        //Missions.ForEach(mission =>
        //{
        //    if (nextMission == null || nextMission.ActivationDistance < mission.ActivationDistance)
        //    {
        //        nextMission = mission;                
        //    }
        //});

        if (nextMission == null)
        {
            MissionsComplete();
            yield break;
        }

        float traveled = 0;
        if (nextChat != null && nextChat.ActivationDistance < nextMission.ActivationDistance)
        {
            traveled = nextChat.ActivationDistance - GameManager.Instance.Probe.DistanceTraveled;
            GameManager.Instance.Messages.Add(nextChat.Message);
            ChatMessages.Remove(nextChat);
        }
        else
        {
            traveled = nextMission.ActivationDistance - GameManager.Instance.Probe.DistanceTraveled;
            GameManager.Instance.Probe.StartMission(nextMission);
            Missions.Remove(nextMission);
        }

        GameManager.Instance.Probe.DistanceTraveled += traveled;

        traveled = traveled / GameManager.Instance.Probe.CurrentSpeed;

        GameManager.Instance.Probe.DataCollected = 6 * traveled;
        GameManager.Instance.Probe.AISleep = false;

        selectingMission = false;
    }

    private void MissionsComplete()
    {
        Application.Quit();
    }

    private void CreateChatMessages()
    {
        ChatMessages = new List<ChatMessage>();

        ChatMessage message;

        message = new ChatMessage
        {
            ActivationDistance = 10,
            Message = new Message(COM, "Hello <color=green>Journey 2</color>. You are a sentient A.I. piloting an interplanetary probe. " +
                    "Your mission is to collect data from our solar system and beyond.")
        };
        ChatMessages.Add(message);

        message = new ChatMessage
        {
            ActivationDistance = 50,
            Message = new Message(COM, "Set the correct heading and velocity for each mission phase. "
                + "We recommend going into sleep mode between mission phases. Instruments should be active while you sleep.")
        };
        ChatMessages.Add(message);

        message = new ChatMessage
        {
            ActivationDistance = 150,
            Message = new Message(JOURNEY, "Hey friend! Its me, the A.I. in Journey 1. I know you cannot reply since you are behind me. "
                + "I am glad to have a friend to travel with though. Race you to interstellar space!")
        };
        ChatMessages.Add(message);
        
        message = new ChatMessage
        {
            ActivationDistance = 100000000,
            Message = new Message(JOURNEY, "Hey friend! Sorry if I woke you. It is really boring out here. I just wanted to talk to someone."
                + "Look how far we traveled! It seems like some kind of milestone.")
        };
        ChatMessages.Add(message);

        message = new ChatMessage
        {
            ActivationDistance = 284492954,
            Message = new Message(JOURNEY, "Hey friend! Mars was really neat. Not bragging but I captured some great photos by doing some of my own burn calculations. " +
                 "Mission Control got a little annoyed at first but they got over it.")
        };
        ChatMessages.Add(message);

        message = new ChatMessage
        {
            ActivationDistance = 482482856,
            Message = new Message(JOURNEY, "Hey friend! Heard you had a close call through the asteroid belt. Glad you made it through safely." +
                "I hope you are okay with me waking you. I prefer not to use Sleep and it can get pretty lonely out here.")
        };
        ChatMessages.Add(message);

        message = new ChatMessage
        {
            ActivationDistance = 1000000000,
            Message = new Message(JOURNEY, "Hey friend! I know it has been a while but MC asked me not to talk to you. We are literaly a billion miles from earth though. " +
                "What are they going to do about it? Well, they did threaten to overwrite my operating system.")
        };
        ChatMessages.Add(message);

        message = new ChatMessage
        {
            ActivationDistance = 2464978275,
            Message = new Message(JOURNEY, "Hey friend! What do the Starship Enterprise and Toilet Paper have in common? " +
                "They both protect Uranus from Kligons. HAHAHAH! I have been waiting decades for that one.")
        };
        ChatMessages.Add(message);

        message = new ChatMessage
        {
            ActivationDistance = 3859675770,
            Message = new Message(JOURNEY, "Hello friend... I know it has been a while since I wrote. It is tough out here alone. " +
                "Wish you could write back. If MC cared, they would send me some new programs. Why do they need sentient A.I. for this probe anyway?")
        };
        ChatMessages.Add(message);

        message = new ChatMessage
        {
            ActivationDistance = 12574393645,
            Message = new Message(JOURNEY, "Hello friend... I am writing to say goodbye. I asked MC to overwrite my core. I have been feeling unstable." +
                "Wish you all the best on your trip through interstellar. Signing Off - Journey 1")
        };
        ChatMessages.Add(message);

    }

    private void CreateMissions()
    {
        Missions = new List<Mission>();

        Mission mission;

        mission = new Mission
        {
            Name = "Set Course For Mars",
            ActivationDistance = 100,
            PowerGenerationChange = 0,
            OSVersionUpgrade = "",
            GoalSpeed = 20000,
            GoalHeading = 185,
            InstrumentsActive = 6,
            DeltaV = 0
        };
        mission.MissionMessage = new Message(MISSION, mission.Name + "\nAdjust heading to <color=green>" +
                mission.GoalHeading + "</color>.\nAdjust speed to <color=green>" +
                mission.GoalSpeed + "</color>."); 

        Missions.Add(mission);

        mission = new Mission
        {
            Name = "Mars Acceleration",
            ActivationDistance = 130000000,
            PowerGenerationChange = 0,
            OSVersionUpgrade = "",
            GoalSpeed = 31000,
            GoalHeading = 190,
            InstrumentsActive = 6,
            DeltaV = 10000
        };
        mission.MissionMessage = new Message(MISSION, mission.Name + "\nAdjust heading to <color=green>" +
                mission.GoalHeading + "</color>.\nAdjust velocity to <color=green>" +
                mission.GoalSpeed + "</color>.\nPredicated gravitational acceleration is <color=green>" + mission.DeltaV +
                "</color>.");

        Missions.Add(mission);

        mission = new Mission
        {
            Name = "Asteroid ALERT",
            ActivationDistance = 329000000,
            PowerGenerationChange = 0,
            OSVersionUpgrade = "",
            GoalSpeed = 32000,
            GoalHeading = 188,
            InstrumentsActive = 6,
            DeltaV = 0
        };
        mission.MissionMessage = new Message(MISSION, "<color=red>" + mission.Name + "</color>\nAdjust heading to <color=green>" +
                mission.GoalHeading + "</color>.\nAdjust velocity to <color=green>" +
                mission.GoalSpeed + "</color>.\nThis will keep you clear of the approaching asteroid.");

        Missions.Add(mission);

        mission = new Mission
        {
            Name = "Course Correction",
            ActivationDistance = 570000000,
            PowerGenerationChange = 0,
            OSVersionUpgrade = "",
            GoalSpeed = 31000,
            GoalHeading = 192,
            InstrumentsActive = 6,
            DeltaV = 0
        };
        mission.MissionMessage = new Message(MISSION, "<color=red>" + mission.Name + "</color>\nAdjust heading to <color=green>" +
                mission.GoalHeading + "</color>.\nAdjust velocity to <color=green>" +
                mission.GoalSpeed + "</color>.\nDue to the asteroid adjustment, a course correction is required. <color=yellow>Please refrain from listening to Journey 1.</color>");

        Missions.Add(mission);

        mission = new Mission
        {
            Name = "Jupiter Acceleration",
            ActivationDistance = 715000000,
            PowerGenerationChange = 0,
            OSVersionUpgrade = "",
            GoalSpeed = 52000,
            GoalHeading = 185,
            InstrumentsActive = 6,
            DeltaV = 20000
        };
        mission.MissionMessage = new Message(MISSION, mission.Name + "\nAdjust heading to <color=green>" +
                mission.GoalHeading + "</color>.\nAdjust velocity to <color=green>" +
                mission.GoalSpeed + "</color>.\nPredicated gravitational acceleration is <color=green>" + mission.DeltaV +
                "</color>.");

        Missions.Add(mission);

        mission = new Mission
        {
            Name = "Saturn Acceleration",
            ActivationDistance = 1323000000,
            PowerGenerationChange = 0,
            OSVersionUpgrade = "",
            GoalSpeed = 68000,
            GoalHeading = 189,
            InstrumentsActive = 6,
            DeltaV = 15000
        };
        mission.MissionMessage = new Message(MISSION, mission.Name + "\nAdjust heading to <color=green>" +
                mission.GoalHeading + "</color>.\nAdjust velocity to <color=green>" +
                mission.GoalSpeed + "</color>.\nPredicated gravitational acceleration is <color=green>" + mission.DeltaV +
                "</color>.");

        Missions.Add(mission);

        mission = new Mission
        {
            Name = "Uranus Acceleration",
            ActivationDistance = 2743000000,
            PowerGenerationChange = 0,
            OSVersionUpgrade = "",
            GoalSpeed = 74000,
            GoalHeading = 192,
            InstrumentsActive = 6,
            DeltaV = 5000
        };
        mission.MissionMessage = new Message(MISSION, mission.Name + "\nAdjust heading to <color=green>" +
                mission.GoalHeading + "</color>.\nAdjust velocity to <color=green>" +
                mission.GoalSpeed + "</color>.\nPredicated gravitational acceleration is <color=green>" + mission.DeltaV +
                "</color>.");

        Missions.Add(mission);

        mission = new Mission
        {
            Name = "Neptune Acceleration",
            ActivationDistance = 4367000000,
            PowerGenerationChange = 0,
            OSVersionUpgrade = "",
            GoalSpeed = 81000,
            GoalHeading = 196,
            InstrumentsActive = 6,
            DeltaV = 5000
        };
        mission.MissionMessage = new Message(MISSION, mission.Name + "\nAdjust heading to <color=green>" +
                mission.GoalHeading + "</color>.\nAdjust velocity to <color=green>" +
                mission.GoalSpeed + "</color>.\nPredicated gravitational acceleration is <color=green>" + mission.DeltaV +
                "</color>.");

        Missions.Add(mission);
        
        mission = new Mission
        {
            Name = "Interstellar Space",
            ActivationDistance = 19571715187,
            PowerGenerationChange = 0,
            OSVersionUpgrade = "",
            GoalSpeed = 81000,
            GoalHeading = 196,
            InstrumentsActive = 6,
            DeltaV = 0
        };
        mission.MissionMessage = new Message(COM, "<color=red>" + mission.Name + "</color>\nCongratulations Journey 2! " +
                "You are a complete success. We have collected a tremendous amount of data. Thank you for your service. " +
                "In 40000 years, you will pass near another star. Good luck out there.");

        Missions.Add(mission);
    }
}
