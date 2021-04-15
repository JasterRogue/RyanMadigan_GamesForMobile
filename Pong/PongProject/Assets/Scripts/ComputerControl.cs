using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControl : MonoBehaviour
{

    float speed = 0.68f;
    GameObject ball;
    Vector2 ballPosition;
    float currentTime; 

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

        if(Time.time - currentTime >= 15)
        {
            speed = 0.68f;
            print("CPU speed back to normal");
        }

    }

    public void lowerCPUSpeed()
    {
        speed = 0.5f;
        currentTime = Time.time;
        print("CPU speed lowered");
    }
}
