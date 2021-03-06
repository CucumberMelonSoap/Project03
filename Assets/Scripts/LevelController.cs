//Project 3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider _healthBar = null;
    [SerializeField] Text _hpTxt = null;
    [SerializeField] Slider _tpBar = null;
    [SerializeField] Text _tpTxt = null;
    [SerializeField] Text _damageToEnemyTxt = null;
    [SerializeField] Text _damageToPlayerTxt = null;
    [SerializeField] Image _portraitNeutral = null;
    [SerializeField] Image _portraitAttack = null;
    [SerializeField] Image _portraitDamaged = null;
    [SerializeField] Image _portraitVictory = null;
    [SerializeField] Image _portraitLose = null;
    [SerializeField] Image _arteName = null;
    [SerializeField] GameObject _victoryPanel = null;
    [SerializeField] GameObject _losePanel = null;

    [Header("TargetScreen")]
    [SerializeField] Material _enemyOriginal;
    [SerializeField] Material _enemyDarken;
    [SerializeField] Material _playerOriginal;
    [SerializeField] Material _playerDarken;
    [SerializeField] TargetController _targetController;
    [SerializeField] Image[] _enemyIcons;

    [Header("Sounds")]
    [SerializeField] AudioClip _battleBGM;
    [SerializeField] AudioClip _victoryBGM;
    [SerializeField] AudioClip _playerDeath;

    private GameObject[] _allEntities;
    private Camera _mainCamera;
    private PlayerMovement _player;
    private EnemyBehavior[] _enemyList;
    private bool _isGameDone;

    // Start is called before the first frame update
    void Start()
    {
        _allEntities = GameObject.FindGameObjectsWithTag("Entity");
        _player = GameObject.FindObjectOfType<PlayerMovement>();
        _enemyList = GameObject.FindObjectsOfType<EnemyBehavior>();
        _mainCamera = GameObject.FindObjectOfType<Camera>();
        
        _damageToEnemyTxt.enabled = false;
        _damageToPlayerTxt.enabled = false;

        _portraitNeutral.enabled = true;
        _portraitAttack.enabled = false;
        _portraitDamaged.enabled = false;
        _portraitVictory.enabled = false;
        _portraitLose.enabled = false;
        _arteName.enabled = false;

        _victoryPanel.SetActive(false);
        _losePanel.SetActive(false);

        _isGameDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isGameDone)
        {
            EnemyIconRotation();

            if (_targetController.GetScreenActive())
                DarkenScreen();
            else
                RevertScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            ExitGame();

        if (Input.GetKeyDown(KeyCode.E))
            ReloadLevel();

    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void ReloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("BattleScene");
        AudioManager.Instance.PlaySong(_battleBGM);
    }

    private void DarkenScreen()
    {
        
        //darken the models
        foreach (GameObject obj in _allEntities)
        {
            if(obj != null)
            {
                MeshRenderer objMesh = obj.GetComponent<MeshRenderer>();

                if (objMesh)
                {
                    if (obj.layer == 9)
                        objMesh.material = _enemyDarken;
                    else
                        objMesh.material = _playerDarken;
                }
            }
        }

        HighlightCurrentEnemy();

        //disable movement
        _player.enabled = false;
        _player.GetComponent<PlayerStats>().enabled = false;
        foreach(EnemyBehavior enemy in _enemyList)
        {
            if(enemy != null)
                enemy.enabled = false;
        }

    }

    private void RevertScreen()
    {
        //lighten the models
        foreach (GameObject obj in _allEntities)
        {
            if(obj != null)
            {
                MeshRenderer objMesh = obj.GetComponent<MeshRenderer>();

                if (objMesh)
                {
                    if (obj.layer == 9)
                        objMesh.material = _enemyOriginal;
                    else
                        objMesh.material = _playerOriginal;
                }
            }
        }

        //reenable movement
        _player.enabled = true;
        _player.GetComponent<PlayerStats>().enabled = true;
        foreach (EnemyBehavior enemy in _enemyList)
        {
           if(enemy != null)
                enemy.enabled = true;
        }
    }

    private void HighlightCurrentEnemy()
    {
        //lighten the models
        EnemyBehavior current = _enemyList[_targetController.GetCurrentIndex()];
        MeshRenderer[] meshList = current.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in meshList)
        {
            mesh.material = _enemyOriginal;
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
        _damageToPlayerTxt.enabled = true;
        _damageToPlayerTxt.text = (_enemyList[_targetController.GetCurrentIndex()].GetDamage() * numAttacks).ToString();
        _damageToPlayerTxt.transform.position = _mainCamera.WorldToScreenPoint(enemyPosition + (Vector3.up * 2));

        yield return new WaitForSecondsRealtime(1f);

        _damageToPlayerTxt.enabled = false;   
    }

    public IEnumerator DisplayPlayerDamage(Vector3 position, int damage)
    {
        _damageToEnemyTxt.enabled = true;
        _damageToEnemyTxt.text = damage.ToString();
        _damageToEnemyTxt.transform.position = _mainCamera.WorldToScreenPoint(position + (Vector3.up * 1.5f));

        yield return new WaitForSecondsRealtime(1f);

        _damageToEnemyTxt.enabled = false;
    }

    public IEnumerator DispalyAttackPortait()
    {
        _portraitAttack.enabled = true;
        _portraitNeutral.enabled = false;
        _portraitDamaged.enabled = false;

        yield return new WaitForSecondsRealtime(0.5f);

        _portraitAttack.enabled = false;
        _portraitNeutral.enabled = true;
    }

    public IEnumerator DispalyArteName()
    {
        _arteName.enabled = true;

        yield return new WaitForSecondsRealtime(0.7f);

        _arteName.enabled = false;
    }

    public IEnumerator DisplayDamagedPortrait()
    {
        _portraitDamaged.enabled = true;
        _portraitNeutral.enabled = false;
        _portraitAttack.enabled = false;

        yield return new WaitForSecondsRealtime(0.5f);

        _portraitDamaged.enabled = false;
        _portraitNeutral.enabled = true;

    }

    public void DisableEnemyIcon()
    {
        CurrentEnemyIcon().GetComponentInChildren<Text>().enabled = false;
        CurrentEnemyIcon().enabled = false;  
    }

    public void Victory()
    {
        _isGameDone = true;
        AudioManager.Instance.PlaySong(_victoryBGM);
        _portraitNeutral.enabled = false;
        _portraitAttack.enabled = false;
        _portraitDamaged.enabled = false;
        _portraitVictory.enabled = true;
        _portraitLose.enabled = false;
        _arteName.enabled = false;
        _player.GetComponent<PlayerStats>().enabled = false;
        _targetController.enabled = false;
        StopAllCoroutines();
        _victoryPanel.SetActive(true);
        Time.timeScale = 0;

    }

    public void Lose()
    {
        _isGameDone = true;
        StopAllCoroutines();
        AudioManager.Instance.PlaySFX(_playerDeath, 0.75f);
        StartCoroutine(AudioManager.Instance.PauseSong(_playerDeath));
        _player.transform.GetComponent<PlayerMovement>().enabled = false;
        _player.GetComponent<PlayerStats>().enabled = false;
        _targetController.enabled = false;

        MeshRenderer[] childrenMesh = _player.transform.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in childrenMesh)
        {
            mesh.material = _playerDarken;
        }

        _portraitNeutral.enabled = false;
        _portraitDamaged.enabled = false;
        _portraitLose.enabled = true;
        _arteName.enabled = false;
        
        
        _losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public bool GetGameState()
    {
        return _isGameDone;
    }
}
