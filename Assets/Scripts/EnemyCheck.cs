using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    private PlayerMovement _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>();   
    }

    private void OnTriggerStay(Collider other)
    {
        //detect if there is an enemy in front
        EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();

        //if its a valid enemy
        if (enemy != null)
        {
            _player.SetEnemyWatch(true);
        }
    }
}
