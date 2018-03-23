using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpell : MonoBehaviour
{

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = new Vector2(0, 0);

    private bool pressed = false;

    public GameObject spell;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space"))
        {
            if (pressed)
            {
                Cursor.SetCursor(null, hotSpot, cursorMode);
                pressed = false;
            }
            else
            {
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
                pressed = true;
            }
        }

        if (pressed && Input.GetMouseButtonDown(0))
        {
            GameObject go = Instantiate(spell, this.transform.position, spell.transform.rotation);
            SpellScript ss = go.GetComponent<SpellScript>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f) && Input.GetMouseButtonDown(0))
            {
                ss.destination = hit.point;
            }
        }

    }
}
