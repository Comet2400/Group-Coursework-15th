using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip GameSound;
    public AudioClip CharacterSound;
    public AudioClip EnemySound;
    public AudioClip EnemySound2;
    public AudioClip EnemySound3;
    public AudioClip FlashLight1;
    public AudioClip FlashLight2;
    public AudioClip ShootingSound1;
    public AudioClip shootingSound2;
    public AudioClip WalkSound1;
    public AudioClip WalkSound2;

    private AudioSource audioSource;


    //Play weapon loading sound:
    public void PlayWeaponLoadingSound()
    {
        audioSource.PlayOneShot(ShootingSound1);
    }

    public void PlayWeaponFiringSound()
    {
        audioSource.PlayOneShot(shootingSound2);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // gets the audio source component
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    public void PlayGameSound()
    {
        audioSource.PlayOneShot(GameSound);
    }

    public void PlayCharacterSound() 
    {
        audioSource.PlayOneShot(CharacterSound);
    }

    public void PlayEnemySound() 
    {
        audioSource.PlayOneShot(EnemySound);
    }

    public void PlayEnemy2Sound()
    {
        audioSource.PlayOneShot(EnemySound2);
    }

    public void PlayEnemy3Sound()
    {
        audioSource.PlayOneShot(EnemySound3);
    }

    public void PlayFlashlightSound(bool flashLightOn)
    {
        audioSource.PlayOneShot(flashLightOn ? FlashLight1 : FlashLight2);
    }

    public void PlayShootingSound(bool useSound1)
    {
        audioSource.PlayOneShot(useSound1 ? ShootingSound1 : shootingSound2);
    }

    public void PlayWalkSound(bool useSound1)
    {
        audioSource.PlayOneShot(useSound1 ? WalkSound1 : WalkSound2);

    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
