namespace VisualNovel {
    using UnityEngine;
    using System.Collections;

    public class VNCharacter {

        internal static int ID = 0;

        internal Texture2D portrait;
        public string name;
        public int id;

        public VNCharacter() {
            id = ++ID;
            name = "Character " + id;
        }



        internal IEnumerator loadPortrait(string path) {
            WWW www = new WWW(path);
            yield return www;
            if (www.error == "")
                portrait = www.texture;
            else
                Debug.Log(www.error);
        }
    }
}