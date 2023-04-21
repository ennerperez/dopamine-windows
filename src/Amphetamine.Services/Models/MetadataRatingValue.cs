using CommunityToolkit.Mvvm.ComponentModel;

namespace Amphetamine.Services.Models
{
	public class MetadataRatingValue : ObservableObject
	{
		private int value;
		private bool isValueChanged;


		public bool IsValueChanged
		{
			get { return this.isValueChanged; }
		}

		public int Value
		{
			get { return this.value; }

			set
			{
				this.value = value;
				this.isValueChanged = true;
				this.OnPropertiesChanged();
			}
		}
		public MetadataRatingValue()
		{
		}

		public MetadataRatingValue(int value)
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
