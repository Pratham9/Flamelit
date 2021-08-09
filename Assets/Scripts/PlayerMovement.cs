using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField]  private float jumpForce = 100f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private float disToGround = 1f;
    [SerializeField] private float maxEnergy = 20f;
    [SerializeField] private float energyGen = 1f;
    private float currentEnergy;


    public EnergyBar energyBar;

    float horizontalMovement = 0f;
    Rigidbody rb;
    private bool jump;
    private bool fire1;
    private bool isGrounded = false;

    private Vector3 m_Velocity = Vector3.zero;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentEnergy = maxEnergy;
        energyBar.SetMaxEnergy(maxEnergy);
    }
    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * speed;
        
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            fire1 = true;
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalMovement * Time.fixedDeltaTime, jump);
        jump = false;
        EnergyGen(fire1, energyGen * Time.fixedDeltaTime);
        fire1 = false;

        
    }

    public void Move(float move, bool jump)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (isGrounded && jump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }
    void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, disToGround + 0.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void EnergyGen(bool firing, float energyGen)
    {
        if (fire1)
        {
            currentEnergy -= 20;
            energyBar.SetEnergy(currentEnergy);
            Debug.Log("fire");
        }
        currentEnergy += energyGen;
        energyBar.SetEnergy(currentEnergy);

    }
}
