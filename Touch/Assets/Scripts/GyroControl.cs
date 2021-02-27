using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroControl : MonoBehaviour
{
    bool gyroEnabled;
    Gyroscope gyro;
    Quaternion rotation; 

    // Start is called before the first frame update
    void Start()
    {
        gyroEnabled = enableGyro();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rotation;
        }
        
    }

    private bool enableGyro()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            rotation = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;

    }
}
