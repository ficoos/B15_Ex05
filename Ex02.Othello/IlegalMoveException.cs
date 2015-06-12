using System;

namespace Ex02.Othello
{
	[Serializable]
	public class IllegalMoveException : Exception
	{
		public IllegalMoveException(string i_Reason) : base(string.Format("Illegal move: {0}", i_Reason))
		{
		}
	}
}