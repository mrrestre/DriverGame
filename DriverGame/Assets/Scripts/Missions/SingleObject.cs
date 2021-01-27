using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObject : MonoBehaviour
{
    public bool objectTouched;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Car")
        {
            objectTouched = true;
        }
    }
}
