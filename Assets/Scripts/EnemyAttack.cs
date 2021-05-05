using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] PlayerStats _player = null;
    [SerializeField] EnemyBehavior _enemy = null;
    private LevelController _levelController = null;

    private void Awake()
    {
        _player = GameObject.FindObjectOfType<PlayerStats>();
        _enemy = GameObject.FindObjectOfType<EnemyBehavior>();
        _levelController = GameObject.FindObjectOfType<LevelController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //detect if its the player
        CharacterController player = other.gameObject.GetComponent<CharacterController>();

        //if its a valid player
        if (player != null)
        {
            int attackHits = Random.Range(1, 5);

            _levelController.StartCoroutine(_levelController.DisplayDamagedPortrait());
            _levelController.StartCoroutine(_levelController.DisplayEnemyDamage(transform.position, attackHits));

            for (int i = 0; i < attackHits; i++)
                _player.DamagePlayer(_enemy.GetDamage());

            _player.transform.GetComponent<CharacterController>().Move(_player.transform.forward * -0.15f);
        }
    }
}
