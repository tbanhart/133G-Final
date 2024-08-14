using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

class DeliveryTargetSteppingStones : DeliveryTarget
{
    [SerializeField] int numberRequired;
    [SerializeField] GameObject bridgePrefab;
    [SerializeField] Transform bridgeLocation;
    [SerializeField] GameObject creekBoundries;
    [SerializeField] GameObject rockPile;

    override public void DoDelivery()
    {
        if (DeliveryCounter < numberRequired) return;
        Destroy(rockPile);

        // Remove boundry boxes
        Destroy(creekBoundries);

        // Create the rock bridge
        var obj = Instantiate(bridgePrefab);
        obj.transform.position = bridgeLocation.position;
    }
}
