namespace VisualNovel {
    using System.Collections.Generic;
    using VisualNovel.Editor;
    using UnityEngine;

    public class VNSaveJSON {

        public List<VNScene> scenes = new List<VNScene>();
        public string storyName = "";
        public int startPoint = -1;

        public VNSaveJSON() { }


        internal List<VNScene> Extract() {
            List<VNNode> allNodes = new List<VNNode>();


            foreach (VNScene scene in scenes) {
                allNodes.Add(scene);
                foreach (VNNode node in scene.nodes) {
                    allNodes.Add(node);
                }
            }

            int id;
            foreach (VNNode item in allNodes) {
                id = item.leadsToID;
                if (id != -1) {
                    item.LeadsTo = allNodes.Find(x => x.id == id);
                }
            }

            List<VNOption> opt = null;
            foreach (VNNode item in allNodes) {
                if (item.GetType() != typeof(VNMessage))
                    continue;
               
                    opt = ((VNMessage)item).options;
                if (opt != null) {
                    foreach (VNOption op in opt) {
                        id = op.leadsToID;
                        if (id != -1) {
                            op.LeadsTo = allNodes.Find(x => x.id == id);
                        }
                    }
                }
            }
            int i = 0;
            foreach (VNNode item in allNodes) {
                if (item.id > i)
                    i = item.id;
            }
            VNNode.FixIndex(i + 1);
            return scenes;
        }

        internal List<VNNode> ToListNode() {
            List<VNNode> list = new List<VNNode>();
            foreach (VNScene item in scenes) {
                list.Add(item);
            }
            return list;
        }
    }
}