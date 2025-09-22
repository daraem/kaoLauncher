﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class GlobalHotKey
{
    private static int currentId = 0;
    private int modifier;
    private int key;
    private IntPtr hWnd;
    private int id;

    public GlobalHotKey(int modifier, Keys key, Form form)
    {
        this.modifier = modifier;
        this.key = (int)key;
        this.hWnd = form.Handle;

        id = System.Threading.Interlocked.Increment(ref currentId);
    }

    public bool Register()
    {
        bool result = RegisterHotKey(hWnd, id, modifier, key);

        if (!result)
        {
            int error = Marshal.GetLastWin32Error();
            MessageBox.Show($"Error {error}", "",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning);
        }

        return result;
    }

    public bool Unregister()
    {
        return UnregisterHotKey(hWnd, id);
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
}

public static class Constants
{
    public const int NOMOD = 0x0000;
    public const int ALT = 0x0001;
    public const int CTRL = 0x0002;
    public const int SHIFT = 0x0004;
    public const int WIN = 0x0008;
    public const int WM_HOTKEY_MSG_ID = 0x0312;
}
