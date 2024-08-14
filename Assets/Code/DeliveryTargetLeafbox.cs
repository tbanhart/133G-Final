using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

namespace Assets.Code
{
    class DeliveryTargetLeafbox : DeliveryTarget
    {
        [SerializeField] GameObject barrier;
        [SerializeField] int requiredLeaves;

        override public void DoDelivery()
        {
            if(DeliveryCounter >= requiredLeaves) 
            {
                Destroy(barrier);
            }
        }
    }
}
