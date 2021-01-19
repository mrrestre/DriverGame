using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image barImage;
    private float currentHealth;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void findImageBar()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
    }

    public void setHealthBarFull()
    {
        this.currentHealth = 1f;
        barImage.fillAmount = currentHealth;
    }

    public void setHealthBarDamage(float damageSize)
    {
        currentHealth -= damageSize;
        barImage.fillAmount = currentHealth;
    }

    public void setHealthBarValue(float healthValue)
    {
        barImage.fillAmount = healthValue;
    }

    public float getCurrentHealth()
    {
        return this.currentHealth;
    }



}
