using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class playerScript : MonoBehaviour {

    private bool isShooting;
    public GameObject gun;
    RaycastHit hit;
    
    public float playerHealth;
    public Text healthNum;
    public Text killNum;
    public int winScore;

    public GameObject zombie;
    private ZombieScript zomb;
    Collider zombieCollider;

    public GameObject plane;
    public GameObject sceneObjectHolder;
    public List<GameObject> myList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 60;
        winScore = 0;
        playerHealth = 100;
        isShooting = false;
        zomb = (ZombieScript)zombie.GetComponent<ZombieScript>();
        
	}

    void Awake()
    {
        for (int i = 0; i < myList.Count; i++)
        {
            //float randomX = Mathf.Clamp(randomRange, Random.Range(-3.0f, -19.0f), Random.Range(3.0f, 19.0f));
            //const float randomY = 0.01f;
            //float randomZ = Mathf.Clamp(randomRange, Random.Range(-3.0f, -19.0f), Random.Range(3.0f, 19.0f));

            float randomX = Random.Range(-19f, 19f);
            const float randomY = 0.01f;
            float randomZ = Random.Range(-19f, 19f);

            while(randomX > -3f && randomX < 3f)
            {
                randomX = Random.Range(Random.Range(-19f, -3f), Random.Range(3f, 19f));
            }
            while (randomZ > -3f && randomZ < 3f)
            {
                randomZ = Random.Range(Random.Range(-19f, -3f), Random.Range(3f, 19f));
            }

            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
            Debug.Log(randomPosition + " random");
        
            GameObject child = Instantiate(myList[i], randomPosition, Quaternion.identity);

            //put the objects into a parent
            foreach (GameObject children in myList)
            {
                child.transform.parent = sceneObjectHolder.transform;
            }
        }
    }

    IEnumerator Shoot()
    {
        winScore++;
        isShooting = true;
        
        GetComponent<AudioSource>().Play();
        gun.GetComponent<Animation>().Play();

        //set the bools from other zomb script
        zomb.GetComponent<ZombieScript>().z_isDead = true;
        zomb.GetComponent<ZombieScript>().z_isAttacking = false;

        zomb.GetComponent<ZombieScript>().ZombieDead(zombieCollider);
        //wait until you can shoot again
        yield return new WaitForSeconds(2.0f);
        isShooting = false;

        //if won after the last kill
        if (winScore == 7)
        {
            yield return new WaitForSeconds(1.5f);
            PlayerWon();
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        //display health & kills
        healthNum.text = playerHealth.ToString();
        killNum.text = winScore.ToString() + " / 7";

        //raycast to shoot out of centre screen and hit zombie, happens once every frame...
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, out hit, 7.5f))
        {
            //if you hit a zombie
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.collider.tag.Contains("zombie"))
            {
                zombieCollider = hit.collider;
                //and your not already shooting
                if (!isShooting)
                {
                    //shoot him
                    StartCoroutine("Shoot");
                }
            }
        }
	}

    void PlayerIsDead()
    {
        SceneManager.LoadScene(2);
    }
    void PlayerWon()
    {
        SceneManager.LoadScene(0);
    }

    //when zombie gets up to camera and starts to attack Player... called every physics update from FixedUpdate
    //atleast one object needs to have RB.
    void OnTriggerStay(Collider Col)
    {
        //set attacking to true
        Col.gameObject.GetComponent<ZombieScript>().z_isAttacking = true;
        //if your alive
        if (playerHealth > 0.0f)
        {
            //if its zombie and hes not dead
            if (Col.tag.Contains("zombie") && Col.gameObject.GetComponent<ZombieScript>().z_isDead == false && Col.gameObject.GetComponent<ZombieScript>().z_isAttacking == true)
            {
                //keep attacking
                zomb.z_isAttacking = true;
                //zomb.ZombieAttack();
                Col.gameObject.GetComponent<ZombieScript>().zAnim.Play("attack");
                Camera.main.GetComponent<playerScript>().playerHealth -= 0.2f;
                if (playerHealth <= 0.0f)
                {
                    PlayerIsDead();
                }
            }
        }
    }
}
