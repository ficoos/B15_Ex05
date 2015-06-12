using Ex02.Othello;

namespace Ex05.Othello.WindowsInterface
{
	using System.Threading;

	public class BoardControlPlayerController : IPlayerController
	{
		private readonly GameBoardControl r_BoardControl;
		private readonly ManualResetEvent r_PositionSelectedResetEvent = new ManualResetEvent(false);
		private BoardPosition m_SelectedPosition;

		private bool m_IsQuitting;

		public BoardControlPlayerController(GameBoardControl i_BoardControl)
		{
			r_BoardControl = i_BoardControl;
			r_BoardControl.CellClick += r_BoardControl_CellClick;
			m_IsQuitting = false;
		}

		public PlayerControllerAction GetAction(GameState i_GameStateCopy)
		{
			r_PositionSelectedResetEvent.Reset();
			r_BoardControl.SetPossibleMoved(i_GameStateCopy.GetLegalMoves());
			r_PositionSelectedResetEvent.WaitOne();
			PlayerControllerAction action;
			if (m_IsQuitting)
			{
				action = new QuitAction();
			}
			else
			{
				action = new PerformMoveAction(m_SelectedPosition);
			}

			return action;
		}

		public void Quit()
		{
			m_IsQuitting = true;
			r_PositionSelectedResetEvent.Set();
		}

		private void r_BoardControl_CellClick(object i_Sender, BoardPosition i_Position)
		{
			m_SelectedPosition = i_Position;
			r_PositionSelectedResetEvent.Set();
		}
	}
}
