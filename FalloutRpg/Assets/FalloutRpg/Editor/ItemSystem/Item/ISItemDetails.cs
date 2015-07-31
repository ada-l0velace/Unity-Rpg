using UnityEngine;
using UnityEditor;
using System.Collections;

namespace FalloutRpg.ItemSystem.Editor {

	public partial class ISItemDatabaseEditor  {
		enum DisplayState {
			NONE,
			DETAILS
		}

		DisplayState _state = DisplayState.NONE;
		ISWeapon tempWeapon = new ISWeapon();
		bool togleNewWeapon = false;
		void ItemDetails () {
			GUILayout.BeginVertical("Box", GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));

			GUILayout.BeginHorizontal (GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));

			switch (_state) {
				case DisplayState.DETAILS:
					{
						if (togleNewWeapon)
							DisplayNewWeapon ();
						break;
					}
				default:
					break;
			}

			/*if (togleNewWeapon) 
				DisplayNewWeapon ();
			*/
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
					_state = DisplayState.DETAILS;
				}
			} else {
				if (GUILayout.Button ("Save")) {
					if (_gridIndex == -1) {
						db.Add (tempWeapon);
					} else {
						db.Replace (_gridIndex, tempWeapon);
					}
					togleNewWeapon = false;
					tempWeapon = null;
					_gridIndex = -1;
					_state = DisplayState.NONE;
					
				}
				if (_gridIndex != -1) {
					if (GUILayout.Button ("Delete")) {
						db.Remove (_gridIndex);
						togleNewWeapon = false;
						tempWeapon = null;
						_gridIndex = -1;
						_state = DisplayState.NONE;						
					}
				}
				if (GUILayout.Button ("Cancel")) {
					togleNewWeapon = false;
					tempWeapon = null;
					_gridIndex = -1;
					_state = DisplayState.NONE;
				}
			}
		}

	}
}
