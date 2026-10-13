using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.ReorderableList;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rd;
    public float playerSpeed = 10;
    public Transform slashSpawn;
    //private float x;
    //private float y;

    private Vector2 input;
    private Vector2 inputA;

    public Animator animCont;
    public bool isAttacking = false;
    private bool alive = true;
    public int health = 3;
    private bool iFramesActive = false;

    InputAction moveAction;
    InputAction attackAction;

    public GameObject swordSlash;
    public GameObject playerSprite;

    private AudioGuy audioGuy;
    private bool audioDelay = true;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        //animCont = GetComponent<Animator>();
        animCont = GetComponentInChildren<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("DirectionalAttack");
        health = 3;
        audioGuy = GameObject.FindGameObjectWithTag("AudioGuy").GetComponent<AudioGuy>();
        audioDelay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            GetInput();
            AnimateGuy();
        } else {
            Die();
            AnimateGuy();
        }
    }
    private void FixedUpdate()
    {
        rd.linearVelocity = input * playerSpeed;
    }
    private void GetInput()
    {
        //x = Input.GetAxis("Horizontal");
        //y = Input.GetAxis("Vertical");
         
        input = moveAction.ReadValue<Vector2>();
        input.Normalize();

        if (attackAction.ReadValue<Vector2>().magnitude != 0.0f && !isAttacking)
        {
            IsAttacking();
        }
    }

    private void AnimateGuy()
    {
        if (input.magnitude > 0.1f || input.magnitude < -0.1f)
        {
            animCont.SetBool("isMoving", true);
            if (audioDelay) PlayPatter();
        } else {
            animCont.SetBool("isMoving", false);
        }

        if (input.x > 0.1f)
        {
            playerSprite.transform.localScale = new Vector3(.48f, .48f, .48f);
        } else {
            if (input.x < -0.1f) playerSprite.transform.localScale = new Vector3(-.48f, .48f, .48f);
        }

        animCont.SetInteger("health", health);

        

    }
    private async Task IsAttacking()
    {
        isAttacking = true;
        inputA = attackAction.ReadValue<Vector2>();
        //Debug.Log(inputA);

        if (inputA.x == -1) slashSpawn.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90); // right
        if (inputA.x == 1) slashSpawn.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90); // left
        if (inputA.y == -1) slashSpawn.gameObject.transform.rotation = Quaternion.Euler(0, 0, 180); // down

        GameObject spawnedSlash = Instantiate(swordSlash, slashSpawn);
        //spawnedSlash.GetComponent<Animator>().SetBool("isAttacking", true);
        //Debug.Log("hi");

        //await Awaitable.WaitForSecondsAsync(0.2f);
        await Awaitable.WaitForSecondsAsync(0.8f);
        slashSpawn.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        GameObject.Destroy(spawnedSlash);
        //spawnedSlash.GetComponent<Animator>().SetBool("isAttacking", false);
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Health"))
        {
            Destroy(collision.gameObject);
            health += 1;
        }
    }

    public async Task TakeDamage()
    {
        iFramesActive = true;
        health -= 1;
        if (health < 1)
        {
            alive = false;
        }
        await Awaitable.WaitForSecondsAsync(.5f);
        iFramesActive = false;
    }

    private void Die()
    {
        input = new Vector2(0, 0);
        inputA = new Vector2(0, 0);
    }

    private async Task PlayPatter()
    {
        audioDelay = false;
        audioGuy.PlayPitterPatter();
        await Awaitable.WaitForSecondsAsync(0.6f);
        audioDelay = true;
    }

}
