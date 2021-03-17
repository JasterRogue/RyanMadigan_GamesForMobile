using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool hasMoved;

    Quaternion initialRotation;
    Vector3 initialScale;
    float initialAngle;

    private const float deltaChangeThreshold = 10f;
    private const float rotationThreshold = 0.1f;
    private float currentAngle;
    private float currentDeltaChange; 

    // Start is called before the first frame update
    void Start()
    {
        GameObject ourCameraPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        planeRenderer = ourCameraPlane.GetComponent<Renderer>();
        planeRenderer.material.SetColor("_Color", Color.red);
        Destroy(planeRenderer);

        ourCameraPlane.transform.position = new Vector3(0, Camera.main.transform.position.y, 0);
        ourCameraPlane.transform.up = (Camera.main.transform.position - ourCameraPlane.transform.position).normalized;

        initialRotation = Quaternion.identity;
        initialScale = Vector3.one;
        initialAngle = 0f;
        initialDistance = 0;
        currentAngle = 0f;
        currentDeltaChange = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentGesture = determineGesture();
        print("Current gesture" + currentGesture);

        switch (currentGesture)
        {
            case Gestures.none:

              

                break;

            case Gestures.determining:

                
                
                break;

            case Gestures.tap:
              //  print("Its a tap!");

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
                        if (selectedObject != null)
                        {
                            selectedObject.objectDeselected();
                        }

                        selectedObject = null;
                    }
                }//end of raycast

                break;


            case Gestures.drag:

                if(selectedObject != null)
                {
                    selectedObject.moveTo(Input.touches[0].position);
                }

                else
                {
                    dragCamere();
                }

                break;

            case Gestures.rotation:
                float val1 = touch.deltaPosition.y * rotationRate;
                float val2 = -touch.deltaPosition.x * rotationRate;
                Vector3 v = new Vector3(val1, val2, 0);
                selectedObject.rotateObject(v);

                
                break;

            case Gestures.zoom:

                float deltaDistance = determineFactor();
                zoom(deltaDistance, touchZoomSpeed);

                break;

            case Gestures.scale:


                 t1 = Input.GetTouch(0).position;
                 t2 = Input.GetTouch(1).position;

                float newDistance = (t1 - t2).sqrMagnitude;

                float changeInDistance = newDistance - initialDistance;

              /* if (Mathf.Approximately(initialDistance, 0))
                {
                    //if bad 
                    break;
                }*/
                    
                float percentageChange = changeInDistance / initialDistance;

                selectedObject.scale(percentageChange);

                
                break;
        }

    }//end of update()

    private Gestures determineGesture()
    {
        //Touch gone from the screen 
        if (Input.touchCount < 1)
        {
            return Gestures.none;
        }

        //touch count of 1 
        if (Input.touchCount == 1)
        {

            touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    timeTouchBegan = Time.time;

                    return Gestures.determining;

                  //  break;

                case TouchPhase.Moved:
                    hasMoved = true;
                    return Gestures.drag;

                case TouchPhase.Ended:
                    // endPos = touch.position;
                    if (IsTap())
                    {
                        return Gestures.tap;
                    }

                    hasMoved = false;

                    return Gestures.none;

                    //break;

                default:
                    return Gestures.determining;


            }//end of switch touch phase



           }//end of touch count == 1

        //touch count of 2
        if (Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(0);

            Touch touch2 = Input.GetTouch(1);

            if (touch.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch.position, touch2.position);
                Vector3 v2 = touch2.position - touch.position;
                initialAngle = Mathf.Atan2(v2.y, v2.x);

                if (selectedObject != null)
                {
                    initialRotation = selectedObject.gameObject.transform.rotation;
                    initialScale = selectedObject.gameObject.transform.localScale;
                }
                else
                {
                    initialRotation = Camera.main.transform.rotation;
                }

                return Gestures.determining;

            }//end of touch.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began

            if (touch.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                currentDeltaChange = 0;
                currentAngle = 0;
            }

            // Makes sure that the 2 finer gesture doesn't change until after the finers after lifted 
            switch (currentGesture)
            {
                case Gestures.rotation:
                    return Gestures.rotation;

                case Gestures.scale:
                    return Gestures.scale;

                case Gestures.zoom:
                    return Gestures.zoom;
            }

            float angle = determineAngle();
            float deltaChange = determineFactor();

            if (deltaChange < 0)
            {
                currentDeltaChange = (deltaChange * -1);
            }

            else
            {
                currentDeltaChange = deltaChange;
            }

            if (angle < 0)
            {
                currentAngle += (angle * -1);
            }

            else
            {
                currentAngle += angle;
            }

            if (currentDeltaChange >= deltaChangeThreshold && selectedObject != null)
            {
                return Gestures.scale;
            }

            if (currentDeltaChange >= deltaChangeThreshold)
            {
                return Gestures.zoom;
            }

            if (currentAngle >= rotationThreshold)
            {
                return Gestures.rotation;
            }

            return Gestures.determining;

            //print("Two touches");

        }//end of touch count == 2

        return Gestures.none;

    }//end of DetermineGesture()

    private float determineFactor()
    {
        Touch tZero = Input.GetTouch(0);
        Touch tOne = Input.GetTouch(1);

        Vector2 tZeroPrevPos = tZero.position - tZero.deltaPosition;
        Vector2 tOnePrevPos = tOne.position - tOne.deltaPosition;

        float oldTouchDistance = Vector2.Distance(tZeroPrevPos, tOnePrevPos);
        float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

        float  deltaDistance = oldTouchDistance - currentTouchDistance;

       // Debug.Log("Delta distance" + deltaDistance);

        return deltaDistance; 
    }

    private bool IsTap()
    {
        float touchTime = Time.time - timeTouchBegan;

        if (touchTime <= tapTimeThreshold && !hasMoved)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public void zoom(float deltaMagnitudeDiff, float speed)
    {
        Camera.main.fieldOfView += deltaMagnitudeDiff * speed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomMinBound, zoomMaxBound);
    }

    public void dragCamere()
    {
        Vector2 touchDeltaPosition = Input.touches[0].deltaPosition * Time.deltaTime;
        Camera.main.transform.Translate(-touchDeltaPosition.x, touchDeltaPosition.y, 0);
    }

    public void onClick()
    {
        SceneManager.LoadScene(0);
    }

    private float determineAngle()
    {
        Vector3 v = Input.touches[1].position - Input.touches[0].position;
        float theta = Mathf.Atan2(v.y, v.x);
        theta = theta - initialAngle;

        return theta;
    }

}

