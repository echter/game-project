using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthbarController : NetworkBehaviour {

    Quaternion rotation;
    void Awake()
    {
        rotation = Quaternion.Euler(60,0,0);
    }
    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
