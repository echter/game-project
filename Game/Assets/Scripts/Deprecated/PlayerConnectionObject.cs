using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
{

    public GameObject PlayerUnitPrefab;

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
    }

    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(PlayerUnitPrefab);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        go.transform.parent = this.gameObject.transform;
    }
}
