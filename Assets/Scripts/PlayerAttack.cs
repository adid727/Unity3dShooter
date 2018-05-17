using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{

    public SphereCollider RightHandCollider;
    public SphereCollider LeftHandCollider;

    public GameObject Rifle;

    // Use this for initialization
    void Start()
    {

        RightHandCollider.enabled = false;
        LeftHandCollider.enabled = false;

    }

    public void SetRightHandCollider(int active)
    {
        RightHandCollider.enabled = (active == 0) ? false : true;
    }

    public void SetLeftHandCollider(int active)
    {
        LeftHandCollider.enabled = (active == 0) ? false : true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowRifle()
    {
        Rifle.SetActive(true);
    }
}