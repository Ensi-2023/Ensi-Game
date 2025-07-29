using UnityEngine;

public class PlayerFootstepSound 
{
    private AudioSource _audioSource;
    private GroundChecker _groundChecker;
    private PlayerStepAudioClips _stepAudioClips;
    
    private float _stepInterval = 0f;
    private float _stepCounter;
    private StepSoundCollection _currentSoundCollection;
    
    [Header("Jump Sound")]
    [SerializeField] private float _jumpVolumeMultiplier = 1.2f;
    [SerializeField] private float _jumpPitchVariationMultiplier = 1.5f;
    
    [Header("Landing Sound")]
    [SerializeField] private float _landingVolumeMultiplier = 1.5f;
    [SerializeField] private float _landingPitchVariationMultiplier = 1.2f;
    
    
    public void Initialize(AudioSource audioSource,
                           GroundChecker groundChecker, 
                           PlayerStepAudioClips stepAudioClips)
    {
        _audioSource = audioSource;
        _groundChecker = groundChecker;
        _stepAudioClips = stepAudioClips;
    }


    public void UpdateStepInterval(float interval)
    {
        _stepInterval = interval;
    }

    
    public void PlayLandingSound()
    {
        UpdateSoundCollection(_groundChecker.GetCurrentSurfaceType());
        
        if (_currentSoundCollection == null) return;
        
        SoundCollectionsFootsteps sounds = _currentSoundCollection.Sounds;
        AudioClip clip = GetLandingSound(sounds);
        
        if (clip == null) 
        {
            Debug.LogWarning("No landing sound available");
            return;
        }

        _audioSource.volume = sounds.Volume * _landingVolumeMultiplier;
        _audioSource.pitch = 1f + Random.Range(
            -sounds.PitchVariation * _landingPitchVariationMultiplier, 
            sounds.PitchVariation * _landingPitchVariationMultiplier
        );
        _audioSource.PlayOneShot(clip);
    }
    
    
    public void PlayJumpSound()
    {
        UpdateSoundCollection(_groundChecker.GetCurrentSurfaceType());
        
        if (_currentSoundCollection == null) return;
        
        SoundCollectionsFootsteps sounds = _currentSoundCollection.Sounds;
        AudioClip clip = GetJumpSound(sounds);
        
        if (clip == null) 
        {
            Debug.LogWarning("No jump sound available");
            return;
        }

        _audioSource.volume = sounds.Volume * _jumpVolumeMultiplier;
        _audioSource.pitch = 1f + Random.Range(
            -sounds.PitchVariation * _jumpPitchVariationMultiplier, 
            sounds.PitchVariation * _jumpPitchVariationMultiplier
        );
        _audioSource.PlayOneShot(clip);
    }
    
    private AudioClip GetLandingSound(SoundCollectionsFootsteps sounds)
    {
        if (sounds.LandingSound != null && sounds.LandingSound.Length > 0)
            return sounds.LandingSound[Random.Range(0, sounds.LandingSound.Length)];
        
        // Fallback to regular step sounds if no landing sounds available
        if (sounds.Clips != null && sounds.Clips.Length > 0)
            return sounds.Clips[Random.Range(0, sounds.Clips.Length)];
        
        return null;
    }
    
    private AudioClip GetJumpSound(SoundCollectionsFootsteps sounds)
    {
        if (sounds.JumpSound != null && sounds.JumpSound.Length > 0)
            return sounds.JumpSound[Random.Range(0, sounds.JumpSound.Length)];
        
        // Fallback to regular step sounds if no jump sounds available
        if (sounds.Clips != null && sounds.Clips.Length > 0)
            return sounds.Clips[Random.Range(0, sounds.Clips.Length)];
        
        return null;
    }
    
    public void HandleFootsteps(float speed,float volume, bool isGrounded)
    {
        if(_stepInterval==0) return;
        
        if (!isGrounded || speed < 0.1f) 
        {
            _stepCounter = 0;
            return;
        }
        
        _stepCounter += speed * Time.deltaTime;

        if (_stepCounter >= _stepInterval)
        {
            PlayFootstep(volume);
            _stepCounter = 0;
        }
    }
    
    private void PlayFootstep(float volume)
    {

        
        // Обновляем тип поверхности перед воспроизведением
        UpdateSoundCollection(_groundChecker.GetCurrentSurfaceType());
        
        if (_currentSoundCollection == null) return;
        
        SoundCollection sounds = _currentSoundCollection.Sounds;
        AudioClip clip = sounds.Clips[Random.Range(0, sounds.Clips.Length)];
        
        _audioSource.volume = sounds.Volume - volume;
        _audioSource.pitch = 1f + Random.Range(-sounds.PitchVariation, sounds.PitchVariation);
        _audioSource.PlayOneShot(clip);
    }
    
    private void UpdateSoundCollection(TypeSound surfaceType)
    {
        
        foreach (var collection in _stepAudioClips.SoundCollection)
        {
            if (collection.TypeSound == surfaceType)
            {
                _currentSoundCollection = collection;
                return;
            }
        }
        _currentSoundCollection = _stepAudioClips.SoundCollection[0];
    }
    
}
