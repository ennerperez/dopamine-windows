using System;

namespace Amphetamine.Core.Settings
{
    public class SettingChangedEventArgs : EventArgs
    {
        public SettingEntry Entry { get; set; }
    }
}