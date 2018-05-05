using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SpellSpawnController : NetworkBehaviour {

    // Drag and drop spell object in editor for this to be initialized
    private GameObject SpellPrefab;

    // Position for the spell to be spawned
    public GameObject SpellSpawn;

    // Easy access to playerObject rigidbody;
    private Rigidbody _rb;

    // Player from the main controller
    public MainController Player;

    private float _primaryCoolDownDuration;
    private float _primarySpellCooldownTimer = 0.0f;
    private bool _primarySpellCooldownBoolean = false;

    private float _secondaryCoolDownDuration;
    private float _secondarySpellCooldownTimer = 0.0f;
    private bool _secondarySpellCooldownBoolean = false;

    // List of bound spells for this player
    private PlayerSpellInventory playerSpells;

    // Use this for initialization
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        Player = gameObject.GetComponent<MainController>();
        playerSpells = gameObject.GetComponent<PlayerSpellInventory>();

        StartCoroutine(SpellInit(0.1f));
    }

    // Put all physics related code here
    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(playerSpells.Primary.EffectName);
        if (!hasAuthority)
        {
            return;
        }

        CanShootPrimarySpell();
        CanShootSecondarySpell();

        if (Player.status != 0)
        {
            // If wanting to shootSpell, initialize a basic spell
            if (Input.GetKeyDown(playerSpells.PrimaryKey))
            {
                if (_primarySpellCooldownBoolean == false)
                {
                    SpellLogic(playerSpells.Primary);

                    // Set cooldown
                    _primarySpellCooldownBoolean = true;
                    _primaryCoolDownDuration = playerSpells.Primary.CoolDownDuration;
                }
            }

            // If wanting to shootSpell, initialize a secondary spell
            if (Input.GetKeyDown(playerSpells.SecondaryKey))
            {
                if (_secondarySpellCooldownBoolean == false)
                {
                    SpellLogic(playerSpells.Secondary);

                    // Set cooldown
                    _secondarySpellCooldownBoolean = true;
                    _secondaryCoolDownDuration = playerSpells.Secondary.CoolDownDuration;
                }
            }
        }
    }

    void CanShootPrimarySpell()
    {
        if (_primarySpellCooldownBoolean)
        {
            _primarySpellCooldownTimer += Time.deltaTime;
        }
        else
        {
            _primarySpellCooldownTimer = 0.0f;
        }

        if (_primarySpellCooldownTimer > _primaryCoolDownDuration)
        {
            _primarySpellCooldownBoolean = false;
            _primarySpellCooldownTimer = 0.0f;
        }
    }

    void CanShootSecondarySpell()
    {
        if (_secondarySpellCooldownBoolean)
        {
            _secondarySpellCooldownTimer += Time.deltaTime;
        }
        else
        {
            _secondarySpellCooldownTimer = 0.0f;
        }

        if (_secondarySpellCooldownTimer > _primaryCoolDownDuration)
        {
            _secondarySpellCooldownBoolean = false;
            _secondarySpellCooldownTimer = 0.0f;
        }
    }

    [Command]
    private void CmdSpawnSpell(string spellName)
    {
        GameObject go = Instantiate(SpellPrefab, SpellSpawn.transform.position, gameObject.transform.rotation);
        SpellController sc = go.GetComponent<SpellController>();
        sc.damage = 20;
        sc.effectName = spellName;
        NetworkServer.Spawn(go);
        Destroy(go, 5.0f);
    }

    [Command]
    private void CmdUpdateRotation(NetworkInstanceId id, Quaternion rotation)
    {
        GameObject go = NetworkServer.FindLocalObject(id);
        go.GetComponent<Rigidbody>().transform.rotation = rotation;
    }

    // Rotate towards a vector position
    void RotateToPosition(Vector3 position)
    {
        gameObject.transform.LookAt(position);

        // Set rigidbody rotation to match transform position
        _rb.rotation = gameObject.transform.rotation;
    }

    IEnumerator SpellInit(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        playerSpells.InitiateSpells();
    }

    void SpellLogic(Spell spell)
    {
        //ShootSpell(playerSpells.primarySpell, SpellPrefab);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 location = hit.point;
            RotateToPosition(location);
        }
        NetworkIdentity id = gameObject.GetComponent<NetworkIdentity>();
        CmdUpdateRotation(id.netId, gameObject.transform.rotation);

        // Set prefab
        SpellPrefab = spell.Prefab;

        CmdSpawnSpell(spell.EffectName);
    }

    public void SetPrimarySpell()
    {
        if (isClient)
        {
            Debug.Log("im a client");
        }
        if (isServer)
        {
            Debug.Log("im a server");
        }
        if (!hasAuthority)
        {
            Debug.Log("denied");
            return;
        }
        Debug.Log("granted");
        Spell spell = gameObject.GetComponent<SpellList>().getFireSpell();
        Debug.Log(spell.EffectName);
        gameObject.GetComponent<PlayerSpellInventory>().Primary = spell;
        Debug.Log(gameObject.GetComponent<PlayerSpellInventory>().Primary.EffectName);
    }

    public void Rebind()
    {
        playerSpells.PrimaryKey = KeyCode.A;
    }
}
