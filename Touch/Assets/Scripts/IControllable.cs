using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IControllable
{
    void youveBeenTouched();

    void moveTo(Vector3 destination);

    void scale(float percentageChange);
}

