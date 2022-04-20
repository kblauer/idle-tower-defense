using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnitFire : MonoBehaviour
{
    private GameObject unit;
    private bool firing = false;
    private GameObject enemyToShoot;
    private Animator anim;

    private float shotTime = 0;
    private List<GameObject> enemiesInRange = new List<GameObject>();

    [SerializeField] GameObject bulletType;
    [SerializeField] Vector3 bulletOffset;
    [SerializeField] float shotsPerSecond = 2f;
    

    private void Start() {
        unit = transform.parent.gameObject;
        anim = unit.GetComponent<Animator>();
    }

    private void Update() {
        // // if there is a bullet in motion, move it
        // if (bulletInMotion) {
        //     moveBullet();
        // }
        
        // if firing but no bullet in motion, fire again at current target
        if (firing) {
            if (Time.time - shotTime > 1/shotsPerSecond) {
                createBullet();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("trigger");
        
        if (other.gameObject.tag == "Enemy") {
            enemiesInRange.Add(other.gameObject);

            setFiring(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        //Debug.Log("enemy exited range");

        if (other.gameObject.tag == "Enemy") {
            enemiesInRange.Remove(other.gameObject);
            
            if (other.gameObject == enemyToShoot) {
                enemyToShoot = null;
            }

            if (enemiesInRange.Count == 0) {
                setFiring(false);
            }
        }
    }

    private void setFiring(bool enabled) {
        firing = enabled;
        anim.SetBool("firing", enabled);
    }

    private void createBullet() {
        // set state to firing, select enemy in range
        firing = true;
        enemyToShoot = getCurrentTarget();
        
        if (enemyToShoot != null) {
            // create projectile game object
            //Debug.Log("Creating bullet");
            Vector3 bulletPos = unit.transform.position + bulletOffset;
            GameObject firedBullet = Instantiate(bulletType, bulletPos, Quaternion.identity);

            // set target enemy for projectile in its object
            MoveBullet moveBullet = firedBullet.GetComponent<MoveBullet>();
            moveBullet.target = enemyToShoot;
            moveBullet.targetSet = true;

            shotTime = Time.time;
        }
    }

    private GameObject getCurrentTarget() {
        if (enemyToShoot != null) return enemyToShoot;
        else if (enemiesInRange[0] != null) {
            enemyToShoot = enemiesInRange[0];
            return enemiesInRange[0];
        }
        else return null;
    }
}
