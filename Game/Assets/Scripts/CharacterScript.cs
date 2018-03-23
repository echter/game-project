using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterScript : MonoBehaviour
{
    public NavMeshAgent character;

    void Awake()
    {
        character = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f) && Input.GetMouseButtonDown(1))
        {
            character.destination = hit.point;
        }
    }

}
