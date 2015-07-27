namespace VisualNovel.Editor {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    public class VNCharacterEditor : MonoBehaviour {


        private static VNCharacterEditor instance;
        private VNEditor m_editor;
        private VNDisplayEditor m_displayer;

        public VNCharacterHolder m_charHolder;
        private VNCharacter m_selectedChar;

        private string[] m_buttonOptions = new string[] { "New", "Delete", "---", "Save", "Load", "---", "Scene Editor", "Display Editor" };


        private ComboBox m_comboBox;
        private List<GUIContent> m_listCB = new List<GUIContent>();
        private List<VNCharacter> m_chars;


        void Awake() {
            if (instance != null)
                return;
            instance = this;
            enabled = false;
            m_editor = GetComponent<VNEditor>();
            m_displayer = GetComponent<VNDisplayEditor>();

            m_comboBox = new ComboBox();
            m_listCB.Add(new GUIContent("None"));

            m_charHolder = new VNCharacterHolder();
            m_chars = m_charHolder.characters = new List<VNCharacter>();


        }


        void OnGUI() {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            drawSideButtons();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            int prevSelected = m_comboBox.GetSelectedItemIndex();
            int selected = m_comboBox.List(GUILayoutUtility.GetRect(100, 30), m_listCB[prevSelected], m_listCB.ToArray(), null);
            if (prevSelected != selected) {
                if (selected == 0)
                    m_selectedChar = null;
                else
                    m_selectedChar = m_chars[selected - 1];
            }
            GUILayout.EndVertical();

            if (m_selectedChar != null) {
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();

                GUILayout.Label("Name: ");

                m_selectedChar.name = GUILayout.TextField(m_selectedChar.name);
                m_listCB[selected].text = m_selectedChar.name;

                GUILayout.EndHorizontal();
                if (GUILayout.Button("Load Portrait")) {
#if UNITY_EDITOR
                    if (m_selectedChar != null) {
                        string path = EditorUtility.OpenFilePanel("Load Portrait", "", "");
                        if (path != string.Empty) {
                            StartCoroutine(loadTexture("file:///" + path));
                        }
                    }
#endif
                }
                GUILayout.EndVertical();

            }

            if (m_selectedChar != null) {
                GUILayout.BeginVertical();
                if (m_selectedChar.portrait != null) {
                    GUILayout.Box(m_selectedChar.portrait);
                }
                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
        }

        private void drawSideButtons() {
            foreach (string option in m_buttonOptions) {
                try {
                    if (GUILayout.Button(option)) {
                        switch (option) {
                            case "New":
                                m_selectedChar = new VNCharacter();
                                m_chars.Add(m_selectedChar);
                                m_listCB.Add(new GUIContent(m_selectedChar.name));
                                m_comboBox.selectedItemIndex = m_listCB.Count - 1;
                                break;
                            case "Save":
                                GameData.SaveString(
                                    JSONSaver.Save<VNCharacterHolder>(m_chars, ""),
                                    m_editor.saveLocation + "/characters.txt", true);
#if UNITY_EDITOR
                                foreach (VNCharacter item in m_chars) {
                                    if (item.portrait != null) {
                                        byte[] bytes = item.portrait.EncodeToPNG();
                                        //System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/Images/Portraits/" + item.name + ".png", bytes);
                                    }
                                }
#endif
                                break;
                            case "Load":
                                m_charHolder = GameData.LoadData<VNCharacterHolder>(
                                    m_editor.saveLocation + "/characters");
                                m_chars = m_charHolder.characters;
                                m_listCB.Clear();
                                m_listCB.Add(new GUIContent("None"));
                                foreach (VNCharacter item in m_chars) {
                                    m_listCB.Add(new GUIContent(item.name));
                                    item.portrait = Resources.Load<Texture2D>("Images/Portraits/" + item.name);
                                }
                                
                                break;
                            case "Scene Editor":
                                enabled = false;
                                m_editor.UpdateFromVNCE();
                                m_editor.enabled = true;
                                break;
                            case "Display Editor":
                                enabled = false;
                                m_displayer.enabled = true;
                                break;
                        }
                    }
                } catch { }
            }
        }


        private IEnumerator loadTexture(string path) {
            WWW www = new WWW(path);
            yield return www;
            m_selectedChar.portrait = www.texture;
        }

    } // class
}// namespace