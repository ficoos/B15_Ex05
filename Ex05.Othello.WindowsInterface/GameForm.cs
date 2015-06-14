using System;
using System.Windows.Forms;
using Ex02.Othello;

namespace Ex05.Othello.WindowsInterface
{
	using System.Threading;

	public class GameForm : Form
	{
		private readonly int r_BoardSize;
		private readonly bool r_PlayAgainstAi;
		private Game m_Game;
		private GameBoardControl m_GameBoard;
		private Thread m_GameThread;
		private bool m_IsRunning;
		private BoardControlPlayerController[] m_PlayerControllers;
		private int m_BlackWins;
		private int m_WhiteWins;

		public GameForm(int i_BoardSize = 6, bool i_PlayAgainstAi = true)
		{
			m_BlackWins = 0;
			m_WhiteWins = 0;
			r_BoardSize = i_BoardSize;
			r_PlayAgainstAi = i_PlayAgainstAi;
			initializeControl();
			resetGame();
		}

		protected override void OnLoad(EventArgs i_Args)
		{
			m_GameThread.Start();
			base.OnLoad(i_Args);
		}

		private void initializeGame(int i_BoardSize, bool i_PlayAgainstAi)
		{
			BoardControlPlayerController playerOneController = new BoardControlPlayerController(this.m_GameBoard);
			IPlayerController playerTwoController;
			if (i_PlayAgainstAi)
			{
				playerTwoController = new MinMaxAiController();
				this.m_PlayerControllers = new[] { playerOneController };
			}
			else
			{
				playerTwoController = new BoardControlPlayerController(this.m_GameBoard);
				this.m_PlayerControllers = new[] { playerOneController, (BoardControlPlayerController)playerTwoController };
			}

			this.m_Game = new Game(
				new PlayerInfo("Player 1", playerOneController),
				new PlayerInfo("Player 2", playerTwoController),
				i_BoardSize);
		}

		protected override void OnClosed(EventArgs i_Args)
		{
			foreach (BoardControlPlayerController controller in this.m_PlayerControllers)
			{
				controller.Quit();
			}

			m_IsRunning = false;
			base.OnClosed(i_Args);
		}

		private DialogResult showGameOverMessageBox()
		{
			return MessageBox.Show(
				this.getGameOverMessage(),
				"Othello",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Information,
				MessageBoxDefaultButton.Button1);
		}

		private string getGameOverMessage()
		{
			string message;
			bool isTie = this.m_Game.Winner == null;
			if (isTie)
			{
				message = 
@"It's a tie
Would you like to play another round?";
			}
			else
			{
				if (this.m_Game.Winner.Color == ePlayerColor.Black)
				{
					this.m_BlackWins++;
				}
				else
				{
					this.m_WhiteWins++;
				}

				message = string.Format(
@"{0} won!! ({1}/{2}) ({3}/{4})
Would you like another round?",
					this.m_Game.Winner.Color == ePlayerColor.Black ? "Black" : "White",
					this.m_Game.Winner.Score,
					this.m_Game.Looser.Score,
					this.m_Game.Winner.Color == ePlayerColor.Black ? this.m_BlackWins : this.m_WhiteWins,
					this.m_Game.Looser.Color == ePlayerColor.Black ? this.m_BlackWins : this.m_WhiteWins);
			}

			return message;
		}

		private void runGame()
		{
			while (m_IsRunning)
			{
				Invoke(new Action(this.updateBoardControl));
				switch (m_Game.Iterate())
				{
					case eIterationResult.GameOver:
						m_IsRunning = false;
						
						if (this.showGameOverMessageBox() == DialogResult.Yes)
						{
							Invoke(new Action(resetGame));
						}
						else
						{
							BeginInvoke(new Action(Close));
						}
						
						break;
					case eIterationResult.GameQuit:
						m_IsRunning = false;
						break;
				}
			}
		}

		private void resetGame()
		{
			initializeGame(r_BoardSize, r_PlayAgainstAi);
			m_GameBoard.Board = m_Game.Board;
			m_GameThread = new Thread(this.runGame);
			m_IsRunning = true;
		}

		private void updateBoardControl()
		{
			m_GameBoard.UpdateState();
		}

		private void initializeControl()
		{
			const int v_Margin = 12;
			m_GameBoard = new GameBoardControl();
			m_GameBoard.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			m_GameBoard.Top = v_Margin;
			m_GameBoard.Left = v_Margin;
			m_GameBoard.Width = ClientSize.Width - (v_Margin * 2);
			m_GameBoard.Height = ClientSize.Height - (v_Margin * 2);
			Controls.Add(m_GameBoard);
		}
	}
}
