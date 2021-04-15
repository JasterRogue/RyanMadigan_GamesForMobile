using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalls : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D hitinfo)
    {
        if(hitinfo.name == "Ball")
        {
            string wallName = transform.name;
            GameManager.score(wallName);
            hitinfo.gameObject.SendMessage("resetBall", 1.0f, SendMessageOptions.RequireReceiver);
        }
    }
}
