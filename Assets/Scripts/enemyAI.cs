using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class enemyAI : MonoBehaviour
{
    public Rigidbody rigidBody;

   

    public float TotalHealth = 100f;
    public float CurrentHealth = 100f;

    public Image healthBarFill;

    const float WANDER_SPEED = 2.5f;
    const float SEEK_SPEED = 5;

    public GameObject player;
    public UnityEngine.AI.NavMeshAgent agent;

    public GameObject model;

    public GameObject WanderTarget;
    public GameObject WanderCenter;
    float wanderAngleTo;
    float turnSpeed = 30;

    public DetectionSphereScript detectionScript;
    public AttackAreaScript attackScript;

    Coroutine JitterCoroutine;
    bool createJitter = false;

    public bool IsAlive = true;
    public bool IsGrounded;
    public int EnemiesKilled;

    public AISTATE currAIState;

    void Awake()
    {
        //if (IsGrounded)
        {
            currAIState = AISTATE.Wander;
            EnemiesKilled = 0;
        }
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        IsGrounded = true;
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        IsGrounded = false;
    }

    public enum AISTATE
    {
        Attack,
        Wander,
        Seek,
        Death
    }

    public bool GetisAlive()
    {
        return IsAlive;
    }

    // Use this for initialization
    void Start()
    {
        //Death();
    }

    public void EnemyHit(float damage)
    {
        CurrentHealth -= damage;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, TotalHealth);

        healthBarFill.fillAmount = CurrentHealth / TotalHealth;

        if (CurrentHealth <= 0)
            SwitchState(AISTATE.Death);
    }

    public void SwitchState(AISTATE nextState)
    {
        if (IsAlive /*&& IsGrounded*/)
        {
            switch (nextState)
            {
                case AISTATE.Wander:
                    agent.enabled = false;
                    break;

                case AISTATE.Seek:
                    agent.enabled = true;
                    agent.Resume();
                    break;

                case AISTATE.Attack:
                    agent.enabled = true;
                    agent.Resume();
                    break;

                case AISTATE.Death:
                    agent.enabled = false;
                    break;
            }
        }
        currAIState = nextState;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAlive/* && IsGrounded*/)
        {
            switch (currAIState)
            {
                case AISTATE.Wander:
                    Wander();
                    break;

                case AISTATE.Seek:
                    Seek();
                    break;

                case AISTATE.Attack:
                    Attack();
                    break;

                case AISTATE.Death:
                    if (IsAlive)
                        Death();
                    break;
            }
        }
    }

    void Death()
    {
        IsAlive = false;
        rigidBody.constraints = RigidbodyConstraints.None;

        detectionScript.gameObject.SetActive(false);
        attackScript.gameObject.SetActive(false);
        GetComponentInChildren<Animator>().SetBool("IsDead", true);
        EnemiesKilled++;
        //rigidBody.AddTorque(new Vector3(1,0,1), ForceMode.Impulse);
        // rigidBody.AddForce(Vector3.up*100, ForceMode.Impulse);
        //rigidBody.AddExplosionForce(2100, transform.position, 40);       
        StartCoroutine(RemoveEnemy());
    }

    void FaceAngle()
    {
        Quaternion rot = model.transform.localRotation;
        model.transform.localRotation = Quaternion.Slerp(rot, Quaternion.identity, 0.2f);
    }

    IEnumerator RemoveEnemy()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    void Seek()
    {
        if (agent.enabled /*&& IsGrounded*/)
            agent.SetDestination(player.transform.position);
        FaceAngle();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (currAIState == AISTATE.Wander /*&& IsGrounded*/)
        {
            if (JitterCoroutine != null)
                StopCoroutine(JitterCoroutine);

            createJitter = true;
            ImmediateJitter(90);
        }
    }

    void OnTriggerStay(Collider collider)
    {

        // Debug.Log("Collider: " + collider.gameObject.name);
    }

    void ImmediateJitter(float angle)
    {
        wanderAngleTo += (Random.Range(0, 2) == 0) ? angle : -angle;
        createJitter = false;
    }

    void Attack()
    {
        if (agent.enabled/* && IsGrounded*/)
            agent.SetDestination(player.transform.position);

        FaceAngle();
    }



    void Wander()
    {
        if (!createJitter/* && IsGrounded*/)
        {
            JitterCoroutine = StartCoroutine(Jitter(5));
            createJitter = true;
        }

        Vector3 dir = WanderTarget.transform.position - WanderCenter.transform.position;

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        

        float deltaAngle = Mathf.DeltaAngle(wanderAngleTo, angle);

        if (Mathf.Abs(deltaAngle) < turnSpeed * Time.deltaTime)
        {
        }
        else
        {
            if (deltaAngle < 0)
            {
                WanderTarget.transform.RotateAround(WanderCenter.transform.position,
                    Vector3.up, -turnSpeed * Time.deltaTime);
            }
            else
            {
                WanderTarget.transform.RotateAround(WanderCenter.transform.position,
                    Vector3.up, turnSpeed * Time.deltaTime);
            }
        }


        Vector3 dirToTarget = WanderTarget.transform.position - model.transform.position;

        Vector3 dirWithoutY = new Vector3(dirToTarget.x, 0, dirToTarget.z);

        dirWithoutY.Normalize();

        Vector3 velocity = new Vector3(dirWithoutY.x * WANDER_SPEED, rigidBody.velocity.y, dirWithoutY.z * WANDER_SPEED);

        rigidBody.velocity = velocity;

        //agent.SetDestination(WanderTarget.transform.position);


        //FACE THE TARGET
        Quaternion rotationToFace = Quaternion.LookRotation(dirWithoutY, Vector3.up);
        model.transform.rotation = rotationToFace;


    }

    /*void ImmediateJitter(float customAngle)
    {
        wanderAngleTo +=  (Random.Range(0, 2) == 0) ? customAngle : -customAngle;
        
        createJitter = false;
    }*/

    IEnumerator Jitter(float time)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(time);

        yield return waitForSeconds;

        wanderAngleTo -= Random.Range(-10, 11) * 10;
        
        createJitter = false;
    }
}
