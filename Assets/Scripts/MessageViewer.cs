using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageViewer : MonoBehaviour
{

    int activeMessage;
    
    Text MessageBox;
    Text Unread;
    List<Message> messages;
    Image MessageBottom;
    Image MessageDown;
    int unread; 

    void Start()
    {
        activeMessage = 0;

        MessageBox = GameObject.Find("MessageDisplay").GetComponent<Text>();
        Unread = GameObject.Find("UnreadValue").GetComponent<Text>();
        MessageBottom = GameObject.Find("MessageBottom").GetComponent<Image>();
        MessageDown = GameObject.Find("MessageDown").GetComponent<Image>();

        messages = GameManager.Instance.Messages;

        UpdateMessage();
    }

    void Update()
    {
        if (activeMessage != messages.Count - 1)
        {
            MessageBottom.color = Color.yellow;
            MessageDown.color = Color.yellow;
        }
        else
        {
            MessageBottom.color = Color.white;
            MessageDown.color = Color.white;
        }

        unread = messages.FindAll(message => (message.Read == false)).Count;

        
        if(unread > 0)
        {
            Unread.text = "<color=red>" + unread + "</color>";
            ScrollBottom();
        }
        else
        {
            Unread.text = unread + "";
        }
    }

    void UpdateMessage()
    {
        MessageBox.text = "<color=yellow>From: " + messages[activeMessage].From + "</color>\n" +
            messages[activeMessage].Content;

        messages[activeMessage].Read = true;


    }

    public void ScrollUp()
    {
        activeMessage--;
        activeMessage = Mathf.Clamp(activeMessage, 0, messages.Count-1);
        UpdateMessage();
    }
    public void ScrollDown()
    {
        activeMessage++;
        activeMessage = Mathf.Clamp(activeMessage, 0, messages.Count-1);
        UpdateMessage();
    }
    public void ScrollBottom()
    {        
        activeMessage = messages.Count-1;
        UpdateMessage();
    }
}
