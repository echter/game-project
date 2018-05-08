using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpellTest : NetworkBehaviour
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
	    if (!hasAuthority && !isServer)
	    {
	        return;
	    }

	    timer += Time.deltaTime;

	    if (timer > 2)
	    {
	        CmdShootSpell();
            timer = 0;
	    }
	}

    [Command]
    void CmdShootSpell()
    {
        GameObject go = Instantiate(spell);
        SpellController sc = go.GetComponent<SpellController>();
        sc.damage = 20;
        sc.effectName = "Explosion";
        go.transform.position = spellSpawn.transform.position;
        NetworkServer.Spawn(go);
        Destroy(go, 5.0f);
    }
}
