using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereControl : MonoBehaviour, IControllable
{
    private Vector3 dragPosition;

    public void moveTo( Vector3 destination)
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
        transform.position += Vector3.right;
    }

    // Start is called before the first frame update
    void Start()
    {
        dragPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, dragPosition, 0.05f);
    }
}
