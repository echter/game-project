using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellInventory : MonoBehaviour
{

    private KeyCode PrimarySpellBinding = KeyCode.Q;
    private KeyCode SecondarySpellBinding = KeyCode.W;
    private KeyCode TertiarySpellBinding;

    private Spell primarySpell;
    private Spell secondarySpell;
    private Spell tertiarySpell;

    private SpellList availableSpells;

    // Use this for initialization
    void Start ()
	{
	    availableSpells = gameObject.GetComponent<SpellList>();
    }

    public void InitiateSpells()
    {
        primarySpell = availableSpells.getBasicSpell();
        secondarySpell = availableSpells.getFireSpell();
    }

    public Spell Primary
    {
        get { return primarySpell; }
        set { primarySpell = value; }
    }

    public Spell Secondary
    {
        get { return secondarySpell; }
        set { secondarySpell = value; }
    }

    public KeyCode PrimaryKey
    {
        get { return PrimarySpellBinding; }
        set { PrimarySpellBinding = value; }
    }

    public KeyCode SecondaryKey
    {
        get { return SecondarySpellBinding; }
        set { SecondarySpellBinding = value; }
    }
}
