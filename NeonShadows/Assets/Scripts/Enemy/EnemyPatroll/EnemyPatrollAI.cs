using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollAI : MonoBehaviour
{
    private GameObject _player;
    private Animator _animator;
    private Ray _ray;
    private RaycastHit _hit;
    [SerializeField] private float _maxDistanceToCheck = 30.0f;
    private float _currentDistanceToPlayer;
    private Vector3 _checkPlayerDirection;
    [SerializeField] private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    [SerializeField] private float _patrolSpeed = 5f;
    private int _currentTarget;
    private float _distanceToTarget;
    [SerializeField] private Transform[] _waypoints;

    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = gameObject.GetComponent<Animator>();
        _currentTarget = 0;
        _navMeshAgent.SetDestination(_waypoints[_currentTarget].position);
        _navMeshAgent.speed = _patrolSpeed;
    }

    private void FixedUpdate() {
        // Cheack distance to player
        _currentDistanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        _animator.SetFloat("distanceToPlayer", _currentDistanceToPlayer);

        // Check if player is visible
        _checkPlayerDirection = _player.transform.position - transform.position;
        _ray = new Ray(transform.position, _checkPlayerDirection);
        if (Physics.Raycast(_ray, out _hit, _maxDistanceToCheck)) {
            if (_hit.collider.gameObject == _player) {
                _animator.SetBool("isPlayerVisible", true);
            } else {
                _animator.SetBool("isPlayerVisible", false);
            }
        } else {
            _animator.SetBool("isPlayerVisible", false);
        }

        // Get distance to next target
        _distanceToTarget = Vector3.Distance(transform.position, _waypoints[_currentTarget].position);
        _animator.SetFloat("distanceToWaypoint", _distanceToTarget);
    }

    public void SetNextWaypoint() {
        // If the enemy's last target was the player, then the next target is the waypoint that is closest to it 
        if (_animator.GetBool("isPlayerAsTarget")) {
            _animator.SetBool("isPlayerAsTarget", false);
            _navMeshAgent.speed = _patrolSpeed; // Set patrol speed

            // Find the closest waypoint to the enemy
            float minDistance = Mathf.Infinity;
            int minDistanceIndex = 0;
            for (int i = 0; i < _waypoints.Length; i++) {
                float distance = Vector3.Distance(transform.position, _waypoints[i].position);
                if (distance < minDistance) {
                    minDistance = distance;
                    minDistanceIndex = i;
                }
            }
            _currentTarget = minDistanceIndex;
        // If the enemy's last target was a waypoint, then the next target is the next waypoint in the array
        } else {
            _currentTarget = (_currentTarget + 1) % _waypoints.Length;
        }
        _navMeshAgent.SetDestination(_waypoints[_currentTarget].position);
    }

    public void SetPlayerAsTarget() {
        _navMeshAgent.SetDestination(_player.transform.position);
        _animator.SetBool("isPlayerAsTarget", true);
    }

    public void SetSpeed(float speed) {
        _navMeshAgent.speed = speed;
    }

    public void Stop() {
        _navMeshAgent.isStopped = true;
    }

    public void Move() {
        _navMeshAgent.isStopped = false;
    }
}
