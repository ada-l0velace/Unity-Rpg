namespace VisualNovel {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class VNScene : VNNode {

        
        public string name;
        public List<VNMessage> nodes = new List<VNMessage>();
        public int startPoint = -1;
        internal Texture2D background;
        internal Rect bgRect;

        public VNScene():base() { }

        public VNScene(SimpleRect drawRect, OnSelectEvent callback, OnDoubleClickEvent doubleClickCB)
            : base(drawRect, callback, doubleClickCB, VNNodeType.Scene) {
                name = "Scene " + id;
        }

        protected override string ToNode() {
            return "Scene " + base.ToNode() + "\nName: " + name;
        }

        internal List<VNNode> ToListNode() {
            List<VNNode> list = new List<VNNode>();
            foreach (VNMessage item in nodes) {
                list.Add(item);
            }
            return list;
        }



        internal Texture2D Background {
            set {
                if (value != null) {
                    bgRect = new Rect(0, 0, value.width, value.height);
                    background = value;
                }
            } 
        }

    } //class
} //namespace