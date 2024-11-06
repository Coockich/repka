using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transforms : MonoBehaviour
{
    private float value;
    private float value1;
    private void FixedUpdate()
    {
        value = value + Time.fixedDeltaTime;
        value1 = value1 - Time.fixedDeltaTime;
        //transform.position = -4*value * Vector2.one;
        transform.position = new Vector3 (value-20, -1, 0);
    }

}
