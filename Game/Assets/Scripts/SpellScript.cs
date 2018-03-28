using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class SpellScript : MonoBehaviour
{

    public GameObject collisionEffect;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Rigidbody>() && col.gameObject.GetComponent<NavMeshAgent>())
        {
            col.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            col.gameObject.GetComponent<Rigidbody>().velocity = (GetComponent<Rigidbody>().velocity);
            Debug.Log(GetComponent<Rigidbody>().velocity + "     " + col.gameObject.GetComponent<Rigidbody>().velocity);
        }
        GameObject go = Instantiate(collisionEffect, this.transform.position, collisionEffect.transform.rotation);
        Destroy(go, 1.1f);
        Destroy(this.gameObject);
    }
}
