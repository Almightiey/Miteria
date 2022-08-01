using UnityEngine;

public class AudioEnemy : MonoBehaviour, ITickUpdate
{
    [Header("Damage Audio")]
    public AudioClip audioClipDamage;
    [Range(0, 1)]
    public float valueAudioClipDamage;


    [Header("audio")]
    public AudioClip audioClip;
    [Range(0, 1)]
    public float valueAudioClip;


    [Header("Death Audio")]
    public AudioClip audioClipDeath;
    [Range(0, 1)]
    public float valueAudioClipDeath;

    [Header("Delay and Distance")]
    public float minDistance;
    public float maxDistance;
    public float mindDelay;
    public float maxDelay;

    private AudioSource audioDamage;
    private AudioSource audioSource;
    private AudioSource audioDeath;
    private float nextDelay;
    private Enemy enemy;



    private void OnEnable()
    {
        UpdateManager.ticks.Add(this);
        nextDelay = Time.time + Random.Range(mindDelay, maxDelay);
        Game.endGame += GameOver;
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.deathEnemy += Death;
        enemy.damageEnemy += DamageAudioPlay;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioDeath = gameObject.AddComponent<AudioSource>();
        audioDamage = gameObject.AddComponent<AudioSource>();


        InitAudio(audioSource, audioClip, valueAudioClip);
        InitAudio(audioDamage, audioClipDamage, valueAudioClipDamage);
        InitAudio(audioDeath, audioClipDeath, valueAudioClipDeath);
    }

    private void InitAudio(AudioSource audio, AudioClip clip, float volume)
    {
        audio.clip = clip;
        audio.volume = volume;
        audio.spatialBlend = 1;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.minDistance = minDistance;
        audio.maxDistance = maxDistance;
        audio.playOnAwake = false;
    }

    public void OnUpdate()
    {
        if(nextDelay < Time.time)
        {
            audioSource.Play();
            nextDelay = Time.time + Random.Range(mindDelay, maxDelay);
        }
    }

    public void Death()
    {
        UpdateManager.ticks.Remove(this);
        audioDeath.Play();
    }

    private void OnDisable()
    {
        UpdateManager.ticks.Remove(this);
        if (enemy.isDeath)
        {
            Destroy(this);
            Destroy(audioSource);
            Destroy(audioDeath);
            Destroy(audioDamage);
        }
    }

    private void DamageAudioPlay(float damage)
    {
        audioDamage.Play();
    }

    private void GameOver()
    {
        Game.endGame -= GameOver;
        UpdateManager.ticks.Remove(this);
    }
}
