using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SpellController : NetworkBehaviour
{
    private String effect = "Explosion";

    public float speed;
    public float damage;
    private Vector3 velocity;
    public float knockbackFactor;

    public float stunTime;

    private NetworkIdentity spellId;


    // Use this for initialization
    void Start ()
	{
	    spellId = gameObject.GetComponent<NetworkIdentity>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        velocity = new Vector3(gameObject.transform.forward.x * speed, 0, gameObject.transform.forward.z * speed);
        gameObject.transform.position += velocity;
	}

    void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
        {
            return;
        }
        // This is the spell gameobject on the server
        GameObject go = NetworkServer.FindLocalObject(gameObject.GetComponent<NetworkIdentity>().netId);

        RpcSpawnEffect(go.transform.position, effect);
        if (collision.gameObject.tag.Equals("player"))
        {
            NetworkIdentity id = collision.gameObject.GetComponent<NetworkIdentity>();
            MainController mainController = collision.gameObject.GetComponent<MainController>();
            Rigidbody hitRB = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 totalVelocity = (velocity * knockbackFactor) + mainController.walkVelocity;
            hitRB.velocity = totalVelocity;
            RpcDoPhysics(id);
            mainController.Health -= damage;
            if (mainController.Health <= 0)
            {
                collision.gameObject.SetActive(false);
                Destroy(collision.gameObject, 0.1f);
                go.SetActive(false);
                Destroy(go, 0.1f);
                return;
            }
        }
        go.SetActive(false);
        Destroy(go, 0.1f);
    }

    [ClientRpc]
    void RpcDoPhysics(NetworkIdentity id)
    {
        GameObject collision = ClientScene.FindLocalObject(id.netId);
        MainController mainController = collision.GetComponent<MainController>();
        Rigidbody hitRB = collision.gameObject.GetComponent<Rigidbody>();
        Vector3 totalVelocity = (velocity * knockbackFactor) + mainController.walkVelocity;
        hitRB.velocity = totalVelocity;
    }

    [ClientRpc]
    void RpcSpawnEffect(Vector3 position, String effectName)
    {
        GameObject e = Instantiate(Resources.Load<GameObject>(effectName));
        e.transform.position = position;
        Destroy(e, 2.0f);
    }
}
