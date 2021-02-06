using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image barImage;
    private float currentHealth;

    public void findImageBar()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
    }

    public void changeHelathBarColor()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
        if (currentHealth <= .30f)
        {
            barImage.color = new Color32(218, 40, 21, 100);
        }
        else
        {
            barImage.color = new Color32(0, 152, 11, 255);
        }
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
        getCurrentHealth();
        barImage.fillAmount = getCurrentHealth() + healthValue;
    }

    public float getCurrentHealth()
    {
        return this.currentHealth;
    }


}
