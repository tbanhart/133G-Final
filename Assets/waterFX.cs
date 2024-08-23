using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterFX : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.transform.parent.GetComponent<AudioSource>().enabled = true;
            this.transform.parent.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.transform.parent.GetComponent<AudioSource>().enabled = false;
        }
    }
}
