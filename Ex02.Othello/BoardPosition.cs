namespace Ex02.Othello
{
    public struct BoardPosition
    {
		private const int k_FormatLetterIndex = 0;
		private const int k_FormatNumberIndex = 1;
		private const char k_FirstLetter = 'A';
		private const char k_FirstDigit = '1';

	    public static char FirstLetter
	    {
		    get
		    {
			    return k_FirstLetter;
		    }
	    }

	    public static char FirstDigit
	    {
		    get
		    {
			    return k_FirstDigit;
		    }
	    }

		public int X { get; set; }

		public int Y { get; set; }

        public BoardPosition(int i_X, int i_Y) : this()
		{
			X = i_X;
			Y = i_Y;
		}

		private static bool isValidFormat(string i_Input)
		{
			return char.IsUpper(i_Input[k_FormatLetterIndex]) && char.IsDigit(i_Input[k_FormatNumberIndex]);
		}

        public static bool TryParse(string i_Input, out BoardPosition o_Position)
        {
			bool isValidInput = isValidFormat(i_Input);

			if (isValidInput)
			{
				int x = i_Input[k_FormatLetterIndex] - k_FirstLetter;
				int y = i_Input[k_FormatNumberIndex] - k_FirstDigit;
				o_Position = new BoardPosition(x, y);
			}
			else
			{
				o_Position = new BoardPosition();
			}
		
            return isValidInput;
        }

		public static bool operator ==(BoardPosition i_LeftArgument, BoardPosition i_RightArgument)
		{
			return i_LeftArgument.X == i_RightArgument.Y && i_LeftArgument.Y == i_RightArgument.Y;
		}

		public static bool operator !=(BoardPosition i_LeftArgument, BoardPosition i_RightArgument)
		{
			return !(i_LeftArgument == i_RightArgument);
		}

		public static BoardPosition operator +(BoardPosition i_LeftArgument, BoardPosition i_RightArgument)
		{
			return new BoardPosition(i_LeftArgument.X + i_RightArgument.X, i_LeftArgument.Y + i_RightArgument.Y);
		}

		public static BoardPosition operator -(BoardPosition i_LeftArgument, BoardPosition i_RightArgument)
		{
			return new BoardPosition(i_LeftArgument.X - i_RightArgument.X, i_LeftArgument.Y - i_RightArgument.Y);
		}

		public override bool Equals(object i_Other)
		{
			BoardPosition? other = i_Other as BoardPosition?;
			return other.HasValue && this == other.Value;
		}

		public override int GetHashCode()
		{
			return X ^ Y;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}", (char)(k_FirstLetter + X), (char)(k_FirstDigit + Y));
		}
    }
}
