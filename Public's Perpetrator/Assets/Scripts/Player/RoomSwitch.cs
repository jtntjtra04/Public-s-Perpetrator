using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitch : MonoBehaviour
{
    public Transform destination;
    public bool isTransition = false;

  private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if((Vector2.Distance(collision.transform.position, transform.position) > 0.3f))
            {
                 StartCoroutine(TransitionRoom(collision));
            } 
         }
    }

    private IEnumerator TransitionRoom(Collider2D collision)
    {
        collision.transform.position = new Vector2(destination.position.x, destination.position.y);
        isTransition = true;
        yield return new WaitForSeconds(0.2f);
        isTransition = false;
    }
}
