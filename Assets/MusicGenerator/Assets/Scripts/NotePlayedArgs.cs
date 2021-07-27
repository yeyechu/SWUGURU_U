using System;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// Arguments for note played event
	/// </summary>
	public class NotePlayedArgs : EventArgs
	{
		/// <summary>
		/// Arguments for note played event
		/// </summary>
		/// <param name="argInstrumentSet"></param>
		/// <param name="instrumentName"></param>
		/// <param name="argNote"></param>
		/// <param name="argVolume"></param>
		/// <param name="argInstrumentIndex"></param>
		public NotePlayedArgs( InstrumentSet argInstrumentSet, string instrumentName, int argNote, float argVolume, int argInstrumentIndex )
		{
			InstrumentSet = argInstrumentSet;
			InstrumentName = instrumentName;
			Note = argNote;
			Volume = argVolume;
			InstrumentIndex = argInstrumentIndex;
		}

		public InstrumentSet InstrumentSet;
		public string InstrumentName;
		public int Note;
		public float Volume;
		public int InstrumentIndex;
	}
}
