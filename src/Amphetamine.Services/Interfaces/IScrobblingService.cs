using System;
using System.Threading.Tasks;
using Amphetamine.Data.Enums;
using Amphetamine.Services.Models;

namespace Amphetamine.Services.Interfaces
{
	public interface IScrobblingService
	{
		SignInState SignInState { get; set; }

		string Username { get; set; }

		string Password { get; set; }


		event Action<SignInState> SignInStateChanged;

		Task SignIn();

		void SignOut();

		Task SendTrackLoveAsync(Track track, bool love);
	}
}
