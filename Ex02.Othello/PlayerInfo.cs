namespace Ex02.Othello
{
	public struct PlayerInfo
	{
		public string Name { get; set; }

		public IPlayerController Controller { get; set; }

		public PlayerInfo(string i_Name, IPlayerController i_Controller) : this()
		{
			Name = i_Name;
			Controller = i_Controller;
		}
	}
}