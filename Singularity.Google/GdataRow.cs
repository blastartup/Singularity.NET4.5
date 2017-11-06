using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace Singularity.Google
{
	public abstract class GdataRow
	{
		public void SetValue(CellEntry cellEntry)
		{
			Cells.Add(cellEntry.Cell.Column, cellEntry);
		}

		protected String GetValue(UInt32 index)
		{
			CellEntry returnCell;
			if (Cells.TryGetValue(index, out returnCell))
			{
				return returnCell.Cell.Value;
			}
			return String.Empty;
		}

		protected void SetValue(UInt32 index, String value)
		{
			CellEntry cellEntry;
			if (Cells.TryGetValue(index, out cellEntry))
			{
				cellEntry.InputValue = value;
				cellEntry.Update();
			}
			else if (CellLink != null && Service != null)
			{
				CellQuery cellQuery = new CellQuery(CellLink.HRef.ToString());
				cellQuery.MinimumRow = cellQuery.MaximumRow = RowNbr;
				cellQuery.MinimumColumn = cellQuery.MaximumColumn = index;
				cellQuery.ReturnEmpty = ReturnEmptyCells.yes;

				CellFeed cellFeed = Service.Query(cellQuery);
				cellEntry = cellFeed.Entries[0] as CellEntry;
				if (cellEntry != null)
				{
					cellEntry.InputValue = value;
					cellEntry.Update();
					Cells.Add(cellEntry.Column, cellEntry);
				}
			}
		}

		public AtomLink CellLink { get; set; }

		public SpreadsheetsService Service { get; set; }

		public UInt32 RowNbr { get; set; }

		public abstract Int32 LastColumn { get; }

		private IDictionary<UInt32, CellEntry> Cells => _cells ?? (_cells = new Dictionary<UInt32, CellEntry>());
		private Dictionary<UInt32, CellEntry> _cells;
	}
}
