using UnityEngine;

[System.Serializable]
public class SoundCollection 
{
     public AudioClip[] Clips;
     [Range(0f, 1f)] public float Volume = 0.8f;
     [Range(0f, 0.5f)] public float PitchVariation = 0.1f;
}


[System.Serializable]
public class SoundCollectionsFootsteps : SoundCollection
{
     public AudioClip[] JumpSound;
     public AudioClip[] LandingSound;
}