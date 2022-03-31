using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(Item))]
    public class Leukocyte : Friendly
    {

        Item item;

        void Start()
        {
            item = GetComponent<Item>();
        }

        protected override void DestroyObject()
        {
            item.RandomItem();
            base.DestroyObject();
        }

    }


}
