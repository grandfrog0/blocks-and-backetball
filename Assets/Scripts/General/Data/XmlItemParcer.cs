using Blocks3D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlItemParser<T>
{
    private string CurrentFile { get; set; }
    private string CurrentPath => Path.Combine(Application.streamingAssetsPath, CurrentFile);

    public T Value;

    private XmlSerializer _serializer;

    public XmlItemParser(string currentFile, T value)
    {
        _serializer = new XmlSerializer(typeof(T));

        CurrentFile = currentFile;
        Value = value;
    }

    public void Load()
    {
        FileInfo file = new FileInfo(CurrentPath);

        if (!file.Exists)
        {
            Debug.Log("file " + CurrentPath + " does not exists!");
            return;
        }

        using (StreamReader reader = new StreamReader(CurrentPath))
        {
            Value = (T)_serializer.Deserialize(reader);
            Debug.Log(Value);
        }
    }


    public void Save()
    {
        using (StreamWriter writer = new StreamWriter(CurrentPath))
        {
            _serializer.Serialize(writer, Value);
        }
    }
}
