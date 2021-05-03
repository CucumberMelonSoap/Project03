//Project 3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider _healthBar = null;
    [SerializeField] Text _hpTxt = null;
    [SerializeField] Slider _tpBar = null;
    [SerializeField] Text _tpTxt = null;
    [SerializeField] Text _damageToEnemyTxt = null;
    [SerializeField] Text _damageToPlayerTxt = null;

    [Header("TargetScreen")]
    [SerializeField] Material _enemyOriginal;
    [SerializeField] Material _enemyDarken;
    [SerializeField] Material _playerOriginal;
    [SerializeField] Material _playerDarken;
    [SerializeField] TargetController _targetController;
    [SerializeField] Image[] _enemyIcons;

    private GameObject[] _allEntities;
    private Camera _mainCamera;
    private PlayerMovement _player;
    private EnemyBehavior[] _enemyList;
    // Start is called before the first frame update
    void Start()
    {
        _allEntities = GameObject.FindGameObjectsWithTag("Entity");
        _player = GameObject.FindObjectOfType<PlayerMovement>();
        _enemyList = GameObject.FindObjectsOfType<EnemyBehavior>();
        _mainCamera = GameObject.FindObjectOfType<Camera>();

        _damageToEnemyTxt.enabled = false;
        _damageToPlayerTxt.enabled = false;
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

        //reenable movement
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

    public void UpdateHealth()
    {
        _healthBar.value = _player.gameObject.GetComponent<PlayerStats>().GetHP();
        _hpTxt.text = _healthBar.value.ToString();
    }

    public void UpdateTP()
    {
        _tpBar.value = _player.gameObject.GetComponent<PlayerStats>().GetTP();
        _tpTxt.text = _tpBar.value.ToString();
    }

    public IEnumerator DisplayEnemyDamage(Vector3 enemyPosition, int numAttacks)
    {
        _damageToEnemyTxt.enabled = true;
        _damageToEnemyTxt.text = (_enemyList[_targetController.GetCurrentIndex()].GetDamage() * numAttacks).ToString();
        _damageToEnemyTxt.transform.position = _mainCamera.WorldToScreenPoint(enemyPosition + (Vector3.up * 3));

        yield return new WaitForSeconds(0.75f);

        _damageToEnemyTxt.enabled = false;   
    }
}
