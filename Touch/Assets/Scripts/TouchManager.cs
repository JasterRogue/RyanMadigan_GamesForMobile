using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    enum Gestures{none, determining, tap, drag, rotation, zoom, scale};
    Gestures currentGesture = Gestures.none;

    Vector3 startPos;
    Vector3 endPos;
    float touchZoomSpeed = 0.1f;
    float zoomMinBound = 0.1f;
    float zoomMaxBound = 179.9f;
    float rotationRate = 3.0f;
    Vector2 t1;
    Vector2 t2;

    IControllable selectedObject;
    private float startingDistanceToSelectedObject;

    IControllable objectHit;
    Renderer planeRenderer;
    float initialDistance;
    Touch touch;
    float timeTouchBegan = 0f;
    float tapTimeThreshold = 0.5f; 

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
        switch (currentGesture)
        {
            case Gestures.none:

                //A touch has been detected so switch to determining to find out what it is
                if(Input.touchCount > 0)
                {
                    currentGesture = Gestures.determining;
                    timeTouchBegan = Time.time;
                }

                break;

            case Gestures.determining:

                Vector2 lastPos = touch.position;

                //Toouch gone from the screen 
                if(Input.touchCount < 1)
                {
                    currentGesture = Gestures.none;
                }

                //touch count of 1 
                if(Input.touchCount == 1)
                {
                    //conditions for a tap 
                    if (Vector2.Distance(lastPos, touch.position) < 5 && tapTimeThreshold <= Time.time - timeTouchBegan && touch.phase == TouchPhase.Ended)
                    {
                        currentGesture = Gestures.tap;
                    }


                    //conditions for a drag
                    if (touch.phase == TouchPhase.Moved && selectedObject != null)
                    {
                        currentGesture = Gestures.drag;
                    }

                }//end of touch count == 1

                //touch count of 2
                if(Input.touchCount == 2)
                {
                    //conditions for rotation
                    if (touch.phase == TouchPhase.Moved && selectedObject != null)
                    {
                        currentGesture = Gestures.rotation;
                    }

                    //conditions for a zoom
                    if (selectedObject == null)
                    {
                        currentGesture = Gestures.zoom;

                    }//end of zoom condition 

                    //conditions for a scale
                    if (selectedObject != null)
                    {
                        initialDistance = (t1 - t2).sqrMagnitude;
                        currentGesture = Gestures.scale;
                    }

                }//end of if input touch == 2


                break;

            case Gestures.tap:
                print("Its a tap!");

                currentGesture = Gestures.none;
                break;


            case Gestures.drag:
                //Method1
                Ray newPositionRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
                selectedObject.moveTo(newPositionRay.GetPoint(startingDistanceToSelectedObject));
                //Method2
                Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                selectedObject.moveTo(point);

                currentGesture = Gestures.none;
                break;

            case Gestures.rotation:
                float val1 = touch.deltaPosition.y * rotationRate;
                float val2 = -touch.deltaPosition.x * rotationRate;
                Vector3 v = new Vector3(val1, val2, 0);
                selectedObject.rotateObject(v);

                currentGesture = Gestures.none;
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

                currentGesture = Gestures.none;
                break;

            case Gestures.scale:

                 t1 = Input.GetTouch(0).position;
                 t2 = Input.GetTouch(1).position;

                float newDistance = (t1 - t2).sqrMagnitude;

                float changeInDistance = newDistance - initialDistance;
                float percentageChange = changeInDistance / initialDistance;

                selectedObject.scale(percentageChange);

                currentGesture = Gestures.none;
                break;
        }


        if (Input.touchCount > 0)
        {

             touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Moved:
                   // Ray newPositionRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
                    //Method1
                   // selectedObject.moveTo(newPositionRay.GetPoint(startingDistanceToSelectedObject));
                    //Method2
                    //Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                  //  selectedObject.moveTo(point);
                    break;

                case TouchPhase.Ended:
                    endPos = touch.position;
                    break;
            }//end of switch touch phase


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
                    selectedObject.objectSelected();
                }

                else
                {
                    if(selectedObject != null)
                    {
                        selectedObject.objectDeselected();
                    }
                    
                    selectedObject = null;
                }
            }//end of raycast

        }//end of input.touchCount

    }//end of update()

    public void zoom(float deltaMagnitudeDiff, float speed)
    {
        Camera.main.fieldOfView += deltaMagnitudeDiff * speed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomMinBound, zoomMaxBound);
    }
}

