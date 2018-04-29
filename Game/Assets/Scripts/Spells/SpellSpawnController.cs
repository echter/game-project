using UnityEngine;
using UnityEngine.Networking;

public class SpellSpawnController : NetworkBehaviour {

    // Drag and drop spell object in editor for this to be initialized
    public GameObject BasicSpell;
    public GameObject SecondarySpell;

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
    }

    // Update is called once per frame
    void Update () {

        if (!hasAuthority)
        {
            return;
        }

        CanShootPrimarySpell();
        CanShootSecondarySpell();

        if (Player.status != 0)
        {
            // If wanting to shootSpell, initialize a basic spell
            if (Input.GetKeyDown(playerSpells.PrimarySpellBinding))
            {
                if (_primarySpellCooldownBoolean == false)
                {
                    CmdShootSpell(playerSpells.primarySpell, BasicSpell);
                    _primarySpellCooldownBoolean = true;
                }
            }

            // If wanting to shootSpell, initialize a secondary spell
            if (Input.GetKeyDown(playerSpells.SecondarySpellBinding))
            {
                if (_secondarySpellCooldownBoolean == false)
                {
                    CmdShootSpell(playerSpells.secondarySpell, SecondarySpell);
                    _secondarySpellCooldownBoolean = true;
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

    // Initializes a basic spell gameObject
    [Command]
    public void CmdShootSpell(Spell spell, GameObject goSpell)
    {

        // Set the rotation of the spellObject to the players rotation so it goes in the right direction
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 location = hit.point;
            RotateToPosition(location);
        }

        GameObject go = Instantiate(goSpell, SpellSpawn.transform.position, gameObject.transform.rotation);
        NetworkServer.Spawn(go);
        SpellController sp = go.GetComponent<SpellController>();

        sp.speed = spell.Speed;
        sp.knockbackFactor = spell.Kockback;
        sp.stunTime = spell.StunTime;
        _primaryCoolDownDuration = spell.CoolDownDuration;

        Destroy(go, 5.0f);
    }

    // Rotate towards a vector position
    void RotateToPosition(Vector3 position)
    {
        gameObject.transform.LookAt(position);

        // Set rigidbody rotation to match transform position
        _rb.rotation = gameObject.transform.rotation;
    }
}
