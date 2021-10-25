using Dopamine.Core.Base;
using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Avalonia.Input;

namespace Dopamine.Core.Extensions
{
    public static class IDropInfoExtensions
    {
        public static bool IsDraggingFilesOrDirectories(this IDropInfo dropInfo)
        {
            DataObject dataObject = dropInfo.Data as DataObject;

            return dataObject != null; // && dataObject.GetDataPresent(DataFormats.FileDrop); TODO:
        }

        public static bool IsDraggingDirectories(this IDropInfo dropInfo)
        {
            DataObject dataObject = dropInfo.Data as DataObject;
            var directoryNames = dataObject.GetFileNames(); //GetFileDropList TODO:
            
            foreach (string directoryName in directoryNames)
            {
                if (System.IO.Directory.Exists(directoryName))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsDraggingMediaFiles(this IDropInfo dropInfo)
        {
            DataObject dataObject = dropInfo.Data as DataObject;
            var filenames = dataObject.GetFileNames(); //.GetFileDropList();TODO:
            string[] supportedExtensions = FileFormats.SupportedMediaExtensions;

            foreach (string filename in filenames)
            {
                if (supportedExtensions.Contains(System.IO.Path.GetExtension(filename.ToLower())))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsDraggingStaticPlaylistFiles(this IDropInfo dropInfo)
        {
            DataObject dataObject = dropInfo.Data as DataObject;
            var filenames = dataObject.GetFileNames(); //.GetFileDropList();TODO:
            string[] supportedExtensions = FileFormats.SupportedStaticPlaylistExtensions;

            foreach (string filename in filenames)
            {
                if (supportedExtensions.Contains(System.IO.Path.GetExtension(filename.ToLower())))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsDraggingSmartPlaylistFiles(this IDropInfo dropInfo)
        {
            DataObject dataObject = dropInfo.Data as DataObject;
            var filenames = dataObject.GetFileNames(); //.GetFileDropList();TODO:
            string[] supportedExtensions = FileFormats.SupportedSmartPlaylistExtensions;

            foreach (string filename in filenames)
            {
                if (supportedExtensions.Contains(System.IO.Path.GetExtension(filename.ToLower())))
                {
                    return true;
                }
            }

            return false;
        }

        public static IList<string> GetDroppedFilenames(this IDropInfo dropInfo)
        {
            DataObject dataObject = dropInfo.Data as DataObject;
            IList<string> filenames = dataObject.GetFileNames().Cast<string>().ToList(); //.GetFileDropList() TODO:

            return filenames;
        }
    }
}
