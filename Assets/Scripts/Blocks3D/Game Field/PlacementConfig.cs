using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks3D
{
    [CreateAssetMenu(fileName = "Placement", menuName = "SO/Blocks3D/Blocks Placement")]
    public class PlacementConfig : ScriptableObject
    {
        public int Size = 2;
        public List<BlocksField> Fields;
    }
}