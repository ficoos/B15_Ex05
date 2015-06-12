using System;
using System.Collections.Generic;

namespace Ex02.Othello
{
	public class GameState
	{
		public class PlayerState
		{
			private readonly GameState r_OwnerState;

			private readonly ePlayerColor r_Color;

			public ePlayerColor Color
			{
				get
				{
					return r_Color;
				}
			}

			public uint Score
			{
				get
				{
					return r_OwnerState.Board.GetPieceCount(Color);
				}
			}

			public PlayerState(GameState i_OwnerState, ePlayerColor i_Color)
			{
				r_OwnerState = i_OwnerState;
				r_Color = i_Color;
			}
		}

		private static readonly BoardPosition[] sr_CardinalDirections =
		{
			new BoardPosition(0, 1), // North
			new BoardPosition(1, 1), // NorthEast
			new BoardPosition(1, 0), // East
			new BoardPosition(1, -1), // SouthEast
			new BoardPosition(0, -1), // South
			new BoardPosition(-1, -1), // SouthWest
			new BoardPosition(-1, 0), // West
			new BoardPosition(-1, 1) // NorthWest
		};

		private readonly PlayerState r_BlackPlayer;

		public PlayerState BlackPlayer
		{
			get
			{
				return r_BlackPlayer;
			}
		}

		private readonly PlayerState r_WhitePlayer;

		public PlayerState WhitePlayer
		{
			get
			{
				return r_WhitePlayer;
			}
		}
		
		private readonly GameBoard r_Board;

		public GameBoard Board
		{
			get
			{
				return r_Board;
			}
		}

		public bool IsDeadEnd
		{
			get
			{
				bool isDeadEnd = false;
				if (GetLegalMoves().Length == 0)
				{
					switchCurrentPlayer();
					if (GetLegalMoves().Length == 0)
					{
						isDeadEnd = true;
					}

					switchCurrentPlayer();
				}

				return isDeadEnd;
			}
		}

		public PlayerState CurrentPlayer { get; private set; }

		public GameState(GameBoard i_Board, ePlayerColor i_CurrentPlayerColor)
		{
			r_BlackPlayer = new PlayerState(this, ePlayerColor.Black);
			r_WhitePlayer = new PlayerState(this, ePlayerColor.White);
			r_Board = i_Board;
			CurrentPlayer = i_CurrentPlayerColor == ePlayerColor.Black ? r_BlackPlayer : r_WhitePlayer;
		}

		private void switchCurrentPlayer()
		{
			if (CurrentPlayer == r_BlackPlayer)
			{
				CurrentPlayer = r_WhitePlayer;
			}
			else
			{
				CurrentPlayer = r_BlackPlayer;
			}
		}

		public void PlacePiece(BoardPosition i_Position)
		{	
			if (!r_Board.IsValidPosition(i_Position))
			{
				throw new IndexOutOfRangeException("Position not in board");
			}
			
			if (!r_Board[i_Position].IsEmpty)
			{
				throw new IllegalMoveException("Cell is not empty");
			}
			
			if (performMove(i_Position) == 0)
			{
				throw new IllegalMoveException("Doesn't flip any enemy tiles");
			}

			this.switchCurrentPlayer();
			if (this.GetLegalMoves().Length == 0)
			{
				this.switchCurrentPlayer();
			}
		}

		private uint performMove(BoardPosition i_Position, bool i_FlipTiles = true)
		{
			uint totalFilpped = 0;

			foreach (BoardPosition offset in sr_CardinalDirections)
			{
				uint numFilpped;
				if (checkDirection(CurrentPlayer.Color, i_Position + offset, offset, i_FlipTiles, out numFilpped))
				{
					totalFilpped += numFilpped;
				}
			}

			if (i_FlipTiles && totalFilpped > 0)
			{
				r_Board[i_Position] = new GameBoard.Cell(CurrentPlayer.Color);
			}

			return totalFilpped;
		}

		private bool checkDirection(ePlayerColor i_PlayerColor, BoardPosition i_CurrentPosition, BoardPosition i_Offset, bool i_FilpTiles, out uint o_NumFlipped)
		{
			bool isValid = true;
			o_NumFlipped = 0;

			if (!r_Board.IsValidPosition(i_CurrentPosition))
			{
				isValid = false;
			}
			else
			{
				GameBoard.Cell currentCell = r_Board[i_CurrentPosition];
				if (currentCell.IsEmpty)
				{
					isValid = false;
				}
				else if (currentCell.ContainsColor(i_PlayerColor))
				{
					o_NumFlipped = 0;
				}
				else if (checkDirection(i_PlayerColor, i_CurrentPosition + i_Offset, i_Offset, i_FilpTiles, out o_NumFlipped))
				{
					if (i_FilpTiles)
					{
						currentCell.Flip();
						r_Board[i_CurrentPosition] = currentCell;
					}

					o_NumFlipped++;
				}
				else
				{
					isValid = false;
				}
			}

			return isValid;
		}

		/// <summary>
		/// Iterates possible child states.
		/// This is useful for AI implementations since GetLegalMoves() is very slow and if
		/// you want to trace all possible moves you will end up running the placing algorithm twice.
		/// </summary>
		/// <returns>A copy GameState with a valid move applied and the current player switched.</returns>
		public IEnumerable<GameState> PossibleChildStates()
		{
			GameState copyToYield = this.DeepCopy();
			for (int x = 0; x < Board.Size; x++)
			{
				for (int y = 0; y < Board.Size; y++)
				{
					if (!Board[x, y].IsEmpty)
					{
						continue;
					}

					BoardPosition position = new BoardPosition(x, y);
					uint score = copyToYield.performMove(position);
					if (score > 0)
					{
						copyToYield.switchCurrentPlayer();
						yield return copyToYield;
						copyToYield = this.DeepCopy();
					}
				}
			}
		}

		public BoardPosition[] GetLegalMoves()
		{
			List<BoardPosition> legalMoves = new List<BoardPosition>();

			for (int x = 0; x < Board.Size; x++)
			{
				for (int y = 0; y < Board.Size; y++)
				{
					if (!Board[x, y].IsEmpty)
					{
						continue;
					}

					BoardPosition position = new BoardPosition(x, y);
					const bool v_FlipTiles = true;
					uint score = performMove(position, !v_FlipTiles);
					if (score > 0)
					{
						legalMoves.Add(position);
					}
				}
			}

			return legalMoves.ToArray();
		}

		public GameState DeepCopy()
		{
			return new GameState(r_Board.DeepCopy(), CurrentPlayer.Color);
		}
	}
}