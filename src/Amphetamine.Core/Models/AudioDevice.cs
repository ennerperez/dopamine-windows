namespace Amphetamine.Core.Models
{
    public class AudioDevice
    {
        public string DeviceId { get; }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        public AudioDevice(string name, string deviceId)
        {
            Name = name;
            DeviceId = deviceId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return DeviceId.Equals(((AudioDevice)obj).DeviceId);
        }

        public override int GetHashCode()
        {
            return new { DeviceId }.GetHashCode();
        }
    }
}
