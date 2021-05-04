using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion end = Quaternion.Euler(50, 0, 0);
        //transform.localRotation = Quaternion.Lerp(transform.localRotation, end, 0.3f);

        Vector3 end = new Vector3(0.55f, -0.1f, 0.5f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, end, 1f);
    }
}
