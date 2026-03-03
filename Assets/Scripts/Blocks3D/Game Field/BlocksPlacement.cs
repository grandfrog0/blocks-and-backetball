using System.Collections.Generic;
using UnityEngine;

namespace Blocks3D
{
    [CreateAssetMenu(fileName = "Placement", menuName = "SO/Blocks3D/Blocks Placement")]
    public class BlocksPlacement : ScriptableObject
    {
        public List<PlacementConfig> Configs = new();

        public PlacementConfig GetRandomConfig()
            => Configs[Random.Range(0, Configs.Count)];
    }
}