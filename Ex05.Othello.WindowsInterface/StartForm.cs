using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ex02.Othello;

namespace Ex05.Othello.WindowsInterface
{
	public class StartForm : Form
	{
		private Button m_BoardSizeButton;
		private Button m_PlayAgainstComputerButton;
		private Button m_PlayAgainstFriendButton;
		private int m_BoardSizeIndex;

		private int selectedBoardSize
		{
			get
			{
				return GameBoard.ValidBoardSizes[m_BoardSizeIndex % GameBoard.ValidBoardSizes.Length];
			}
		}

		public StartForm()
		{
			this.m_BoardSizeIndex = 0;
			this.initializeComponent();
		}

		private void initializeComponent()
		{
			const int v_Margin = 12;
			this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			MaximizeBox = false;
			this.Text = "Othello - Game Settings";

			m_BoardSizeButton = new Button();
			m_BoardSizeButton.Text = string.Format("Board Size = {0}x{0} (Click to increase)", this.selectedBoardSize);
			m_BoardSizeButton.Top = v_Margin;
			m_BoardSizeButton.Left = v_Margin;
			m_BoardSizeButton.Width = ClientSize.Width - (v_Margin * 2);
			m_BoardSizeButton.Height = 40;
			m_BoardSizeButton.Click += m_BoardSizeButton_Click;
			this.Controls.Add(m_BoardSizeButton);

			m_PlayAgainstComputerButton = new Button();
			m_PlayAgainstComputerButton.Text = "Play against the computer";
			m_PlayAgainstComputerButton.Top = m_BoardSizeButton.Bottom + v_Margin;
			m_PlayAgainstComputerButton.Left = v_Margin;
			m_PlayAgainstComputerButton.Width = (m_BoardSizeButton.Width - v_Margin) / 2;
			m_PlayAgainstComputerButton.Height = 40;
			m_PlayAgainstComputerButton.Click += this.playButtons_Click;
			this.Controls.Add(m_PlayAgainstComputerButton);

			m_PlayAgainstFriendButton = new Button();
			m_PlayAgainstFriendButton.Text = "Play against your friend";
			m_PlayAgainstFriendButton.Top = m_PlayAgainstComputerButton.Top;
			m_PlayAgainstFriendButton.Left = m_PlayAgainstComputerButton.Right + v_Margin;
			m_PlayAgainstFriendButton.Width = m_PlayAgainstComputerButton.Width;
			m_PlayAgainstFriendButton.Height = m_PlayAgainstComputerButton.Height;
			m_PlayAgainstFriendButton.Click += this.playButtons_Click;
			this.Controls.Add(m_PlayAgainstFriendButton);
			

			Height = (Height - ClientSize.Height) + m_PlayAgainstComputerButton.Bottom + v_Margin;
		}

		private void m_BoardSizeButton_Click(object i_Sender, EventArgs i_Args)
		{
			this.m_BoardSizeIndex++;
			m_BoardSizeButton.Text = string.Format("Board Size = {0}x{0} (Click to increase)", this.selectedBoardSize);
		}

		private void playButtons_Click(object i_Sender, EventArgs i_Args)
		{
			GameForm gameForm = new GameForm(selectedBoardSize, i_Sender == m_PlayAgainstComputerButton);
			this.Hide();
			gameForm.ShowDialog();
			this.Close();
		}
	}
}
