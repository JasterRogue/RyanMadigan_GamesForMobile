using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeControl : MonoBehaviour, IControllable
{
    Renderer objectRenderer;
    Color32 objectColour;


    void Start()
    {

        objectRenderer = GetComponent<Renderer>();
        objectColour = GetComponent<Renderer>().material.color;


        if (SystemInfo.supportsGyroscope)
        {
           // Debug.Log("gyro");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void moveTo(Vector3 destination)
    {
        //Drag method 1
        // get the touch position from the screen touch to world point
        Vector3 touchedPos = Camera.main.ScreenToWorldPoint(destination);
        // lerp and set the position of the current object to that of the touch, but smoothly over time.
        transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
    }

    public void objectDeselected()
    {
        objectRenderer.material.SetColor("_Color", objectColour);
    }

    public void objectSelected()
    {
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
        print("" + newScale);

        transform.localScale = newScale;
    }

    public void youveBeenTouched()
    {

    }

    // Start is called before the first frame update


}
