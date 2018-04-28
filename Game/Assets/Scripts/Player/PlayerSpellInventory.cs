using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellInventory : MonoBehaviour
{

    public KeyCode PrimarySpellBinding = KeyCode.Q;
    public KeyCode SecondarySpellBinding = KeyCode.W;
    public KeyCode TertiarySpellBinding;

    public Spell primarySpell = new Spell(0.1f, 10.0f, 2.0f, 1.0f, 50.0f);
    public Spell secondarySpell = new Spell(0.3f, 1000f, 5.0f, 5.0f, 100.0f);
    public Spell tertiarySpell;

    // Use this for initialization
    void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Rebind()
    {
    }
}
