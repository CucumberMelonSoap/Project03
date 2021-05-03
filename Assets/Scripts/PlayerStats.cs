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

    private LevelController _levelController;
    private PlayerMovement _player;
    private EnemyBehavior _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
        _player = FindObjectOfType<PlayerMovement>();
        _enemy = FindObjectOfType<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DamagePlayer(int damageAmount)
    {
        //deal damage
        _playerHealth -= damageAmount;

        //effects

        //update slider
        _levelController.UpdateHealth();
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
}
