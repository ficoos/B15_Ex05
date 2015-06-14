using System;
using System.Windows.Forms;

using Ex02.Othello;

namespace Ex05.Othello.WindowsInterface
{
	public class GameBoardControl : Control
	{
		public delegate void CellClickDelegate(object i_Sender, BoardPosition i_BoardPosition);

		public event CellClickDelegate CellClick;

		private CellButton[,] m_CellButtons;

		private GameBoard m_Board;

		public GameBoard Board
		{
			get
			{
				return m_Board;
			}

			set
			{
				m_Board = value;
				this.m_CellButtons = new CellButton[m_Board.Size, m_Board.Size];
				this.populateControl();
			}
		}
		
		protected override void OnResize(EventArgs i_Args)
		{
			base.OnResize(i_Args);
			if (m_Board != null)
			{
				this.resizeCellButtons();
			}
		}

		private void resizeCellButtons()
		{
			int boardSize = m_Board.Size;
			int buttonHeight = Height / boardSize;
			int buttonWidth = Width / boardSize;

			for (int y = 0; y < boardSize; y++)
			{
				for (int x = 0; x < boardSize; x++)
				{
					CellButton cellButton = this.m_CellButtons[x, y];
					cellButton.Left = buttonWidth * x;
					cellButton.Top = buttonHeight * y;
					cellButton.Height = buttonHeight;
					cellButton.Width = buttonWidth;
				}
			}
		}

		private void disableCellButtons()
		{
			int boardSize = m_Board.Size;
			for (int y = 0; y < boardSize; y++)
			{
				for (int x = 0; x < boardSize; x++)
				{
					this.m_CellButtons[x, y].Selectable = false;
				}
			}
		}

		private void populateControl()
		{
			Controls.Clear();
			int boardSize = m_Board.Size;
			for (int y = 0; y < boardSize; y++)
			{
				for (int x = 0; x < boardSize; x++)
				{
					CellButton cellButton = new CellButton(new BoardPosition(x, y));		
					cellButton.Click += cellButton_Click;
					this.m_CellButtons[x, y] = cellButton;
					this.Controls.Add(cellButton);
				}
			}

			this.resizeCellButtons();
			this.disableCellButtons();
			this.UpdateState();
		}

		private void cellButton_Click(object i_Sender, EventArgs i_Args)
		{
			OnCellClick(((CellButton)i_Sender).BoardPosition);
		}

		public void UpdateState()
		{
			int boardSize = m_Board.Size;
			for (int y = 0; y < boardSize; y++)
			{
				for (int x = 0; x < boardSize; x++)
				{
					this.m_CellButtons[x, y].Content = m_Board[x, y].Content;
				}
			}
		}

		protected virtual void OnCellClick(BoardPosition i_BoardPosition)
		{
			this.disableCellButtons();
			CellClickDelegate handler = this.CellClick;
			if (handler != null)
			{
				handler(this, i_BoardPosition);
			}
		}

		public void SetPossibleMoved(BoardPosition[] i_LegalMoves)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<BoardPosition[]>(this.SetPossibleMoved), i_LegalMoves);
			}
			else
			{
				foreach (BoardPosition boardPosition in i_LegalMoves)
				{
					CellButton cellButton = this.m_CellButtons[boardPosition.X, boardPosition.Y];
					cellButton.Selectable = true;
				}
			}
		}
	}
}
