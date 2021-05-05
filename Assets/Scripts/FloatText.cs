using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatText : MonoBehaviour
{
    [SerializeField] float _floatSpeed = 0.2f;

    // Update is called once per frame
    void Update()
    {  
        if(transform.position.y < 200)
            transform.position += new Vector3(0, _floatSpeed, 0);
        else if (Time.timeScale == 0 && transform.position.y > 120)
            Destroy(gameObject);

    }
}
