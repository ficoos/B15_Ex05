namespace Ex02.Othello
{
	public interface IPlayerController
	{
		PlayerControllerAction GetAction(GameState i_GameStateCopy);
	}
}
