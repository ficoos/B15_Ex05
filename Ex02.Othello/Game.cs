using System;

namespace Ex02.Othello
{
	public class Game
	{
		private readonly Player r_BlackPlayer;

		public Player BlackPlayer
		{
			get
			{
				return r_BlackPlayer;
			}
		}

		private readonly Player r_WhitePlayer;

		public Player WhitePlayer
		{
			get
			{
				return r_WhitePlayer;
			}
		}

		public Player CurrentPlayer
		{
			get
			{
				return r_BlackPlayer.Color == r_InternalGameState.CurrentPlayer.Color ? r_BlackPlayer : r_WhitePlayer;
			}
		}

		private readonly GameState r_InternalGameState;

		public GameBoard Board
		{
			get
			{
				return r_InternalGameState.Board;
			}
		}

		public Player Winner
		{
			get
			{
				Player winner;

				if (IsRunning)
				{
					throw new InvalidOperationException("There is no winner, Game is still in progress.");
				}

				if (BlackPlayer.Score > WhitePlayer.Score)
				{
					winner = BlackPlayer;
				}
				else if (WhitePlayer.Score > BlackPlayer.Score)
				{
					winner = WhitePlayer;
				}
				else
				{
					winner = null;
				}

				return winner;
			}
		}

		public Player Looser
		{
			get
			{
				Player looser;
				Player winner = Winner;
				if (winner == null)
				{
					looser = null;
				}
				else
				{
					looser = winner == BlackPlayer ? WhitePlayer : BlackPlayer;
				}

				return looser;
			}
		}

		public bool IsRunning { get; private set; }

		public Game(PlayerInfo i_BlackPlayer, PlayerInfo i_WhitePlayer, int i_BoardSize)
		{
			r_InternalGameState = new GameState(new GameBoard(i_BoardSize), ePlayerColor.Black);
			r_BlackPlayer = new Player(r_InternalGameState.BlackPlayer, i_BlackPlayer);
			r_WhitePlayer = new Player(r_InternalGameState.WhitePlayer, i_WhitePlayer);
			IsRunning = true;
		}
		
		private eIterationResult handleQuitAction()
		{
			IsRunning = false;
			return eIterationResult.GameQuit;
		}

		private eIterationResult handleAction(PlayerControllerAction i_Action)
		{
			eIterationResult result;

			if (i_Action is QuitAction)
			{
				result = handleQuitAction();
			}
			else if (i_Action is PerformMoveAction)
			{
				result = this.handlePerformMoveAction((PerformMoveAction)i_Action);
			}
			else
			{
				throw new ArgumentException("Got unknown action from user");
			}

			return result;
		}

		private eIterationResult handlePerformMoveAction(PerformMoveAction i_Action)
		{
			eIterationResult result;
			BoardPosition newPiecePosition = i_Action.Position;

			try
			{
				GameState.PlayerState currentPlayer = r_InternalGameState.CurrentPlayer;
				r_InternalGameState.PlacePiece(newPiecePosition);
				if (currentPlayer == r_InternalGameState.CurrentPlayer)
				{
					result = eIterationResult.PlayerSkipped;
				}
				else
				{
					result = eIterationResult.Success;
				}
			}
			catch (IndexOutOfRangeException)
			{
				result = eIterationResult.MoveOutOfBounds;
			}
			catch (IllegalMoveException)
			{
				result = eIterationResult.IllegalMove;
			}

			return result;
		}

		public eIterationResult Iterate()
		{
			eIterationResult result;
			
			if (r_InternalGameState.IsDeadEnd)
			{
				result = eIterationResult.GameOver;
				IsRunning = false;
			}
			else
			{
				result = handleAction(CurrentPlayer.GetAction(r_InternalGameState.DeepCopy()));
			}

			return result;
		}
	}
}
