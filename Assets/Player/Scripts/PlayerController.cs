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

    InputAction moveAction;
    InputAction attackAction;

    public GameObject swordSlash;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        //animCont = GetComponent<Animator>();
        animCont = GetComponentInChildren<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("DirectionalAttack");
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        AnimateGuy();
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
        }
        else
        {
            animCont.SetBool("isMoving", false);
        }

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
        Debug.Log("hi");

        //await Awaitable.WaitForSecondsAsync(0.2f);
        await Awaitable.WaitForSecondsAsync(0.8f);
        slashSpawn.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        GameObject.Destroy(spawnedSlash);
        //spawnedSlash.GetComponent<Animator>().SetBool("isAttacking", false);
        isAttacking = false;
    }

}
