using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transforms1 : MonoBehaviour
{
    private float value;
    private float value1;
    private void FixedUpdate()
    {
        value = value - Time.fixedDeltaTime;
        value1 = value1 - Time.fixedDeltaTime;
        //transform.position = -4*value * Vector2.one;
        transform.position = new Vector3(value + 40, 6, 0);
    }
}
