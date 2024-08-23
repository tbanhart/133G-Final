using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] public InteractableType interactableType;

    public void DoInteraction(PlayerController sender)
    {
        switch(interactableType)
        {
            case InteractableType.PICKUP:
                Destroy(this.gameObject);
            break;

            case InteractableType.TALK:
                //Debug.Log(this.GetComponent<NPC>().DialogueText);    
            break;

            case InteractableType.DELIVER:
                GetComponent<DeliveryTarget>().DeliveryCounter++;
            break;
        }
    }
}
