using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpellController : NetworkBehaviour
{

    public GameObject effect;

    public float speed;
    private Vector3 velocity;
    public float knockbackFactor;

    public float stunTime;


	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        velocity = new Vector3(gameObject.transform.forward.x * speed, 0, gameObject.transform.forward.z * speed);
        gameObject.transform.position += velocity;
	}

    void OnCollisionEnter(Collision collision)
    {
        GameObject e = Instantiate(effect);
        
        e.transform.position = gameObject.transform.position;
        Destroy(e, 2.0f);

        if (collision.gameObject.tag.Equals("player"))
        {
            DoPhysics(collision.gameObject);
        }

        Destroy(gameObject);
    }


    void DoPhysics(GameObject collision)
    {
        Debug.Log("doing");
        MainController mainController = collision.GetComponent<MainController>();
        Rigidbody hitRB = collision.GetComponent<Rigidbody>();
        Vector3 totalVelocity = (velocity * knockbackFactor) + mainController.walkVelocity;
        hitRB.velocity = totalVelocity;
    }
}
