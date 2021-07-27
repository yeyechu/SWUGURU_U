using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// Available Staff Player Colors. These have long since stopped matching their names to their colors.
	/// </summary>
	public enum StaffPlayerColors
	{
		Brown = 0,
		Blue = 1,
		Red = 2,
		LightBlue = 3,
		Black = 4,
		White = 5,
		Pink = 6,
		Green = 7,
		Orange = 8
	}

	/// <summary>
	/// Available time signatures.
	/// </summary>
	[Serializable] public enum TimeSignatures
	{
		FourFour = 0,
		ThreeFour = 1,
		FiveFour = 2
	}

	/// <summary>
	/// State of the clip.
	/// </summary>
	public enum ClipState
	{
		None = 0,
		Play = 1,
		Pause = 2,
		Stop = 3,
		Finished = 4,
	}

	/// <summary>
	/// The type of succession. melody, rhythm or lead
	/// </summary>
	[Serializable] public enum SuccessionType
	{
		Melody = 0,
		Rhythm = 1,
		Lead = 2
	}

	/// <summary>
	/// Musical key
	/// </summary>
	[Serializable] public enum Key
	{
		C = 0,
		CSharp = 1,
		D = 2,
		DSharp = 3,
		E = 4,
		F = 5,
		FSharp = 6,
		G = 7,
		GSharp = 8,
		A = 9,
		ASharp = 10,
		B = 11
	}

	/// <summary>
	/// Music Generator repeat options
	/// </summary>
	[Serializable] public enum ThemeRepeatOptions
	{
		None = 0,
		Theme = 1,
		Repeat = 2,
		Leitmotif = 3
	}

	/// <summary>
	/// Music Generator timesteps
	/// </summary>
	[Serializable] public enum Timestep
	{
		Sixteenth = 0,
		Eighth = 1,
		Quarter = 2,
		Half = 3,
		Whole = 4
	}

	/// <summary>
	/// Rate at which we roll for new musical groups of instruments
	/// </summary>
	[Serializable] public enum GroupRate
	{
		Measure = 0,
		Progression = 1
	}

	/// <summary>
	/// State of the music generator
	/// </summary>
	[Serializable]
	public enum GeneratorState
	{
		None = -1,
		Initializing = 0,
		Ready = 1,
		Stopped = 2,
		Playing = 3,
		Repeating = 4,
		Paused = 5,
		ManualPlay = 6
	}

	/// <summary>
	/// Music generator Volume states
	/// </summary>
	[Serializable] public enum VolumeState
	{
		Idle = 0,
		FadedOutIdle = 1,
		FadingIn = 2,
		FadingOut = 3
	}

	/// <summary>
	/// Music Generator modes
	/// </summary>
	[Serializable] public enum Mode
	{
		Ionian = 0,
		Dorian = 1,
		Phrygian = 2,
		Lydian = 3,
		Mixolydian = 4,
		Aoelean = 5,
		Locrian = 6
	}

	/// <summary>
	/// Music Generator scales.
	/// </summary>
	[Serializable] public enum Scale
	{
		Major = 0,
		NatMinor = 1,
		mMelodicMinor = 2,
		HarmonicMinor = 3,
		HarmonicMajor = 4
	}

	/// <summary>
	/// Style of dynamics: linear, Random. The way in which we choose which groups to play.
	/// </summary>
	[Serializable] public enum DynamicStyle
	{
		Linear = 0,
		Random = 1
	}

	public static class MusicConstants
	{
		public const float Version = 2.0f;
		public const string PMGAddressableName = "PMGAddressableAssetSettings";
		public const string PMGAddressableDictionaryName = "ProcGenMusicAddressableDictionary";
		public const string PMGGeneratorPrefabName = "PMGMusicGenerator";

		public static string StreamingClipsPath => Path.Combine( Application.streamingAssetsPath, "MusicGenerator", "InstrumentClips" );
		public static string PersistentClipsPath => Path.Combine( Application.persistentDataPath, "InstrumentClips" );
		public static string StreamingSavesPath => Path.Combine( Application.streamingAssetsPath, "MusicGenerator", "InstrumentSaves" );
		public static string ConfigurationStreamingDataPath => Path.Combine( Application.streamingAssetsPath, "MusicGenerator", "ConfigurationData" );
		public static string ConfigurationStreamingModDataPath => Path.Combine( Application.streamingAssetsPath, "MusicGenerator", "ConfigurationModData" );
		public static string ConfigurationPersistentDataPath => Path.Combine( Application.persistentDataPath, "ConfigurationData" );
		public static string ConfigurationPersistentModDataPath => Path.Combine( Application.persistentDataPath, "ModData" );
		public const string ConfigurationFileName = "ConfigurationData";

		/// <summary>
		/// The scales steps in melodic minor
		/// TO NOTE: Melodic minor typically has an ascending/descending scale. But, it was causing issues so
		/// now just uses the ascending scale in both ascend/descending melodies, which is super inaccurate for classical music theory.
		/// It's on the wishlist to resolve, but is problematic for a few reasons.
		/// </summary>
		public static readonly int[] MelodicMinor = {FullStep, HalfStep, FullStep, FullStep, FullStep, FullStep, HalfStep};

		/// <summary>
		/// The scales steps in natural minor
		/// </summary>
		public static readonly int[] NaturalMinor = {FullStep, HalfStep, FullStep, FullStep, HalfStep, FullStep, FullStep};

		/// <summary>
		/// The scales steps in harmonic minor
		/// </summary>
		public static readonly int[] HarmonicMinor = {FullStep, HalfStep, FullStep, FullStep, HalfStep, FullPlusHalf, HalfStep};

		/// <summary>
		/// The scales steps in the Major scale
		/// </summary>
		public static readonly int[] MajorScale = {FullStep, FullStep, HalfStep, FullStep, FullStep, FullStep, HalfStep};

		/// <summary>
		/// The scales steps in the Harmonic Major scale
		/// </summary>
		///
		public static readonly int[] HarmonicMajor = {FullStep, FullStep, HalfStep, FullStep, HalfStep, FullPlusHalf, HalfStep};

		/// <summary>
		/// A container of our scale-steps for each scale
		/// </summary>
		private static readonly int[][]
			MusicScales = {MajorScale, NaturalMinor, MelodicMinor, HarmonicMinor, HarmonicMajor};

		/// <summary>
		/// Returns scale given the type index
		/// </summary>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static IReadOnlyList<int> GetScale( Scale scale )
		{
			return MusicScales[(int) scale];
		}

		///<summary>
		/// based on decibels. Needs to match vol slider on ui (if using ui). Edit at your own risk :)
		/// </summary>
		public const float MaxVolume = 15;

		///<summary>
		/// based on decibels. Needs to match vol slider
		/// </summary> on ui (if using ui). Edit at your own risk :)
		public const float MinVolume = -100.0f;

		/// <summary>
		/// Value for ascending lead scale
		/// </summary>
		public const int AscendingInfluence = 1;

		/// <summary>
		/// value for descending lead scale
		/// </summary>
		public const float DescendingInfluence = -1;

		///<summary>
		/// max number of notes per instrument
		/// </summary>
		public const int MaxInstrumentNotes = 36;

		/// <summary>
		/// Max number of times we may repeat.
		/// </summary>
		public const int MaxRepeatCount = 4;

		/// <summary>
		/// Max Tempo
		/// </summary>
		public const float MaxTempo = 1000.0f;

		/// <summary>
		/// Minimum tempo
		/// </summary>
		public const float MinTempo = 1f;

		///<summary>
		/// max number of steps per chord progression. Currently only support 4
		/// </summary>
		public const int MaxFullstepsTaken = 4;

		/// <summary>
		/// Maximum number of notes that can be generated per beat (used for arpeggio/other repeating notes for example to clamp generated notes)
		/// </summary>
		public const int MaxGeneratedNotesPerBeat = 5;

		/// <summary>
		/// Number of measures
		/// </summary>
		public const int NumMeasures = 4;

		///<summary>
		/// number of notes in an octave
		/// </summary>
		public const int OctaveSize = 12;

		/// <summary>
		/// number of octaves in our staff player
		/// </summary>
		public const int NumOctaves = 3;

		/// <summary>
		/// Length of a scale
		/// </summary>
		public const int ScaleLength = 7;

		/// <summary>
		/// Number of scale notes per measure
		/// </summary>
		public const int TotalScaleNotes = ScaleLength * NumOctaves;

		/// <summary>
		/// Number of pentatonic avoid notes.
		/// </summary>
		public const int NumPentatonicAvoids = 2;

		/// <summary>
		/// Tonic Chords values (0-based)
		/// </summary>
		/// <value></value>
		public static readonly int[] TonicChords = {0, 2, 5};

		/// <summary>
		/// Subdominant Chords values (0-based)
		/// </summary>
		/// <value></value>
		public static readonly int[] SubdominantChords = {1, 3};

		/// <summary>
		/// Dominant Chord value (0-based)
		/// </summary>
		/// <value></value>
		public static readonly int[] DominantChords = {4, 6};

		///<summary>
		/// is frankly how many will fit at the moment...Only made 20 mixer groups. In theory there's no hard limit if you want to add mixer groups and expose their variables.
		/// </summary>
		public const int MaxInstruments = 20;

		/// <summary>
		/// Max number of steps per measure (assumes 5/4 is max signature)
		/// </summary>
		public const int MaxStepsPerMeasure = 20;

		/// <summary>
		/// Max number of steps per timestep (assumes 5/4 is max signature)
		/// </summary>
		public const int MaxStepsPerTimestep = 5;

		/// <summary>
		/// Our standard pentatonic avoid notes for the major scale
		/// </summary>
		/// <value></value>
		public static readonly int[] MajorPentatonicAvoid = {3, 6};

		/// <summary>
		/// Our standard pentatonic avoid notes for the minor scale
		/// </summary>
		/// <value></value>
		public static readonly int[] MinorPentatonicAvoid = {1, 5};

		/// <summary>
		/// Value for unplayed note
		/// </summary>
		public const int UnplayedNote = -1;

		/// <summary>
		/// scale steps in a seventh chord
		/// </summary>
		public static readonly int[] SeventhChord = {0, 2, 4, 6};

		///<summary>
		/// number of notes in a triad :P
		/// </summary>
		public const int TriadCount = 3;

		///<summary>
		/// steps between notes. Used in scales:
		/// </summary>
		public const int HalfStep = 1;

		///<summary>
		/// steps between notes. Used in scales:
		/// </summary>
		public const int FullStep = 2;

		///<summary>
		/// steps between notes. Used in scales:
		/// </summary>
		public const int FullPlusHalf = 3;

		/// <summary>
		/// for exotic scales. Currently not implemented
		/// </summary>
		public const int DoubleStep = 4;

		///<summary>
		/// amount to adjust tritone chords/ notes.
		/// </summary>
		public const int TritoneStep = 5;

		/// <summary>
		/// Minimum number of strum notes
		/// </summary>
		public const int MinNumStrumNotes = 2;

		/// <summary>
		/// Minimum number of times we may repeat the arpeggio pattern
		/// </summary>
		public const int MinArpeggioRepeat = 1;

		/// <summary>
		/// maximum number of times we may repeat the arpeggio pattern
		/// </summary>
		public const int MaxArpeggioRepeat = 15;

		/// <summary>
		/// Base number of times we may repeat the arpeggio pattern
		/// </summary>
		public const int BaseArpeggioRepeat = 8;

		/// <summary>
		/// Base number of strum notes
		/// </summary>
		public const int BaseNumStrumNotes = 3;

		/// <summary>
		/// Maximum number of strum notes
		/// </summary>
		public const int MaxNumStrumNotes = 5;

		public const string MixerVolumeName = "Volume";
		public const string MixerRoomSizeName = "RoomSize";
		public const string MixerReverbName = "Reverb";
		public const string MixerReverbDryName = "ReverbDry";
		public const string MixerReverbDelayName = "ReverbDelay";
		public const string MixerReverbRoomHFName = "ReverbRoomHF";
		public const string MixerReverbDecayTimeName = "ReverbDecayTime";
		public const string MixerReverbDecayHFRatioName = "ReverbDecayHFRatio";
		public const string MixerReverbReflectionsName = "ReverbReflections";
		public const string MixerReverbReflectDelayName = "ReverbReflectDelay";
		public const string MixerReverbDiffusionName = "ReverbDiffusion";
		public const string MixerReverbDensityName = "ReverbDensity";
		public const string MixerReverbHFReferenceName = "ReverbHFReference";
		public const string MixerReverbLFReferenceName = "ReverbLFReference";
		public const string MixerReverbRoomLFName = "ReverbRoomLF";
		public const string MixerEchoName = "Echo";
		public const string MixerEchoDryName = "EchoDry";
		public const string MixerEchoDelayName = "EchoDelay";
		public const string MixerEchoDecayName = "EchoDecay";
		public const string MixerFlangeName = "Flange";
		public const string MixerFlangeDryName = "FlangeDry";
		public const string MixerFlangeDepthName = "FlangeDepth";
		public const string MixerFlangeRateName = "FlangeRate";
		public const string FlangeRateName = "FlangeRate";
		public const string FlangeDepthName = "FlangeDepth";
		public const string MixerDistortionName = "Distortion";
		public const string MixerChorusName = "Chorus";
		public const string MixerChorus2Name = "ChorusTapTwo";
		public const string MixerChorus3Name = "ChorusTapThree";
		public const string MixerChorusDryName = "ChorusDry";
		public const string MixerChorusDelayName = "ChorusDelay";
		public const string MixerChorusRateName = "ChorusRate";
		public const string MixerChorusDepthName = "ChorusDepth";
		public const string MixerChorusFeedbackName = "ChorusFeedback";
		public const string MixerCenterFreq = "CenterFreq";
		public const string MixerOctaveRange = "OctaveRange";
		public const string MixerFreqGain = "FreqGain";
		public const string MixerLowpassCutoffFreq = "LowpassCutoffFreq";
		public const string MixerLowpassResonance = "LowpassResonance";
		public const string MixerHighpassCutoffFreq = "HighpassCutoffFreq";
		public const string MixerHighpassResonance = "HighpassResonance";
		public const string MasterVolName = "MasterVol";

		public const float VolFadeRateMin = 1f;
		public const float VolFadeRateMax = 20f;
		public const int RepeatMeasuresNumMin = 1;
		public const int RepeatMeasuresNumMax = 4;
		public const int KeyStepsMin = -7;
		public const int KeyStepsMax = 7;
		public const float OddsMin = 0f;
		public const float OddsMax = 100f;
		public const float OddsMid = 50f;
		public const float MixerGroupVolumeMin = -80f;
		public const float MixerGroupVolumeMax = 20f;
		public const float Min100 = 0f;
		public const float Max100 = 100f;
		public const float NormalizedMin = 0f;
		public const float NormalizedMax = 1f;
		public const float NormalizedMid = .5f;

		// This is just used to bring the values of the mixer group within 0-100f.
		public const float MixerGroupVolOffset = 80f;

		// Reset values
		public const float BaseMasterVolume = 0;
		public const float BaseVolFadeRate = 2f;
		public const float BaseSetThemeOdds = 10f;
		public const float BasePlayThemeOdds = 15f;

		public const float BaseTempo = 300f;

		public const int InstrumentPatternLengthMin = 0;
		public const int InstrumentPatternLengthMax = 8;
		public const int InstrumentPatternRepeatMin = 1;
		public const int InstrumentPatternRepeatMax = 15;
		public const int BaseInstrumentPatternRepeat = 4;

#region InstrumentEffects

		public const float BaseReverbDry = 0.0f;
		public const float MinReverbDry = -10000f;
		public const float MaxReverbDry = 0.0f;

		public const float BaseReverbRoom = -10000f;
		public const float MinReverbRoom = -10000f;
		public const float MaxReverbRoom = 0.0f;

		public const float BaseReverbRoomHF = 0f;
		public const float MinReverbRoomHF = -10000f;
		public const float MaxReverbRoomHF = 0.0f;

		public const float BaseReverbDecayTime = 1f;
		public const float MinReverbDecayTime = .1f;
		public const float MaxReverbDecayTime = 20f;

		public const float BaseReverbDecayHFRatio = .5f;
		public const float MinReverbDecayHFRatio = .1f;
		public const float MaxReverbDecayHFRatio = 2f;

		public const float BaseReverbReflections = -10000f;
		public const float MinReverbReflections = -10000f;
		public const float MaxInstrumentReverbReflections = 1000f;

		public const float BaseReverbReflectDelay = .02f;
		public const float MinReverbReflectDelay = 0f;
		public const float MaxReverbReflectDelay = 0.3f;

		public const float BaseReverb = -10000f;
		public const float MinReverb = -10000f;
		public const float MaxReverb = 2000f;

		public const float BaseReverbDelay = .04f;
		public const float MinReverbDelay = 0f;
		public const float MaxReverbDelay = .1f;

		public const float BaseReverbDiffusion = 100f;
		public const float MinReverbDiffusion = 0f;
		public const float MaxReverbDiffusion = 100f;

		public const float BaseReverbDensity = 100f;
		public const float MinReverbDensity = 0f;
		public const float MaxReverbDensity = 100f;

		public const float BaseReverbHFReference = 5000f;
		public const float MinReverbHFReference = 20f;
		public const float MaxReverbHFReference = 20000f;

		public const float BaseReverbRoomLF = 0f;
		public const float MinReverbRoomLF = -10000f;
		public const float MaxReverbRoomLF = 0f;

		public const float BaseReverbLFReference = 250f;
		public const float MinReverbLFReference = 20f;
		public const float MaxReverbLFReference = 1000f;

		public const float BaseEchoDelay = 1f;
		public const float MinEchoDelay = 1f;
		public const float MaxEchoDelay = 5000f;

		public const float BaseEchoDecay = 0f;
		public const float MinEchoDecay = 0f;
		public const float MaxEchoDecay = 100f;

		public const float BaseEchoDry = 1f;
		public const float MinEchoDry = 0f;
		public const float MaxEchoDry = 1f;

		public const float BaseEchoWet = 0f;
		public const float MinEchoWet = 0f;
		public const float MaxEchoWet = 1f;

		public const float BaseFlangeDry = 1f;
		public const float MinFlangeDry = 0f;
		public const float MaxFlangeDry = 1f;

		public const float BaseFlangeWet = 0f;
		public const float MinFlangeWet = 0f;
		public const float MaxFlangeWet = 1f;

		public const float BaseFlangeDepth = 1f;
		public const float MinFlangeDepth = .1f;
		public const float MaxFlangeDepth = 1f;

		public const float BaseFlangeRate = .1f;
		public const float MinFlangeRate = 0f;
		public const float MaxFlangeRate = 20f;

		public const float BaseDistortion = 0f;
		public const float MinDistortion = 0f;
		public const float MaxDistortion = 100f;

		public const float BaseChorusDry = 1f;
		public const float MinChorusDry = 0f;
		public const float MaxChorusDry = 1f;

		public const float BaseChorusWetMixTap1 = 0f;
		public const float MinChorusWetMixTap1 = 0f;
		public const float MaxChorusWetMixTap1 = 1f;

		public const float BaseChorusWetMixTap2 = 0f;
		public const float MinChorusWetMixTap2 = 0f;
		public const float MaxChorusWetMixTap2 = 1f;

		public const float BaseChorusWetMixTap3 = 0f;
		public const float MinChorusWetMixTap3 = 0f;
		public const float MaxChorusWetMixTap3 = 1f;

		public const float BaseChorusDelay = 40f;
		public const float MinChorusDelay = 0f;
		public const float MaxChorusDelay = 100f;

		public const float BaseChorusRate = .80f;
		public const float MinChorusRate = 0f;
		public const float MaxChorusRate = 20f;

		public const float BaseChorusDepth = .16f;
		public const float MinChorusDepth = 0f;
		public const float MaxChorusDepth = 1f;

		public const float BaseChorusFeedback = .46f;
		public const float MinChorusFeedback = -1f;
		public const float MaxChorusFeedback = 1f;

		public const float BaseParamEQCenterFreq = 8000f;
		public const float MinParamEQCenterFreq = 20f;
		public const float MaxParamEQCenterFreq = 22000f;

		public const float BaseParamEQOctaveRange = 1f;
		public const float MinParamEQOctaveRange = .20f;
		public const float MaxParamEQOctaveRange = 5f;

		public const float BaseParamEQFreqGain = 1f;
		public const float MinParamEQFreqGain = .5f;
		public const float MaxParamEQFreqGain = 3f;

		public const float BaseLowpassCutoffFreq = 22000f;
		public const float MinLowpassCutoffFreq = 10f;
		public const float MaxLowpassCutoffFreq = 22000f;

		public const float BaseLowpassResonance = 1f;
		public const float MinLowpassResonance = 1f;
		public const float MaxLowpassResonance = 10f;

		public const float BaseHighpassCutoffFreq = 10f;
		public const float MinHighpassCutoffFreq = 10f;
		public const float MaxHighpassCutoffFreq = 22000f;

		public const float BaseHighpassResonance = 1f;
		public const float MinHighpassResonance = 1f;
		public const float MaxHighpassResonance = 10f;

#endregion InstrumentEffects

		//EndInstrumentEffects

		public const int BaseInstrumentPatternLength = 4;
		public const int BaseInstrumentPatternRelease = 2;
		public const int BaseInstrumentLeadMaxSteps = 3;

		public const float BaseProgressionChangeOdds = 25f;
		public const float BaseSuccessivePlayOdds = 75f;

		public const int NumGroups = 4;

		// Misc
		public const int HeaderSpace = 5;

		public static Color InvertColor( Color rgbColor, bool hsvInvert = true )
		{
			if ( hsvInvert )
			{
				float h, s, v;
				Color.RGBToHSV( rgbColor, out h, out s, out v );
				return Color.HSVToRGB( ( h + 0.5f ) % 1, s, ( v + 0.5f ) % 1 );
			}
			else
			{
				return new Color( 1f - rgbColor.r, 1f - rgbColor.g, 1f - rgbColor.b );
			}
		}

		private const float cTextColorBrightnessMax = .6f;
		private const float cTextColorDim = .5f;
		private const float cTextColorBoost = 2f;

		public static Color InvertTextColor( Color rgbColor )
		{
			var color = InvertColor( rgbColor ) * ( rgbColor.grayscale > cTextColorBrightnessMax ? cTextColorDim : cTextColorBoost );
			color.a = 1f;
			return color;
		}

		public static int SafeLoop( int note, int min, int max )
		{
			if ( min < 0 || max < 0 || max < min )
			{
				Debug.LogError( "Please ensure max is great than min and both max and min are positive values" );
				return note;
			}

			if ( note >= max )
			{
				return min + ( note % ( max - min ) );
			}

			if ( note < min )
			{
				return max - ( ( min - note ) % ( max - min ) );
			}

			return note;
		}

		public static int GetBeat( TimeSignatures timeSignature, int step )
		{
			switch ( timeSignature )
			{
				case TimeSignatures.ThreeFour:
					return step / 3;
				case TimeSignatures.FourFour:
					return step / 4;
				case TimeSignatures.FiveFour:
					return step / 5;
				default:
					return step / 4;
			}
		}

		public static int GetBeatStep( TimeSignatures timeSignature, int step )
		{
			switch ( timeSignature )
			{
				case TimeSignatures.ThreeFour:
					return step % 3;
				case TimeSignatures.FourFour:
					return step % 4;
				case TimeSignatures.FiveFour:
					return step % 5;

				default:
					return step % 4;
			}
		}

		public static void GetBeatInfo( TimeSignatures timeSignature, int step, out int beat, out int beatStep )
		{
			beat = GetBeat( timeSignature, step );
			beatStep = GetBeatStep( timeSignature, step );
		}

		public static Vector2Int GetBeatInfo( TimeSignatures timeSignature, int step )
		{
			return new Vector2Int( GetBeat( timeSignature, step ), GetBeatStep( timeSignature, step ) );
		}

		public static int GetTimestepLength( TimeSignatures timeSignatures )
		{
			switch ( timeSignatures )
			{
				case TimeSignatures.ThreeFour:
					return 3;
				case TimeSignatures.FourFour:
					return 4;
				case TimeSignatures.FiveFour:
					return 5;
				default:
					return 4;
			}
		}
	}
}
