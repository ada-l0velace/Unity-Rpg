using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextLog : MonoBehaviour {

    public static TextLog instance;
    public List<MessageType> messageType = new List<MessageType>();
    public List<Message> messages = new List<Message>();
    public int maxMessageCount;
    private string m_display;
    private Vector2 m_menuWindowSlider;
    public GUIStyle options;
    public Rect drawArea;



    void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void OnGUI() {
        GUILayout.BeginArea(drawArea, GUI.skin.window);
        m_menuWindowSlider = GUILayout.BeginScrollView(m_menuWindowSlider);
        GUILayout.Label(m_display, options);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    public static void AddMessage(Message message) {
        if (instance.messages.Count > instance.maxMessageCount)
            instance.messages.RemoveAt(0);
        instance.messages.Add(message);
        string s = "";
        foreach (Message item in instance.messages) {
            if (item.type.active)
                s += item.message + "\n";
        }
        instance.m_display = s;
        instance.m_menuWindowSlider.y = 100;
    }





    internal static void addType(MessageType type) {
        instance.messageType.Add(type);
    }
} //class

public struct MessageType {
    public string name;
    public bool active;

    public MessageType(string name, bool active) {
        this.name = name;
        this.active = active;
    }
}

public struct Message {
    public MessageType type;
    public string message;

    public Message(MessageType type, string message) {
        this.type = type;
        this.message = message;
    }

    internal static void Write(MessageType messageType, string message) {
        TextLog.AddMessage(new Message(messageType, message));
    }
}