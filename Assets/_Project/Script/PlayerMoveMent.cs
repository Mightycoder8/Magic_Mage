using System.Collections;
using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Transform Projectile_Spawner;
    [SerializeField] private GameObject Projectile_Prefab;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundmask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;

    private CharacterController characterController;
    private Animator anim;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(Attack());
        }


    }
    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundmask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float MoveZ = Input.GetAxis("Vertical");
        float MoveX = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(MoveX, 0, MoveZ);
        moveDirection = transform.TransformDirection(moveDirection);


        if (isGrounded)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
                

            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();

            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            moveDirection *= moveSpeed;
        }
        characterController.Move(moveDirection * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }
    void Rotatetowards()
    {
        Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
    }
    void Idle()
    {
        anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
       
    }
    void Walk()
    {
        moveSpeed = walkSpeed;
        Rotatetowards();
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
      
    }
    void Run()
    {
        moveSpeed = runSpeed;
        Rotatetowards();
        anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
       
    }
    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
    IEnumerator Attack()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack_Layer"), 1);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack_Layer"), 0);
        
    }

    void FireBallCreater()
    {
        GameObject FireBall = Instantiate(Projectile_Prefab, Projectile_Spawner);
        FireBall.transform.parent = null;
    }
}
