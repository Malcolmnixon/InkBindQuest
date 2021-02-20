using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFace : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    private void Update()
    {
        transform.LookAt(target.transform.position);
        transform.Rotate(0, 180, 0);
    }
}
