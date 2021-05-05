using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] Image _crosshairImg;
    [SerializeField] Image _iconIndicatorImg;

    [Header("Controllers")]
    [SerializeField] PlayerMovement _playerMove;
    [SerializeField] LevelController _levelController;

    private EnemyBehavior _enemyTarget;
    private bool _currentlyLockedOn;
    private bool _lockScreenActive;
    private int _enemyIndex;
    private int _enemiesLeft;

    public static EnemyBehavior[] nearbyEnemies;

    // Start is called before the first frame update
    void Start()
    {
        _crosshairImg.enabled = false;
        _iconIndicatorImg.enabled = false;
        _lockScreenActive = false;
        _currentlyLockedOn = false;
        _enemyIndex = 0;
        _enemiesLeft = 2;
        nearbyEnemies = FindObjectsOfType<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_currentlyLockedOn && _enemiesLeft != 0)
            InitialTargetLockOn();

        //Turn Off Lock On When No Enemies
        if (_enemiesLeft == 0)
        {
            _currentlyLockedOn = false;
            _crosshairImg.enabled = false;
            _iconIndicatorImg.enabled = false;
            _enemyIndex = 0;
            _enemyTarget = null;

            _levelController.Victory();
        }
        else if(_enemiesLeft == 1)
        {
            _enemyTarget = nearbyEnemies[_enemyIndex];

            if (_enemyTarget != null)
                //Determine Crosshair Location Based On The Current Target
                gameObject.transform.position = _mainCamera.WorldToScreenPoint(_enemyTarget.transform.position + (Vector3.up * 1.5f));

            if(Input.GetKey(KeyCode.U))
            {
                _lockScreenActive = true;
                _iconIndicatorImg.enabled = true;
                _iconIndicatorImg.transform.position = _levelController.CurrentEnemyIcon().transform.position + new Vector3(0, 20, 0);
                _playerMove.LookAtEnemy();

            }
            else
            {
                _lockScreenActive = false;
                _iconIndicatorImg.enabled = false;
            }
        }
        else
        {
            //Target Switcher 
            if (Input.GetKey(KeyCode.U))
            {
                _lockScreenActive = true;

                if (Input.GetKeyDown(KeyCode.L))
                {
                    if (_enemyIndex == nearbyEnemies.Length - 1)
                    {
                        //If End Of List Has Been Reached, Start Over
                        _enemyIndex = 0;
                        _enemyTarget = nearbyEnemies[_enemyIndex];
                    }
                    else
                    {
                        //Move To Next Enemy In List
                        _enemyIndex++;
                        _enemyTarget = nearbyEnemies[_enemyIndex];
                    }

                    if(_levelController.CurrentEnemyIcon().IsActive())
                    {
                        //Determine Crosshair Location Based On The Current Target
                        _iconIndicatorImg.enabled = true;
                        _iconIndicatorImg.transform.position = _levelController.CurrentEnemyIcon().transform.position + new Vector3(0, 20, 0);
                    }

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
                _enemyTarget = nearbyEnemies[_enemyIndex];

                if (_enemyTarget != null)
                    //Determine Crosshair Location Based On The Current Target
                    gameObject.transform.position = _mainCamera.WorldToScreenPoint(_enemyTarget.transform.position + (Vector3.up * 1.5f));

            }
        }
        
    }

    private void InitialTargetLockOn()
    {
        _currentlyLockedOn = true;
        _crosshairImg.enabled = true;

        //Lock On To First Enemy In List By Default
        _enemyIndex = 0;
        _enemyTarget = nearbyEnemies[_enemyIndex];

        //Determine Crosshair Location Based On The Current Target
        gameObject.transform.position = _mainCamera.WorldToScreenPoint(_enemyTarget.transform.position);

        _playerMove.LookAtEnemy();
    }
    
    public EnemyBehavior GetCurrentTarget()
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

    public void ReduceEnemyCount()
    {
        _enemiesLeft--;
    }

    public void MoveToNextEnemy()
    {
        if (_enemyIndex == nearbyEnemies.Length - 1)
        {
            //If End Of List Has Been Reached, Start Over
            _enemyIndex = 0;
            _enemyTarget = nearbyEnemies[_enemyIndex];
        }
        else
        {
            //Move To Next Enemy In List
            _enemyIndex++;
            _enemyTarget = nearbyEnemies[_enemyIndex];
        }

        //Determine Crosshair Location Based On The Current Target
        _iconIndicatorImg.enabled = true;
        _iconIndicatorImg.transform.position = _levelController.CurrentEnemyIcon().transform.position + new Vector3(0, 20, 0);

        _playerMove.LookAtEnemy();
    }
}
