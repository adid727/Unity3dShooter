using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackAreaScript : MonoBehaviour
{

    public enemyAI AIScript;
    public Animator animator;
    public GameObject Player;

    public playerController playerScript;

    bool attackStarted = false;

    const float ENEMY_DMG = 4f;
    const float ENEMY_ATTACK_SPEED = 1.5f;

    //public float totalHealth = 100f;
    //public float currentHealth = 100f;

    public Image HealthBarFill;

    //Vector3 Range = new Vector3(4, 4, 4);
    //Vector3 playerPosition;
    //Vector3 EnemyPosition;
    //Vector3 Distance;

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AIScript.SwitchState(enemyAI.AISTATE.Attack);

            //InvokeRepeating("AttackProcess", 0, 1);
            StartCoroutine("AttackProcs");
            animator.SetBool("LeftTrigger", true);

            //AttackProcess();
        }
    }

    void OnTriggerStay(Collider collider)
    {
        
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) ;
        {
            if (AIScript.currAIState == enemyAI.AISTATE.Attack)
            {
                if (!attackStarted)
                {
                    attackStarted = true;
                }
            }
        }
    }

    //void AttackProcess()
    //{
    //    playerPosition = Player.transform.position;
    //    EnemyPosition = transform.position;
    //    Distance.x = Mathf.Abs(playerPosition.x - EnemyPosition.x);
    //    Distance.y = Mathf.Abs(playerPosition.y - EnemyPosition.y);
    //    Distance.z = Mathf.Abs(playerPosition.z - EnemyPosition.z);
    //    Debug.Log("Distance " + Distance);
    //    if (Distance.x < Range.x && Distance.y < Range.y && Distance.z < Range.z)
    //    {
    //        playerScript.CurrentHealth -= ENEMY_DMG;
    //        playerScript.CurrentHealth = Mathf.Clamp(playerScript.CurrentHealth, 0, playerScript.TotalHealth);
    //        HealthBarFill.fillAmount = playerScript.CurrentHealth / playerScript.TotalHealth;
    //    }
    //}

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetBool("LeftTrigger", false);
            AIScript.SwitchState(enemyAI.AISTATE.Seek);
        }
    }

    IEnumerator AttackProcs()
    {
        
        playerScript.CurrentHealth -= ENEMY_DMG;

        Debug.Log("!!!DMG!!!");

        yield return new WaitForSeconds(ENEMY_ATTACK_SPEED);
        playerScript.CurrentHealth -= ENEMY_DMG;
        playerScript.CurrentHealth = Mathf.Clamp(playerScript.CurrentHealth, 0, playerScript.TotalHealth);
        HealthBarFill.fillAmount = playerScript.CurrentHealth / playerScript.TotalHealth;

        attackStarted = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
