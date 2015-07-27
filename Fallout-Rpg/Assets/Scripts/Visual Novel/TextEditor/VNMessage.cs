namespace VisualNovel {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class VNMessage : VNNode {

        public static VNOption SelectedVNO;

        public string message = "";
        public int characterID = -1;
        public List<VNOption> options;
        public delegate void OnOptionSelectedEvent(VNOption option, VNNode node);
        internal OnOptionSelectedEvent OnOptionSelected;

        public static GUIStyle optionNormalStyle;
        public static GUIStyle optionSelectedStyle;
        public static GUIStyle optionHoverStyle;

        public VNMessage():base() {

        }

        public VNMessage(SimpleRect drawRect, OnSelectEvent callback, OnDoubleClickEvent doubleClickCB, OnOptionSelectedEvent optionEvent, VNNodeType type = VNNodeType.Message)
            : base(drawRect, callback, doubleClickCB, type) {
                OnOptionSelected = optionEvent;
        }
        
        public override void drawGUI(Vector2 mouse) {
            base.drawGUI(mouse);
            if (options != null && Selected == this) {
                
                foreach (VNOption vno in options) {
                    if (SelectedVNO == vno)
                        GUI.Box(vno.position, vno.displayText, optionSelectedStyle);
                    else if (vno.position.Contains(mouse)) {
                        if (Input.GetMouseButtonDown(0)) {
                            SelectedVNO = vno;
                            Selected = this;
                            OnOptionSelected(vno, this);
                        }
                        GUI.Box(vno.position, vno.displayText, optionHoverStyle);
                    } else
                        GUI.Box(vno.position, vno.displayText, optionNormalStyle);
                }
            }
        }

        internal override bool drag(bool mouseDown, bool mousePress, Vector2 m, Vector2 offSet) {
            bool flag = base.drag(mouseDown, mousePress, m, offSet);
            if (options != null) {
                Rect r = new Rect(position);
                r.y += r.height - 25 * options.Count;
                r.height = 25;
                r.x += 10;
                foreach (VNOption vno in options) {
                    vno.position = r;
                    r = new Rect(r);
                    r.y += r.height + 2;
                }
            }
            return flag;
        }


        internal void addOption() {
            if (options == null)
                options = new List<VNOption>();
            if (LeadsTo != null)
                LeadsTo = null;
            options.Add(new VNOption(""));
            position.height += 25;
            drag(false, false, Vector2.zero, Vector2.zero);
        }

        internal void RemoveAt(int i) {
            if (options.Count < i) {
                options.RemoveAt(i);
                position.height -= 25;
                if (options.Count == 0)
                    options = null;
            }
        }
    } //class

    public class VNOption {
        public string displayText;
        internal Rect position;
        public int leadsToID = -1;

        private VNNode m_leadsTo;

        internal VNNode LeadsTo {
            get { return m_leadsTo; }
            set {
                m_leadsTo = value;
                if (value == null)
                    leadsToID = -1;
                else
                    leadsToID = value.id;
            }
        }

        public VNOption() { }

        public VNOption(string text) {
            displayText = text;
            LeadsTo = null;
            position = new Rect();
        }

        public VNOption(string text, Rect r) {
            displayText = text;
            LeadsTo = null;
            position = r;
        }

        public VNOption(string text, VNNode node, Rect r) {
            displayText = text;
            LeadsTo = node;
            position = r;
        }
        
    }

} //namespace
