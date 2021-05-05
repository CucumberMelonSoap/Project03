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
    private GameObject _sword;
    private GameObject _swordHolder;
    private bool _isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
        _player = FindObjectOfType<PlayerMovement>();
        _sword = GameObject.Find("Sword");
        _swordHolder = GameObject.Find("Weapon");
    }

    // Update is called once per frame
    void Update()
    {
        Transform swordTransform = _sword.transform;

        //if game is not finished, accept input
        if(!_levelController.GetGameState())
        {
            if(!_isAttacking)
            {
                if (Input.GetKeyDown(KeyCode.E) && _playerTP >= _arteTPCost && _player.GetIsGrounded())
                {
                    StartCoroutine(ArteAttack(swordTransform));
                }
                else if (Input.GetKeyDown(KeyCode.Q) && _player.GetIsGrounded())
                {
                    StartCoroutine(BasicAttack(swordTransform));
                }
            }
        }

    }

    private IEnumerator ArteAttack(Transform sword)
    {
        _isAttacking = true;

        Destroy(Instantiate(_arteAttack, sword.position + (Vector3.down * 0.6f), _player.transform.rotation), 1.25f);
        _levelController.StartCoroutine(_levelController.DispalyAttackPortait());
        _levelController.StartCoroutine(_levelController.DispalyArteName());
        UseTP(_arteTPCost);

        yield return new WaitForSecondsRealtime(0.1f);

        _isAttacking = false;
    }

    private IEnumerator BasicAttack(Transform sword)
    {
        _isAttacking = true;
        Vector3 swordAttackMove = (sword.up * -0.2f) + (sword.forward * 0.75f);
        sword.position += swordAttackMove;
        _levelController.StartCoroutine(_levelController.DispalyAttackPortait());

        yield return new WaitForSecondsRealtime(0.1f);

        sword.position = _swordHolder.transform.position;
        _isAttacking = false;
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
