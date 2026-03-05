using UnityEngine;
using System;
using UnityEngine.Events;

public class ArgumentReceiver : MonoBehaviour
{
    public UnityEvent<string> OnConfigPathGot = new();
    private void Awake()
    {
        string[] args = Environment.GetCommandLineArgs();

        // args[0] — всегда путь к EXE-файлу.

        string configPath;

        if (args.Length > 1)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Debug.Log($"Аргумент {i}: {args[i]}");

                // Пример поиска конкретного ключа
                if (args[i] == "-data" && i + 1 < args.Length)
                {
                    configPath = args[i + 1];
                    Debug.Log("Config path: " + configPath);

                    OnConfigPathGot.Invoke(configPath);
                }
            }
        }
    }
}