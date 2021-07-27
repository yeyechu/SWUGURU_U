using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// Instrument Panel with full Instrument settings
	/// </summary>
	public class InstrumentPanelUI : UIPanel
	{
#region public

		/// <summary>
		/// Reference to our instrument
		/// </summary>
		public Instrument Instrument { get; private set; }

		/// <summary>
		/// Clears our current instrument
		/// This leaves the panel empty
		/// </summary>
		public void ClearInstrument()
		{
			Instrument = null;
			mInstrumentPanelVisibleObject.SetActive( false );
			mInstrumentEffectVisibleObject.SetActive( false );
		}

		///<inheritdoc/>
		public override void Initialize( UIManager uiManager, bool isEnabled = true )
		{
			Instrument = uiManager.MusicGenerator.InstrumentSet.Instruments.Count > 0
				? uiManager.MusicGenerator.InstrumentSet.Instruments[0]
				: null;
			base.Initialize( uiManager, isEnabled );
			mInstrumentPanelVisibleObject.SetActive( false );
			mInstrumentEffectVisibleObject.SetActive( false );
		}

		/// <summary>
		/// Sets our instrument and updates values.
		/// </summary>
		/// <param name="instrument"></param>
		public void SetInstrument( Instrument instrument )
		{
			Instrument = instrument;
			UpdateUIElementValues();
		}

		/// <summary>
		/// Breaks our display (naming to match other panels that use this nomenclature. This just updates our ui elements)
		/// </summary>
		public void BreakDisplay()
		{
			UpdateUIElementValues();
		}

		///<inheritdoc/>
		public override void UpdateUIElementValues()
		{
			if ( mIsInitialized == false || Instrument == null )
			{
				return;
			}

			if ( mListenersHaveInitialized == false )
			{
				InitializeListeners();
				return;
			}

			var fxIsOn = mFXToggle.Option.isOn;
			mInstrumentEffectVisibleObject.SetActive( fxIsOn );
			mInstrumentPanelVisibleObject.SetActive( fxIsOn == false );

			SetLeadAvoidNoteListeners( initialize: false );

			mUseSevenths.Option.isOn = Instrument.InstrumentData.ChordSize == 4;

			mArpeggio.Option.isOn = Instrument.InstrumentData.Arpeggio && Instrument.InstrumentData.IsPercussion == false;
			mArpeggioRepeat.Option.value = Instrument.InstrumentData.ArpeggioRepeat;
			mNumStrumNotes.Option.value = Instrument.InstrumentData.NumStrumNotes;
			mKeepMelodicRhythm.Option.isOn = Instrument.InstrumentData.KeepMelodicRhythm;
			mPattern.Option.isOn = Instrument.InstrumentData.UsePattern;
			mStrumLength.Option.value = Instrument.InstrumentData.StrumLength;
			mReverseStrumToggle.Option.isOn = Instrument.InstrumentData.ReverseStrum;
			mStrumVariation.Option.maxValue = mMusicGenerator.InstrumentSet.BeatLength * mMusicGenerator.InstrumentSet.TimeSignature.StepsPerMeasure;
			mStrumVariation.Option.value = Instrument.InstrumentData.StrumVariation / mMusicGenerator.InstrumentSet.BeatLength;
			mOddsOfPlaying.Option.value = Instrument.InstrumentData.OddsOfPlaying;
			mPentatonic.Option.isOn = Instrument.InstrumentData.IsPentatonic;
			mPentatonicParent.SetActive( Instrument.InstrumentData.SuccessionType == SuccessionType.Lead && mPentatonic.Option.isOn == false );

			if ( Instrument.InstrumentData.ForceBeat )
			{
				mTimestep.Option.value = -1;
			}
			else
			{
				mTimestep.Option.value = Enum.GetNames( typeof(Timestep) ).Length - (int) Instrument.InstrumentData.TimeStep - 1;
			}

			mMinMelodicRhythmTimestep.Option.value = Enum.GetNames( typeof(Timestep) ).Length - (int) Instrument.InstrumentData.MinMelodicRhythmTimeStep - 1;
			mStereoPan.Option.value = Instrument.InstrumentData.StereoPan * 100f;
			mPatternLength.Option.value = Instrument.InstrumentData.PatternLength;
			mPatternRepeat.Option.value = Instrument.InstrumentData.PatternRepeat;
			mPatternRelease.Option.value = Instrument.InstrumentData.PatternRelease;
			mLeadVariation.Option.value = Instrument.InstrumentData.AscendDescendInfluence;
			mLeadMaxSteps.Option.value = Instrument.InstrumentData.LeadMaxSteps;
			mSuccessivePlayOdds.Option.value = Instrument.InstrumentData.SuccessivePlayOdds;
			mVolume.Option.value = Instrument.InstrumentData.Volume * 100f;
			mOddsOfPlayingChordNote.Option.value = Instrument.InstrumentData.OddsOfUsingChordNotes;
			mOddsOfPlayingChordNote.VisibleObject.SetActive( Instrument.InstrumentData.IsPercussion == false );
			mMixerGroupVolume.Option.value = Instrument.InstrumentData.MixerGroupVolume;

			SetSuccessionType( (int) Instrument.InstrumentData.SuccessionType );
			ToggleManualBeat( Instrument.InstrumentData.ForceBeat );

			//reverb:
			mReverb.Option.value = Instrument.InstrumentData.Reverb;
			mRoomSize.Option.value = Instrument.InstrumentData.RoomSize;
			mReverbDry.Option.value = Instrument.InstrumentData.ReverbDry;
			mReverbDelay.Option.value = Instrument.InstrumentData.ReverbDelay;
			mReverbRoomHF.Option.value = Instrument.InstrumentData.ReverbRoomHF;
			mReverbDecayTime.Option.value = Instrument.InstrumentData.ReverbDecayTime;
			mReverbDecayHFRatio.Option.value = Instrument.InstrumentData.ReverbDecayHFRatio;
			mReverbReflections.Option.value = Instrument.InstrumentData.ReverbReflections;
			mReverbReflectDelay.Option.value = Instrument.InstrumentData.ReverbReflectDelay;
			mReverbDiffusion.Option.value = Instrument.InstrumentData.ReverbDiffusion;
			mReverbDensity.Option.value = Instrument.InstrumentData.ReverbDensity;
			mReverbHFReference.Option.value = Instrument.InstrumentData.ReverbHFReference;
			mReverbRoomLF.Option.value = Instrument.InstrumentData.ReverbRoomLF;
			mReverbLFReference.Option.value = Instrument.InstrumentData.ReverbLFReference;

			//echo:
			mEcho.Option.value = Instrument.InstrumentData.Echo * 100f;
			mEchoDry.Option.value = Instrument.InstrumentData.EchoDry * 100f;
			mEchoDelay.Option.value = Instrument.InstrumentData.EchoDelay;
			mEchoDecay.Option.value = Instrument.InstrumentData.EchoDecay * 100f;

			//flange:
			mFlange.Option.value = Instrument.InstrumentData.Flanger * 100f;
			mFlangeDry.Option.value = Instrument.InstrumentData.FlangeDry * 100f;
			mFlangeDepth.Option.value = Instrument.InstrumentData.FlangeDepth;
			mFlangeRate.Option.value = Instrument.InstrumentData.FlangeRate;

			//distortion:
			mDistortion.Option.value = Instrument.InstrumentData.Distortion * 100f;

			//chorus:
			mChorusWetMixTap1.Option.value = Instrument.InstrumentData.Chorus * 100f;
			mChorusWetMixTap2.Option.value = Instrument.InstrumentData.Chorus2 * 100f;
			mChorusWetMixTap3.Option.value = Instrument.InstrumentData.Chorus3 * 100f;
			mChorusDry.Option.value = Instrument.InstrumentData.ChorusDry * 100f;
			mChorusDelay.Option.value = Instrument.InstrumentData.ChorusDelay;
			mChorusRate.Option.value = Instrument.InstrumentData.ChorusRate;
			mChorusDepth.Option.value = Instrument.InstrumentData.ChorusDepth;
			mChorusFeedback.Option.value = Instrument.InstrumentData.ChorusFeedback;

			//paramEQ:
			mParamEQCenterFreq.Option.value = Mathf.Log10( Instrument.InstrumentData.ParamEQCenterFreq / ( MusicConstants.MaxParamEQCenterFreq * .1f ) );
			mParamEQFreqGain.Option.value = Instrument.InstrumentData.ParamEQFreqGain;
			mParamEQOctaveRange.Option.value = Instrument.InstrumentData.ParamEQOctaveRange;

			//lowpass:
			mLowpassCutoffFreq.Option.value = Mathf.Log10( Instrument.InstrumentData.LowpassCutoffFreq / ( MusicConstants.MaxLowpassCutoffFreq * .1f ) );
			mLowpassResonance.Option.value = Instrument.InstrumentData.LowpassResonance;

			//highpass:
			mHighpassCutoffFreq.Option.value = Mathf.Log10( Instrument.InstrumentData.HighpassCutoffFreq / ( MusicConstants.MaxHighpassCutoffFreq * .1f ) );
			mHighpassResonance.Option.value = Instrument.InstrumentData.HighpassResonance;

			mOctavesToUse.Clear();
			for ( var index = 0; index < Instrument.InstrumentData.OctavesToUse.Count; index++ )
			{
				mOctavesToUse.Add( Instrument.InstrumentData.OctavesToUse[index] );
			}

			mOctave1.Option.isOn = Instrument.InstrumentData.OctavesToUse.Contains( 0 );
			mOctave2.Option.isOn = Instrument.InstrumentData.OctavesToUse.Contains( 1 );
			mOctave3.Option.isOn = Instrument.InstrumentData.OctavesToUse.Contains( 2 );

			//update this last as it enables/disables other objects
			mSuccession.Option.value = (int) Instrument.InstrumentData.SuccessionType;
			UpdateTitleColor();
		}

		public void ToggleFXPanel()
		{
			mFXToggle.Option.isOn = mFXToggle.Option.isOn == false;
		}

#endregion public

#region protected

		///<inheritdoc/>
		protected override void InitializeListeners()
		{
			if ( Instrument == null )
			{
				return;
			}

			SetLeadAvoidNoteListeners( initialize: true );

			mUseSevenths.Initialize( ( value ) =>
			{
				var chordSize = value ? 4 : 3;
				mUseSevenths.Text.text = value ? "Dominant 7ths Enabled" : "Dominant 7ths Disabled";
				Instrument.InstrumentData.ChordSize = chordSize;
			}, Instrument.InstrumentData.ChordSize == 4 );

			mArpeggio.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.Arpeggio = value;
				mArpeggio.Text.text = value ? "Arpeggio Enabled" : "Arpeggio Disabled";
				SetSuccessionType( (int) mSuccession.Option.value );
			}, Instrument.InstrumentData.Arpeggio && Instrument.InstrumentData.IsPercussion == false );

			mArpeggioRepeat.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ArpeggioRepeat = (int) value;
				mArpeggioRepeat.Text.text = $"{value}";
			}, Instrument.InstrumentData.ArpeggioRepeat );

			mNumStrumNotes.Initialize( ( value ) =>
				{
					Instrument.InstrumentData.NumStrumNotes = (int) value;
					Instrument.GenerateArpeggioPattern();
					mNumStrumNotes.Text.text = $"{value}";
				}, Instrument.InstrumentData.NumStrumNotes,
				createDividers: true,
				addressableManager: mMusicGenerator.AddressableManager );

			mKeepMelodicRhythm.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.KeepMelodicRhythm = value;
				SetSuccessionType( (int) mSuccession.Option.value );
			}, Instrument.InstrumentData.Arpeggio );

			mPattern.Initialize( ( value ) =>
			{
				mPattern.Option.isOn = value;
				mPattern.Text.text = value ? "Pattern Enabled" : "Pattern Disabled";
				Instrument.InstrumentData.UsePattern = value;
				mPatternRelease.VisibleObject.SetActive( value );
				mPatternLength.VisibleObject.SetActive( value );
				mPatternRepeat.VisibleObject.SetActive( value );

				var activeElements = value ? 4 : 1;
				var padding = mPatternGroup.padding;
				mPatternRect.SetSizeWithCurrentAnchors(
					RectTransform.Axis.Vertical,
					( activeElements - 1 ) * mPatternGroup.spacing +
					activeElements * mElementHeight +
					padding.top +
					padding.bottom
				);
			}, Instrument.InstrumentData.UsePattern && Instrument.InstrumentData.SuccessionType != SuccessionType.Lead );

			mStrumLength.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.StrumLength = value;
				mStrumLength.Text.text = $"{value:0.00}";
			}, Instrument.InstrumentData.StrumLength );

			mReverseStrumToggle.Initialize( ( value ) => { Instrument.InstrumentData.ReverseStrum = value; }, Instrument.InstrumentData.ReverseStrum );

			mStrumVariation.Initialize( ( value ) =>
				{
					Instrument.InstrumentData.StrumVariation = value * mMusicGenerator.InstrumentSet.BeatLength;
					mStrumVariation.Text.text = $"{value:0.00}";
				},
				mMusicGenerator.InstrumentSet.BeatLength == 0
					? 0
					: Instrument.InstrumentData.StrumVariation / mMusicGenerator.InstrumentSet.BeatLength );

			mOddsOfPlaying.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.OddsOfPlaying = (int) value;
				mOddsOfPlaying.Text.text = $"{value}%";
			}, Instrument.InstrumentData.OddsOfPlaying );

			mPentatonic.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.IsPentatonic = value;
				// all avoid notes share a visible object
				OnPentatonicStateChanged();
				SetSuccessionType( (int) mSuccession.Option.value );
			}, Instrument.InstrumentData.IsPentatonic );
			mPentatonic.Option.onValueChanged.AddListener( ( x ) => mUIManager.DirtyEditorDisplays() );

			mTimestep.Initialize( ( value ) =>
				{
					var forceBeat = value < 0;

					ToggleManualBeat( forceBeat );

					if ( forceBeat )
					{
						mTimestep.Text.text = "Manual";
						return;
					}

					Instrument.InstrumentData.TimeStep = (Timestep) ( Enum.GetNames( typeof(Timestep) ).Length - value - 1 );
					mTimestep.Text.text = $"{Instrument.InstrumentData.TimeStep}";
				}, Instrument.InstrumentData.ForceBeat ? -1 : Enum.GetNames( typeof(Timestep) ).Length - (int) Instrument.InstrumentData.TimeStep - 1,
				resetValue: null,
				createDividers: true,
				mUIManager.MusicGenerator.AddressableManager );
			mTimestep.Option.onValueChanged.AddListener( ( x ) => mUIManager.DirtyEditorDisplays() );

			InitializeManualBeatToggles();

			mMinMelodicRhythmTimestep.Initialize( ( value ) =>
				{
					Instrument.InstrumentData.MinMelodicRhythmTimeStep = (Timestep) ( Enum.GetNames( typeof(Timestep) ).Length - value - 1 );
					mMinMelodicRhythmTimestep.Text.text = $"{Instrument.InstrumentData.MinMelodicRhythmTimeStep}";
				}, Enum.GetNames( typeof(Timestep) ).Length - (int) Instrument.InstrumentData.MinMelodicRhythmTimeStep - 1,
				resetValue: null,
				createDividers: true,
				mUIManager.MusicGenerator.AddressableManager );
			mMinMelodicRhythmTimestep.Option.onValueChanged.AddListener( ( x ) => mUIManager.DirtyEditorDisplays() );

			mStereoPan.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.StereoPan = value / 100f;
				mStereoPan.Text.text = $"{value}%";
			}, Instrument.InstrumentData.StereoPan * 100f );

			mPatternRepeat.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.PatternRepeat = (int) value;
				mPatternRepeat.Text.text = $"{value}";
			}, Instrument.InstrumentData.PatternRepeat );

			mPatternRelease.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.PatternRelease = (int) mPatternRelease.Option.value;
				mPatternRelease.Text.text = $"{value}";
			}, Instrument.InstrumentData.PatternRelease );

			mPatternLength.Initialize( ( value ) =>
			{
				mPatternLength.Text.text = $"{value}";
				Instrument.InstrumentData.PatternLength = (int) value;
				mPatternRelease.Option.maxValue = value - 1;
			}, Instrument.InstrumentData.PatternLength );

			mLeadVariation.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.AscendDescendInfluence = value;
				mLeadVariation.Text.text = $"{value}";
			}, Instrument.InstrumentData.AscendDescendInfluence );

			mLeadMaxSteps.Initialize( ( value ) =>
				{
					Instrument.InstrumentData.LeadMaxSteps = (int) value;
					mLeadMaxSteps.Text.text = $"{(int) value}";
				}, Instrument.InstrumentData.LeadMaxSteps,
				resetValue: null,
				createDividers: true,
				mUIManager.MusicGenerator.AddressableManager );

			mSuccessivePlayOdds.Initialize( ( value ) =>
			{
				mSuccessivePlayOdds.Text.text = $"{(int) value}";
				Instrument.InstrumentData.SuccessivePlayOdds = value;
			}, Instrument.InstrumentData.SuccessivePlayOdds );

			mVolume.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.Volume = value / 100f;
				mVolume.Text.text = $"{value}%";
			}, Instrument.InstrumentData.Volume * 100f );

			mEchoFXToggle.Initialize( ( value ) => { mEchoFXPanel.SetActive( value ); }, initialValue: false );
			mReverbFXToggle.Initialize( ( value ) => { mReverbFXPanel.SetActive( value ); }, initialValue: false );
			mFlangeFXToggle.Initialize( ( value ) => { mFlangeFXPanel.SetActive( value ); }, initialValue: false );
			mDistortionFXToggle.Initialize( ( value ) => { mDistortionFXPanel.SetActive( value ); }, initialValue: false );
			mChorusFXToggle.Initialize( ( value ) => { mChorusFXPanel.SetActive( value ); }, initialValue: false );
			mEQFXToggle.Initialize( ( value ) => { mEQFXPanel.SetActive( value ); }, initialValue: false );
			mLowpassFXToggle.Initialize( ( value ) => { mLowpassFXPanel.SetActive( value ); }, initialValue: false );
			mHighpassFXToggle.Initialize( ( value ) => { mHighpassFXPanel.SetActive( value ); }, initialValue: false );

