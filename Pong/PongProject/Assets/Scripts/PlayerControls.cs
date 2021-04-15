using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    float speed = 0.1f;
    //Create a modified version of this script for the other paddle. 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate(new Vector3(0, Input.acceleration.y, 0) * speed);

        if(transform.position.y < -3)
        {
            transform.position = new Vector3(-4, 0, 0);
        }

        if(transform.position.y > 3)
        {
            transform.position = new Vector3(-4, 0, 0);
        }

    }
}
