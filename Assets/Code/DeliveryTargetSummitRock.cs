using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DeliveryTargetSummitRock : DeliveryTarget
{
    [SerializeField] GameObject flagPrefab;

    override public void DoDelivery()
    {
        var obj = Instantiate(flagPrefab);
        obj.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
}
