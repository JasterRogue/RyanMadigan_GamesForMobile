using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CylinderControl : MonoBehaviour, IControllable
{
    Renderer objectRenderer;
    Color32 objectColour;
    Vector3 dragPosition; 

    void Start()
    {
        dragPosition = transform.position;
        objectRenderer = GetComponent<Renderer>();
        objectColour = GetComponent<Renderer>().material.color;


        if (SystemInfo.supportsGyroscope)
        {
            //Debug.Log("gyro");
        }

    }

    // Update is called once per frame
    void Update()
    {
       
        transform.position = Vector3.Lerp(transform.position, dragPosition, 0.5f);
    }

    public void moveTo(Vector3 destination)
    {
        //Drag method 3
        Ray newPositionRay = Camera.main.ScreenPointToRay(destination);
        RaycastHit[] hits = Physics.RaycastAll(newPositionRay);
        int groundMask = LayerMask.NameToLayer("Ground");

        float closest = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.layer == groundMask)
            {
                if (hit.distance < closest)
                {
                    closest = hit.distance;
                    dragPosition = hit.point;
                }
            }
        }

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
