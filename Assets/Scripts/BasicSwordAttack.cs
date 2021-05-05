using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwordAttack : MonoBehaviour
{
    private PlayerStats _playerStats;
    private LevelController _levelController;

    private void Start()
    {
        _playerStats = GameObject.FindObjectOfType<PlayerStats>();
        _levelController = GameObject.FindObjectOfType<LevelController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //detect if its the enemy
        EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();

        //if its a valid enemy
        if (enemy != null)
        {
            int damage = _playerStats.GetBasicAttackDamage();
            enemy.DamageEnemy(damage);
            _levelController.StartCoroutine(_levelController.DisplayPlayerDamage(enemy.transform.position, damage));
            //restore 1 tp on succesful hit
            _playerStats.GainTP(1);
        }
    }
}
