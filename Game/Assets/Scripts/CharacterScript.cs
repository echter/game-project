using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    private Rigidbody characterBody;

    private float speed = 5f;
    private float spacing = 0.05f;
    private Vector3 pos;
 
    void Start()
    {
        characterBody = GetComponent<Rigidbody>();
        pos = transform.position;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
           characterBody.velocity = new Vector3(0, 0, speed);
        if (Input.GetKey(KeyCode.DownArrow))
            characterBody.velocity = new Vector3(0, 0, -speed);
        if (Input.GetKey(KeyCode.LeftArrow))
            characterBody.velocity = new Vector3(-speed, 0, 0);
        if (Input.GetKey(KeyCode.RightArrow))
            characterBody.velocity = new Vector3(speed, 0, 0);

        if (characterBody.velocity.magnitude > 20)
        {
            characterBody.velocity = Vector3.forward * 20;
        }

    }
}
