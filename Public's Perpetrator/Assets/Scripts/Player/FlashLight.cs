using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    [Header("Movement settings")]
    public float minRadius = 0.5f; 
    public float maxRadius = 3f;
    public DialogueManager dialogue_manager;
    public AudioManager audio_manager;
    

    [Header("Highlighting objects")]
    public LayerMask interactableLayer;

    private Light2D flashlight_light;
    private Transform player_transform;
    private Camera mainCamera;


    private void Awake()
    {
        mainCamera = Camera.main;
        player_transform = transform.parent;

        flashlight_light = GetComponent<Light2D>();       
        flashlight_light.enabled = false;               
    }


    private void Update()
    {
        if (dialogue_manager != null && dialogue_manager.dialoguebox_on)
        {
            return; 
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight_light.enabled = !flashlight_light.enabled;
            audio_manager.PlaySFX(audio_manager.flashlight_click, 2f, 0.45f);
        }

        if (flashlight_light.enabled)
        {
            MoveAndRotateFlashlight();                  
        }
    }


    private void MoveAndRotateFlashlight()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; 

        Vector3 directionToMouse = mousePosition - player_transform.position;


        float rawDistance = directionToMouse.magnitude;
        float clampedDistance = Mathf.Clamp(rawDistance, minRadius, maxRadius);

        Vector3 clampedPosition = directionToMouse.normalized * clampedDistance;
        transform.position = player_transform.position + clampedPosition;                       // move the flashlight's position relative to the player and clamped distance to mouse

        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;      
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);                             // adjust angle offset depending on native orientation & cursor (-90f if pointing right)
                        

        RaycastHit2D hit = Physics2D.Raycast(player_transform.position, directionToMouse, maxRadius, interactableLayer);    // object is highlighted when it touches the invisible ray line
        if (hit.collider != null)
        {
            HighlightObject item = hit.collider.GetComponent<HighlightObject>();

            if (item != null)
            {
                item.ShineLight();
            }
        }
    }
}
