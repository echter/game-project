using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
{

    public GameObject PlayerUnitPrefab;
    public GameObject spell;
    public GameObject spellSpawn;
    public NavMeshAgent navMesh;
    private float timer;

    void Start()
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        CmdSpawnMyUnit();
    }

    void Update()
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer > 3 && navMesh)
        {
            navMesh.enabled = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            ShootSpell();
        }
        else if (Input.GetMouseButtonDown(0))
        {
           walk();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<NavMeshAgent>() != null)
        {
            navMesh = GetComponent<NavMeshAgent>();
            navMesh.enabled = false;
        }
    }

    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(PlayerUnitPrefab);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        navMesh = go.GetComponent<NavMeshAgent>();
        go.transform.parent = this.gameObject.transform;
    }

    void walk()
    {
        Vector3 direction = Vector3.forward;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            navMesh.destination = hit.point;
        }
    }

    //[ClientRpc]
    void ShootSpell()
    {
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

        go.GetComponent<Rigidbody>().velocity = new Vector3(go.transform.forward.x, 0, go.transform.forward.z) * 15;
        NetworkServer.Spawn(go);
        Destroy(go, 5.0f);
    }
}
