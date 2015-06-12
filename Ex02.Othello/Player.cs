namespace Ex02.Othello
{
	public class Player
	{
		private readonly GameState.PlayerState r_PlayerState;
		private readonly PlayerInfo r_PlayerInfo;

		public uint Score
		{
			get
			{
				return r_PlayerState.Score;
			}
		}
	
		public ePlayerColor Color
		{
			get
			{
				return r_PlayerState.Color;
			}
		}

		public string Name
		{
			get
			{
				return r_PlayerInfo.Name;
			}
		}

		internal Player(GameState.PlayerState i_PlayerState, PlayerInfo i_PlayerInfo)
		{
			r_PlayerState = i_PlayerState;
			r_PlayerInfo = i_PlayerInfo;
		}

		public PlayerControllerAction GetAction(GameState i_GameStateCopy)
		{
			return r_PlayerInfo.Controller.GetAction(i_GameStateCopy);
		}
	}
}
