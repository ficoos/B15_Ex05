using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Ex05.Othello.WindowsInterface
{
	using System.Drawing;

	using Ex02.Othello;

	public class CellButton : Control
	{
		private readonly Button r_Button = new Button();

		private readonly BoardPosition r_BoardPosition;

		private eCellContent m_Content;

		public eCellContent Content
		{
			get
			{
				return m_Content;
			}

			set
			{
				m_Content = value;
				updateButton();
			}
		}

		public BoardPosition BoardPosition
		{
			get
			{
				return this.r_BoardPosition;
			}
		}

		private bool m_Selectable;

		public bool Selectable {
			get
			{
				return m_Selectable;
			}

			set
			{
				m_Selectable = value;
				if (m_Selectable)
				{
					this.r_Button.BackColor = Color.DarkGray;
					this.r_Button.Enabled = true;
				}
				else
				{
					this.r_Button.BackColor = Color.LightGray;
					this.r_Button.Enabled = false;
				}
			} 
		}

		private void updateButton()
		{
			switch (Content)
			{
				case eCellContent.Black:
					this.r_Button.BackColor = Color.Black;
					this.r_Button.ForeColor = Color.White;
					this.r_Button.Text = "O";
					break;

				case eCellContent.White:
					this.r_Button.BackColor = Color.White;
					this.r_Button.ForeColor = Color.Black;
					this.r_Button.Text = "O";
					break;

				case eCellContent.None:
					this.r_Button.BackColor = Color.LightGray;
					this.r_Button.Text = string.Empty;
					break;
			}
		}

		public CellButton(BoardPosition i_BoardPosition)
		{
			r_BoardPosition = i_BoardPosition;
			this.r_Button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			this.r_Button.Top = 0;
			this.r_Button.Left = 0;
			this.r_Button.Width = Width;
			this.r_Button.Height = Height;
			this.r_Button.Click += m_Button_Click;
			this.Controls.Add(this.r_Button);
		}

		private void m_Button_Click(object sender, EventArgs eventArgs)
		{
			OnClick(eventArgs);
		}
	}
}