#region protected.reverb

			mReverb.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.Reverb = value;
				mReverb.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( MusicConstants.MixerReverbName + Instrument.InstrumentIndex, value );
			}, Instrument.InstrumentData.Reverb, resetValue: MusicConstants.BaseReverb );

			mRoomSize.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.RoomSize = value;
				mRoomSize.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerRoomSizeName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.RoomSize, resetValue: MusicConstants.BaseReverbRoom );

			mReverbDry.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbDry = value;
				mReverbDry.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbDryName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbDry, resetValue: MusicConstants.BaseReverbDry );

			mReverbDelay.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbDelay = value;
				mReverbDelay.Text.text = $"{value:0.00}s";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbDelayName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbDelay, resetValue: MusicConstants.BaseReverbDelay );


			mReverbRoomHF.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbRoomHF = value;
				mReverbRoomHF.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbRoomHFName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbRoomHF, resetValue: MusicConstants.BaseReverbRoomHF );

			mReverbDecayTime.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbDecayTime = value;
				mReverbDecayTime.Text.text = $"{value:0.00}s";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbDecayTimeName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbDecayTime, resetValue: MusicConstants.BaseReverbDecayTime );

			mReverbDecayHFRatio.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbDecayHFRatio = value;
				mReverbDecayHFRatio.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbDecayHFRatioName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbDecayTime, resetValue: MusicConstants.BaseReverbDecayHFRatio );

			mReverbReflections.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbReflections = value;
				mReverbReflections.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbReflectionsName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbReflections, resetValue: MusicConstants.BaseReverbReflections );

			mReverbReflectDelay.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbReflectDelay = value;
				mReverbReflectDelay.Text.text = $"{value:0.00}s";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbReflectDelayName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbReflectDelay, resetValue: MusicConstants.BaseReverbReflectDelay );

			mReverbDiffusion.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbDiffusion = value;
				mReverbDiffusion.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbDiffusionName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbDiffusion, resetValue: MusicConstants.BaseReverbDiffusion );

			mReverbDensity.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbDensity = value;
				mReverbDensity.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbDensityName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbDensity, resetValue: MusicConstants.BaseReverbDensity );

			mReverbHFReference.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbHFReference = value;
				mReverbHFReference.Text.text = $"{value:0.00}Hz";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbHFReferenceName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbHFReference, resetValue: MusicConstants.BaseReverbHFReference );

			mReverbRoomLF.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbRoomLF = value;
				mReverbRoomLF.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbRoomLFName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbRoomLF, resetValue: MusicConstants.BaseReverbRoomLF );


			mReverbLFReference.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ReverbLFReference = value;
				mReverbLFReference.Text.text = $"{value:0.00}Hz";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerReverbLFReferenceName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ReverbLFReference, resetValue: MusicConstants.BaseReverbLFReference );

