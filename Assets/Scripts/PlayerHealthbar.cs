using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    public Image healthBar;
    [SerializeField] PlayerController player;
    float health, maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        health = player.currentHealth;
        maxHealth = player.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = player.currentHealth;
        maxHealth = player.maxHealth;
        HealthBarFiller();
        ChangeBarColor();
    }

    public void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, 1);
    }

    public void ChangeBarColor()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        healthBar.color = healthColor;
    }
}
