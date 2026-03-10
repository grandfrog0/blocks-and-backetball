using Blocks3D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ApplicationDataParser<T> : Parser<T>
{
    protected string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlocksAndBasketball");
    protected override string CurrentPath => Path.Combine(DataPath, CurrentFolder);

    public ApplicationDataParser(string currentFolder, List<T> values) : base(currentFolder, values)
    {
    }

    public override void Save()
    {
        if (!Directory.Exists(DataPath))
        {
            Directory.CreateDirectory(DataPath);
            Debug.Log("directory " + CurrentPath + " does not exists! Created.");
        }
        base.Save();
    }
}
