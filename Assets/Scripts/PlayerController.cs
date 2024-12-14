using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private const int V = 0;

    //  ############################################################################################################
    //  ############################################  VARIABLES ####################################################
    //  ############################################################################################################

    //###############
    //  Who is Who ?
    //###############
    private bool isPlayer01 = false;
    private bool isPlayer02 = false;

    //###############
    //  Managers 
    //###############
    private UIManager uiManager;
    private AudioManager audioManager;

    //###############
    //  Health 
    //###############
    [Header("Health")]
    public int currentHealthPoints = 100;
    public int maxHealthPoints = 100;

    //###############
    //  Death
    //###############
    [Header("Death")]
    [SerializeField] private GameObject lifeMesh;
    [SerializeField] private GameObject deathMesh;

    //###############
    //  Deplacement
    //###############
    [Header("Deplacement")]
    [SerializeField]    private float normalPlayerSpeed = 5f;
    [SerializeField]    private float normalPlayerRotationSpeed = 50f;
    private float playerSpeed;
    private float playerRotationSpeed;

    //###############
    //  Shooting
    //###############
    [Header("Shooting")]
    public int bulletMunitions = 10;
    public int maxBulletMunitions = 10;
    [SerializeField]    private float maxForcePoints = 100;
    [SerializeField]    private float bulletSpeed = 2f;
    [SerializeField]    private Transform bulletStartingPoint;
    [SerializeField]    private GameObject bulletPrefab;
    private float keyPressDuration = 0;
    private float keyPressedStartTime = 0;
    private bool isKeyPressed = false;

    //###############
    //  Shooting Line
    //###############
    [Header("Shooting Line")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int numPoints = 50;
    [SerializeField] private float timeBetweenPoints = 0.1f;
    [SerializeField] private LayerMask CollidableLayers;

    //###############
    //  Mines
    //###############
    [Header("Mines")]
    [SerializeField] private Transform mineStartingPoint;
    public int mineMunitions = 0;
    public int maxMineMunitions = 10;
    public GameObject minePrefab;

    //###############
    //  Particles 
    //###############
    [Header("Particles")]
    [SerializeField] private ParticleSystem particleFiring;
    [SerializeField] private ParticleSystem particleMoving;
    [SerializeField] private ParticleSystem particleExploding1;
    [SerializeField] private ParticleSystem particleExploding2;
    [SerializeField] private ParticleSystem particlePowerUp;

    //###############
    //  Power Up
    //###############
    [Header("Power Up")]
    public bool isPoweredUp = false;

    //  ############################################################################################################
    //  #########################################  START / UPDATE  #################################################
    //  ############################################################################################################
    void Start()
    {

        //  What number is the player ?
        if (CompareTag("Player01"))
        {
            isPlayer01 = true;
        }
        else if(CompareTag("Player02"))
        {
            isPlayer02 = true;
        }

        //  Setting the audio manager
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        //  Setting the UI relative to the Player
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uiManager.DisableAllScoreText();
        uiManager.SetMaxHealth(maxHealthPoints);
        uiManager.SetMaxForce(maxForcePoints);

        //  Setting the normal Speed
        SettingNormalSpeed();

        particlePowerUp.Stop();

    }

    void Update()
    {
        playerDeplacement();
        playerShooting();
        playerMining();
        isPlayerDead();

        //  Updating Munitions
        if (isPlayer01)
        {
            uiManager.UpdateBulletMunitions_P1(bulletMunitions);
            uiManager.UpdateMineMunitions_P1(mineMunitions);
            uiManager.SetHealthSlider_P1(currentHealthPoints);
        }
        else if(isPlayer02) 
        {
            uiManager.UpdateBulletMunitions_P2(bulletMunitions);
            uiManager.UpdateMineMunitions_P2(mineMunitions);
            uiManager.SetHealthSlider_P2(currentHealthPoints);
        }

        //  If player is Powered Up
        if(isPoweredUp) 
        {
            powerIsUp();
        }
    }


    //  ############################################################################################################
    //  ########################################  PLAYER DEPLACEMENT  ##############################################
    //  ############################################################################################################
    private void playerDeplacement()
    {
        float horizontalInput = 0;
        float verticalInput = 0;

        if (isPlayer01)
        {
            horizontalInput = Input.GetAxis("Horizontal_P1");
            verticalInput = Input.GetAxis("Vertical_P1");
        }
        else if (isPlayer02)
        {
            horizontalInput = Input.GetAxis("Horizontal_P2");
            verticalInput = Input.GetAxis("Vertical_P2");
        }

        Vector3 movement = new Vector3(0f, 0f, verticalInput) * playerSpeed * Time.deltaTime;
        float rotationAmount = horizontalInput * playerRotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAmount);
        transform.Translate(movement);

        particleMoving.Play();
    }

    //  ############################################################################################################
    //  ########################################  PLAYER SHOOTING  #################################################
    //  ############################################################################################################
    private void playerShooting()
    {
        if ( ( (isPlayer01 && Input.GetKeyDown(KeyCode.Keypad0)) || (isPlayer02 && Input.GetKeyDown(KeyCode.Space)) ) && bulletMunitions > 0)
        {
            audioManager.PlayBulletCharging();
            keyPressedStartTime = Time.time;
            isKeyPressed = true;
        }

        if (    isKeyPressed && (   ( isPlayer01 && Input.GetKey(KeyCode.Keypad0) ) || ( isPlayer02 && Input.GetKey(KeyCode.Space) ) ) )
        {
            keyPressDuration = Time.time - keyPressedStartTime;
            drawShootingPath(   checkForce(keyPressDuration * bulletSpeed)  );
        }

        if (    ((isPlayer01 && Input.GetKeyUp(KeyCode.Keypad0) ) || (isPlayer02 && Input.GetKeyUp(KeyCode.Space)) )  && bulletMunitions > 0)
        {
            isKeyPressed = false;
            shooting(   checkForce(keyPressDuration * bulletSpeed)  );

            keyPressDuration = 0;
            lineRenderer.positionCount = V;

            uiManager.SetForceSlider_P1(0.0f);
            uiManager.SetForceSlider_P2(0.0f);

            audioManager.StopBulletCharging();
            audioManager.PlayBulletFiring();

            bulletMunitions--;
        }

    }
    private float checkForce(float force ) 
    {
        if (force < 10.0f)
        {
            force = 10.0f;
        }

        if (force >= 100.0f)
        {
           force = 100.0f;
        }

        return force;
    }
    private void shooting(float bulletForce)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletStartingPoint.position, bulletStartingPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(bulletStartingPoint.forward * bulletForce, ForceMode.Impulse);

        particleFiring.Play();
    }
    private void drawShootingPath(float bulletForce)
    {
        if(isPlayer01)
        {
             uiManager.SetForceSlider_P1(bulletForce);
        }
        else if(isPlayer02)
        {
            uiManager.SetForceSlider_P2(bulletForce);
        }

        lineRenderer.positionCount = (int)numPoints;
        List<Vector3> points = new List<Vector3>();
        Vector3 startingPosition = bulletStartingPoint.position;
        Vector3 startingVelocity = bulletStartingPoint.forward * bulletForce;

        for(float t = 0; t < numPoints; t+=timeBetweenPoints) 
        {
            Vector3 newPoint = startingPosition + t * startingVelocity;
            newPoint.y = startingPosition.y + startingVelocity.y * t + (Physics.gravity.y/2f)*t*t;
            points.Add(newPoint);

            if(Physics.OverlapSphere(newPoint, 0.5f, CollidableLayers).Length > 0)
            {
                lineRenderer.positionCount = points.Count;
                break;
            }
            
        }
        lineRenderer.SetPositions(points.ToArray());
    }

    //  ############################################################################################################
    //  #########################################  PLAYER MINING  ##################################################
    //  ############################################################################################################
    private void playerMining()
    {
        if (((isPlayer01 && Input.GetKeyDown(KeyCode.Keypad3)) || (isPlayer02 && Input.GetKeyDown(KeyCode.N))) && mineMunitions > 0)
        {
            GameObject mine = Instantiate(minePrefab, mineStartingPoint.position, mineStartingPoint.rotation);
            mineMunitions--;
        }
    }

    //  ############################################################################################################
    //  ######################################  PLAYER TRIGGER BOX  ################################################
    //  ############################################################################################################
    private void OnTriggerEnter(Collider other)
    {
        /*
         *   @return void
         *   When a Bullet Object enters the Player collider : 
         *   -  Player get damaged by the damaged points of the bullet
         *   -  The bullet exploding particle system plays
         *   -  The bullet gets destroyed after 2secs
         * 
        */

        if (other.gameObject.CompareTag("Bullet"))
        {
            damageHealth(other.gameObject.GetComponent<BulletManager>().damagePoints);
            other.gameObject.GetComponent<BulletManager>().ps_bulletExploding.Play();
            Destroy(other.gameObject, 2.0f);
        }
    }

    //  ############################################################################################################
    //  #####################################  PLAYER HEALTH MANAGEMENT  ###########################################
    //  ############################################################################################################
    public void damageHealth(int damage)
    {
        currentHealthPoints -= damage;

        if (isPlayer01)
        {
            uiManager.SetHealthSlider_P1(currentHealthPoints);
        }
        else if (isPlayer02)
        {
            uiManager.SetHealthSlider_P2(currentHealthPoints);
        }
    }

    //  ############################################################################################################
    //  #######################################  POWER UP MANAGEMENT  ##############################################
    //  ############################################################################################################
    public void powerIsUp()
    {
        playerSpeed*=1.5f;
        playerRotationSpeed*=1.5f;
        isPoweredUp = false;

        if(isPlayer01)
        {
            uiManager.ActivatePowerUpInfo_P1();
        }
        else if (isPlayer02)
        {
            uiManager.ActivatePowerUpInfo_P2();
        }

        particlePowerUp.Play();

        StartCoroutine(DelayedPowerUp());
    }
    IEnumerator DelayedPowerUp()
    {
        yield return new WaitForSeconds(10f);

        SettingNormalSpeed();

        if (isPlayer01)
        {
            uiManager.DesactivatePowerUpInfo_P1();
        }
        else if (isPlayer02)
        {
            uiManager.DesactivatePowerUpInfo_P2();
        }
        particlePowerUp.Stop();
    }
    public void SettingNormalSpeed()
    {
        playerSpeed = normalPlayerSpeed;
        playerRotationSpeed = normalPlayerRotationSpeed;
    }

    //  ############################################################################################################
    //  ##########################################  PLAYER IS DEAD ##################################################
    //  ############################################################################################################      
    public void isPlayerDead()
    {
        if (currentHealthPoints <= 0)
        {
            if (isPlayer01)
            {
                isPlayer01 = false;
                uiManager.ActivateLosingUi_P1();
                uiManager.ActivateWinningUi_P2();
                particleExploding1.Play();
                particleExploding2.Play();
                StartCoroutine(DelayedEndingGame());
                audioManager.PlayTankExploding();
            }
            else if (isPlayer02)
            {
                isPlayer02 = false;
                uiManager.ActivateLosingUi_P2();
                uiManager.ActivateWinningUi_P1();
                particleExploding1.Play();
                particleExploding2.Play();
                StartCoroutine(DelayedEndingGame());
                audioManager.PlayTankExploding();
            }

            lifeMesh.SetActive(false);
            deathMesh.SetActive(true);
        }
    }
    IEnumerator DelayedEndingGame()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("Level");
    }
}