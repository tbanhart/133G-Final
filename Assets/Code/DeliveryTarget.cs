using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTarget : MonoBehaviour
{
    [SerializeField] public ItemType DeliveryItem;
    [SerializeField] public int DeliveryCounter;

    public virtual void DoDelivery() { }
}
