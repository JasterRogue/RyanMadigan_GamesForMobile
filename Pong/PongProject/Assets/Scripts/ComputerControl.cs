using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControl : MonoBehaviour
{

    float speed = 0.65f;
    GameObject ball;
    Vector2 ballPosition;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        if(!ball)
        {
            ball = GameObject.FindGameObjectWithTag("Ball");
        }

        else
        {
            ballPosition = ball.transform.position;
            transform.position = new Vector2(4, ballPosition.y * speed);
        }

        
    }
}
