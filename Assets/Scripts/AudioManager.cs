using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //  ############################################################################################################
    //  ############################################  VARIABLES ####################################################
    //  ############################################################################################################

    //###############
    //  AudioSource
    //###############
    public static AudioManager instance;
    //--For BACKGROUND MUSIC
    private AudioSource audioSourceMusic;
    //--For BULLET
    private AudioSource audioSourceExplosion;
    private AudioSource audioSourceCharging;
    private AudioSource audioSourceFiring;
    //--For TANK
    private AudioSource audioSourceExploding;

    //###############
    //  AudioClips
    //###############
    //--For BACKGROUND MUSIC
    public AudioClip backgroundMusic;
    //--For BULLET
    public AudioClip bulletExplosion;
    public AudioClip bulletCharging;
    public AudioClip bulletFiring;
    //--For TANK
    public AudioClip tankExploding;


    //  ############################################################################################################
    //  ##############################################  AWAKE ######################################################
    //  ############################################################################################################
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSourceMusic = gameObject.AddComponent<AudioSource>();
            audioSourceCharging = gameObject.AddComponent<AudioSource>();
            audioSourceExplosion = gameObject.AddComponent<AudioSource>();
            audioSourceFiring = gameObject.AddComponent<AudioSource>();
            audioSourceExploding = gameObject.AddComponent<AudioSource>();


            audioSourceMusic.clip = backgroundMusic;
            audioSourceMusic.volume = 0.5f;
            audioSourceMusic.loop = true;
            audioSourceMusic.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //  ############################################################################################################
    //  ###########################################  BULLET VFX ####################################################
    //  ############################################################################################################

    //--BULLET EXPLOSION (AT IMPACT)
    public void PlayBulletExplosion()
    {
        if (bulletExplosion != null && audioSourceExplosion != null)
        {
            audioSourceExplosion.volume = 1.0f;
            audioSourceExplosion.PlayOneShot(bulletExplosion);
        }
    }

    //--BULLET CHARGING
    public void PlayBulletCharging()
    {
        if (bulletCharging != null && audioSourceCharging != null)
        {
            audioSourceCharging.PlayOneShot(bulletCharging);
        }
    }
    public void StopBulletCharging()
    {
        if (bulletCharging != null && audioSourceCharging != null)
        {
            audioSourceCharging.Stop();
        }
    }

    //--BULLET FIRING
    public void PlayBulletFiring()
    {
        if (bulletFiring != null && audioSourceFiring != null)
        {
            audioSourceFiring.volume = 1.0f;
            audioSourceFiring.PlayOneShot(bulletFiring);
        }
    }

    //  ############################################################################################################
    //  #############################################  TANK VFX ####################################################
    //  ############################################################################################################

    //--TANK EXPLODING
    public void PlayTankExploding()
    {
        if (tankExploding != null && audioSourceExploding != null)
        {
            audioSourceExploding.volume = 1.0f;
            audioSourceExploding.PlayOneShot(tankExploding);
        }
    }
}