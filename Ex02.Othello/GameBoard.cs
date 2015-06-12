using System;
using System.Collections.Generic;

namespace Ex02.Othello
{
	public class GameBoard : IEnumerable<GameBoard.Cell>
	{
		private static readonly int[] sr_ValidBoardSizes = { 6, 8, 10, 12 };

		public static int[] ValidBoardSizes
		{ 
			get
			{
				return sr_ValidBoardSizes;
			}
		}

		public struct Cell
		{
			public eCellContent Content { get; set; }

			public Cell(eCellContent i_Content = eCellContent.None) : this()
			{
				Content = i_Content;
			}

			public Cell(ePlayerColor i_Color) : this()
			{
				switch(i_Color)
				{
					case ePlayerColor.White:
						Content = eCellContent.White;
						break;
					case ePlayerColor.Black:
						Content = eCellContent.Black;
						break;
				}
			}

			public bool ContainsColor(ePlayerColor i_Color)
			{
				bool result;

				switch(Content)
				{
					case eCellContent.Black:
						result = i_Color == ePlayerColor.Black;
						break;
					case eCellContent.White:
						result = i_Color == ePlayerColor.White;
						break;
					default:
						result = false;
						break;
				}

				return result;
			}

			public void Flip()
			{
				switch(Content)
				{
					case eCellContent.None:
						throw new InvalidOperationException("Cell is empty and cannot be flipped");
					case eCellContent.Black:
						Content = eCellContent.White;
						break;
					case eCellContent.White:
						Content = eCellContent.Black;
						break;
				}
			}

			public bool IsEmpty
			{
				get
				{
					return Content == eCellContent.None;
				}
			}
			
			public override string ToString()
			{
				return string.Format("{0}[{1}]", this.GetType().FullName, Content);
			}
		}

		private readonly Cell[,] r_BoardMatrix;

		private readonly int r_Size;

		public int Size
		{
			get
			{
				return r_Size;
			}
		}

		public static bool IsValidBoardSize(int i_Size)
		{
			const int v_ItemNotFound = -1;
			return Array.IndexOf(sr_ValidBoardSizes, i_Size) != v_ItemNotFound;
		}

		public GameBoard(int i_Size)
		{
			if (!IsValidBoardSize(i_Size))
			{
				throw new ArgumentOutOfRangeException("i_Size", string.Format("{0} is not a valid board size", i_Size));
			}

			r_BoardMatrix = new Cell[i_Size, i_Size];
			initializeBoardMatrix();
			r_Size = i_Size;
		}

		public Cell this[int i_X, int i_Y]
		{
			get
			{
				return r_BoardMatrix[i_X, i_Y];
			}

			set
			{
				r_BoardMatrix[i_X, i_Y] = value;
			}
		}

		public Cell this[BoardPosition i_Position]
		{
			get
			{
				return this[i_Position.X, i_Position.Y];
			}

			set
			{
				this[i_Position.X, i_Position.Y] = value;
			}
		}

		private void initializeBoardMatrix()
		{
            int length = r_BoardMatrix.GetLength(0);

            r_BoardMatrix[(length / 2) - 1, (length / 2) - 1].Content = eCellContent.White;
            r_BoardMatrix[(length / 2), (length / 2)].Content = eCellContent.White;
			r_BoardMatrix[(length / 2) - 1, (length / 2)].Content = eCellContent.Black;
			r_BoardMatrix[(length / 2), (length / 2) - 1].Content = eCellContent.Black;
		}

		public uint GetPieceCount(ePlayerColor i_Color)
		{
			uint count = 0;
			foreach (Cell cell in r_BoardMatrix)
			{
				if (cell.ContainsColor(i_Color))
				{
					count++;
				}
			}

			return count;
		}

		public GameBoard DeepCopy()
		{
			GameBoard cloneBoard = new GameBoard(Size);
			Array.Copy(r_BoardMatrix, cloneBoard.r_BoardMatrix, Size * Size);
			return cloneBoard;
		}

		public bool IsValidPosition(BoardPosition i_Position)
		{
			return IsValidPosition(i_Position.X, i_Position.Y);
		}

        public bool IsValidPosition(int i_X, int i_Y)
        {
            return i_X >= 0 && i_Y >= 0 && i_X < Size && i_Y < Size;
        }

		public IEnumerator<Cell> GetEnumerator()
		{
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					yield return this[x, y];
				}
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
