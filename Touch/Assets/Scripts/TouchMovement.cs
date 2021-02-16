using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMovement : MonoBehaviour, IControllable
{

    /*public void moveTo(Vector3 destination)
    {
        transform.position = destination;
    }*/

    public void moveTo(Vector3 destination)
    {
        // get the touch position from the screen touch to world point
        Vector3 touchedPos = Camera.main.ScreenToWorldPoint(destination);
        // lerp and set the position of the current object to that of the touch, but smoothly over time.
        transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
    }

    public void scale(float percentageChange)
    {
        Vector3 newScale = transform.localScale;
        newScale += percentageChange * transform.localScale;

        transform.localScale = newScale;
    }

    public void youveBeenTouched()
    {
        transform.position += Vector3.down;
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
}
