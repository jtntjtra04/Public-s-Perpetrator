using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public enum WorldSceneType
{
    Epilogue,
    Closet_trap
}


public class WorldSceneManager : MonoBehaviour
{
    public static WorldSceneType world_scene_ToPlay;
    public static bool world_scene_pending = false;
    public PlayableDirector director;

    public TimelineAsset epilogue;
    public TimelineAsset closet_trap;


    private void Start()
    {
        PlayCutscene(world_scene_ToPlay);
    }


    public void PlayCutscene(WorldSceneType world_scene_type)
    {
        switch (world_scene_type)
        {
            case WorldSceneType.Epilogue:
                director.playableAsset = epilogue;
                break;

            case WorldSceneType.Closet_trap:
                director.playableAsset = closet_trap;
                break;
        }

        director.Play();
    }
}
