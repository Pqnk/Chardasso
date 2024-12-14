using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //  ############################################################################################################
    //  ############################################  VARIABLES ####################################################
    //  ############################################################################################################

    //###################
    //  Health Bars
    //###################
    [Header("Health Bars")]
    public RectTransform p1_health;
    public RectTransform p2_health;

    //###################
    //  Win or Lose
    //###################
    [Header("Win / Lose")]
    public TMP_Text p1_winning;
    public TMP_Text p1_losing;
    public TMP_Text p2_winning;
    public TMP_Text p2_losing;

    //###################
    //  BulletForce Bars
    //###################
    [Header("Bullet Force Bars")]
    public RectTransform p1_force;
    public RectTransform p2_force;

    //###################
    //  Munitions
    //###################
    [Header("Munitions")]
    public TMP_Text p1_bulletMunitions;
    public TMP_Text p2_bulletMunitions;
    public TMP_Text p1_mineMunitions;
    public TMP_Text p2_mineMunitions;

    //###################
    //  Power Up
    //###################
    [Header("Power Up")]
    public TMP_Text p1_powerUpInfo;
    public TMP_Text p2_powerUpInfo;

    //###################
    //  Collectibles
    //###################
    [Header("Collectible")]
    [SerializeField] private float collectible_TimeToReappear = 10.0f;
    [SerializeField] private GameObject[] gameObjectsToInstantiate;
    [SerializeField] private Transform parent_collectiblePositions;
    private int[] gameObjectsToInstantiateNumber;
    private int index = 0;

    //  ############################################################################################################
    //  #########################################  START / UPDATE  #################################################
    //  ############################################################################################################
    void Start()
    {
        RandomizeCollectiblePosition();
        DesactivatePowerUpInfo_P1();
        DesactivatePowerUpInfo_P2();
    }

    void Update()
    {
    }
   

    //  ############################################################################################################
    //  ######################################  MANAGING HEALTH BARS  ##############################################
    //  ############################################################################################################
    public void SetMaxHealth(int maxHealth)
    {
        p1_health.GetComponent<Slider>().maxValue = maxHealth;
        p1_health.GetComponent<Slider>().value = maxHealth;

        p2_health.GetComponent<Slider>().maxValue = maxHealth;
        p2_health.GetComponent<Slider>().value = maxHealth;
    }
    public void SetHealthSlider_P1(int health)
    {
        p1_health.GetComponent<Slider>().value = health;
    }
    public void SetHealthSlider_P2(int health)
    {
        p2_health.GetComponent<Slider>().value = health;
    }

    //  ############################################################################################################
    //  ####################################  MANAGING BULLET FORCE BARS  ##########################################
    //  ############################################################################################################
    public void SetMaxForce(float maxForce)
    {
        p1_force.GetComponent<Slider>().maxValue = maxForce;
        p1_force.GetComponent<Slider>().value = 0;

        p2_force.GetComponent<Slider>().maxValue = maxForce;
        p2_force.GetComponent<Slider>().value = 0;
    }
    public void SetForceSlider_P1(float force)
    {
        p1_force.GetComponent<Slider>().value = force;
    }
    public void SetForceSlider_P2(float force)
    {
        p2_force.GetComponent<Slider>().value = force;
    }

    //  ############################################################################################################
    //  #########################################  MANAGING MUNITIONS  #############################################
    //  ############################################################################################################
    public void UpdateBulletMunitions_P1(int munitions_p1)
    {
        p1_bulletMunitions.text = munitions_p1.ToString();
    }
    public void UpdateBulletMunitions_P2(int munitions_p2)
    {
        p2_bulletMunitions.text = munitions_p2.ToString();
    }
    public void UpdateMineMunitions_P1(int munitions_p1)
    {
        p1_mineMunitions.text = munitions_p1.ToString();
    }
    public void UpdateMineMunitions_P2(int munitions_p2)
    {
        p2_mineMunitions.text = munitions_p2.ToString();
    }

    //  ############################################################################################################
    //  ########################################  MANAGING COLLECTIBLES  ###########################################
    //  ############################################################################################################
    public void ActivateCollectibleAfter5Seconds(GameObject collectible)
    {
        StartCoroutine(DelayedActivation(collectible));
    }
    IEnumerator DelayedActivation(GameObject collectible)
    {
        yield return new WaitForSeconds(collectible_TimeToReappear);
        collectible.SetActive(true);
    }
    public void RandomizeCollectiblePosition()
    {
        gameObjectsToInstantiateNumber = new int[gameObjectsToInstantiate.Length];

        for (int j = 0; j < gameObjectsToInstantiate.Length; j++)
        {
            gameObjectsToInstantiateNumber[j] = 0;
        }

        for (int i = 0; i < parent_collectiblePositions.childCount; i++)
        {
            do
            {
                index = UnityEngine.Random.Range(0, gameObjectsToInstantiate.Length);
            }
            while (gameObjectsToInstantiateNumber[index] >= 3);

            GameObject selectedObject = gameObjectsToInstantiate[index];
            gameObjectsToInstantiateNumber[index]++;

            Instantiate(selectedObject, parent_collectiblePositions.GetChild(i).position, Quaternion.identity, parent_collectiblePositions.GetChild(i));
        }
    }

    //  ############################################################################################################
    //  ########################################  POWER UP INFORMATIONS  ###########################################
    //  ############################################################################################################
    public void ActivatePowerUpInfo_P1()
    {
        p1_powerUpInfo.enabled = true;
    }
    public void DesactivatePowerUpInfo_P1()
    {
        p1_powerUpInfo.enabled = false;
    }
    public void ActivatePowerUpInfo_P2()
    {
        p2_powerUpInfo.enabled = true;
    }
    public void DesactivatePowerUpInfo_P2()
    {
        p2_powerUpInfo.enabled = false;
    }

    //  ############################################################################################################
    //  ####################################  WINNING / LOSING INFORMATIONS  #######################################
    //  ############################################################################################################
    public void ActivateWinningUi_P1()
    {
        p1_winning.enabled = true;
    }
    public void ActivateLosingUi_P1()
    {
        p1_losing.enabled = true;
    }
    public void ActivateWinningUi_P2()
    {
        p2_winning.enabled = true;
    }
    public void ActivateLosingUi_P2()
    {
        p2_losing.enabled = true;
    }
    public void DisableAllScoreText()
    {
        p1_winning.enabled = false;
        p1_losing.enabled = false;
        p2_winning.enabled = false;
        p2_losing.enabled = false;
    }
}
