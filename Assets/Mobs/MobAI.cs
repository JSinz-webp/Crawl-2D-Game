using Pathfinding;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class MobAI : MonoBehaviour
{

    public Rigidbody2D rd;
    public float mobKnockback = 20;

    public Animator animCont;
    public bool isAttacking = false;

    public AIPath aiPath;
    public Vector2 direction;

    public int health;
    private int maxHealth = 2;

    private InputAction moveAction;

    private IAstarAI ai;

    public Transform player;
    //private float sightRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        animCont = GetComponentInChildren<Animator>();
        health = maxHealth;
        moveAction = InputSystem.actions.FindAction("Move");
        ai = GetComponent<IAstarAI>();
        ai.canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0) Die();
        AnimateGuy();

        if (RaycastPlayer()) ai.canMove = true;
    }

    private void FixedUpdate()
    {
        rd.linearVelocity = direction * mobKnockback;
    }

    private void AnimateGuy()
    {
        if (aiPath.desiredVelocity.x != 0f || aiPath.desiredVelocity.y != 0f)
        {
            animCont.SetBool("isMoving", true);
        }
        else
        {
            animCont.SetBool("isMoving", false);
        }
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hitbox entered slime");
        if (collision.gameObject.CompareTag("Sword"))
        {
            //Debug.Log("slime attacked");
            health -= 1;
            //await Damaged();
            Debug.Log("Damaged");

            //animCont.SetBool("damaged", false);
            animCont.SetBool("damaged", true);
            //animCont.SetBool("damaged", false);
            ai.canMove = false;
            Debug.Log("Should have stopped");
            
            Vector2 input = moveAction.ReadValue<Vector2>();
            direction.Set(input.x, input.y);

            await Awaitable.WaitForSecondsAsync(1f);
            if (health > 0)
            { 
                animCont.SetBool("damaged", false);
                ai.canMove = true;
                Debug.Log("Should be mobile again");
            }
            
            //ai.canMove = true;
        }
        //if (collision.gameObject.CompareTag("Player"))
        //{
             
        //}
    }
    private async Task Damaged()
    {
        Debug.Log("Damaged");
        animCont.SetBool("damaged", true);
        ai.canMove = false;
        Debug.Log("Should have stopped");
        await Awaitable.WaitForSecondsAsync(1f);
        animCont.SetBool("damaged", false);
        ai.canMove = true;
        Debug.Log("Should be mobile again");
    }

    private async Task Die()
    {
        await Awaitable.WaitForSecondsAsync(.3f);
        Destroy(gameObject);
    }
    private bool RaycastPlayer()
    {
        //float castDist = dist;

        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, player.transform.position - transform.position, 1 << LayerMask.NameToLayer("Raycastable"));

        if(hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
                return true;
            } else {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                return false;
            }
        }

        return false;
    }
}
