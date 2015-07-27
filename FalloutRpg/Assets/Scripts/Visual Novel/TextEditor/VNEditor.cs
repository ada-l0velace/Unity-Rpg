namespace VisualNovel.Editor {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
#if UNITY_EDITOR
    //using UnityEditor;
#endif
    [RequireComponent(typeof(VNDisplayEditor))]
    public class VNEditor : MonoBehaviour {

        #region GUI settings
        public Rect drawRegion;
        public Rect buttonDrawRegion;
        public Rect editorDrawRegion;
        public Rect nodeSpawnPosition = new Rect(100, 100, 100, 50);
        public GUIStyle nodeStyle;
        public GUIStyle selectedNodeStyle;
        public GUIStyle hoverNodeStyle;
        public GUIStyle optionNodeStyle;
        public GUIStyle optionSelectedNodeStyle;
        public GUIStyle optionHoverNodeStyle;
        public Rect gameNameRect;
        public Rect gameNameEditRect;
        public Texture2D selectedText;
        public Rect selectedOffset;
        private ComboBox m_comboBox;
        private GUIContent[] m_listCB;
        #endregion

        public string saveLocation;

        private string[] m_buttonOptions = new string[] { "Add Scene", "---", "Save", "Load", "---", "1st Scene", "Display Editor", "Character Editor", "---", "Delete Node" };
        private string[] m_buttonSceneOptions = new string[] { "View Scenes", "Add Message", "---", "Save", "Load", "---", "1st Message", "Display Editor", "Character Editor", "---", "Delete Node" };
        private List<VNNode> m_nodes;
        private List<VNScene> m_scenes;
        private Vector2 m_scrollview;
        private Vector2 m_editorTextScrollview;
        private Vector2 m_editorOptionsScrollview;
        private VNNode m_selectedNode;
        private VNOption m_selectedOption;
        private VNScene m_currentScene;
        private VNScene m_returnScene;
        private bool m_linkToScene;
        private VNSaveJSON data;
        private VNDisplayEditor displayEditor;
        private VNCharacterEditor m_characterEditor;
        private VNCharacterHolder m_chars;

        //private bool m_editingScene = false;
        private bool m_editingNode = false;
        private VNNodeType type;
        private Rect internalSVRect = new Rect();


        void Start() {
            displayEditor = GetComponent<VNDisplayEditor>();
            m_characterEditor = GetComponent<VNCharacterEditor>();
            m_chars = m_characterEditor.m_charHolder;
            VNNode.normalStyle = nodeStyle;
            VNNode.selectedStyle = selectedNodeStyle;
            VNNode.hoverStyle = hoverNodeStyle;
            VNMessage.optionNormalStyle = optionNodeStyle;
            VNMessage.optionHoverStyle = optionHoverNodeStyle;
            VNMessage.optionSelectedStyle = optionSelectedNodeStyle;

            data = new VNSaveJSON();
            m_scenes = data.scenes;
            m_nodes = data.ToListNode();

            m_comboBox = new ComboBox();
            m_listCB = new GUIContent[] { new GUIContent("None") };
        }


        void OnGUI() {
            if (m_editingNode) {
                editMessage();
                return;
            }

            if (m_currentScene == null && m_selectedNode != null) {
                    Texture2D t = ((VNScene)m_selectedNode).background;
                    if (t != null) {
                        GUI.DrawTexture(new Rect(0, 0, t.width, t.height), t);
                    }
                
            }

            bool mouseRightPressed = Input.GetMouseButtonDown(1);
            bool mouseIsDown = Input.GetMouseButton(0);
            bool mousePressed = Input.GetMouseButtonDown(0);
            Vector2 m = Event.current.mousePosition;
            Vector2 mOffset = m + m_scrollview;
            VNNode node = null;


            if (m_currentScene != null) {
                GUI.Label(gameNameRect, "Scene ID: " + m_currentScene.id);
                m_currentScene.name = GUI.TextField(gameNameEditRect, m_currentScene.name);

            } else {
                GUI.Label(gameNameRect, "Game Name: ");
                data.storyName = GUI.TextField(gameNameEditRect, data.storyName);
                if (m_selectedNode != null) {
                    if (GUI.Button(new Rect(gameNameRect.xMax + 10f, gameNameRect.y, 120f, 20f), "Set background")) {
#if UNITY_EDITOR
                        /*string path = EditorUtility.OpenFilePanel("Load Background", "", "");
                        if (path != string.Empty) {
                            StartCoroutine(loadData("file:///" + path));
                        }*/
#endif
                    }

                }
            }

            if (m_selectedNode != null && (m_currentScene != null || m_linkToScene)) {
                if (m_selectedNode.GetType() == typeof(VNMessage) && ((VNMessage)m_selectedNode).options != null && ((VNMessage)m_selectedNode).options.Count > 0) {
                    if (VNMessage.SelectedVNO != null) {
                        Drawing.DrawLine(VNMessage.SelectedVNO.position.center - m_scrollview, m, Color.green);
                        if (mouseRightPressed) {
                            VNNode n = GetHoveredNode(mOffset);
                            if (n != null) {
                                VNMessage.SelectedVNO.LeadsTo = n;
                                if (m_linkToScene && n != null) {
                                    m_linkToScene = false;
                                    OnDoubleClick(m_returnScene);
                                    m_returnScene = null;
                                }
                            }
                        }
                    }
                } else {
                    Drawing.DrawLine(m_selectedNode.position.center - m_scrollview, m, Color.green);
                    if (mouseRightPressed) {
                        VNNode n = GetHoveredNode(mOffset);
                        if (n != null) {
                            m_selectedNode.LeadsTo = n;
                            if (m_linkToScene) {
                                m_linkToScene = false;
                                OnDoubleClick(m_returnScene);
                                m_returnScene = null;
                                m_selectedNode = null;
                            }
                        }
                    }
                }
            }


            if (mouseRightPressed && m_selectedNode != null && editorDrawRegion.Contains(m)) { //clear data
                m_selectedNode = null;
                VNNode.Selected = null;
                VNNode.dragging = null;
                VNMessage.SelectedVNO = null;
            }

            if (m_nodes.Count > 0 && editorDrawRegion.Contains(m)) { //block dragging and selecting if not in range
                if (mousePressed || mouseIsDown) {
                    for (int i = m_nodes.Count - 1; i >= 0; --i) {
                        node = m_nodes[i];
                        if (node.drag(mouseIsDown, mousePressed, mOffset, m_scrollview)) {
                            if (internalSVRect.width < node.position.xMax)
                                internalSVRect.width = node.position.xMax;
                            if (internalSVRect.height < node.position.yMax)
                                internalSVRect.height = node.position.yMax;
                            break;
                        }
                    }
                } else if (!mouseIsDown) {
                    VNNode.dragging = null;
                }
            }

            GUILayout.BeginArea(editorDrawRegion); // editor draw region
            m_scrollview = GUILayout.BeginScrollView(m_scrollview, true, true);

            GUILayoutUtility.GetRect(internalSVRect.width, internalSVRect.height);

            VNMessage vnm;
            foreach (VNNode item in m_nodes) { //draw links betwen nodes/options
                if (item.GetType() == typeof(VNMessage)) {
                    vnm = (VNMessage)item;
                    if (vnm.options != null && vnm.options.Count > 0) {
                        foreach (VNOption opt in vnm.options) {
                            if (opt.LeadsTo != null) {
                                if (opt.LeadsTo.type != VNNodeType.Scene)
                                    Drawing.DrawLine(opt.position.center, opt.LeadsTo.position.center, Color.cyan);
                                else
                                    Drawing.DrawLine(opt.position.center, opt.LeadsTo.position.center, Color.magenta);
                            }
                        }
                    } else if (item.LeadsTo != null) {
                        if (item.LeadsTo.type != VNNodeType.Scene)
                            Drawing.DrawLine(item.position.center, item.LeadsTo.position.center, Color.cyan);
                        else
                            Drawing.DrawLine(item.position.center, item.LeadsTo.position.center, Color.magenta);
                    }
                } else if (item.LeadsTo != null) {
                    if (item.LeadsTo.type != VNNodeType.Scene)
                        Drawing.DrawLine(item.position.center, item.LeadsTo.position.center, Color.cyan);
                    else
                        Drawing.DrawLine(item.position.center, item.LeadsTo.position.center, Color.magenta);
                }
            }

            foreach (VNNode item in m_nodes) {
                if (m_currentScene == null) {
                    if (item.id == data.startPoint)
                        GUI.DrawTexture(selectedOffset.Offset(item.position), selectedText);
                } else if (m_currentScene.nodes.Count > 0) {
                    if (item.id == m_currentScene.startPoint)
                        GUI.DrawTexture(selectedOffset.Offset(item.position), selectedText);
                }
                item.drawGUI(mOffset);
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea(); //end editor draw region
            #region side buttons
            GUILayout.BeginArea(buttonDrawRegion);  // side buttons drawing
            string[] options = (m_currentScene != null) ? m_buttonSceneOptions : m_buttonOptions;

            foreach (string option in options) {
                try {
                    if (GUILayout.Button(option)) {
                        switch (option) {
                            case "View Scenes":
                                m_currentScene = null;
                                m_selectedNode = null;
                                m_nodes = data.ToListNode();
                                break;
                            case "Add Scene":
                                VNScene vns = new VNScene(new SimpleRect(nodeSpawnPosition.RectSumVector2(m_scrollview)), OnSelectChanged, OnDoubleClick);
                                m_scenes.Add(vns);
                                m_nodes.Add(vns);
                                break;
                            case "Add Message":
                                vnm = new VNMessage(new SimpleRect(nodeSpawnPosition.RectSumVector2(m_scrollview)), OnSelectChanged, OnDoubleClick, OnSelectChanged);
                                m_currentScene.nodes.Add(vnm);
                                m_nodes.Add(vnm);
                                if (m_currentScene.nodes.Count == 1)
                                    m_currentScene.startPoint = vnm.id;
                                break;
                            case "Delete Node":
                                if (m_selectedNode != null) {
                                    if (m_currentScene == null) {
                                        if (m_selectedNode.id == data.startPoint && m_nodes.Count > 0)
                                            data.startPoint = m_currentScene.nodes[0].id;
                                    } else {
                                        if (m_selectedNode.id == m_currentScene.startPoint) {
                                            if (m_currentScene.nodes.Count > 0)
                                                m_currentScene.startPoint = m_currentScene.nodes[0].id;
                                        }
                                        m_currentScene.nodes.Remove((VNMessage)m_selectedNode);
                                    }
                                    m_nodes.Remove(m_selectedNode);
                                    m_selectedNode.Dispose();
                                }
                                break;

                            case "Save":
                                foreach (VNScene n in m_scenes) {
                                    n.FixPos();
#if UNITY_EDITOR
                                    if (n.background != null) {
                                        //byte[] bytes = n.background.EncodeToPNG();
                                        //System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/Images/Scene/sceneBG_" + n.m_id + ".png", bytes);
                                    }
#endif
                                    foreach (VNMessage item in n.nodes)
                                        item.FixPos();
                                }
                                GameData.SaveString(
                                    JSONSaver.Save<VNSaveJSON>(data, ""),
                                    saveLocation + "\\story.txt", true);
                                break;
                            case "Load":
                                data = GameData.LoadData<VNSaveJSON>(
                                    saveLocation + "/story");
                                m_scenes = data.Extract();
                                m_nodes = data.ToListNode();
                                foreach (VNScene n in m_nodes) {
                                    n.OnDoubleClick = OnDoubleClick;
                                    n.OnSelect = OnSelectChanged;
                                    n.FixPosition();
                                    foreach (VNMessage item in n.nodes) {
                                        item.OnDoubleClick = OnDoubleClick;
                                        item.OnSelect = OnSelectChanged;
                                        item.OnOptionSelected = OnSelectChanged;
                                        item.FixPosition();
                                    }
                                }
                                break;

                            case "Display Editor":
                                enabled = false;
                                displayEditor.enabled = true;
                                break;
                            case "Character Editor":
                                enabled = false;
                                m_characterEditor.enabled = true;
                                break;
                            case "1st Message":
                                if (m_selectedNode.type == VNNodeType.Message)
                                    m_currentScene.startPoint = m_selectedNode.id;
                                break;
                            case "1st Scene":
                                if (m_selectedNode.type == VNNodeType.Scene)
                                    data.startPoint = m_selectedNode.id;
                                break;
                        }
                    }
                } catch { }
            }
            try {
                if (m_currentScene != null && m_selectedNode != null) {
                    if (GUILayout.Button("Link to Scene")) {
                        m_linkToScene = true;
                        m_returnScene = m_currentScene;
                        m_currentScene = null;
                        m_nodes = data.ToListNode();
                    }
                }
            } catch { }
            GUILayout.EndArea(); //end side buttons drawing
            #endregion
        }

        public void editMessage() {
            VNMessage vnm = ((VNMessage)m_selectedNode);

            GUILayout.BeginArea(buttonDrawRegion);
            try {
                if (GUILayout.Button("Finish Editing"))
                    m_editingNode = false;
                if (GUILayout.Button("Add Option"))
                    vnm.addOption();
            } catch { }

            GUILayout.EndArea();


            GUILayout.BeginArea(editorDrawRegion);
            //try {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Speaking Character:");
            int prevSelected = m_comboBox.GetSelectedItemIndex();
            vnm.characterID = m_comboBox.List(GUILayoutUtility.GetRect(100, 30), m_listCB[prevSelected], m_listCB, null);
            //int.TryParse(GUILayout.TextField(vnm.characterID.ToString(), GUILayout.Width(30)), out vnm.characterID);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            List<VNOption> options = vnm.options;
            if (options != null) {
                VNOption opt;
                m_editorOptionsScrollview = GUILayout.BeginScrollView(m_editorOptionsScrollview);
                for (int i = 0; i < options.Count; i++) {
                    opt = options[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Option " + i);
                    opt.displayText = GUILayout.TextField(opt.displayText, GUILayout.Width(430));
                    options[i] = opt;
                    if (i != 0 && GUILayout.Button("UP")) {
                        options[i] = options[i - 1];
                        options[i - 1] = opt;
                    }
                    if (i != options.Count - 1 && GUILayout.Button("DOWN")) {
                        options[i] = options[i + 1];
                        options[i + 1] = opt;
                    }
                    if (GUILayout.Button("REMOVE")) {
                        vnm.RemoveAt(i);
                    }
                    vnm.drag(false, false, Vector2.zero, Vector2.zero); //slacker code fix it
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            m_editorTextScrollview = GUILayout.BeginScrollView(m_editorTextScrollview);
            vnm.message = GUILayout.TextArea(vnm.message);
            GUILayout.EndScrollView();

            //} catch { }
            GUILayout.EndArea();
        }

        public VNNode GetHoveredNode(Vector2 mOffset) {
            foreach (VNNode item in m_nodes) {
                if (item.position.Contains(mOffset)) {
                    if (item != m_selectedNode) {
                        return item;
                    }
                }
            }
            return null;
        }

        #region events
        public void OnSelectChanged(VNNode node) {
            m_selectedNode = node;
        }

        public void OnSelectChanged(VNOption option, VNNode node) {
            m_selectedNode = node;
            m_selectedOption = option;
        }

        public void OnDoubleClick(VNNode node) {
            if (node.type == VNNodeType.Scene) {
                m_currentScene = (VNScene)node;
                m_nodes = m_currentScene.ToListNode();
                m_selectedNode = null;
            } else
                m_editingNode = true;
        }
        #endregion

        internal void UpdateFromVNCE() {
            m_listCB = new GUIContent[m_characterEditor.m_charHolder.characters.Count + 1];
            m_listCB[0] = new GUIContent("None");
            int i = 0;
            m_characterEditor.m_charHolder.characters.ForEach(
                x => m_listCB[++i] = new GUIContent(x.name)
                );
        }

        public IEnumerator loadData(string path) {
            WWW www = new WWW(path);

            yield return www;
            ((VNScene)m_selectedNode).background = www.texture;
        }

    } //class
} //namespace