using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    // Move Camera Script

    private float speed = 0.001f;
 
    void FixedUpdate()
    {
        Vector2 mouseXY = new Vector2(Input.mousePosition.x - Screen.width / 2,
            Input.mousePosition.y - Screen.height / 2);

        if (Mathf.Abs(mouseXY.x) > 500 || Mathf.Abs(mouseXY.y) > 200)
        {
           // transform.Translate(mouseXY * speed);
        }

    }
}
