using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceYSort : MonoBehaviour
{
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        cam.transparencySortMode = TransparencySortMode.CustomAxis;
        cam.transparencySortAxis = new Vector3(0, 1, 0);
    }
}
