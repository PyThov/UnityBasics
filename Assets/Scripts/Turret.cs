using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform turretBase;
    [SerializeField] private Transform gunBase;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpread, shootForce, timeBetweenShots, bulletLifetime;
    [SerializeField] private bool shooting = false;
    private List<Transform> entitiesInRange = new List<Transform>();

    private Transform getClosestEntity()
    {
        if (entitiesInRange == null || entitiesInRange.Count == 0)
        {
            return null;
        }

        Transform tempTransform = null;
        float closestDistance = 0;

        foreach(Transform t in entitiesInRange)
        {
            float distance = Vector3.Distance(t.position, turretBase.position);
            if (tempTransform == null || distance < closestDistance)
            {
                tempTransform = t;
            }
        }

        return tempTransform;
    }

    private void shoot()
    {
        if (target != null)
        {
            // Main direction
            Vector3 direction = target.position - gunBase.position;

            // Bullet Spread
            float xSpread = Random.Range(-bulletSpread, bulletSpread);
            float ySpread = Random.Range(-bulletSpread, bulletSpread);
            direction = direction + new Vector3(xSpread, ySpread, 0);

            // Create Bullet and shoot
            GameObject currentBullet = Instantiate(bullet, gunBase.position, Quaternion.identity);
            currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
            Destroy(currentBullet, bulletLifetime);
        }
        shooting = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || target.tag != "Enemy")
        {
            return;
        }

        Vector3 targetPosition = target.position;
        targetPosition.y = turretBase.position.y;
        turretBase.LookAt(targetPosition);
        gunBase.LookAt(target);

        if (!shooting)
        {
            shooting = true;
            Invoke("shoot", timeBetweenShots);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (target == null)
            {
                target = other.transform;
            }

            entitiesInRange.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            entitiesInRange.Remove(other.transform);
            target = getClosestEntity();
        }
    }
}
