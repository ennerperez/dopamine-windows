namespace Amphetamine.Core.Enums
{
    internal enum SHFileOperationReturnCode : ulong
    {
        SUCCESSFUL = 0,
        ERROR_SHARING_VIOLATION = 32, // 0x0000000000000020
        DE_ERROR_MAX = 183, // 0x00000000000000B7
        ERRORONDEST = 65536, // 0x0000000000010000
    }
}