using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSounds : MonoBehaviour
{
    private AudioManager audiomanager;
    public PlayerMovement player;


    private void Awake()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    public void PlayScreech(float baseVolume)
    {
        audiomanager.PlaySpatialSFX(audiomanager.metal_screech, transform.position, 12f, baseVolume);
    }


    public void FlickerLight(float baseVolume2)
    {
        audiomanager.PlaySpatialSFX(audiomanager.lamp_flicker, transform.position, 12f, baseVolume2);
    }


    public void WaterDripping(float baseVolume3)
    {
        audiomanager.PlaySpatialSFX(audiomanager.water_drip, transform.position, 12f, baseVolume3);
    }


    public void FridgeHum(float baseVolume4)
    {
        audiomanager.PlaySpatialSFX(audiomanager.fridge_hum, transform.position, 12f, baseVolume4);
    }
}
