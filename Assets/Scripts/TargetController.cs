using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    private DetectEnemy _enemyTarget;
    [SerializeField] Image _crosshairImg;
    [SerializeField] Image _iconIndicatorImg;
    [SerializeField] PlayerMovement _playerMove;
    [SerializeField] LevelController _levelController;

    private bool _currentlyLockedOn;
    private bool _lockScreenActive;
    private int _enemyIndex;

    //List of nearby enemies
    public static List<DetectEnemy> nearByEnemies = new List<DetectEnemy>();

    // Start is called before the first frame update
    void Start()
    {
        _crosshairImg.enabled = false;
        _iconIndicatorImg.enabled = false;
        _lockScreenActive = false;
        _currentlyLockedOn = false;
        _enemyIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_currentlyLockedOn && nearByEnemies.Count != 0)
            InitialTargetLockOn();

        //Turn Off Lock On When No Enemies
        if (nearByEnemies.Count == 0)
        {
            _currentlyLockedOn = false;
            _crosshairImg.enabled = false;
            _enemyIndex = 0;
            _enemyTarget = null;
        }

        //Target Switcher Right
        if (Input.GetKey(KeyCode.U))
        {
            _lockScreenActive = true;

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (_enemyIndex == nearByEnemies.Count - 1)
                {
                    //If End Of List Has Been Reached, Start Over
                    _enemyIndex = 0;
                    _enemyTarget = nearByEnemies[_enemyIndex];
                }
                else
                {
                    //Move To Next Enemy In List
                    _enemyIndex++;
                    _enemyTarget = nearByEnemies[_enemyIndex];
                }

                //Determine Crosshair Location Based On The Current Target
                _iconIndicatorImg.enabled = true;
                _iconIndicatorImg.transform.position = _levelController.CurrentEnemyIcon().transform.position + new Vector3(0, 15, 0);

                _playerMove.LookAtEnemy();
            }
        }
        else
        {
            _lockScreenActive = false;
            _iconIndicatorImg.enabled = false;
        }



        //Display Crosshair
        if (_currentlyLockedOn)
        {
            _enemyTarget = nearByEnemies[_enemyIndex];

            //Determine Crosshair Location Based On The Current Target
            gameObject.transform.position = _mainCamera.WorldToScreenPoint(_enemyTarget.transform.position + Vector3.up);

            //Rotate Crosshair
            //gameObject.transform.Rotate(new Vector3(0, 0, -1));
        }
    }

    private void InitialTargetLockOn()
    {
        _currentlyLockedOn = true;
        _crosshairImg.enabled = true;

        //Lock On To First Enemy In List By Default
        _enemyIndex = 0;
        _enemyTarget = nearByEnemies[_enemyIndex];

        //Determine Crosshair Location Based On The Current Target
        gameObject.transform.position = _mainCamera.WorldToScreenPoint(_enemyTarget.transform.position);

        _playerMove.LookAtEnemy();
    }
    
    public DetectEnemy GetCurrentTarget()
    {
        return _enemyTarget;
    }

    public int GetCurrentIndex()
    {
        return _enemyIndex;
    }

    public bool GetScreenActive()
    {
        return _lockScreenActive;
    }
}
