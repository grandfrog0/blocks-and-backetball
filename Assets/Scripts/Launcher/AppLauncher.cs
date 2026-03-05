using UnityEngine;
using System.Diagnostics;
using System.IO;

public class AppLauncher : MonoBehaviour
{
    public void LaunchExternalApp(string exePath, string dataToSend)
    {
        if (!File.Exists(exePath))
        {
            UnityEngine.Debug.LogError("Файл не найден по пути: " + exePath);
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = exePath;

        // data is config path
        startInfo.Arguments = $"-data \"{dataToSend}\"";

        try
        {
            Process.Start(startInfo);
            UnityEngine.Debug.Log("Программа запущена с аргументами: " + startInfo.Arguments);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Ошибка при запуске: " + e.Message);
        }
    }
}