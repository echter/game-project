using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellList : MonoBehaviour {

    public GameObject basicSpellPrefab;
    public GameObject fireSpellPrefab;
    public GameObject iceSpellPrefab;
    public GameObject gravitySpellPrefab;

    public Spell getBasicSpell()
    {
        Spell s = new Spell(0.4f, 10.0f, 2.0f, 1.0f, 50.0f, basicSpellPrefab, "Explosion");
        return s;
    }

    public Spell getFireSpell()
    {
        return new Spell(0.25f, 10.0f, 2.0f, 1.0f, 50.0f, fireSpellPrefab, "FireEffect");
    }
}
