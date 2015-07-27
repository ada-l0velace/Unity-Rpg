namespace VisualNovel {
    using UnityEngine;
    using System.Collections;

    public class VNNode {

        public static readonly Rect RECT_ZERO = new Rect(0, 0, 0, 0);
        public static VNNode dragging;
        public static VNNode Selected;
        public static Vector2 dragOffSet = Vector2.zero;

        public static GUIStyle normalStyle;
        public static GUIStyle selectedStyle;
        public static GUIStyle hoverStyle;

        public delegate void OnSelectEvent(VNNode node);
        internal OnSelectEvent OnSelect;

        public delegate void OnDoubleClickEvent(VNNode node);
        internal OnDoubleClickEvent OnDoubleClick;

        private static int ID = 0;

        public int id;
        internal Rect position;
        public SimpleRect pos;


        private VNNode m_leadsTo;
        public int leadsToID = -1;
        public VNNodeType type;

        internal VNNode LeadsTo {
            get { return m_leadsTo; }
            set {
                if (value == null) {
                    m_leadsTo = null;
                    leadsToID = -1;
                } else {
                    m_leadsTo = value;
                    leadsToID = value.id;
                }
            }
        }

        public VNNode() { }

        public VNNode(SimpleRect drawRect, OnSelectEvent selectCB, OnDoubleClickEvent doubleClickCB, VNNodeType type = VNNodeType.Basic) {
            id = ID++;
            pos = drawRect;
            position = new Rect(pos.x, pos.y, pos.width, pos.height);
            OnSelect += selectCB;
            OnDoubleClick = doubleClickCB;
            this.type = type;
        }

        public virtual void drawGUI(Vector2 mouse) {
            if (Selected == this)
                GUI.Box(position, ToNode(), selectedStyle);
            else if (position.Contains(mouse))
                GUI.Box(position, ToNode(), hoverStyle);
            else
                GUI.Box(position, ToNode(), normalStyle);
        }

        protected virtual string ToNode() {
            string s = "ID: " + id;
            if (LeadsTo != null)
                s += "\nLeads To ID: " + LeadsTo.id + ((LeadsTo.type == VNNodeType.Scene) ? "s" : "m");
            return s;
        }

        internal virtual bool drag(bool mouseDown, bool mousePress, Vector2 m, Vector2 offSet) {
            if (dragging == null) {
                if (mousePress) {
                    if (position.Contains(m)) {
                        OnSelect(this);
                        if (Event.current.clickCount >= 2)
                            OnDoubleClick(this);
                        Selected = this;
                        dragging = this;
                        dragOffSet.x = m.x - position.x - offSet.x;
                        dragOffSet.y = m.y - position.y - offSet.y;
                        return true;
                    }
                }
            } else if (dragging == this) {
                if (mouseDown) {
                    Vector2 v = Event.current.mousePosition;
                    position.x = v.x - dragOffSet.x;
                    position.y = v.y - dragOffSet.y;
                    return true;
                } else if (mousePress && Event.current.clickCount >= 2) {
                    OnDoubleClick(this);
                } else
                    dragging = null;
            }
            return false;
        }


        internal static void FixIndex(int index) {
            ID = index;
        }

        internal void FixPosition() {
            position.x = pos.x;
            position.y = pos.y;
            position.width = pos.width;
            position.height = pos.height;
        }

        internal void FixPos() {
            pos.x = position.x;
            pos.y = position.y;
            pos.width = position.width;
            pos.height = position.height;
        }

        internal virtual void Dispose() {
            OnSelect = null;
            OnDoubleClick = null;
            m_leadsTo = null;
        }

    } //class

    public enum VNNodeType {
        Basic,
        Message,
        Option,
        Scene
    }

} //namespace

