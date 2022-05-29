using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSudoku
{
	public class Value
	{
		private static int nextOrder = 0;
		private int arrivalOrder;
		private int row, col;
		List<char> values;

		public Value(int row, int col)
		{
			this.row = row;
			this.col = col;
			values = new List<char>();
			arrivalOrder = nextOrder;
			nextOrder++;
		}

		public void add(char value)
		{
			values.Add(value);
		}

		public int size()
		{
			return values.Count();
		}

		public List<char> getValues()
		{
			return values;
		}

		public int compareTo(Value val)
		{
			if (values.Count() > val.size())
				return 1;
			else if (values.Count() < val.size())
				return -1;
			else if (arrivalOrder < val.arrivalOrder)
				return -1;
			return 1;
		}

		public int getRow()
		{
			return row;
		}

		public int getCol()
		{
			return col;
		}
	}
}
