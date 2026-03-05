using System;
using System.Runtime.InteropServices;
using UnityEngine;
using MainStore;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class FilePicker : MonoBehaviour
{
    // Settings for build
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenFileName
    {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public string filter = null;
        public string customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public string file = null;
        public int maxFile = 0;
        public string fileTitle = null;
        public int maxFileTitle = 0;
        public string initialDir = null;
        public string title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public string defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public string templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }

    [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

    // Main method
    public void SelectExeFile()
    {
        string selectedPath = "";

#if UNITY_EDITOR
        // In editor
        selectedPath = EditorUtility.OpenFilePanel("Выберите EXE файл", "", "exe");
#else
        // In build
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "Execute Files (*.exe)\0*.exe\0All Files (*.*)\0*.*\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.initialDir = Application.streamingAssetsPath;
        ofn.title = "Выберите программу";
        ofn.flags = 0x00000008 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000002;

        if (GetOpenFileName(ofn))
        {
            selectedPath = ofn.file;
        }
#endif

        if (!string.IsNullOrEmpty(selectedPath))
        {
            Debug.Log($"Успешно выбрано: {selectedPath}");
            GlobalManager.Instance.AddMinigame(selectedPath);
        }
        else
        {
            Debug.Log("Выбор отменен");
        }
    }
}