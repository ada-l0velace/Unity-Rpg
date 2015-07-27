namespace VisualNovel {

    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class VNEngine : MonoBehaviour {

        internal static VNEngine instance;

        public delegate void OnCompletionEvent();
        public OnCompletionEvent OnCompletion;
        
        internal VNSaveJSON data;
        internal VNTextDisplayer displayer;

        private VNMessage m_currentMessage;
        private VNScene m_currentScene;

        public float textSpawnSpeed = 0.02f;

        private char[] breakMessage;

        public float skipSpeedLimit = 0.2f;
        private float m_nextAllowedSkip = 0f;
        private bool m_hasOptions = false;
        private string[] m_messageBreakdown;
        private int m_currentMBIndex;
        private List<string> m_previousMessages;

        private VNCharacterHolder m_chars;

        void Awake() {
            if (instance != null)
                throw new System.Exception("Only one instance of VNEngine is allowed.");
            data = GameData.LoadData<VNSaveJSON>("Data/story");
            displayer = GameData.LoadData<VNTextDisplayer>("Data/gui");
            m_chars = GameData.LoadData<VNCharacterHolder>("Data/characters");
            StartCoroutine(displayer.loadDataFromFile());
            m_chars.characters.ForEach(x => x.portrait = Resources.Load<Texture2D>("Images/Portraits/" + x.name));
            data.scenes.ForEach(x => x.Background = Resources.Load<Texture2D>("Images/Scene/sceneBG_" + x.id));
            
            
            breakMessage = new char[] { '-', '-', '-' };

            if (data.startPoint == -1) {
                Debug.LogException(new System.Exception("Story must have a starting point."));
                return;
            }
            data.Extract();
            displayer.OnChoiceSelected = OnChoiceSelected;

            m_currentScene = data.scenes.Find(x => x.id == data.startPoint);
            m_currentMessage = m_currentScene.nodes.Find(x => x.id == m_currentScene.startPoint);
            m_messageBreakdown = m_currentMessage.message.Split(breakMessage);
            m_currentMBIndex = -1;

            init();
        }

        void OnGUI() {
            if (m_currentScene.background != null)
                GUI.DrawTexture(m_currentScene.bgRect, m_currentScene.background);
            displayer.DrawGUI();

            if (m_nextAllowedSkip < Time.time) {
                if (Input.GetAxis("VNNext") > 0) {
                    if (!m_hasOptions || !displayer.showingOptions) {
                        m_nextAllowedSkip = Time.time + skipSpeedLimit;
                        if (displayer.wroteMessage)
                            displayer.MessageToWrite = nextMessage();
                        else
                            displayer.SkipToEnd();
                    } //else wait for choice input
                }
            }
        }


        public void init() {
            m_currentScene = data.scenes.Find(x => x.id == data.startPoint);
            m_currentMessage = m_currentScene.nodes.Find(x => x.id == m_currentScene.startPoint);
            m_messageBreakdown = m_currentMessage.message.Split(breakMessage);
            m_currentMBIndex = -1;
            displayer.MessageToWrite = nextMessage();
            if (m_currentMessage.characterID != -1) {
                displayer.character = m_chars.Get(m_currentMessage.characterID);
            } else {
                displayer.character = null;
            }
        }


        private void nextNode() {
            if (m_currentMessage.LeadsTo != null) {
                if (m_currentMessage.LeadsTo.type == VNNodeType.Scene) {
                    m_currentScene = (VNScene)m_currentMessage.LeadsTo;
                    int id = m_currentScene.startPoint;
                    m_currentMessage = m_currentScene.nodes.Find(x => x.id == id);
                } else {
                    m_currentMessage = (VNMessage)m_currentMessage.LeadsTo;
                }
                breakdownMessage();
                if (m_currentMessage.characterID != -1) {
                    displayer.character = m_chars.Get(m_currentMessage.characterID);
                } else {
                    displayer.character = null;
                }
            } else {
                displayer.MessageToWrite = "Reached the end!";
            }
        }
        

        private string nextMessage() {
            if (m_messageBreakdown.Length - 1 <= m_currentMBIndex)
                nextNode();
            ++m_currentMBIndex;
            if (m_hasOptions && m_currentMBIndex == m_messageBreakdown.Length - 1)
                displayer.choices = m_currentMessage.options;
            if (m_currentMBIndex < m_messageBreakdown.Length)
                return m_messageBreakdown[m_currentMBIndex];
            else {
                if (OnCompletion != null)
                    OnCompletion();
                return "No messages to display. If you are reading this something is broken.";
            }
        }



        private void OnChoiceSelected(VNOption choice) {
            displayer.choices = null;
            m_hasOptions = false;
            if (choice.LeadsTo.type == VNNodeType.Scene) {
                Debug.Log("New Scene.");
                m_currentScene = (VNScene)choice.LeadsTo;
                m_currentMessage = m_currentScene.nodes.Find(x => x.id == m_currentScene.startPoint);
            } else {
                m_currentMessage = (VNMessage)choice.LeadsTo;
            }
            breakdownMessage();
            displayer.MessageToWrite = nextMessage();
        }


        private void breakdownMessage() {
            m_messageBreakdown = m_currentMessage.message.Split(breakMessage);
            m_currentMBIndex = -1;
            m_hasOptions = (m_currentMessage.options.Count > 0);
        }
    } //class
} //namespace