#endregion protected.reverb

#region protected.echo

			mEcho.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.Echo = value / 100f;
				mEcho.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerEchoName}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.Echo * 100f, resetValue: MusicConstants.BaseEchoWet * 100f );

			mEchoDry.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.EchoDry = value / 100f;
				mEchoDry.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerEchoDryName}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.EchoDry * 100f, resetValue: MusicConstants.BaseEchoDry * 100f );

			mEchoDelay.Initialize( ( value ) =>
			{
				mEchoDelay.Text.text = $"{value:0.00}ms";
				Instrument.InstrumentData.EchoDelay = value;
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerEchoDelayName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.EchoDelay, resetValue: MusicConstants.BaseEchoDelay * 100f );

			mEchoDecay.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.EchoDecay = value / 100f;
				mEchoDecay.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerEchoDecayName}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.EchoDecay * 100f, resetValue: MusicConstants.BaseEchoDecay * 100f );

#endregion protected.echo

#region protected.flange

			mFlange.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.Flanger = value / 100f;
				mFlange.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerFlangeName}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.Flanger, resetValue: MusicConstants.BaseFlangeWet * 100f );

			mFlangeDry.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.FlangeDry = value / 100f;
				mFlangeDry.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerFlangeDryName}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.FlangeDry * 100f, resetValue: MusicConstants.BaseFlangeDry * 100f );

			mFlangeDepth.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.FlangeDepth = value;
				mFlangeDepth.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerFlangeDepthName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.FlangeDepth, resetValue: MusicConstants.BaseFlangeDepth );

			mFlangeRate.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.FlangeRate = value;
				mFlangeRate.Text.text = $"{value:0.00}Hz";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerFlangeRateName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.FlangeRate, resetValue: MusicConstants.BaseFlangeRate );

