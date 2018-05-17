using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{

    public GameObject[] enemies;
    public int amount;
    private Vector3 spawnPoint;
    public LayerMask mask = -1;
    UnityEngine.AI.NavMeshHit hit;
    float radius;
    void Start()
    {


    }
    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        amount = enemies.Length;

        if (amount < 10)
        {
            InvokeRepeating("spawnEnemy", 1f, 2f);
        }

    }

    void spawnEnemy()
    {
        Debug.Log("spawn called");
        spawnPoint.x = Random.Range(-1792.94f, -1892.94f);
        spawnPoint.y = Random.Range(150f, 110f);
        spawnPoint.z = Random.Range(686.2f, 786.2f);
        Vector3 Ybase = new Vector3(spawnPoint.x, 0.0f, spawnPoint.z);


        //find where the enemy should spawn on the y axis

        Ray YRay = new Ray(spawnPoint, Ybase);
        RaycastHit hitInfo;
        float m_GroundCheckDistance = 2.0f;
        RaycastHit[] hitTargets = Physics.RaycastAll(YRay, Mathf.Infinity);
        bool ishit = Physics.Raycast(spawnPoint, Ybase, out hitInfo, m_GroundCheckDistance);
        
        if (hitTargets.Length > 0)
        {
            for (int i = 0; i < hitTargets.Length; i++)
            {
                //

                UnityEngine.AI.NavMesh.SamplePosition(spawnPoint, out hit, Mathf.Infinity, 1);
                //
                Debug.Log("SpawnPoint: " + spawnPoint.y);
                spawnPoint.x = hit.position.x;
                spawnPoint.z = hit.position.z;

                Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length - 1)], spawnPoint, Quaternion.identity);
            }

            CancelInvoke();
        }

    }

    
}
