using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    Vector3 Velocity = Vector3.zero;
    public Camera playerCamera;
    public float speed = 1f;
    public float horizTurnSpeed = 3f;
    public float VerticalTurnSpeed = 3f;
    public float m_GroundCheckDistance = 2f;
    public Rigidbody rigidbody;
    public Vector3 m_GroundNormal;
    public bool m_IsGrounded;
    public float minimunX = -70.0f;
    public float maximunX = 70.0f;

    public float rotationX = 0f;
    
    public float vertiTurnSpeed = 2f;

    public delegate void MouseClick();
    public static event MouseClick OnMouseClick;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = gameObject.transform.position;

        // left and right up and down

        Velocity = playerCamera.transform.forward * Input.GetAxis("Forward") * speed;

        Velocity += playerCamera.transform.right * Input.GetAxis("Horizontal") * speed;

        CheckGroundStatus();

        Velocity = Vector3.ProjectOnPlane(Velocity, m_GroundNormal);

        if (!m_IsGrounded)
            rigidbody.AddForce(Vector3.down* 10, ForceMode.Impulse);

        rigidbody.velocity = Velocity;
        

        // look around using mouse
            ApplyPlayerRotation();
            ApplyCameraRotation();

        if (Input.GetMouseButtonDown(0))
        {
            if(OnMouseClick != null)
            {
                OnMouseClick();
            }
        }
        CheckGroundStatus();
    }

    // look around using mouse

    void ApplyPlayerRotation()
    {
        gameObject.transform.Rotate(0, Input.GetAxis("Mouse X") * horizTurnSpeed, 0);
    }

    //void ApplyCameraRotation()
    //{
    //    playerCamera.transform.Rotate(Input.GetAxis("Mouse Y") * VerticalTurnSpeed, 0, 0);

    //}
    void ApplyCameraRotation()
    {
        rotationX -= Input.GetAxis("Mouse Y") * vertiTurnSpeed;
        rotationX = Mathf.Clamp(rotationX, minimunX, maximunX);

        playerCamera.transform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }

    void CheckGroundStatus()
    {
#if UNITY_EDITOR
        //helps to visualize the ground ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.01f), transform.position + (Vector3.down * 0.1f) + (Vector3.down * m_GroundCheckDistance), Color.red);
#endif
        RaycastHit hitInfo;

        bool ishit = Physics.Raycast(transform.position + (Vector3.up * 0.01f), Vector3.down, out hitInfo, m_GroundCheckDistance);
        if (ishit)
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        }
        else
        {
            m_GroundNormal = Vector3.up;
            m_IsGrounded = false;
        }
    }
}