using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{

    private Rigidbody2D rb2d;
    AdMobBanner myAdMob;

    void goBall()
    {
        float random = Random.Range(0, 2);

        if(random < 1)
        {
            rb2d.AddForce(new Vector2(20, -15));
        }

        else
        {
            rb2d.AddForce(new Vector2(-20, -15));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke("goBall", 2);
        myAdMob = GetComponent<AdMobBanner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!myAdMob)
        {
            //myAdMob = GetComponent<AdMobBanner>();
            myAdMob = GameObject.Find("AdManager").GetComponent<AdMobBanner>();
        }
        
    }

    void resetBall()
    {
        rb2d.velocity = Vector2.zero;
        transform.position = Vector2.zero;

        if (GameManager.PlayerScore1 != 10 || GameManager.PlayerScore2 != 10)
        {
            Invoke("goBall", 1);
        }
        
    }

    void restartGame()
    {
        resetBall();

        myAdMob.requestInterstetialGoogle();

        myAdMob.whichInterstetialToShow();

        Invoke("goBall", 1);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            Vector2 vel;
            vel.x = rb2d.velocity.x;
            vel.y = (rb2d.velocity.y / 2) + (coll.collider.attachedRigidbody.velocity.y / 3);
            rb2d.velocity = vel;
        }
    }
}
