using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTest : MonoBehaviour
{

    public GameObject spell;
    public GameObject spellSpawn;

    private float timer = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    timer += Time.deltaTime;

	    if (timer > 2)
	    {
	        ShootSpell();
	        timer = 0;
	    }
	}

    void ShootSpell()
    {
        GameObject go = Instantiate(spell);
        go.transform.position = spellSpawn.transform.position;
        Destroy(go, 5.0f);
    }
}
