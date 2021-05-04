using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Normal Stats")]
    [SerializeField] int _playerHealth = 691;
    [SerializeField] int _playerTP = 68;

    [Header("Attack Stats")]
    [SerializeField] int _basicAtkLow = 50;
    [SerializeField] int _basicAtkHigh = 58;
    [SerializeField] int _arteAtkLow = 80;
    [SerializeField] int _arteAtkHigh = 90;
    [SerializeField] int _arteTPCost = 4;
    [SerializeField] GameObject _arteAttack = null;

    private LevelController _levelController;
    private PlayerMovement _player;
    private EnemyBehavior _enemy;
    private GameObject _sword;

    // Start is called before the first frame update
    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
        _player = FindObjectOfType<PlayerMovement>();
        _enemy = FindObjectOfType<EnemyBehavior>();
        _sword = GameObject.Find("Sword");
    }

    // Update is called once per frame
    void Update()
    {
        Transform swordTransform = _sword.transform;

        //if game is not finished, accept input
        if(!_levelController.GetGameState())
        {
            if (Input.GetKeyDown(KeyCode.E) && _playerTP >= _arteTPCost && _player.GetIsGrounded())
            {

                Destroy(Instantiate(_arteAttack, swordTransform.position + (Vector3.down * 0.8f), _player.transform.rotation), 1.5f);
                _levelController.StartCoroutine(_levelController.DispalyAttackPortait());
                _levelController.StartCoroutine(_levelController.DispalyArteName());
                UseTP(_arteTPCost);
            }
        }

    }
    
    public void DamagePlayer(int damageAmount)
    {
        //deal damage
        _playerHealth -= damageAmount;

        //effects

        //update slider
        _levelController.UpdateHealth();

        if (_playerHealth <= 0)
            _levelController.Lose();
    }

    public void GainTP(int tpRecover)
    {
        _playerTP += tpRecover;
        _levelController.UpdateTP();
    }

    public void UseTP(int tpCost)
    {
        _playerTP -= tpCost;
        _levelController.UpdateTP();
    }

    public int GetHP()
    {
        return _playerHealth;
    }

    public int GetTP()
    {
        return _playerTP;
    }

    public int GetBasicAttackDamage()
    {
        return Random.Range(_basicAtkLow, _basicAtkHigh + 1);
    }

    public int GetArteAttackDamage()
    {
        return Random.Range(_arteAtkLow, _arteAtkHigh + 1);
    }
}
