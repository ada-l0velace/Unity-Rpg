namespace VisualNovel.Editor {
    using UnityEngine;
#if UNITY_EDITOR
    //using UnityEditor;
#endif
    using System;
    using System.Collections;

    [RequireComponent(typeof(VNEditor))]
    public class VNDisplayEditor : MonoBehaviour {

        private VNEditor editor;

        #region GUI settings
        public Rect drawRegion;
        public Rect buttonDrawRegion;
        public Rect editorDrawRegion;
        private string[] m_buttonOptions = new string[] { "Load Background", "Load Button", "---", "Save", "Load", "Scene Editor", "Character Editor" };

        private ColorPicker m_cp;

        private GUIContent[] comboBoxList;
        private ComboBox comboBoxControl;
        #endregion

        private VNCharacterEditor m_characterEditor;
        private VNTextDisplayer m_visualizer;

        private string[] enumNames;
        private Array enumValues;

        void Awake() {
            editor = GetComponent<VNEditor>();
            enabled = false;

            m_visualizer = new VNTextDisplayer();
            m_characterEditor = GetComponent<VNCharacterEditor>();
            m_visualizer.MessageToWrite = "This is a text message of unknown texting.";

            m_cp = GetComponent<ColorPicker>();
            m_cp.receiver = gameObject;
            m_cp.SetColor(m_visualizer.textColor);

            #region init combobox
            comboBoxControl = new ComboBox();
            enumNames = Enum.GetNames(typeof(TextAnchor));

            comboBoxList = new GUIContent[enumNames.Length];
            for (int i = 0; i < enumNames.Length; i++) {
                comboBoxList[i] = new GUIContent(enumNames[i]);
            }
            #endregion


        }


        void OnGUI() {
            m_visualizer.DrawGUI();
            GUILayout.BeginArea(buttonDrawRegion);

            #region buttons
            foreach (string option in m_buttonOptions) {
                try {
                    if (GUILayout.Button(option)) {
                        switch (option) {
                            case "Load Background":
#if UNITY_EDITOR
                            string path = "";
                                //string path = EditorUtility.OpenFilePanel("Load Background", "", "");
                                if (path != string.Empty) {
                                    m_visualizer.textureBG = "file:///" + path;
                                    StartCoroutine(m_visualizer.loadData());
                                }
#endif
                                break;
                            case "Load Button":

                                break;
                            case "Save":
                                GameData.SaveString(
                                    JSONSaver.Save<VNTextDisplayer>(m_visualizer, ""),
                                    editor.saveLocation + "/gui.txt", true);
#if UNITY_EDITOR

                                if (m_visualizer.textureBG != "") {
                                    byte[] bytes = m_visualizer.textBG.EncodeToPNG();
                                    //System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/Images/" + m_visualizer.textureBG + ".png", bytes);
                                }
#endif
                                break;
                            case "Load":
                                m_visualizer = GameData.LoadData<VNTextDisplayer>(
                                    editor.saveLocation + "/gui");
                                StartCoroutine(m_visualizer.loadDataFromFile());
                                m_cp.SetColor(m_visualizer.textColor);
                                m_visualizer.MessageToWrite = "This is a text message of unknown texting.";
                                break;
                            case "Scene Editor":
                                enabled = false;
                                m_cp.enabled = false;
                                editor.enabled = true;
                                break;
                            case "Character Editor":
                                enabled = false;
                                m_characterEditor.enabled = true;
                                break;
                        }
                    }
                } catch { }
            }
            #endregion
            float h;


            rectGUI("Draw Area", ref m_visualizer.textBGArea);
            rectGUI("Portrait", ref m_visualizer.portraitRect);

            #region textoffset
            GUILayout.Label("TextOffset");
            GUILayout.BeginHorizontal();
            GUILayout.Label("X");
            float.TryParse(GUILayout.TextField(m_visualizer.textOffset.x.ToString()), out h);
            if (!float.IsNaN(h))
                m_visualizer.textOffset.x = h;

            GUILayout.Label("Y");
            float.TryParse(GUILayout.TextField(m_visualizer.textOffset.y.ToString()), out h);
            if (!float.IsNaN(h))
                m_visualizer.textOffset.y = h;
            GUILayout.EndHorizontal();

            #endregion

            #region Text Alignment
            int selectedItemIndex, newIndex;
            selectedItemIndex = comboBoxControl.GetSelectedItemIndex();
            newIndex = comboBoxControl.List(GUILayoutUtility.GetRect(100f, 20f),
                comboBoxList[selectedItemIndex].text, comboBoxList, null);
            if (selectedItemIndex != newIndex) {
                m_visualizer.align = (TextAnchor)Enum.Parse(typeof(TextAnchor), comboBoxList[newIndex].text);
            }
            #endregion



            GUILayout.EndArea();

            m_cp._DrawGUI();

            m_visualizer.updateGUI();
        }

        void OnEnable() {
            m_cp.enabled = true;
        }

        public void OnSetNewColor(Color color) {
            m_visualizer.textColor = color;
        }

        public void rectGUI(string name, ref Rect r) {
            float h;
            GUILayout.Label(name);
            GUILayout.BeginHorizontal();
            GUILayout.Label("X");
            float.TryParse(GUILayout.TextField(r.x.ToString()), out h);
            if (!float.IsNaN(h))
                r.x = h;

            GUILayout.Label("Y");
            float.TryParse(GUILayout.TextField(r.y.ToString()), out h);
            if (!float.IsNaN(h))
                r.y = h;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("W");
            float.TryParse(GUILayout.TextField(r.width.ToString()), out h);
            if (!float.IsNaN(h))
                r.width = h;

            GUILayout.Label("H");
            float.TryParse(GUILayout.TextField(r.height.ToString()), out h);
            if (!float.IsNaN(h))
                r.height = h;
            GUILayout.EndHorizontal();
        }



        
    }
}