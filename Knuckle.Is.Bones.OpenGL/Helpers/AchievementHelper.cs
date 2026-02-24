using Knuckle.Is.Bones.Core.Models.Saves;
using Steamworks;

namespace Knuckle.Is.Bones.OpenGL.Helpers
{
	public static class AchievementHelper
	{
		public static void UpdateAchievements(UserSaveDefinition user)
		{
			if (!SteamHelpers.IsInitialized)
				return;

			foreach (var key in user.CompletedItems.Keys)
			{
				int value = user.GetCompletedTimes(key);
				switch (key.ToString())
				{
					// Opponents
					case "9dd2041e-f8bc-4c44-8fcd-5424f9748ca6":
						SteamUserStats.SetStat("FrankWins", value);
						break;
					case "42244cf9-6ad3-4729-8376-a0d323440a18":
						SteamUserStats.SetStat("JerryWins", value);
						break;
					case "dde23f52-7a66-4dc6-bc47-eea2b493d9cd":
						SteamUserStats.SetStat("JohnWins", value);
						break;

					// Dice
					case "230b2d31-941f-4972-8d61-288b5c0b55ef":
						SteamUserStats.SetStat("D2Wins", value);
						break;
					case "f2ab41c6-3519-4d86-ab0d-feec8fd0f18f":
						SteamUserStats.SetStat("D3Wins", value);
						break;
					case "fb539a3a-9989-4623-88d1-bf216320f717":
						SteamUserStats.SetStat("D6Wins", value);
						break;
					case "9c935082-05b7-4b7e-9064-d6beb077ea8c":
						SteamUserStats.SetStat("D10Wins", value);
						break;
					case "a80ca420-879b-47b7-adbf-7ada87b5939c":
						SteamUserStats.SetStat("D50Wins", value);
						break;

					// Boards
					case "9cbc4760-d71b-482c-ab06-f05fecedd741":
						SteamUserStats.SetStat("SmallBoardWins", value);
						break;
					case "907bddf8-cbe1-49f4-a1f8-92ad5266f116":
						SteamUserStats.SetStat("NormalBoardWins", value);
						break;
					case "09144a57-a719-47bd-a36c-1cfb287c295d":
						SteamUserStats.SetStat("LargeBoardWins", value);
						break;
					case "7d7a3947-eed2-4e79-b20d-f75235fb04e2":
						SteamUserStats.SetStat("GigaBoardWins", value);
						break;
					case "aff95738-a568-4628-9b04-c3f8d649fd6f":
						SteamUserStats.SetStat("GiantBoardWins", value);
						break;
					case "a501943e-e6be-47d8-9b2c-01ca54c01c36":
						SteamUserStats.SetStat("StepsBoardWins", value);
						break;
					case "322dbaa5-c806-4b56-af9d-164b135a6d42":
						SteamUserStats.SetStat("TallBoardWins", value);
						break;
					case "0ac3da70-4cb8-4e97-9af9-4b9759499dd7":
						SteamUserStats.SetStat("WaveBoardWins", value);
						break;
				}
			}
			SteamUserStats.StoreStats();
		}
	}
}
