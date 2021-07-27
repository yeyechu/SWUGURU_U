using System;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// Arguments for note played event
	/// </summary>
	public class NotesGeneratedArgs : EventArgs
	{
		/// <summary>
		/// Arguments for note played event
		/// </summary>
		/// <param name="argInstrumentSet"></param>
		/// <param name="instrumentName"></param>
		/// <param name="argNote"></param>
		/// <param name="argVolume"></param>
		/// <param name="argInstrumentIndex"></param>
		public NotesGeneratedArgs( InstrumentSet argInstrumentSet, string instrumentName, int[] argNote, float argVolume, int argInstrumentIndex )
		{
			InstrumentSet = argInstrumentSet;
			InstrumentName = instrumentName;
			Notes = argNote;
			Volume = argVolume;
			InstrumentIndex = argInstrumentIndex;
		}

		public InstrumentSet InstrumentSet;
		public string InstrumentName;
		public int[] Notes;
		public float Volume;
		public int InstrumentIndex;
	}
}
