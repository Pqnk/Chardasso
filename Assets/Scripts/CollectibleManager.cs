using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    //  ############################################################################################################
    //  ############################################  VARIABLES ####################################################
    //  ############################################################################################################

    //###################
    //  What is What ?
    //###################
    private bool isMineRefill = false;
    private bool isBulletRefill = false;
    private bool isHealthRefill = false;
    private bool isPowerUp = false;

    public int maxCollectibleNumber = 3;

    //####################
    //  Refill Power
    //####################
    public UIManager uiManager;
    [SerializeField] private int numberMineRefill = 5;
    [SerializeField] private int numberBulletRefill = 5;
    [SerializeField] private int numberHealthRefill = 25;

    //  ############################################################################################################
    //  #########################################  START / UPDATE  #################################################
    //  ############################################################################################################
    void Start()
    {
        if(CompareTag("MineRefill"))
        {
            isMineRefill = true;
        }
        else if(CompareTag("BulletRefill"))
        {
            isBulletRefill = true;
        }
        else if(CompareTag("HealthRefill"))
        {
            isHealthRefill = true;
        }
        else if (CompareTag("PowerUp"))
        {
            isPowerUp = true;
        }

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void Update()
    {
       transform.Rotate(Vector3.up, 200 * Time.deltaTime);
    }

    //  ############################################################################################################
    //  ###########################################  WHEN COLLIDES #################################################
    //  ############################################################################################################
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player01") || other.CompareTag("Player02"))
        {
           // CASE of a MINE COLLECTIBLE
           if(isMineRefill)
            {
                other.GetComponent<PlayerController>().mineMunitions += numberMineRefill;

                if (other.GetComponent<PlayerController>().mineMunitions > other.GetComponent<PlayerController>().maxMineMunitions)
                {
                    other.GetComponent<PlayerController>().mineMunitions = other.GetComponent<PlayerController>().maxMineMunitions;
                }
            }

           // CASE of a BULLET COLLECTIBLE
           if (isBulletRefill)
            {
                other.GetComponent<PlayerController>().bulletMunitions += numberBulletRefill;

                if (other.GetComponent<PlayerController>().bulletMunitions > other.GetComponent<PlayerController>().maxBulletMunitions)
                {
                    other.GetComponent<PlayerController>().bulletMunitions = other.GetComponent<PlayerController>().maxBulletMunitions;
                }
            }

           // CASE of a HEALTH COLLECTIBLE
           if (isHealthRefill)
            {
                other.GetComponent<PlayerController>().currentHealthPoints += numberHealthRefill;

                if (other.GetComponent<PlayerController>().currentHealthPoints > other.GetComponent<PlayerController>().maxHealthPoints)
                {
                    other.GetComponent<PlayerController>().currentHealthPoints = other.GetComponent<PlayerController>().maxHealthPoints;
                }
            }

           // CASE of a POWER UP
           if (isPowerUp)
            {
                other.GetComponent<PlayerController>().isPoweredUp = true;
            }

           // DESACTIVATE THE OBJECT DURING SOME TIME
           gameObject.SetActive(false);
           uiManager.ActivateCollectibleAfter5Seconds(gameObject);
        }
    }
}
