using System;
using System.Drawing;
using System.Windows.Forms;

using Ex02.Othello;

namespace Ex05.Othello.WindowsInterface
{
	

	public class CellButton : Control
	{
		private const string k_DiscText = "O";

		private static readonly Color sr_NonSelectableBackColor = Color.LightGray;

		private static readonly Color sr_SelectableBackColor = Color.DarkGray;

		private static readonly Color sr_BlackPlayerButtonBackColor = Color.Black;

		private static readonly Color sr_BlackPlayerButtonForeColor = Color.White;

		private static readonly Color sr_WhitePlayerButtonBackColor = Color.White;

		private static readonly Color sr_WhitePlayerButtonForeColor = Color.Black;

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
		
		public bool Selectable
		{
			get
			{
				return m_Selectable;
			}

			set
			{
				m_Selectable = value;
				if (m_Selectable)
				{
					this.r_Button.BackColor = sr_SelectableBackColor;
					this.r_Button.Enabled = true;
				}
				else
				{
					this.r_Button.BackColor = sr_NonSelectableBackColor;
					this.r_Button.Enabled = false;
				}
			} 
		}

		private void updateButton()
		{
			switch (Content)
			{
				case eCellContent.Black:
					this.r_Button.BackColor = sr_BlackPlayerButtonBackColor;
					this.r_Button.ForeColor = sr_BlackPlayerButtonForeColor;
					this.r_Button.Text = k_DiscText;
					break;

				case eCellContent.White:
					this.r_Button.BackColor = sr_WhitePlayerButtonBackColor;
					this.r_Button.ForeColor = sr_WhitePlayerButtonForeColor;
					this.r_Button.Text = k_DiscText;
					break;

				case eCellContent.None:
					this.r_Button.BackColor = sr_NonSelectableBackColor;
					this.r_Button.Text = string.Empty;
					break;
			}
		}

		public CellButton(BoardPosition i_BoardPosition)
		{
			r_BoardPosition = i_BoardPosition;
			setUpButton();
		}

		private void setUpButton()
		{
			this.r_Button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			this.r_Button.Top = 0;
			this.r_Button.Left = 0;
			this.r_Button.Width = this.Width;
			this.r_Button.Height = this.Height;
			this.r_Button.Click += this.m_Button_Click;
			this.Controls.Add(this.r_Button);
		}

		private void m_Button_Click(object i_Sender, EventArgs i_EventArgs)
		{
			OnClick(i_EventArgs);
		}
	}
}
