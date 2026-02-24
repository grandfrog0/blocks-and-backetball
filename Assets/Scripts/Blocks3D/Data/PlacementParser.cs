using Blocks3D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlacementParser : MonoBehaviour
{
    private static string CurrentFolder => Path.Combine("Blocks3D", "Placements");

    [SerializeField] BlocksPlacement blocksPlacement;
    private Parser<PlacementConfig> _parser;

    public void Load()
    {
        _parser = new Parser<PlacementConfig>(CurrentFolder, blocksPlacement.Configs);
        _parser.Load();
    }

    public void Save()
    {
        _parser.Save();
    }

    private void Awake()
    {
        Load();
    }
}
