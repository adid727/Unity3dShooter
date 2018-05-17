using UnityEngine;
using System.Collections;

public class DetectionSphereScript : MonoBehaviour
{

    public enemyAI AIScript;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AIScript.SwitchState(enemyAI.AISTATE.Seek);
            Debug.Log("SWITCH TO SEEK");
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AIScript.SwitchState(enemyAI.AISTATE.Wander);
            Debug.Log("SWITCH TO WANDER");
        }
    }
}
