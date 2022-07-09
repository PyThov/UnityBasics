using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float StartingHealth, Speed;
    [SerializeField] private GameObject Nexus;
    public float Health;
    private Rigidbody rb;
    private Vector3 direction;

    void checkForDeath()
    {
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Health = StartingHealth;

        // Main direction
        // direction = GetComponent<Rigidbody>() - gunBase.position;

        Debug.Log("Starting with " + StartingHealth.ToString() + " health");
        rb.velocity = new Vector3(direction.x * Speed, 0, direction.z * Speed);
    }

    // Update is called once per frame
    void Update()
    {
        rb.position = Vector3.MoveTowards(rb.position, Nexus.transform.position, Speed);
        checkForDeath();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            float damage = collision.relativeVelocity.magnitude * collision.gameObject.GetComponent<Bullet>().BaseDamage;
            Health -= damage;
            Debug.Log("Took " + damage.ToString() + " damage");
            Debug.Log("Current Health: " + Health.ToString());
            checkForDeath();
        }
    }
}
