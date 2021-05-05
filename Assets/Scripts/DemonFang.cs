using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonFang : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _arteSpeed = 2f;
    [SerializeField] PlayerStats _player = null;
    [SerializeField] ParticleSystem _onHit = null;
    private LevelController _levelController = null;

    private void Awake()
    {
        _player = GameObject.FindObjectOfType<PlayerStats>();
        _levelController = GameObject.FindObjectOfType<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * _arteSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //detect if its the enemy
        EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();

        //if its a valid enemy
        if (enemy != null)
        {
            int arteAttack = _player.GetArteAttackDamage();

            _levelController.StartCoroutine(_levelController.DisplayPlayerDamage(transform.position, arteAttack));

            enemy.DamageEnemy(arteAttack);
            Instantiate(_onHit, transform.position + (transform.forward * 0.6f), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
