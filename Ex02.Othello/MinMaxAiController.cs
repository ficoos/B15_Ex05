using System;

namespace Ex02.Othello
{
	public class MinMaxAiController : IPlayerController
	{
		private readonly uint r_SearchDepth;

		public MinMaxAiController(uint i_SearchDepth = 4)
		{
			r_SearchDepth = i_SearchDepth;
		}

		public PlayerControllerAction GetAction(GameState i_GameStateCopy)
		{
			return new PerformMoveAction(getBestMove(i_GameStateCopy));
		}

		private BoardPosition getBestMove(GameState i_GameState)
		{
			BoardPosition[] legalMoves = i_GameState.GetLegalMoves();
			BoardPosition bestMove = legalMoves[0];
			uint maxScore = 0;
			foreach (BoardPosition legalMove in legalMoves)
			{
				GameState gameStateCopy = i_GameState.DeepCopy();
				gameStateCopy.PlacePiece(legalMove);
				uint score = scoreGameState(gameStateCopy, r_SearchDepth, false);
				if (score > maxScore)
				{
					maxScore = score;
					bestMove = legalMove;
				}
			}

			return bestMove;
		}

		private uint scoreGameState(GameState i_GameState, uint i_SearchDepth, bool i_IsMaximizing)
		{
			uint score;
			if (i_SearchDepth == 0)
			{
				if (i_IsMaximizing)
				{
					score = i_GameState.CurrentPlayer.Score;
				}
				else
				{
					score = i_GameState.CurrentPlayer == i_GameState.BlackPlayer ? i_GameState.WhitePlayer.Score : i_GameState.BlackPlayer.Score;
				}
			}
			else
			{
				if (i_IsMaximizing)
				{
					score = 0;
				}
				else
				{
					score = uint.MaxValue;
				}

				foreach (GameState childState in i_GameState.PossibleChildStates())
				{
					uint result = this.scoreGameState(childState, i_SearchDepth - 1, !i_IsMaximizing);
					if (i_IsMaximizing)
					{
						score = Math.Max(score, result);
					}
					else
					{
						score = Math.Min(score, result);
					}
				}
			}

			return score;
		}
	}
}