using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [SerializeField] private float Health;
    
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
        
    }

    // Update is called once per frame
    void Update()
    {
        checkForDeath();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Health -= enemy.Health;
            enemy.Health = 0;
            checkForDeath();
        }
    }
}
