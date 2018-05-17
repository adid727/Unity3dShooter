using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    public AudioClip shootFile;
    public AudioSource shootSource;

    public Animator playerAnimator;
    public Animator enemyAnimator;
    public Camera playerCam;
    public float punchForce = 200000f;
    public enemyAI enemyScript;
    enemyAI enemy;

    public float TotalHealth = 250f;
    public float CurrentHealth = 250f;

    float overrideAmount = 0;

    public GameObject Rifle;
    public GameObject Target;
    public float RifleDamage = 100.0f;
    
    

    // Use this for initialization
    void Awake()
    {
        shootSource = GetComponent<AudioSource>();
    }
    void Start()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        ////
        //Debug.Log("HIT");

        //
        ContactPoint[] contacts = col.contacts;
        if (contacts[0].thisCollider.gameObject.layer == LayerMask.NameToLayer("Fist")
            && contacts[0].otherCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Debug.Log("HIT");
            enemyAI enemy = contacts[0].otherCollider.gameObject.GetComponent<enemyAI>();
            enemy.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * punchForce, ForceMode.Impulse);
        }

        if((contacts[0].otherCollider.gameObject.layer == LayerMask.NameToLayer("Boat"))){
            UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
        }

    }
    void Shoot()
    {
        shootSource.PlayOneShot(shootFile, 1.2f);
        Ray ray = new Ray(Target.transform.position, Target.transform.forward);
        RaycastHit[] hitTargets = Physics.RaycastAll(ray, 15f);
        
        //bool IsHit = Physics.Raycast(ray, out rayCastHit, Mathf.Infinity);

        Debug.DrawLine(ray.origin, ray.direction, Color.red, 10);

        if (hitTargets.Length > 0)
        {
            for (int i = 0; i < hitTargets.Length; i++)
                // GameObject enemy = rayCastHit.collider.gameObject;
                if (hitTargets[i].collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {

                    enemyAI enemy = hitTargets[i].collider.gameObject.GetComponent<enemyAI>();
                    enemy.EnemyHit(RifleDamage);
                    
                }
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //  playerAnimator.SetTrigger("LeftTrigger");
            Shoot();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            playerAnimator.SetTrigger("LeftTrigger");
            Rifle.SetActive(false);
        }

        if (Input.GetKey(KeyCode.F))
        {
            overrideAmount += 0.1f * Time.deltaTime;
        }
        else
        {
            overrideAmount -= 0.1f * Time.deltaTime;
        }

        overrideAmount = Mathf.Clamp(overrideAmount, 0, 1);
        if (enemyAnimator)
        {
            enemyAnimator.SetLayerWeight(1, overrideAmount);
        }

    }
}