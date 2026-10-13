using UnityEditor.Build;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private PlayerController parent; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            parent.TakeDamage();
        }
    }
}
