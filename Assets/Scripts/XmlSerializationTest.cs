using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlSerializationTest : MonoBehaviour
{
    private void Start()
    {
        TestData data = new TestData()
        {
            playerName = "PlayerBobka",
            points = new() { new Vector2Int(-1, 1), new Vector2Int(2, 2), new Vector2Int(0, 0) },
            time = Time.time
        };

        XmlSerializer serializer = new XmlSerializer(typeof(TestData));

        using (StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/data.xml"))
        {
            serializer.Serialize(writer, data);
        }

        using (StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/data.xml"))
        {
            TestData result = (TestData)serializer.Deserialize(reader);
            Debug.Log(result);
        }
    }

    [Serializable]
    public class TestData
    {
        public string playerName;
        public List<Vector2Int> points;
        public float time;

        public override string ToString()
            => $"playerName: {playerName}, points: [{string.Join(", ", points)}], time: {time}";
    }
}