using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemy : MonoBehaviour
{
    private Camera _mainCamera;
    private bool _addEnemyOnce;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = FindObjectOfType<Camera>();
        _addEnemyOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        //First Create A Vector3 With Dimensions Based On The Camera's Viewport
        Vector3 enemyPosition = _mainCamera.WorldToViewportPoint(gameObject.transform.position);

        //If The X And Y Values Are Between 0 And 1, The Enemy Is On Screen
        bool onScreen = enemyPosition.z > 0 && enemyPosition.x > 0 && enemyPosition.x < 1 && enemyPosition.y > 0 && enemyPosition.y < 1;

        //If The Enemy Is On Screen Add It To The List Of Nearby Enemies Only Once
        if (onScreen && _addEnemyOnce)
        {
            Debug.Log("added");
            _addEnemyOnce = false;
            //TargetController.nearByEnemies.Add(this);
        }
    }
}
