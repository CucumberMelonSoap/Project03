using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatText : MonoBehaviour
{
    [SerializeField] float _floatSpeed = 0.2f;
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, _floatSpeed, 0);
    }
}
