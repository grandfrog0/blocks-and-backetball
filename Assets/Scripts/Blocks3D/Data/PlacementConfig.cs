using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks3D
{
    [Serializable]
    public class PlacementConfig
    {
        public int Size = 2;
        public List<Vector2Int> Left = new();
        public List<Vector2Int> Up = new();
        public List<Vector2Int> Right = new();
        public List<Vector2Int> Down = new();

        public List<BlocksField> GetPlacements()
            => new()
            {
                new(){ placement = new() },
                new(){ placement = Left }, 
                new(){ placement = Up }, 
                new(){ placement = Right }, 
                new(){ placement = Down }
            };
    }
}