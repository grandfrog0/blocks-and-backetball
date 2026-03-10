using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;

public static class WinIconHelper
{
    [StructLayout(LayoutKind.Sequential)]
    struct ICONINFO
    {
        public int fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct BITMAP
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public ushort bmPlanes;
        public ushort bmBitsPixel;
        public IntPtr bmBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("gdi32.dll")]
    static extern int GetObject(IntPtr hgdiobj, int cbBuffer, out BITMAP lpvObject);

    [DllImport("gdi32.dll")]
    static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    static extern int GetDIBits(IntPtr hdc, IntPtr hbmp, uint uStartScan, uint cScanLines, [Out] byte[] lpvBits, ref BITMAPINFOHEADER lpbmi, uint uUsage);

    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    public static Texture2D GetIcon(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;

        path = path.Replace("/", "\\");
        IntPtr[] largeIcons = new IntPtr[1];
        uint readIcons = ExtractIconEx(path, 0, largeIcons, null, 1);

        if (readIcons == 0 || largeIcons[0] == IntPtr.Zero) return null;

        IntPtr hIcon = largeIcons[0];
        Texture2D texture = null;

        try
        {
            if (GetIconInfo(hIcon, out ICONINFO iconInfo))
            {
                IntPtr hBitmap = (iconInfo.hbmColor != IntPtr.Zero) ? iconInfo.hbmColor : iconInfo.hbmMask;

                BITMAP bmp;
                GetObject(hBitmap, Marshal.SizeOf(typeof(BITMAP)), out bmp);

                int width = bmp.bmWidth;
                int height = (iconInfo.hbmColor != IntPtr.Zero) ? bmp.bmHeight : bmp.bmHeight / 2;

                texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                byte[] pixels = new byte[width * height * 4];

                BITMAPINFOHEADER bmi = new BITMAPINFOHEADER();
                bmi.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
                bmi.biWidth = width;
                bmi.biHeight = height;
                bmi.biPlanes = 1;
                bmi.biBitCount = 32;
                bmi.biCompression = 0;

                IntPtr hdc = GetDC(IntPtr.Zero);
                GetDIBits(hdc, hBitmap, 0, (uint)height, pixels, ref bmi, 0);
                ReleaseDC(IntPtr.Zero, hdc);

                bool hasAlpha = false;

                for (int i = 0; i < pixels.Length; i += 4)
                {
                    if (pixels[i + 3] != 0) hasAlpha = true;

                    // BGRA -> RGBA
                    byte b = pixels[i];
                    pixels[i] = pixels[i + 2];
                    pixels[i + 2] = b;
                }

                if (!hasAlpha)
                {
                    for (int i = 3; i < pixels.Length; i += 4)
                    {
                        pixels[i] = 255;
                    }
                }

                texture.LoadRawTextureData(pixels);
                texture.Apply();

                if (iconInfo.hbmColor != IntPtr.Zero) DeleteObject(iconInfo.hbmColor);
                if (iconInfo.hbmMask != IntPtr.Zero) DeleteObject(iconInfo.hbmMask);
            }
        }
        finally
        {
            DestroyIcon(hIcon);
        }

        return texture;
    }
}