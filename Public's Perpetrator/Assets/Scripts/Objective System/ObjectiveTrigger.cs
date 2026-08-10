using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectiveTrigger : MonoBehaviour
{
    public ObjectiveManager objective_manager;
    public ObjectiveType objective_set;
    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            objective_manager.SetObjective(objective_set);
            Destroy(gameObject);
        }
    }
}
