using CommunityToolkit.Mvvm.ComponentModel;

namespace Amphetamine.Services.Models
{
	public class MetadataArtworkValue : ObservableObject
	{
		private byte[] value;
		private bool isValueChanged;

		public bool IsValueChanged
		{
			get { return this.isValueChanged; }
		}

		public byte[] Value
		{
			get { return this.value; }
			set
			{
				this.value = value;
				this.isValueChanged = true;
				this.OnPropertiesChanged();
			}
		}

		public MetadataArtworkValue()
		{
		}

		public MetadataArtworkValue(byte[] value)
		{
			this.value = value;
			this.OnPropertiesChanged();
		}

		private void OnPropertiesChanged()
		{
			OnPropertyChanged(nameof(this.Value));
			OnPropertyChanged(nameof(this.IsValueChanged));
		}
	}
}
