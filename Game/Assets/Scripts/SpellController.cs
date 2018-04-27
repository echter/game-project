using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{

    public GameObject effect;
    public float speed = 0.1f;

    private Vector3 velocity;
    private float knockbackFactor = 150.0f;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
        velocity = new Vector3(gameObject.transform.forward.x, 0, gameObject.transform.forward.z) * speed;
        gameObject.transform.position += velocity;
	}

    void OnCollisionEnter(Collision collision)
    {
        GameObject e = Instantiate(effect);
        e.transform.position = gameObject.transform.position;
        Destroy(e, 2.0f);

        if (collision.gameObject.tag.Equals("player"))
        {
            MainController mainController = collision.gameObject.GetComponent<MainController>();
            Rigidbody hitRB = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 totalVelocity = (velocity * knockbackFactor) + mainController.walkVelocity;
            hitRB.velocity = totalVelocity;
        }

        Destroy(gameObject);
    }
}
