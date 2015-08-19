using UnityEngine;
using System.Collections;
using FalloutRpg.ItemSystem;

[DisallowMultipleComponent]
public class Demo : MonoBehaviour {
	public ISWeaponDatabase _database;
	
	void Awake() {
		//_database = ISWeaponDatabase.GetDatabase<ISWeaponDatabase> (DATABASE_NAME, FILE_NAME);
	}

	void OnGUI () {
		for (int i = 0; i < _database.Count; i++) {
			if (GUILayout.Button (_database.Get(i).Name)) {
				Spawn (i);
			}
		}
	}
	// Use this for initialization
	void Start () {
		//_database = ISWeaponDatabase.GetDatabase<ISWeaponDatabase> (DATABASE_NAME, FILE_NAME);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Spawn (int index) {
		GameObject go = GameObject.Find ("PlayerCharacter");
		ISWeapon isw = _database.Get (index);
		GameObject weapon = (GameObject) Instantiate (isw.Prefab, go.transform.position, Quaternion.Euler(0, 0, 0));
		weapon.name = isw.Name;
		weapon.transform.parent = transform.parent;
		Weapon myWeapon = weapon.AddComponent<Weapon> ();
		myWeapon.weapon = new ISWeapon(isw);
		Debug.Log (myWeapon.weapon.Name);
		//myWeapon.Clone (isw);
	}
}
