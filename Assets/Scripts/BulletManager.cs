using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    //  ############################################################################################################
    //  ############################################  VARIABLES ####################################################
    //  ############################################################################################################

    //###############
    //  Damage
    //###############
    public int damagePoints = 5;

    //###############
    //  Particles
    //###############
    public ParticleSystem ps_bulletExploding;

    private AudioManager audioManager;


    //  ############################################################################################################
    //  #########################################  START / UPDATE  #################################################
    //  ############################################################################################################
    void Start()
    {
       audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {

    }


    //  ############################################################################################################
    //  ##########################################  BULLET COLLISION  ##############################################
    //  ############################################################################################################
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            ps_bulletExploding.Play();
            Destroy(gameObject,0.5f);
        }
        else if (collision.gameObject.CompareTag("Player01") || collision.gameObject.CompareTag("Player02"))
        {
            audioManager.GetComponent<AudioManager>().PlayBulletExplosion();
        }
    }
}
