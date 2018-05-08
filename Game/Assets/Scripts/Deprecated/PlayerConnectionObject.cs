using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
{

    public GameObject PlayerUnitPrefab;

    private GameObject player;

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
    }

    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(PlayerUnitPrefab);
        player = go;
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        go.transform.parent = gameObject.transform;
    }

    [Command]
    public void CmdRebindPrimary()
    {
        TargetUpdateVariable(connectionToClient, player.GetComponent<NetworkIdentity>());
    }

    [Command]
    public void CmdRebind()
    {
        TargetUpdateKeybindings(connectionToClient, player.GetComponent<NetworkIdentity>());
    }

    [TargetRpc]
    void TargetUpdateVariable(NetworkConnection target, NetworkIdentity id)
    {
        GameObject go = ClientScene.FindLocalObject(id.netId);
        Spell fire = go.GetComponent<SpellList>().getFireSpell();
        go.GetComponent<PlayerSpellInventory>().Primary = fire;
    }

    [TargetRpc]
    void TargetUpdateKeybindings(NetworkConnection target, NetworkIdentity id)
    {
        GameObject go = ClientScene.FindLocalObject(id.netId);
        go.GetComponent<PlayerSpellInventory>().PrimaryKey = KeyCode.A;
    }
}
