using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float follow_speed;
    [SerializeField] private float offset;

    private void Update()
    {
        Vector3 update_pos = new Vector3(target.position.x, target.position.y + offset, -10f);
        transform.position = Vector3.Slerp(transform.position, update_pos, follow_speed * Time.deltaTime);
    }
}
