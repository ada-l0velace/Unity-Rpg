using UnityEngine;
using System.Collections;

public class UnitCombatStats : MonoBehaviour {

    protected int totalHP;
    protected int hp;
    protected int damage;
    protected int range;

    void Start() {
        totalHP = hp = 10;
        damage = 5;
        range = 1;
    }

    void Update() {

    }

    public void takeDamage(int damage) {
        hp -= 5;
        if (hp <= 0) {
            Debug.Log("Unit dead. Missing code.");
        }
    }

    public int Damage {
        get { return damage; }
    }
    public int Range {
        get { return range; }
    }
}
