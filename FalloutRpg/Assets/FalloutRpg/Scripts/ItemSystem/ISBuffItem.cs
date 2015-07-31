using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace FalloutRpg.ItemSystem {

	[System.Serializable]
	public abstract class ISBuffItem : ISItem {
		[SerializeField] 
		public List <ISBuff<StatName>> _pBuffsL ;
		[SerializeField] 
		public List <ISBuff<DerivedName>> _dBuffsL = new List <ISBuff<DerivedName>>();
		private int _buffNum = 1;

		#region set and geters
		public List <ISBuff<StatName>> PBuffsL {
			get {
				return _pBuffsL;
			}
			set {
				_pBuffsL = value;
			}
		}
		public List <ISBuff<DerivedName>> DBuffsL {
			get {
				return _dBuffsL;
			}
			set {
				_dBuffsL = value;
			}
		}
		#endregion

		public void DisplayBuffs() {

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Buffs");
			if(GUILayout.Button ("+") && _pBuffsL.Count < Enum.GetValues (typeof(StatName)).Length){
				_pBuffsL.Add(new ISBuff<StatName>((StatName)0,0));
				//_buffNum++;
			}
			if(GUILayout.Button ("-") && _pBuffsL.Count > 0){
				_pBuffsL.RemoveAt(_pBuffsL.Count-1);
				//_buffNum--;
			}
			EditorGUILayout.EndHorizontal ();
			for (int i = 0; i < _pBuffsL.Count; i++) {
				EditorGUILayout.BeginHorizontal ("box");
				_pBuffsL[i].Stat = (StatName) EditorGUILayout.EnumPopup("Item", _pBuffsL[i].Stat);
				_pBuffsL [i].Value = Convert.ToInt32(EditorGUILayout.TextField ("Value", _pBuffsL [i].Value.ToString()));
				EditorGUILayout.EndHorizontal ();
			}
		}

	}
}