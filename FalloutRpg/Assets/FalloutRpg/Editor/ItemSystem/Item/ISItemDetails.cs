using UnityEngine;
using System.Collections;

namespace FalloutRpg.ItemSystem.Editor {
	public partial class ISItemDatabaseEditor  {
		ISWeapon tempWeapon = new ISWeapon();
		bool togleNewWeapon = false;
		void ItemDetails () {
			GUILayout.BeginVertical("Box", GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));

			GUILayout.BeginHorizontal (GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));
			if (togleNewWeapon) 
				DisplayNewWeapon ();
			GUILayout.EndHorizontal ();
					
			GUILayout.BeginHorizontal (GUILayout.ExpandWidth (true));
			DisplayButtons ();
			GUILayout.EndHorizontal ();

			GUILayout.EndVertical();
		}

		void DisplayNewWeapon() {
			GUILayout.BeginHorizontal (GUILayout.ExpandWidth(true), GUILayout.ExpandHeight (true));
			tempWeapon.OnGUI ();
			GUILayout.EndHorizontal ();



		}

		void DisplayButtons() {
			if (!togleNewWeapon) {
				if (GUILayout.Button ("Create new Weapon")) {
					tempWeapon = new ISWeapon ();
					togleNewWeapon = true;
				}
			} else {
				if (GUILayout.Button ("Save")) {
					db.Add (tempWeapon);

					togleNewWeapon = false;
					tempWeapon = null;
				}

				if (GUILayout.Button ("Cancel")) {
					togleNewWeapon = false;
					tempWeapon = null;
				}
			}
		}

	}
}