#endregion protected.flange

#region protected.distortion

			mDistortion.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.Distortion = value / 100f;
				mDistortion.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerDistortionName}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.Distortion * 100f, resetValue: MusicConstants.BaseDistortion * 100f );

#endregion protected.distortion

#region protected.chorus

			mChorusWetMixTap1.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.Chorus = value / 100f;
				mChorusWetMixTap1.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerChorusName}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.Chorus * 100f, resetValue: MusicConstants.BaseChorusWetMixTap1 * 100f );

			mChorusWetMixTap2.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.Chorus2 = value / 100f;
				mChorusWetMixTap2.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerChorus2Name}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.Chorus2 * 100f, resetValue: MusicConstants.BaseChorusWetMixTap2 * 100f );

			mChorusWetMixTap3.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.Chorus3 = value / 100f;
				mChorusWetMixTap3.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerChorus3Name}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.Chorus3 * 100f, resetValue: MusicConstants.BaseChorusWetMixTap3 * 100f );

			mChorusDry.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ChorusDry = value / 100f;
				mChorusDry.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerChorusDryName}{Instrument.InstrumentIndex}", value / 100f );
			}, Instrument.InstrumentData.ChorusDry * 100f, resetValue: MusicConstants.BaseChorusDry * 100f );

			mChorusDelay.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ChorusDelay = value;
				mChorusDelay.Text.text = $"{value:0.00}ms";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerChorusDelayName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ChorusDelay, resetValue: MusicConstants.BaseChorusDelay );

			mChorusRate.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ChorusRate = value;
				mChorusRate.Text.text = $"{value:0.00}Hz";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerChorusRateName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ChorusRate, resetValue: MusicConstants.BaseChorusRate );

			mChorusDepth.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ChorusDepth = value;
				mChorusDepth.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerChorusDepthName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ChorusDepth, resetValue: MusicConstants.BaseChorusDepth );

			mChorusFeedback.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ChorusFeedback = value;
				mChorusFeedback.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerChorusFeedbackName}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ChorusFeedback, resetValue: MusicConstants.BaseChorusFeedback );

#endregion protected.chorus

#region protected.paramEQ

			mParamEQCenterFreq.Initialize( ( value ) =>
				{
					// This godawful...thing...below, is my clumsy attempt to smooth out unity's logarithmic scale for their audio mixer params.
					// If you know a better way, please let me know. Or, don't. Either/or, I'd almost prefer to never look at this method again :/
					value = Mathf.Pow( 10f, value ) * ( MusicConstants.MaxParamEQCenterFreq * .1f );
					Instrument.InstrumentData.ParamEQCenterFreq = value;
					mParamEQCenterFreq.Text.text = $"{value:0.00}Hz";
					mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerCenterFreq}{Instrument.InstrumentIndex}", value );
				}, Mathf.Log10( Instrument.InstrumentData.ParamEQCenterFreq / ( MusicConstants.MaxParamEQCenterFreq * .1f ) ),
				resetValue: Mathf.Log10( MusicConstants.BaseParamEQCenterFreq / ( MusicConstants.MaxParamEQCenterFreq * .1f ) ) );

			mParamEQFreqGain.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ParamEQFreqGain = value;
				mParamEQFreqGain.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerFreqGain}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ParamEQFreqGain, resetValue: MusicConstants.BaseParamEQFreqGain );

			mParamEQOctaveRange.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.ParamEQOctaveRange = value;
				mParamEQOctaveRange.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerOctaveRange}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.ParamEQOctaveRange, resetValue: MusicConstants.BaseParamEQOctaveRange );

#endregion protected.paramEQ

