namespace Ex02.Othello
{
	public sealed class PerformMoveAction : PlayerControllerAction
	{
		private readonly BoardPosition r_Position;

		public BoardPosition Position
		{
			get
			{
				return r_Position;
			}
		}

		public PerformMoveAction(BoardPosition i_Position)
		{
			r_Position = i_Position;
		}
	}
}