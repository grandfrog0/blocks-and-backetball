using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks3D
{
    [Serializable]
    public class Blocks3DShopConfig
    {
        public List<(float Coins, float Cost)> Price;
    }
}