#region protected.lowpass

			mLowpassCutoffFreq.Initialize( ( value ) =>
				{
					value = Mathf.Pow( 10f, value ) * ( MusicConstants.MaxLowpassCutoffFreq * .1f );
					Instrument.InstrumentData.LowpassCutoffFreq = value;
					mLowpassCutoffFreq.Text.text = $"{value:0.00}Hz";
					mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerLowpassCutoffFreq}{Instrument.InstrumentIndex}", value );
				}, Mathf.Log10( Instrument.InstrumentData.LowpassCutoffFreq / ( MusicConstants.MaxLowpassCutoffFreq * .1f ) ),
				resetValue: Mathf.Log10( MusicConstants.BaseLowpassCutoffFreq / ( MusicConstants.MaxLowpassCutoffFreq * .1f ) ) );

			mLowpassResonance.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.LowpassResonance = value;
				mLowpassResonance.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerLowpassResonance}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.LowpassResonance, resetValue: MusicConstants.BaseLowpassResonance );

#endregion protected.lowpass

#region protected.highpass

			mHighpassCutoffFreq.Initialize( ( value ) =>
				{
					value = Mathf.Pow( 10f, value ) * ( MusicConstants.MaxHighpassCutoffFreq * .1f );
					Instrument.InstrumentData.HighpassCutoffFreq = value;
					mHighpassCutoffFreq.Text.text = $"{value:0.00}Hz";
					mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerHighpassCutoffFreq}{Instrument.InstrumentIndex}", value );
				}, Mathf.Log10( Instrument.InstrumentData.HighpassCutoffFreq / ( MusicConstants.MaxHighpassCutoffFreq * .1f ) ),
				resetValue: Mathf.Log10( MusicConstants.BaseHighpassCutoffFreq / ( MusicConstants.MaxHighpassCutoffFreq * .1f ) ) );

			mHighpassResonance.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.HighpassResonance = value;
				mHighpassResonance.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerHighpassResonance}{Instrument.InstrumentIndex}", value );
			}, Instrument.InstrumentData.HighpassResonance, resetValue: MusicConstants.BaseHighpassResonance );

#endregion protected.highpass

			mOddsOfPlayingChordNote.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.OddsOfUsingChordNotes = value;
				mOddsOfPlayingChordNote.Text.text = $"{value}%";
			}, Instrument.InstrumentData.OddsOfUsingChordNotes );

			mMixerGroupVolume.Initialize( ( value ) =>
			{
				Instrument.InstrumentData.MixerGroupVolume = value;
				mMusicGenerator.UpdateEffectValue( $"{MusicConstants.MixerVolumeName}{Instrument.InstrumentIndex}", value );
				mMixerGroupVolume.Text.text = $"{value:0.00}dB";
			}, Instrument.InstrumentData.MixerGroupVolume );

			mFXToggle.Initialize( ( value ) =>
			{
				mInstrumentPanelVisibleObject.SetActive( value == false );
				mInstrumentEffectVisibleObject.SetActive( value );
			}, initialValue: false );

			mOctavesToUse.Clear();
			AddOctaveListener( ref mOctave1, 0 );
			AddOctaveListener( ref mOctave2, 1 );
			AddOctaveListener( ref mOctave3, 2 );

			//update this last as it enables/disables other objects
			mSuccession.Initialize( ( value ) =>
				{
					var successionType = (SuccessionType) value;
					SetSuccessionType( (int) value );
					LeadOctaveContiguousCheck( successionType == SuccessionType.Lead );
					if ( successionType == SuccessionType.Lead )
					{
						mStrumLength.Option.value = 0;
					}

					Instrument.InstrumentData.SuccessionType = (SuccessionType) value;
					mSuccession.Text.text = $"{Instrument.InstrumentData.SuccessionType}";
				}, (int) Instrument.InstrumentData.SuccessionType,
				resetValue: null,
				createDividers: true,
				mUIManager.MusicGenerator.AddressableManager );
			mSuccession.Option.onValueChanged.AddListener( ( x ) => mUIManager.DirtyEditorDisplays() );

			UpdateTitleColor();

			mListenersHaveInitialized = true;
		}

#endregion protected

