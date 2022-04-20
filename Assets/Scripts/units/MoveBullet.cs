using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public GameObject target;
    public bool targetSet = false;
    private Vector3 currentMove;

    [SerializeField] float bulletDamage = 10f;
    [SerializeField] float bulletSpeed = 8f;

    // Update is called once per frame
    void Update()
    {
        if (target != null && targetSet) {
            moveBullet();
        } if (target == null && targetSet) {
            Destroy(gameObject);
        }
    }

    private void moveBullet() {
        // move the bullet towards its intended target
        currentMove = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * bulletSpeed);
        transform.position = currentMove;

        // check if bullet has reached its destination
        if (Vector3.Distance(transform.position, target.transform.position) < .1f) {

            // deal damage to the enemy
            target.GetComponent<UnitHealth>().TakeDamage(bulletDamage);

            //Debug.Log("Enemy Hit!");
            Destroy(gameObject);
        }
    }
}
