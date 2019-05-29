using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float enemyHealth;
    public float enemyMaxHealth;

    [Header("Enemy Health Bar")]
    public Slider enemyHealthBar;

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyHealthBar = gameObject.transform.GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyHealthBar.value = Mathf.Clamp01(enemyHealth / enemyMaxHealth);
    }
}