#region private

		/// <summary>
		/// List of octaves we're currently using
		/// </summary>
		private readonly List<int> mOctavesToUse = new List<int>();

		[SerializeField, Tooltip( "Reference to our octaves game object" )]
		private GameObject mOctavesParent;

		/// <summary>
		/// Whether our listeners have initialized (to avoid duplicated calls)
		/// </summary>
		private bool mListenersHaveInitialized;

		[SerializeField, Tooltip( "Reference to our FX Toggle" )]
		private UIToggle mFXToggle;

		[SerializeField, Tooltip( "Reference to our Echo FX Toggle" )]
		private UIToggle mEchoFXToggle;

		[SerializeField, Tooltip( "Reference to our echo fx panel" )]
		private GameObject mEchoFXPanel;

		[SerializeField, Tooltip( "Reference to our Reverb FX Toggle" )]
		private UIToggle mReverbFXToggle;

		[SerializeField, Tooltip( "Reference to our reverb fx panel" )]
		private GameObject mReverbFXPanel;

		[SerializeField, Tooltip( "Reference to our Flange FX Toggle" )]
		private UIToggle mFlangeFXToggle;

		[SerializeField, Tooltip( "Reference to our flange fx panel" )]
		private GameObject mFlangeFXPanel;

		[SerializeField, Tooltip( "Reference to our Distortion FX Toggle" )]
		private UIToggle mDistortionFXToggle;

		[SerializeField, Tooltip( "Reference to our distortion fx panel" )]
		private GameObject mDistortionFXPanel;

		[SerializeField, Tooltip( "Reference to our Chorus FX Toggle" )]
		private UIToggle mChorusFXToggle;

		[SerializeField, Tooltip( "Reference to our chorus fx panel" )]
		private GameObject mChorusFXPanel;

		[SerializeField, Tooltip( "Reference to our EQ FX Toggle" )]
		private UIToggle mEQFXToggle;

		[SerializeField, Tooltip( "Reference to our eq fx panel" )]
		private GameObject mEQFXPanel;

		[SerializeField, Tooltip( "Reference to our Lowpass FX Toggle" )]
		private UIToggle mLowpassFXToggle;

		[SerializeField, Tooltip( "Reference to our lowpass fx panel" )]
		private GameObject mLowpassFXPanel;

		[SerializeField, Tooltip( "Reference to our Highpass FX Toggle" )]
		private UIToggle mHighpassFXToggle;

		[SerializeField, Tooltip( "Reference to our highpass fx panel" )]
		private GameObject mHighpassFXPanel;

		[SerializeField, Tooltip( "Reference to our Instrument Panel Object" )]
		private GameObject mInstrumentPanelVisibleObject;

		[SerializeField, Tooltip( "Reference to our FX Panel Object" )]
		private GameObject mInstrumentEffectVisibleObject;

		[SerializeField, Tooltip( "Reference to our Title image" )]
		private Image mInstrumentTitleImage;

		[SerializeField, Tooltip( "Reference to our Title text" )]
		private TMPro.TMP_Text mInstrumentTitleText;

		[SerializeField, Tooltip( "Reference to our odds of playing slider" )]
		private UISlider mOddsOfPlaying;

		[SerializeField, Tooltip( "Reference to our Volume slider" )]
		private UISlider mVolume;

		[SerializeField, Tooltip( "Reference to our pattern length slider" )]
		private UISlider mPatternLength;

		[SerializeField, Tooltip( "Reference to our pattern repeat slider" )]
		private UISlider mPatternRepeat;

		[SerializeField, Tooltip( "Reference to our strum variation slider" )]
		private UISlider mStrumVariation;

		[SerializeField, Tooltip( "Reference to our odds of playing chord slider" )]
		private UISlider mOddsOfPlayingChordNote;

		[SerializeField, Tooltip( "Reference to our successive play odds slider" )]
		private UISlider mSuccessivePlayOdds;

		[SerializeField, Tooltip( "Reference to our lead mad steps slider" )]
		private UISlider mLeadMaxSteps;

		[SerializeField, Tooltip( "Reference to our succession type slider" )]
		private UISlider mSuccession;

		[SerializeField, Tooltip( "Reference to our timestep slider" )]
		private UISlider mTimestep;

		[SerializeField, Tooltip( "Reference to our manual beat object" )]
		private GameObject mManualBeatVisibleObject;

		[SerializeField, Tooltip( "Reference to our list of manual beat toggles" )]
		private List<UIToggle> mManualBeatToggles;

		[SerializeField, Tooltip( "Reference to our Manual Beat Toggle Grid" )]
		private GridLayoutGroup mManualGridLayout;

		[SerializeField, Tooltip( "Reference to our minimum melodic rhythm slider" )]
		private UISlider mMinMelodicRhythmTimestep;

		[SerializeField, Tooltip( "Reference to our pattern toggle" )]
		private UIToggle mPattern;

		[SerializeField, Tooltip( "Reference to the pattern group game object" )]
		private GameObject mPatternParent;

		[SerializeField, Tooltip( "Reference to our stereo pan slider" )]
		private UISlider mStereoPan;

		[SerializeField, Tooltip( "Reference to our mixer group Volume slider" )]
		private UISlider mMixerGroupVolume;

		[SerializeField, Tooltip( "Reference to our pattern release slider" )]
		private UISlider mPatternRelease;

		[SerializeField, Tooltip( "Reference to our Sevenths slider" )]
		private UIToggle mUseSevenths;

		[SerializeField, Tooltip( "Reference to our Strum Length slider" )]
		private UISlider mStrumLength;

		[SerializeField, Tooltip( "Reference to our Reverse Strum Toggle" )]
		private UIToggle mReverseStrumToggle;

		[SerializeField, Tooltip( "Reference to our lead variation slider" )]
		private UISlider mLeadVariation;

		[SerializeField, Tooltip( "Reference to our reverb slider" )]
		private UISlider mReverb;

		[SerializeField, Tooltip( "Reference to our ReverbDry slider" )]
		private UISlider mReverbDry;

		[SerializeField, Tooltip( "Reference to our ReverbRoomHF slider" )]
		private UISlider mReverbRoomHF;

		[SerializeField, Tooltip( "Reference to our ReverbDecayTime slider" )]
		private UISlider mReverbDecayTime;

		[SerializeField, Tooltip( "Reference to our ReverbDecayHFRatio slider" )]
		private UISlider mReverbDecayHFRatio;

		[SerializeField, Tooltip( "Reference to our ReverbReflections slider" )]
		private UISlider mReverbReflections;

		[SerializeField, Tooltip( "Reference to our ReverbReflectDelay slider" )]
		private UISlider mReverbReflectDelay;

		[SerializeField, Tooltip( "Reference to our ReverbDelay slider" )]
		private UISlider mReverbDelay;

		[SerializeField, Tooltip( "Reference to our ReverbDiffusion slider" )]
		private UISlider mReverbDiffusion;

		[SerializeField, Tooltip( "Reference to our ReverbDensity slider" )]
		private UISlider mReverbDensity;

		[SerializeField, Tooltip( "Reference to our ReverbHFReference slider" )]
		private UISlider mReverbHFReference;

		[SerializeField, Tooltip( "Reference to our ReverbRoomLF slider" )]
		private UISlider mReverbRoomLF;

		[SerializeField, Tooltip( "Reference to our ReverbLFReference slider" )]
		private UISlider mReverbLFReference;

		[SerializeField, Tooltip( "Reference to our room size slider" )]
		private UISlider mRoomSize;

		[SerializeField, Tooltip( "Reference to our ChorusDry slider" )]
		private UISlider mChorusDry;

		[SerializeField, Tooltip( "Reference to our ChorusWetMixTap1 slider" )]
		private UISlider mChorusWetMixTap1;

		[SerializeField, Tooltip( "Reference to our ChorusWetMixTap2 slider" )]
		private UISlider mChorusWetMixTap2;

		[SerializeField, Tooltip( "Reference to our ChorusWetMixTap3 slider" )]
		private UISlider mChorusWetMixTap3;

		[SerializeField, Tooltip( "Reference to our ChorusDelay slider" )]
		private UISlider mChorusDelay;

		[SerializeField, Tooltip( "Reference to our ChorusRate slider" )]
		private UISlider mChorusRate;

		[SerializeField, Tooltip( "Reference to our ChorusDepth slider" )]
		private UISlider mChorusDepth;

		[SerializeField, Tooltip( "Reference to our ChorusFeedback slider" )]
		private UISlider mChorusFeedback;

		[SerializeField, Tooltip( "Reference to our ParamEQCenterFreq slider" )]
		private UISlider mParamEQCenterFreq;

		[SerializeField, Tooltip( "Reference to our ParamEQOctaveRange slider" )]
		private UISlider mParamEQOctaveRange;

		[SerializeField, Tooltip( "Reference to our ParamEQFreqGain slider" )]
		private UISlider mParamEQFreqGain;

		[SerializeField, Tooltip( "Reference to our LowpassCutoffFreq slider" )]
		private UISlider mLowpassCutoffFreq;

		[SerializeField, Tooltip( "Reference to our LowpassResonance slider" )]
		private UISlider mLowpassResonance;

		[SerializeField, Tooltip( "Reference to our HighpassCutoffFreq slider" )]
		private UISlider mHighpassCutoffFreq;

		[SerializeField, Tooltip( "Reference to our HighpassResonance slider" )]
		private UISlider mHighpassResonance;

		[SerializeField, Tooltip( "Reference to our Flange slider" )]
		private UISlider mFlange;

		[SerializeField, Tooltip( "Reference to our FlangeDry slider" )]
		private UISlider mFlangeDry;

		[SerializeField, Tooltip( "Reference to our FlangeDepth slider" )]
		private UISlider mFlangeDepth;

		[SerializeField, Tooltip( "Reference to our FlangeRate slider" )]
		private UISlider mFlangeRate;

		[SerializeField, Tooltip( "Reference to our distortion slider" )]
		private UISlider mDistortion;

		[SerializeField, Tooltip( "Reference to our echo slider" )]
		private UISlider mEcho;

		[SerializeField, Tooltip( "Reference to our echo delay slider" )]
		private UISlider mEchoDelay;

		[SerializeField, Tooltip( "Reference to our echo decay slider" )]
		private UISlider mEchoDecay;

		[SerializeField, Tooltip( "Reference to our echo dry slider" )]
		private UISlider mEchoDry;

		[SerializeField, Tooltip( "Reference to our first octave Toggle" )]
		private UIToggle mOctave1;

		[SerializeField, Tooltip( "Reference to our second octave Toggle" )]
		private UIToggle mOctave2;

		[SerializeField, Tooltip( "Reference to our third octave Toggle" )]
		private UIToggle mOctave3;

		[SerializeField, Tooltip( "Reference to our arpeggio Toggle" )]
		private UIToggle mArpeggio;

		[SerializeField, Tooltip( "Reference to our arpeggio repeat slider" )]
		private UISlider mArpeggioRepeat;

		[SerializeField, Tooltip( "Reference to our arpeggio length slider" )]
		private UISlider mNumStrumNotes;

		[SerializeField, Tooltip( "Reference to our pentatonic Toggle" )]
		private UIToggle mPentatonic;

		[SerializeField, Tooltip( "Reference to our keep melodic rhythm Toggle" )]
		private UIToggle mKeepMelodicRhythm;

		[SerializeField, Tooltip( "Reference to our lead avoid step Toggles" )]
		private UIToggle[] mLeadAvoidSteps;

		[SerializeField, Tooltip( "Parent game object to pentatonic avoid toggles" )]
		private GameObject mPentatonicParent;

		[SerializeField, Tooltip( "Reference to our Vertical layout group for succession groups" )]
		private VerticalLayoutGroup mSuccessionGroup;

		[SerializeField, Tooltip( "Reference to our rect transform for succession" )]
		private RectTransform mSuccessionRect;

		[SerializeField, Tooltip( "Reference to our vertical layout group for pattern variables" )]
		private VerticalLayoutGroup mPatternGroup;

		[SerializeField, Tooltip( "Reference to our rect transform for pattern variables" )]
		private RectTransform mPatternRect;

		[SerializeField, Tooltip( "Height of our elements" )]
		private float mElementHeight = 40f;

		/// <summary>
		/// Updates our title color
		/// </summary>
		private void UpdateTitleColor()
		{
			mInstrumentTitleText.text = Instrument.InstrumentData.InstrumentType;
			var titleColor = mUIManager.Colors[(int) Instrument.InstrumentData.StaffPlayerColor];
			mInstrumentTitleText.color = MusicConstants.InvertTextColor( titleColor );
			mInstrumentTitleImage.color = titleColor;
		}

		/// <summary>
		/// Sets our lead avoid note listeners. 
		/// </summary>
		/// <param name="initialize"></param>
		private void SetLeadAvoidNoteListeners( bool initialize )
		{
			var avoidNotes = Instrument.InstrumentData.LeadAvoidNotes;
			var firstAvoid = avoidNotes[0] >= 0 ? MusicConstants.SafeLoop( avoidNotes[0], 0, MusicConstants.ScaleLength ) : -1;
			var secondAvoid = avoidNotes[1] >= 0 ? MusicConstants.SafeLoop( avoidNotes[1], 0, MusicConstants.ScaleLength ) : -1;

			for ( var avoidIndex = 0; avoidIndex < mLeadAvoidSteps.Length; avoidIndex++ )
			{
				var isOn = avoidIndex == firstAvoid || avoidIndex == secondAvoid;
				var toggleIndex = avoidIndex;
				if ( initialize )
				{
					mLeadAvoidSteps[avoidIndex].Initialize( ( value ) => { OnLeadAvoidValueChanged( value, toggleIndex ); }, initialValue: isOn );
					mLeadAvoidSteps[avoidIndex].Option.onValueChanged.AddListener( ( x ) => mUIManager.DirtyEditorDisplays() );
				}
				else
				{
					mLeadAvoidSteps[avoidIndex].Option.isOn = isOn;
				}
			}
		}

		/// <summary>
		/// Adds an octave listener. We want to enforce various conditions on which octaves can be enabled/disabled
		/// </summary>
		/// <param name="toggle"></param>
		/// <param name="index"></param>
		private void AddOctaveListener( ref UIToggle toggle, int index )
		{
			toggle.Initialize( ( value ) =>
			{
				var contains = mOctavesToUse.Contains( index );
				switch ( value )
				{
					case false when contains:
						mOctavesToUse.Remove( index );
						break;
					case true when contains == false:
						mOctavesToUse.Add( index );
						break;
				}

				// safety check. We have to use at least one octave
				if ( mOctavesToUse.Count == 0 )
				{
					mOctavesToUse.Add( 0 );
					mOctave1.Option.isOn = true;
				}

				LeadOctaveContiguousCheck( Instrument.InstrumentData.SuccessionType == SuccessionType.Lead );

				Instrument.InstrumentData.OctavesToUse.Clear();
				foreach ( var octave in mOctavesToUse )
				{
					Instrument.InstrumentData.OctavesToUse.Add( octave );
				}
			}, Instrument.InstrumentData.OctavesToUse.Contains( index ) );
		}

		/// <summary>
		/// Enforces contiguity for lead instruments 
		/// </summary>
		private void LeadOctaveContiguousCheck( bool isLead )
		{
			if ( isLead == false )
			{
				return;
			}

			if ( mOctavesToUse.Contains( 0 ) && mOctavesToUse.Contains( 2 ) )
			{
				mOctave2.Option.isOn = true;
			}
		}

		/// <summary>
		/// Invoked when lead avoid changes, to handle changes to other relevant values 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="avoidIndex"></param>
		private void OnLeadAvoidValueChanged( bool value, int avoidIndex )
		{
			if ( value )
			{
				// We only want 2 avoid notes for any pentatonic scale.
				// we're enforcing here by just grabbing the first two.
				// TODO: Something more elegant? This is pretty bad
				var dataIndex = 0;
				for ( var avoidStepIndex = 0; avoidStepIndex < mLeadAvoidSteps.Length; avoidStepIndex++ )
				{
					if ( mLeadAvoidSteps[avoidStepIndex].Option.isOn == false )
					{
						continue;
					}

					if ( dataIndex < MusicConstants.NumPentatonicAvoids )
					{
						var avoidNote = avoidStepIndex;
						Instrument.InstrumentData.LeadAvoidNotes[dataIndex] = avoidNote;
						dataIndex++;
					}
					else
					{
						mLeadAvoidSteps[avoidStepIndex].Option.isOn = false;
					}
				}
			}
			else
			{
				if ( Instrument.InstrumentData.LeadAvoidNotes[0] == avoidIndex )
				{
					Instrument.InstrumentData.LeadAvoidNotes[0] = -1;
				}
				else if ( Instrument.InstrumentData.LeadAvoidNotes[1] == avoidIndex )
				{
					Instrument.InstrumentData.LeadAvoidNotes[1] = -1;
				}
			}
		}

		/// <summary>
		/// Invoked when pentatonic state changes to handle relevant changes
		/// </summary>
		private void OnPentatonicStateChanged()
		{
			if ( Instrument.InstrumentData.IsPentatonic == false )
			{
				return;
			}

			foreach ( var toggle in mLeadAvoidSteps )
			{
				toggle.Option.isOn = false;
			}

			// If our 'standard' pentatonic is enabled, force values to generic pentatonic.
			var newPentatonic = mMusicGenerator.ConfigurationData.Scale == Scale.Major ||
			                    mMusicGenerator.ConfigurationData.Scale == Scale.HarmonicMajor
				? MusicConstants.MajorPentatonicAvoid
				: MusicConstants.MinorPentatonicAvoid;

			for ( var index = 0; index < mLeadAvoidSteps.Length; index++ )
			{
				var isOn =
					index == MusicConstants.SafeLoop( newPentatonic[0], 0, MusicConstants.ScaleLength ) ||
					index == MusicConstants.SafeLoop( newPentatonic[1], 0, MusicConstants.ScaleLength );

				mLeadAvoidSteps[index].Option.isOn = isOn;
			}
		}

		/// <summary>
		/// Sets our succession type. We enabled/disable a ton of dependent elements conditionally here
		/// </summary>
		/// <param name="successionType"></param>
		private void SetSuccessionType( int successionType )
		{
			var isRhythm = successionType == (int) SuccessionType.Rhythm;
			var isLead = successionType == (int) SuccessionType.Lead;
			var isPercussion = Instrument.InstrumentData.IsPercussion;
			var activeElements = 1;

			mOddsOfPlaying.VisibleObject.SetActive( isRhythm == false );
			mSuccessivePlayOdds.VisibleObject.SetActive( isRhythm == false );
			mKeepMelodicRhythm.VisibleObject.SetActive( isRhythm == false );
			activeElements += isRhythm == false ? 3 : 0;

			mPentatonic.VisibleObject.SetActive( isLead );
			activeElements += isLead ? 1 : 0;

			mPentatonicParent.SetActive( isLead && mPentatonic.Option.isOn == false );
			activeElements += isLead && mPentatonic.Option.isOn == false ? 1 : 0;

			mLeadMaxSteps.VisibleObject.SetActive( isLead && isPercussion == false );
			mLeadVariation.VisibleObject.SetActive( isLead && isPercussion == false );
			activeElements += isLead && isPercussion == false ? 2 : 0;

			mMinMelodicRhythmTimestep.VisibleObject.SetActive( isRhythm == false && mKeepMelodicRhythm.Option.isOn );
			activeElements += isRhythm == false && mKeepMelodicRhythm.Option.isOn ? 1 : 0;

			mOddsOfPlayingChordNote.VisibleObject.SetActive( isPercussion == false );
			activeElements += isPercussion == false ? 1 : 0;

			mStrumLength.VisibleObject.SetActive( isLead == false && isPercussion == false );
			mReverseStrumToggle.VisibleObject.SetActive( isLead == false && isPercussion == false );
			mStrumVariation.VisibleObject.SetActive( isLead == false && isPercussion == false );
			mArpeggio.VisibleObject.SetActive( isLead == false && isPercussion == false );
			mNumStrumNotes.VisibleObject.SetActive( isLead == false && isPercussion == false );
			activeElements += isLead == false && isPercussion == false ? 5 : 0;

			mArpeggioRepeat.VisibleObject.SetActive( isLead == false && mArpeggio.Option.isOn && isPercussion == false );
			activeElements += isLead == false && mArpeggio.Option.isOn && isPercussion == false ? 1 : 0;

			mOctavesParent.SetActive( isPercussion == false );

			// true in all succession types
			// mOddsOfPlayingChordNote.VisibleObject.SetActive( true );

			var padding = mSuccessionGroup.padding;
			mSuccessionRect.SetSizeWithCurrentAnchors(
				RectTransform.Axis.Vertical,
				( activeElements - 1 ) * mSuccessionGroup.spacing +
				activeElements * mElementHeight +
				padding.top +
				padding.bottom
			);

			if ( mUIManager.MeasureEditorIsActive )
			{
				mUIManager.UIMeasureEditor.ToggleHelperNotes();
			}

			mPatternParent.SetActive( isLead == false );
		}

		private void InitializeManualBeatToggles()
		{
			for ( var index = 0; index < mManualBeatToggles.Count; index++ )
			{
				var beatIndex = index;
				mManualBeatToggles[index].Initialize( ( value ) => { Instrument.InstrumentData.ForcedBeats[beatIndex] = value; },
					Instrument.InstrumentData.ForcedBeats[beatIndex] );
			}
		}

		private void ToggleManualBeat( bool isEnabled )
		{
			Instrument.InstrumentData.ForceBeat = isEnabled;
			mManualBeatVisibleObject.gameObject.SetActive( isEnabled );

			if ( isEnabled == false )
			{
				return;
			}

			var numBeats = 0;
			switch ( mMusicGenerator.InstrumentSet.TimeSignature.Signature )
			{
				case TimeSignatures.ThreeFour:
					numBeats = 12;
					break;
				case TimeSignatures.FourFour:
					numBeats = 16;
					break;
				case TimeSignatures.FiveFour:
					numBeats = 20;
					break;
			}

			mManualGridLayout.constraintCount = numBeats / 2;

			for ( var index = 0; index < 20; index++ )
			{
				mManualBeatToggles[index].Option.isOn = Instrument.InstrumentData.ForcedBeats[index];
				mManualBeatToggles[index].gameObject.SetActive( index < numBeats );
			}
		}

#endregion private
	}
}
