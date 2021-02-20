using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[Serializable]
public class WalkWayPoint
{
    [Tooltip("Transform of way-point to visit.")]
    public Transform transform;

    [Tooltip("Minimum dwell time at the way-point.")]
    public float minDwellTime = 3F;

    [Tooltip("Maximum dwell time at way-point.")]
    public float maxDwellTime = 6F;

    public float DwellTime() => UnityEngine.Random.Range(minDwellTime, maxDwellTime);
}

[RequireComponent(typeof(NavMeshAgent))]
public class Walker : MonoBehaviour
{
    /// <summary>
    /// Walker state
    /// </summary>
    private enum State
    {
        Start,
        Walk,
        Dwell
    }

    [Tooltip("Patrol Way-points")]
    public WalkWayPoint[] wayPoints;

    [Tooltip("Event when walker starts walking.")]
    public UnityEvent startWalking;

    [Tooltip("Event when walker stops walking.")]
    public UnityEvent stopWalking;

    /// <summary>
    /// NavMesh agent
    /// </summary>
    private NavMeshAgent _agent;

    /// <summary>
    /// Current walker state
    /// </summary>
    private State _state = State.Start;

    /// <summary>
    /// Current walker dwell timeout
    /// </summary>
    private float _dwellTimeout;

    /// <summary>
    /// Next way-point in the list
    /// </summary>
    private int _currentWayPoint;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Process walker state
        switch (_state)
        {
            case State.Start:
                // If no way-points then stay at start state
                if (wayPoints.Length == 0)
                    return;

                // Start walking to the first way-point
                _agent.SetDestination(wayPoints[_currentWayPoint].transform.position);
                startWalking.Invoke();
                _state = State.Walk;
                break;

            case State.Walk:
                // Skip if still moving
                if (_agent.hasPath || _agent.pathPending)
                    return;

                // Pick a dwell time
                _dwellTimeout = wayPoints[_currentWayPoint].DwellTime();
                if (_dwellTimeout <= 0)
                { 
                    // Advance to the next way-point and start walking to it
                    _currentWayPoint = (_currentWayPoint + 1) % wayPoints.Length;
                    _agent.SetDestination(wayPoints[_currentWayPoint].transform.position);
                    return;
                }

                // Start dwelling
                stopWalking.Invoke();
                _state = State.Dwell;
                break;

            case State.Dwell:
                // Count dwell timing
                _dwellTimeout -= Time.deltaTime;
                if (_dwellTimeout > 0)
                    return;

                // Advance to the next way-point and start walking to it
                _currentWayPoint = (_currentWayPoint + 1) % wayPoints.Length;
                _agent.SetDestination(wayPoints[_currentWayPoint].transform.position);
                startWalking.Invoke();
                _state = State.Walk;
                break;
        }
    }
}
