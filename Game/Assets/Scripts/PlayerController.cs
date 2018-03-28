    using System.Collections;
using System.Collections.Generic;
    using System.Security;
    using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    public GameObject spell;
    public GameObject spellSpawn;

    private Rigidbody characterBody;
    private Vector3 walkGoal = Vector3.zero;
    private Vector3 oldWalkGoal;
    private float speed = 0.1f;

    void Start()
    {
        characterBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Walk();
        }
        if (Input.GetMouseButtonDown(0))
        {
            CmdShootSpell();
        }
    }

    void FixedUpdate()
    {
        if (walkGoal != Vector3.zero)
        {
            CmdWalkLogic();
            Debug.Log(characterBody.position);
        }
    }

    [Command]
    void CmdWalkLogic()
    {
        transform.position = Vector3.MoveTowards(transform.position, walkGoal, speed);
        float distance = (walkGoal - transform.position).magnitude;

        if (distance > 1)
        {
            // Rotate
            Vector3 dir = (cutYdimention(walkGoal) - cutYdimention(transform.position)).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);

            // Snap if distance is small
            if (distance < 2)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 50);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5);
        }
    }

    void Walk()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            oldWalkGoal = walkGoal;
            walkGoal = hit.point;
        }
    }

    [Command]
    void CmdShootSpell()
    {
        characterBody.velocity = Vector3.zero;
        Vector3 direction = Vector3.forward;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            direction = (hit.point - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 50);
        }

        GameObject go = Instantiate(spell);
        go.transform.rotation = Quaternion.LookRotation(direction);
        go.transform.position = spellSpawn.transform.position;

        Physics.IgnoreCollision(go.GetComponent<Collider>(), this.GetComponent<Collider>(), true);

        go.GetComponent<Rigidbody>().velocity = new Vector3(go.transform.forward.x, 0, go.transform.forward.z) * 15;
        NetworkServer.Spawn(go);
        Destroy(go, 5.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    Vector3 cutYdimention(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
}
