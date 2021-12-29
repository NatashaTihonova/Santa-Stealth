using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConeVision : MonoBehaviour
{

    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] public Transform _player;
    [SerializeField] private Wandering _wanderingCsript;

    private bool _isAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isAttack = true;
            _wanderingCsript.SetNewState(State.Pursuit);
        }
    }

    private void Update()
    {
        if (_isAttack)
        {
            navMesh.SetDestination(_player.position);
        }
    }
}
