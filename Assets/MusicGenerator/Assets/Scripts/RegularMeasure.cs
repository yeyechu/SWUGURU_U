using System;
using UnityEngine;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// A regular, non-repeating measure.
	/// </summary>
	public class RegularMeasure : Measure
	{
		/// <summary>
		/// Plays through the next step in the measure.
		/// </summary>
		/// <param name="set"></param>
		public override void PlayMeasure( InstrumentSet set )
		{
			set.UpdateTempo();
			set.SixteenthStepTimer -= Time.deltaTime;

			if ( set.MusicGenerator == false || ( set.SixteenthStepTimer > 0 ) )
			{
				return;
			}

			if ( set.SixteenthStepsTaken == set.TimeSignature.StepsPerMeasure )
			{
				set.MusicGenerator.GenerateNewProgression();
				ResetMeasure( set, set.MusicGenerator.SetThemeRepeat );
				return;
			}

			PlayBeat( set );
		}

		/// <summary>
		/// Plays a beat for this measure. If manually overriding, use this to advance
		/// </summary>
		/// <param name="set"></param>
		public void PlayBeat( InstrumentSet set )
		{
			if ( set.MusicGenerator == false )
			{
				return;
			}

			if ( set.SixteenthStepsTaken % set.Data.ProgressionRate == set.TimeSignature.Whole )
			{
				set.ProgressionStepsTaken += 1;
				set.ProgressionStepsTaken = set.ProgressionStepsTaken % set.MusicGenerator.CurrentChordProgression.Count;
				set.MusicGenerator.CheckKeyChange();
			}

			if ( set.SixteenthStepsTaken % set.TimeSignature.Half == 0 )
			{
				TakeStep( set, Timestep.Eighth, set.ProgressionStepsTaken );
			}

			if ( set.SixteenthStepsTaken % set.TimeSignature.Quarter == 0 )
			{
				TakeStep( set, Timestep.Quarter, set.ProgressionStepsTaken );
			}

			if ( set.SixteenthStepsTaken % set.TimeSignature.Eighth == 0 )
			{
				TakeStep( set, Timestep.Half, set.ProgressionStepsTaken );
			}

			if ( set.SixteenthStepsTaken % set.TimeSignature.Sixteenth == 0 )
			{
				TakeStep( set, Timestep.Whole, set.ProgressionStepsTaken );
				set.MeasureStartTimer = -set.SixteenthStepTimer;
			}

			TakeStep( set, Timestep.Sixteenth, set.ProgressionStepsTaken );

			set.SixteenthStepTimer = set.BeatLength + set.SixteenthStepTimer;
			set.SixteenthStepsTaken += 1;
		}

		/// <summary>
		/// /// Plays the next step in the measure.
		/// </summary>
		/// <param name="set"></param>
		/// <param name="timeStepIN"></param>
		/// <param name="stepsTaken"></param>
		public override void TakeStep( InstrumentSet set, Timestep timeStepIN, int stepsTaken = 0 )
		{
			for ( var instIndex = 0; instIndex < set.Instruments.Count; instIndex++ )
			{
				var instrument = set.Instruments[instIndex];
				var groupIsPlaying = set.GroupIsPlaying[instrument.InstrumentData.Group];
				var useLeitmotif = set.MusicGenerator.ConfigurationData.ThemeRepeatOptions == ThemeRepeatOptions.Leitmotif &&
				                   instrument.InstrumentData.Leitmotif.IsEnabled &&
				                   set.MusicGenerator.LeitmotifIsTemporarilySuspended == false &&
				                   IsUsingLeitmotif && set.RepeatCount < set.MusicGenerator.ConfigurationData.NumLeitmotifMeasures &&
				                   ( groupIsPlaying || set.MusicGenerator.ConfigurationData.LeitmotifIgnoresGroups );
				var useForcedPercussion = set.Instruments[instIndex].InstrumentData.UseForcedPercussion;
				var forceBeat = instrument.InstrumentData.ForceBeat && instrument.InstrumentData.ForcedBeats[set.SixteenthStepsTaken] && timeStepIN == Timestep.Sixteenth;

				switch ( useLeitmotif )
				{
					// prioritize leitmotif over forced percussion.
					case true when timeStepIN == Timestep.Sixteenth && instrument.InstrumentData.IsMuted == false:
						PlayLeitmotifNotes( set, instrument, stepsTaken, instIndex );
						break;
					case false when timeStepIN == Timestep.Sixteenth && groupIsPlaying && instrument.InstrumentData.IsMuted == false &&
					                useForcedPercussion:
						PlayForcedPercussion( set, instrument, instIndex );
						break;
					case false when ( instrument.InstrumentData.TimeStep == timeStepIN && groupIsPlaying &&
					                  instrument.InstrumentData.IsMuted == false && useForcedPercussion == false && instrument.InstrumentData.ForceBeat == false ) ||
					                ( forceBeat && instrument.InstrumentData.IsMuted == false && groupIsPlaying ):
						PlayNotes( set, instrument, stepsTaken, instIndex );
						break;
				}
			}
		}

		/// <summary>
		/// Exits a non-repeating measure, resetting values to be able to play the next:
		/// </summary>
		/// <param name="set"></param>
		/// <param name="setThemeRepeat"></param>
		/// <param name="hardReset"></param>
		/// <param name="isRepeating"></param>
		public override void ResetMeasure( InstrumentSet set, Action setThemeRepeat = null, bool hardReset = false, bool isRepeating = true )
		{
			ResetRegularMeasure( set, setThemeRepeat, hardReset, isRepeating );
		}

		/// <summary>
		/// Plays the notes for this timestep
		/// </summary>
		/// <param name="set"></param>
		/// <param name="instrument"></param>
		/// <param name="stepsTaken"></param>
		/// <param name="instIndex"></param>
		private static void PlayNotes( InstrumentSet set, Instrument instrument, int stepsTaken, int instIndex )
		{
			var progressionStep = set.MusicGenerator.CurrentChordProgression[stepsTaken];

			if ( instrument.InstrumentData.StrumLength == 0.0f || instrument.InstrumentData.SuccessionType == SuccessionType.Lead )
			{
				foreach ( var note in instrument.GetProgressionNotes( progressionStep ) )
				{
					if ( note != MusicConstants.UnplayedNote )
					{
						try
						{
							set.MusicGenerator.PlayAudioClip( set, instrument.InstrumentData.InstrumentType, note, instrument.InstrumentData.Volume,
								instIndex );
						}
						catch ( ArgumentOutOfRangeException e )
						{
							throw new ArgumentOutOfRangeException( e.Message );
						}
					}
				}
			}
			else
				set.Strum( instrument.GetProgressionNotes( progressionStep ), instIndex );
		}

		/// <summary>
		/// Plays a forced percussion note
		/// </summary>
		/// <param name="set"></param>
		/// <param name="instrument"></param>
		/// <param name="instIndex"></param>
		private static void PlayForcedPercussion( InstrumentSet set, Instrument instrument, int instIndex )
		{
			var step = MusicConstants.GetBeat( set.Data.TimeSignature, set.SixteenthStepsTaken );
			var stepsTaken = MusicConstants.GetBeatStep( set.Data.TimeSignature, set.SixteenthStepsTaken );
			if ( instrument.InstrumentData.ForcedPercussiveNotes.Measures[set.PercussionRepeatCount % set.Data.NumForcedPercussionMeasures]
				.Timesteps[step]
				.Notes[stepsTaken] )
			{
				set.MusicGenerator.PlayAudioClip( set, instrument.InstrumentData.InstrumentType, 0, instrument.InstrumentData.Volume, instIndex );
			}
		}

		/// <summary>
		/// Plays a note from the leitmotif
		/// </summary>
		/// <param name="set"></param>
		/// <param name="instrument"></param>
		/// <param name="stepsTaken"></param>
		/// <param name="instIndex"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		private static void PlayLeitmotifNotes( InstrumentSet set, Instrument instrument, int stepsTaken, int instIndex )
		{
			MusicConstants.GetBeatInfo( set.TimeSignature.Signature, set.SixteenthStepsTaken, out var beat, out var beatStep );
			foreach ( var note in instrument.InstrumentData.Leitmotif.GetLeitmotifNotes( set.RepeatCount, beat, beatStep ) )
			{
				if ( note.ScaledNote == MusicConstants.UnplayedNote )
				{
					continue;
				}

				try
				{
					var finalNote = instrument.InstrumentData.IsPercussion ? 0 : Leitmotif.GetUnscaledNoteIndex( note, set.MusicGenerator );
					set.MusicGenerator.PlayAudioClip( set, instrument.InstrumentData.InstrumentType, finalNote, instrument.InstrumentData.Volume,
						instIndex );
				}
				catch ( ArgumentOutOfRangeException e )
				{
					throw new ArgumentOutOfRangeException( e.Message );
				}
			}
		}
	}
}
