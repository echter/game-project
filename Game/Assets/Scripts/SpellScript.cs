using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{

    public Vector3 destination;
    private Vector3 direction;

    private float spellSpeed = 0.3f;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        // TODO
	    transform.Translate(destination * spellSpeed);
	}
}
