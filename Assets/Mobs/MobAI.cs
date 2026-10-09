using UnityEngine;
using UnityEngine.Windows;

public class MobAI : MonoBehaviour
{

    public Rigidbody2D rd;
    public float mobSpeed = 6;

    public Animator animCont;
    public bool isAttacking = false;

    public Vector2 direction;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        animCont = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateGuy();
    }

    private void FixedUpdate()
    {
        rd.linearVelocity = direction * mobSpeed;
    }

    private void AnimateGuy()
    {
        if (direction.magnitude > 0.1f || direction.magnitude < -0.1f)
        {
            animCont.SetBool("isMoving", true);
        }
        else
        {
            animCont.SetBool("isMoving", false);
        }
    }
}
