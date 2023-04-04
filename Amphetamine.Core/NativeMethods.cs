using System.Runtime.InteropServices;

namespace Amphetamine.Core
{
    internal static class NativeMethods
    {
        // [DllImport("user32.dll")]
        // public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //
        // [DllImport("user32.dll", SetLastError = true)]
        // public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        //
        // [DllImport("kernel32.dll")]
        // public static extern void SetLastError(int dwErrorCode);
        //
        // [DllImport("user32.dll")]
        // internal static extern int SetWindowCompositionAttribute(
        //     IntPtr hwnd,
        //     ref WindowCompositionAttributeData data);
        //
        // [DllImport("shell32.dll", SetLastError = true)]
        // public static extern IntPtr SHAppBarMessage(ABM dwMessage, [In] ref APPBARDATA pData);
        //
        // [DllImport("user32.dll", SetLastError = true)]
        // public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //
        // [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        // public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        //
        // [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        // public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        //
        // public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong) => IntPtr.Size == 4 ? NativeMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong) : NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        //
        // [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        // public static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);
    }
}