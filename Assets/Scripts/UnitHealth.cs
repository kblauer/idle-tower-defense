using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{

    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    [SerializeField] HealthSlider healthSlider;

    private void Start() {
        currentHealth = maxHealth;
        healthSlider.setHealthSlider(currentHealth, maxHealth);
    }

    public void TakeDamage(float amountTaken) {
        currentHealth -= amountTaken;
        //Debug.Log("damage taken! health = " + currentHealth);

        healthSlider.setHealthSlider(currentHealth, maxHealth);

        if (currentHealth <= 0) {
            killUnit();
        }
    }

    void killUnit() {
        //Debug.Log("killing unit");

        GameObject controllerObj = GameObject.Find("GameController");
        GameController controller = controllerObj.GetComponent<GameController>();
        controller.EnemyKilled();

        Destroy(gameObject);
    }
}
