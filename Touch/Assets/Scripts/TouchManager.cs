using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{

    enum Gestures{none, determining, tap, longTap, drag, rotation, zoom, scale};
    Gestures currentGesture = Gestures.none;

    Vector3 startPos;
    Vector3 endPos;
    float timer = 0f;
    bool timerOn = false;
    bool hasMoved;
    float touchZoomSpeed = 0.1f;
    float zoomMinBound = 0.1f;
    float zoomMaxBound = 179.9f;
    Vector2 t1;
    Vector2 t2;

    IControllable selectedObject;
    private float startingDistanceToSelectedObject;

    IControllable objectHit;
    Renderer planeRenderer;
    float initialDistance;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ourCameraPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        planeRenderer = ourCameraPlane.GetComponent<Renderer>();
        planeRenderer.material.SetColor("_Color", Color.red);

        ourCameraPlane.transform.position = new Vector3(0, Camera.main.transform.position.y, 0);
        ourCameraPlane.transform.up = (Camera.main.transform.position - ourCameraPlane.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {


        switch(currentGesture)
        {
            case Gestures.none:

                //A touch has been detected so switch to determining to find out what it is
                if(Input.touchCount > 0)
                {
                    currentGesture = Gestures.determining;
                }

                break;

            case Gestures.determining:

                //conditions for a tap 


                //conditions for a long tap


                //conditions for a drag


                //conditions for rotation
                


                //conditions for a zoom
                if (Input.touchCount == 2)
                {
                        currentGesture = Gestures.zoom;

                }//end of touchCount ==2

                //conditions for a scale
                if(Input.touchCount == 2 && selectedObject != null)
                {
                    currentGesture = Gestures.scale;
                    initialDistance = (t1 - t2).sqrMagnitude;
                }


                break;

            case Gestures.tap:
                break;

            case Gestures.longTap:
                break;

            case Gestures.drag:
                break;

            case Gestures.rotation:
                break;

            case Gestures.zoom:

                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);

                Vector2 tZeroPrevPos = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevPos = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevPos, tOnePrevPos);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                float deltaDistance = oldTouchDistance - currentTouchDistance;
                zoom(deltaDistance, touchZoomSpeed);

                break;

            case Gestures.scale:

                 t1 = Input.GetTouch(0).position;
                 t2 = Input.GetTouch(1).position;

                float newDistance = (t1 - t2).sqrMagnitude;

                float changeInDistance = newDistance - initialDistance;
                float percentageChange = changeInDistance / initialDistance;

                selectedObject.scale(percentageChange);

                break;
        }


        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    timerOn = true;
                    startPos = touch.position;
                    hasMoved = false;
                    break;

                case TouchPhase.Moved:
                    hasMoved = true;
                    Ray newPositionRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
                    //Method1
                    selectedObject.moveTo(newPositionRay.GetPoint(startingDistanceToSelectedObject));
                    //Method2
                    Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    selectedObject.moveTo(point);
                    break;

                case TouchPhase.Ended:
                    timerOn = false;
                    endPos = touch.position;
                    isaTap(hasMoved, timer);
                    break;
            }

 

            RaycastHit info;
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

            Debug.DrawRay(ray.origin, 30 * ray.direction);

            if (Physics.Raycast(ray, out info))
            {
                objectHit = info.transform.GetComponent<IControllable>();

                if (objectHit != null)
                {
                    objectHit.youveBeenTouched();
                    selectedObject = objectHit;
                    startingDistanceToSelectedObject = Vector3.Distance(Camera.main.transform.position, info.transform.position);                    
                }
            }//end of raycast

            if (timerOn)
            {
                timer += Time.deltaTime;
            }//end of timerON


        }//end of input.touchCount

    }//end of update()


    public void isaTap(bool hasMoved, float time)
    {
        if (!hasMoved && time <= 0.5f)
        {
            print("Its a tap!");
        }

        else
        {
            print("Not a tap");
        }

    }//end of isaTap()

    public void zoom(float deltaMagnitudeDiff, float speed)
    {
        Camera.main.fieldOfView += deltaMagnitudeDiff * speed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomMinBound, zoomMaxBound);
    }
}

