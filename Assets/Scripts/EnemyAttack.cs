using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] PlayerStats _player = null;
    [SerializeField] EnemyBehavior _enemy = null;

    private void Awake()
    {
        _player = GameObject.FindObjectOfType<PlayerStats>();
        _enemy = GameObject.FindObjectOfType<EnemyBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //detect if its the player
        CharacterController player = other.gameObject.GetComponent<CharacterController>();

        //if its a valid player
        if (player != null)
        {
            int attackHits = _enemy._attackHits;
            
            for(int i = 0; i < attackHits; i++)
                _player.DamagePlayer(_enemy.GetDamage());
        }
    }
}
