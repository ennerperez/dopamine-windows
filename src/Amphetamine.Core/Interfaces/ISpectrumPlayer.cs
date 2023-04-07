using System.ComponentModel;
// ReSharper disable InconsistentNaming

namespace Amphetamine.Core.Interfaces
{
	public interface ISpectrumPlayer : INotifyPropertyChanged
	{
		bool IsPlaying { get; }
		bool GetFFTData(ref float[] fftDataBuffer);
		int GetFFTFrequencyIndex(int frequency);
	}
}
