using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereControl : MonoBehaviour, IControllable
{
    private Vector3 dragPosition;
    Renderer objectRenderer;
    Color32 objectColour;
    float initialDistance; 


    public void moveTo(Vector3 pos)
    {
        //Drag method 2
        Ray newPositionRay = Camera.main.ScreenPointToRay(pos);
        Vector3 destination = newPositionRay.GetPoint(initialDistance);
        dragPosition = destination;
    }

    public void objectDeselected()
    {
        objectRenderer.material.SetColor("_Color", objectColour);
    }

    public void objectSelected()
    {
        initialDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
        objectRenderer.material.SetColor("_Color", Color.yellow);
    }

    public void rotateObject(Vector3 v)
    {
        transform.Rotate(v, Space.World);
    }

    public void scale(float percentageChange)
    {
        Vector3 newScale = transform.localScale;
        newScale += percentageChange * transform.localScale;

        transform.localScale = newScale;
    }

    public void youveBeenTouched()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        dragPosition = transform.position;
        objectRenderer = GetComponent<Renderer>();
        objectColour = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, dragPosition, 0.05f);
    }
}
