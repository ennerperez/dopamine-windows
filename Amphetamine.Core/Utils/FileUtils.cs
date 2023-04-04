using System;
using System.IO;
using System.Text.RegularExpressions;
using Amphetamine.Core.Enums;

namespace Amphetamine.Core.Utils
{
  public static class FileUtils
  {
    public static string DirectoryName(string path) => Path.GetDirectoryName(path);

    public static string FileName(string path) => Path.GetFileName(path);

    public static string FileNameWithoutExtension(string path) => Path.GetFileNameWithoutExtension(path);

    public static long SizeInBytes(string path) => new FileInfo(path).Length;

    public static DateTime DateCreated(string path) => new FileInfo(path).CreationTime;

    public static long DateCreatedTicks(string path) => new FileInfo(path).CreationTime.Ticks;

    public static DateTime DateModified(string path) => new FileInfo(path).LastWriteTime;

    public static long DateModifiedTicks(string path) => new FileInfo(path).LastWriteTime.Ticks;

    public static bool IsPathTooLong(string path) => path.Length >= 248;

    public static bool IsAbsolutePath(string path) => new Regex("^(([a-zA-Z]:\\\\)|(//)).*").Match(path).Success;

    public static string SanitizeFilename(string filename)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      return filename.Replace("\\", empty2).Replace("/", empty2).Replace(":", empty2).Replace("*", empty2).Replace("?", empty2).Replace("\"", empty2).Replace("<", empty2).Replace(">", empty2).Replace("|", empty2);
    }

    private static bool SendToRecycleBin(string path, FileOperationFlags flags)
    {
      SHFILEOPSTRUCT FileOp = new SHFILEOPSTRUCT()
      {
        wFunc = FileOperationType.FO_DELETE,
        pFrom = path + "\0\0",
        fFlags = FileOperationFlags.FOF_ALLOWUNDO | flags
      };
      //TODO:
      // SHFileOperationReturnCode operationReturnCode = (SHFileOperationReturnCode) NativeMethods.SHFileOperation(ref FileOp);
      // switch (operationReturnCode)
      // {
      //   case SHFileOperationReturnCode.SUCCESSFUL:
      //     return true;
      //   case SHFileOperationReturnCode.ERROR_SHARING_VIOLATION:
      //     throw new IOException("The process cannot access the file '" + Path.GetFullPath(path) + "' because it is being used by another process.");
      //   case SHFileOperationReturnCode.DE_ERROR_MAX:
      //     throw new IOException("The length of target path '" + Path.GetFullPath(path) + "' is over MAX_PATH");
      //   case SHFileOperationReturnCode.ERRORONDEST:
      //     throw new IOException("An unspecified error occurred on the destination.");
      //   default:
      //     throw new NotImplementedException("Not supported SHFileOperation return code: " + (object) operationReturnCode);
      // }
      return true;
    }

    public static bool SendToRecycleBinInteractive(string path) => FileUtils.SendToRecycleBin(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_WANTNUKEWARNING);

    public static bool SendToRecycleBinSilent(string path) => FileUtils.SendToRecycleBin(path, FileOperationFlags.FOF_SILENT | FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI);
  }
}