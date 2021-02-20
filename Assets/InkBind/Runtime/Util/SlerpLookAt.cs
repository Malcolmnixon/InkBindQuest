using UnityEngine;

public class SlerpLookAt : MonoBehaviour
{
    [Tooltip("Target to look at.")]
    public Transform target;

    [Tooltip("Slerp rate.")]
    public float rate;

    /// <summary>
    /// Enable look-at
    /// </summary>
    /// <param name="t">Target transform</param>
    public void Enable(Transform t)
    {
        target = t;
    }

    /// <summary>
    /// Disable look-at
    /// </summary>
    public void Disable()
    {
        target = null;
    }

    private void Update()
    {
        if (target == null)
            return;

        var direction = target.position - transform.position;
        direction.y = 0.0F;

        var rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rate);
    }
}
