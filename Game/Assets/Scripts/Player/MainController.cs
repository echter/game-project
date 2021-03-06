﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class MainController : NetworkBehaviour
{

    public float MaxHealth = 100.0f;
    [SyncVar] public float Health = 100.0f;
    public Image HealthBar;

    // The desired goal of the character
    private Vector3 walkGoal;

    // Direction character is facing and waling towards
    private Vector3 direction;

    // Character movementspeed
    private float movementSpeed = 5.0f;

    // Velocity exclusive for walking, enables physics to be set on collision instead of auto
    public Vector3 walkVelocity = Vector3.zero;

    // Initialize stun timer
    private float stunTimer = 0.0f;

    // Easy access to playerObject rigidbody;
    private Rigidbody rb;

    // Determines the state of the character[ 0 = stunned, 1 = walking, 2 = idle]
    public int status = 2;

    private Terrain terrain;
    private float lavaTimer = 0.0f;

    // Use this for initialization
    void Start ()
    {
        terrain = Terrain.activeTerrain;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, (float)(Health / MaxHealth), 10f);

        if (!hasAuthority)
        {
            return;
        }

        if (getTerrainTextureAt(gameObject.transform.position).name.Equals("emissive"))
        {
            lavaTimer += Time.deltaTime;
            if (lavaTimer > 0.5f)
            {
                CmdTakeLavaDamage();
                lavaTimer = 0;
            }
        }

        if (status != 0)
        {
            // If wanting to walk, set the walk goal
            if (Input.GetMouseButtonDown(1))
            {
                setWalkLocation();
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (!hasAuthority)
	    {
	        return;
	    }
        // If not walking, walk velocity is 0
        if (status != 1)
	    {
	        walkVelocity = Vector3.zero;
	    }

        // If stunned, run stun code
        if (status == 0)
	    {
	        Stunned();
	    }
	    else
	    {
            // If walking, run walk code
            if (status == 1)
	        {
	            Walk();
	        }
        }	
	}

    // Unable to perform actions untill stun is over
    void Stunned()
    {
        stunTimer += Time.deltaTime;
        if (stunTimer > 0.5f)
        {
            status = 2;
            stunTimer = 0;
        }
    }

    // Rotate towards a vector position
    void rotateToPosition(Vector3 position)
    {
        gameObject.transform.LookAt(position);

        // Set rigidbody rotation to match transform position
        rb.rotation = gameObject.transform.rotation;
    }

    // Sets the walkGoal based on where the mouse was clicked
    void setWalkLocation()
    {

        // Get the point on the ground from where mouse is pointing relative to the camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Sets walk goal to the point hit by the mouse
            walkGoal = cutYComponent(hit.point);

            // set state to walking
            status = 1;

            // rotate character to look at the position
            rotateToPosition(walkGoal);
        }
    }

    // Walks towards the walk goal (moves the rigidbody and sets a velocity unrelated)
    void Walk()
    {
        // Finds the direnction inwhich we want to walk
        direction = (walkGoal - transform.position).normalized;

        // If destination is more or less reached, set status to Idle and remove velocity
        if ((walkGoal  - transform.position).magnitude < 0.2)
        {
            walkVelocity = Vector3.zero;
            status = 2;
            return;
        }

        // Move position and set velocity
        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
        walkVelocity = direction * movementSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("spell"))
        {
            status = 0;
        }
    }

    Vector3 cutYComponent(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    public Texture getTerrainTextureAt(Vector3 position)
    {
        // Set up:
        Texture retval = new Texture();
        Vector3 TS; // terrain size
        Vector2 AS; // control texture size

        TS = Terrain.activeTerrain.terrainData.size;
        AS.x = Terrain.activeTerrain.terrainData.alphamapWidth;
        AS.y = Terrain.activeTerrain.terrainData.alphamapHeight;
        var terrainPos = terrain.transform.position;
        var terrainData = terrain.terrainData;

        // Lookup texture we are standing on:
        var mapX = ((gameObject.transform.position.x - terrainPos.x) / TS.x) * terrainData.alphamapWidth;
        var mapZ = ((gameObject.transform.position.z - terrainPos.z) / TS.z) * terrainData.alphamapHeight;
        float[,,] TerrCntrl = Terrain.activeTerrain.terrainData.GetAlphamaps((int)mapX, (int)mapZ, 1, 1);

        TerrainData TD = Terrain.activeTerrain.terrainData;

        for (int i = 0; i < TD.splatPrototypes.Length; i++)
        {
            if (TerrCntrl[0, 0, i] > .5f)
            {
                retval = TD.splatPrototypes[i].texture;
            }

        }


        return retval;
    }

    [Command]
    void CmdTakeLavaDamage()
    {
        Health -= 5.0f;
        if (Health <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.1f);
        }
    }
}
