using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] Material _enemyOriginal;
    [SerializeField] Material _enemyDarken;
    [SerializeField] Material _playerOriginal;
    [SerializeField] Material _playerDarken;
    [SerializeField] TargetController _targetController;
    [SerializeField] Image[] _enemyIcons;

    private GameObject[] _allEntities;
    private PlayerMovement _player;
    private EnemyBehavior[] _enemyList;
    // Start is called before the first frame update
    void Start()
    {
        _allEntities = GameObject.FindGameObjectsWithTag("Entity");
        _player = GameObject.FindObjectOfType<PlayerMovement>();
        _enemyList = GameObject.FindObjectsOfType<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyIconRotation();

        if (_targetController.GetScreenActive())
            DarkenScreen();
        else
            RevertScreen();
    }

    private void DarkenScreen()
    {
        //darken the models
        foreach(GameObject obj in _allEntities)
        {
            MeshRenderer objMesh = obj.GetComponent<MeshRenderer>();

            if(obj != null && objMesh)
            {
                if (obj.layer == 9)
                    objMesh.material = _enemyDarken;
                else
                    objMesh.material = _playerDarken;
            }
        }

        //disable movement
        _player.enabled = false;
        foreach(EnemyBehavior enemy in _enemyList)
        {
            enemy.enabled = false;
        }

    }

    private void RevertScreen()
    {
        //darken the models
        foreach (GameObject obj in _allEntities)
        {
            MeshRenderer objMesh = obj.GetComponent<MeshRenderer>();

            if (obj != null && objMesh)
            {
                if (obj.layer == 9)
                    objMesh.material = _enemyOriginal;
                else
                    objMesh.material = _playerOriginal;
            }
        }

        //disable movement
        _player.enabled = true;
        foreach (EnemyBehavior enemy in _enemyList)
        {
            enemy.enabled = true;
        }
    }

    private void EnemyIconRotation()
    {
        for(int i = 0; i < _enemyIcons.Length; i++)
        {
            if (i == _targetController.GetCurrentIndex())
                _enemyIcons[_targetController.GetCurrentIndex()].transform.Rotate(0, 0.9f, 0);
            else
                _enemyIcons[i].transform.rotation = Quaternion.identity;
        }
    }

    public Image CurrentEnemyIcon()
    {
        return _enemyIcons[_targetController.GetCurrentIndex()];
    }
}
