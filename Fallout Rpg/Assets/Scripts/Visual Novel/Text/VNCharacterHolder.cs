namespace VisualNovel {
    using UnityEngine;
    using System.Collections.Generic;

    public class VNCharacterHolder {
        public List<VNCharacter> characters;

        internal VNCharacter Get(int id) {
            return characters.Find(x => x.id == id);
        }
    }


}