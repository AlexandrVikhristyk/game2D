using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [SerializeField] private int lives = 10;

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value < 5) lives = value;
            livesBar.Refresh();
        }
    }

    private LivesBar livesBar;
    
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float jumpForce = 15.0f;
    private bool isGrounded;
    private Bullet bullet;
    [SerializeField] private LayerMask layerMask;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set {animator.SetInteger("State", (int)value);}
    }

    private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake()
    {
        livesBar = FindObjectOfType<LivesBar>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded) State = CharState.Idle;
        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0f;

        if (isGrounded) State = CharState.Run;
    }

    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        Vector3 position = transform.position; position.y += 0.8F;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);
    }

    public override void ReceiveDamage()
    {
        Lives--;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 15.0f, ForceMode2D.Impulse);
    }
    
    private void CheckGround()
    {

        isGrounded = Physics2D.OverlapCircle(transform.position,0.3f, layerMask);

//        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);

//        isGrounded = colliders.Length > 1 && !colliders[1].gameObject.CompareTag("Bullet");

        if (!isGrounded)  State = CharState.Jump;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        
        if(bullet && bullet.Parent != gameObject) ReceiveDamage();
        if (Lives == 0)
        {
            Destroy(gameObject);
        }
    }
}

public enum CharState
{
    Idle, Run, Jump
} 
