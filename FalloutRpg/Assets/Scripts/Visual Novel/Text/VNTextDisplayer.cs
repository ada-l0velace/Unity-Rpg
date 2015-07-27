namespace VisualNovel {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class VNTextDisplayer {

        private static readonly string IMAGE_LOCATION = "file:///" + Application.dataPath + "/Resources/Images/";
        private static readonly Rect RECT_ZERO = new Rect();

        public delegate void OnChoiceSelectedEvent(VNOption choice);
        internal OnChoiceSelectedEvent OnChoiceSelected;

        internal Texture2D textBG;
        internal Texture2D textBtn;
        public string textureBG = "";
        public string textureButton = "";
        public TextAnchor align;
        public Color textColor = Color.black;
        //public Font font;
        public Rect textBGArea;
        public Rect textArea;
        public Vector2 textOffset;
        public Rect portraitRect;
        public Rect choiceSize;
        public Vector2 choiceOffset;
        public bool inline;

        private GUIStyle m_textStyle;

        private string messageToWrite = "";
        private string messageWritten = "";

        internal List<VNOption> choices;
        internal bool showingOptions;
        internal float spawnLetterSpeed;
        private float m_nextLetter;

        internal VNCharacter character;

        internal string MessageToWrite {
            get { return messageToWrite; }
            set {
                //do things!
                messageToWrite = value;
                messageWritten = "";
            }
        }

        public VNTextDisplayer() {
            textBG = new Texture2D(1, 1);
        }

        public void DrawGUI() {
            if (m_textStyle == null)
                m_textStyle = new GUIStyle(GUI.skin.box);

            if (messageToWrite.Length != messageWritten.Length) {
                if (m_nextLetter < Time.time) {
                    m_nextLetter = Time.time + spawnLetterSpeed;
                    messageWritten += messageToWrite[messageWritten.Length];
                    if (messageToWrite.Length == messageWritten.Length) {
                        if (choices != null) {
                            showingOptions = true;
                        }
                    }
                }
            }

            if (textBGArea != RECT_ZERO) //always draw text dialog
                GUI.DrawTexture(textBGArea, textBG);
            try {
                GUILayout.BeginArea(textBGArea);

                GUILayout.BeginHorizontal();
                if (portraitRect != RECT_ZERO && character != null && character.portrait != null)  //portrait
                    GUI.DrawTexture(GUILayoutUtility.GetRect(portraitRect.width, portraitRect.height), character.portrait);
                
                GUILayout.BeginVertical();
                GUILayout.Space(3);
                GUILayout.Label(messageWritten, m_textStyle); //text
                if (showingOptions) {  //options
                    foreach (VNOption item in choices) {
                        if (GUILayout.Button(item.displayText)) {
                            showingOptions = false;
                            OnChoiceSelected(item);
                        }
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            } catch { }
        }

        public void updateGUI() {
            if (m_textStyle == null)
                m_textStyle = new GUIStyle();

            m_textStyle.normal.textColor = textColor;
            m_textStyle.normal.background = null;
            m_textStyle.alignment = align;

            textArea = new Rect(textBGArea).RectSumVector2(textOffset);
            textArea.width -= textOffset.x;
            textArea.height -= textOffset.y;

        }

        public IEnumerator loadDataFromFile() {
            string s = "file:///" + Application.dataPath + "/Resources/Images/";
            if (textureBG != "") {
                WWW www = new WWW(s + textureBG + ".png");
                yield return www;
                textBG = www.texture;
            }

            if (textureButton != "") {
                WWW www = new WWW(s + textureButton + ".png");
                yield return www;
                textBtn = www.texture;
            }

            updateGUI();
        }

        public IEnumerator loadData() {
            if (textureBG != "") {
                WWW www = new WWW(textureBG);

                yield return www;
                textBG = www.texture;
                string[] breakdown = textureBG.Split('/');
                textureBG = breakdown[breakdown.Length - 1].Split('.')[0];
            }

            if (textureButton != "") {
                WWW www = new WWW(textureButton);

                yield return www;
                textBtn = www.texture;
                string[] breakdown = textureButton.Split('/');
                textureBG = breakdown[breakdown.Length - 1].Split('.')[0];
            }

        }

        internal void SkipToEnd() {
            messageWritten = messageToWrite;
        }

        public bool wroteMessage {
            get {
                return (messageToWrite.Length == messageWritten.Length);
            }
        }


    } //class
} //namespace