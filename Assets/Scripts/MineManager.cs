using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MineManager : MonoBehaviour
{
    //  ############################################################################################################
    //  ############################################  VARIABLES ####################################################
    //  ############################################################################################################

    [SerializeField]    private int mineDamage = 50;
    [SerializeField]    private ParticleSystem mineExplosion;
    [SerializeField]    private GameObject mineObject;
    private SphereCollider sphereCollider;
    private AudioManager audioManager;

    //  ############################################################################################################
    //  #########################################  START / UPDATE  #################################################
    //  ############################################################################################################
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        
    }

    //  ############################################################################################################
    //  ##########################################  WHEN COLLIDES  #################################################
    //  ############################################################################################################
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerController>().damageHealth(mineDamage);
        mineObject.SetActive(false);
        mineExplosion.Play();
        audioManager.PlayTankExploding();
        sphereCollider.enabled = false;
        StartCoroutine(DelayedDestroyedMine());
    }

    IEnumerator DelayedDestroyedMine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
