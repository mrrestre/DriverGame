using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Image barImage;
    private float maxHealth = 1f;
    private float currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();

        currentHealth = maxHealth;

        setHealthBarFull(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHealthBarFull(float maxHealth)
    {
        barImage.fillAmount = maxHealth;
    }

    public void setHealthBar(float damageSize)
    {
        currentHealth -= damageSize;
        barImage.fillAmount = currentHealth;
    }


    
}
