using UnityEngine;
using UnityEngine.Events;

public class ProximityTrigger : MonoBehaviour
{
    [Tooltip("Target to perform proximity check on.")]
    public Transform target;

    [Tooltip("Target range for arrival.")]
    public float arriveRange = 1;

    [Tooltip("Target range for departure.")]
    public float departRange = 2;

    [Tooltip("Event invoked when the target arrives.")]
    public UnityEvent onArrive;

    [Tooltip("Event invoked when the target departs.")]
    public UnityEvent onDepart;

    /// <summary>
    /// Current target-
    /// </summary>
    private bool _present;

    private void Update()
    {
        // Get the distance to target
        var distance = (target.position - transform.position).magnitude;

        // Detect entering
        if (!_present && distance < arriveRange)
        {
            _present = true;
            onArrive.Invoke();
        }

        // Detect leaving
        if (_present && distance > departRange)
        {
            _present = false;
            onDepart.Invoke();
        }
    }
}
