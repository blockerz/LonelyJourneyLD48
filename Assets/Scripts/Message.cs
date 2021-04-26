using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message 
{
    public string From;
    public string Content;
    public float Time;
    public bool Read;

    public Message(string from, string content)
    {
        From = from;
        Content = content;
        Time = GameManager.Instance.Probe.missionTime;
        Read = false;
    }
}
