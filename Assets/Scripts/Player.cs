using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
public class Player : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    private float moveInput;
    private Rigidbody2D rb;
    public GameObject Body;
    private bool facingRight = true;

    private bool isGrounded;
    public Transform GroundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;

    Animator anim;


    public GameObject projectile;
    public Vector2 velocity;
    bool canShoot = true;
    public float posy = 0;

    public float cooldown;

    int flipValue;


    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Shoot();

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, whatIsGround);

      
        if (Input.GetAxis("Horizontal") == 0)
        {
            anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Run", true);
        }
        moveInput = Input.GetAxis("Horizontal");     
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (facingRight == false && moveInput > 0)
        {
            Flip();
            flipValue = 1;
        }
        else if (facingRight == true && moveInput<0)
        {
            Flip();
            flipValue = -1;
        }

    }

    void Shoot()
    {
        if (canShoot && Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("Shoot");
            GameObject go = (GameObject)
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + posy, 0f), Quaternion.identity);         
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(flipValue*velocity.x, 0f);                      
            StartCoroutine(CanShoot());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            anim.SetTrigger("Jump");
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }

        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded)
        {
            anim.SetTrigger("Jump");
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    void Flip()
    {

        facingRight = !facingRight;
        Vector3 Scaler = Body.transform.localScale;
        Scaler.x *= -1;
        Body.transform.localScale = Scaler;
    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

}