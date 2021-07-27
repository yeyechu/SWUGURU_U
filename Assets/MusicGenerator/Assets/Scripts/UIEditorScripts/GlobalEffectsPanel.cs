﻿using UnityEngine;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// Settings panel controlling the Global Effects
	/// </summary>
	public class GlobalEffectsPanel : UIPanel
	{
#region public

		///<inheritdoc/>
		public override void UpdateUIElementValues()
		{
			//reverb:
			mReverb.Option.value = mMusicGenerator.ConfigurationData.Reverb;
			mRoomSize.Option.value = mMusicGenerator.ConfigurationData.RoomSize;
			mReverbDecay.Option.value = mMusicGenerator.ConfigurationData.ReverbDecay;
			mReverbDry.Option.value = mMusicGenerator.ConfigurationData.ReverbDry;
			mReverbDensity.Option.value = mMusicGenerator.ConfigurationData.ReverbDensity;
			mReverbDelay.Option.value = mMusicGenerator.ConfigurationData.ReverbDelay;
			mReverbRoomHF.Option.value = mMusicGenerator.ConfigurationData.ReverbRoomHF;
			mReverbDecayHFRatio.Option.value = mMusicGenerator.ConfigurationData.ReverbDecayHFRatio;
			mReverbReflections.Option.value = mMusicGenerator.ConfigurationData.ReverbReflections;
			mReverbReflectDelay.Option.value = mMusicGenerator.ConfigurationData.ReverbReflectDelay;
			mReverbDiffusion.Option.value = mMusicGenerator.ConfigurationData.ReverbDiffusion;
			mReverbHFReference.Option.value = mMusicGenerator.ConfigurationData.ReverbHFReference;
			mReverbRoomLF.Option.value = mMusicGenerator.ConfigurationData.ReverbRoomLF;
			mReverbLFReference.Option.value = mMusicGenerator.ConfigurationData.ReverbLFReference;

			//echo
			mEchoWet.Option.value = mMusicGenerator.ConfigurationData.EchoWet * 100f;
			mEchoDry.Option.value = mMusicGenerator.ConfigurationData.EchoDry * 100f;
			mEchoDelay.Option.value = mMusicGenerator.ConfigurationData.EchoDelay;
			mEchoDecay.Option.value = mMusicGenerator.ConfigurationData.EchoDecay * 100f;

			//flange
			mFlange.Option.value = mMusicGenerator.ConfigurationData.Flanger * 100f;
			mFlangeDry.Option.value = mMusicGenerator.ConfigurationData.FlangeDry * 100f;
			mFlangeDepth.Option.value = mMusicGenerator.ConfigurationData.FlangeDepth;
			mFlangeRate.Option.value = mMusicGenerator.ConfigurationData.FlangeRate;

			//distortion:
			mDistortion.Option.value = mMusicGenerator.ConfigurationData.Distortion * 100f;

			//chorus:
			mChorusWetMixTap1.Option.value = mMusicGenerator.ConfigurationData.Chorus * 100f;
			mChorusWetMixTap2.Option.value = mMusicGenerator.ConfigurationData.Chorus2 * 100f;
			mChorusWetMixTap3.Option.value = mMusicGenerator.ConfigurationData.Chorus3 * 100f;
			mChorusDry.Option.value = mMusicGenerator.ConfigurationData.ChorusDry * 100f;
			mChorusDelay.Option.value = mMusicGenerator.ConfigurationData.ChorusDelay;
			mChorusRate.Option.value = mMusicGenerator.ConfigurationData.ChorusRate;
			mChorusDepth.Option.value = mMusicGenerator.ConfigurationData.ChorusDepth;
			mChorusFeedback.Option.value = mMusicGenerator.ConfigurationData.ChorusFeedback;

			//paramEQ:
			mParamEQCenterFreq.Option.value = Mathf.Log10( mMusicGenerator.ConfigurationData.ParamEQCenterFreq / ( MusicConstants.MaxParamEQCenterFreq * .1f ) );
			mParamEQFreqGain.Option.value = mMusicGenerator.ConfigurationData.ParamEQFreqGain;
			mParamEQOctaveRange.Option.value = mMusicGenerator.ConfigurationData.ParamEQOctaveRange;

			//lowpass:
			mLowpassCutoffFreq.Option.value = Mathf.Log10( mMusicGenerator.ConfigurationData.LowpassCutoffFreq / ( MusicConstants.MaxLowpassCutoffFreq * .1f ) );
			mLowpassResonance.Option.value = mMusicGenerator.ConfigurationData.LowpassResonance;

			//highpass:
			mHighpassCutoffFreq.Option.value = Mathf.Log10( mMusicGenerator.ConfigurationData.HighpassCutoffFreq / ( MusicConstants.MaxHighpassCutoffFreq * .1f ) );
			mHighpassResonance.Option.value = mMusicGenerator.ConfigurationData.HighpassResonance;
		}

#endregion public

#region protected

		///<inheritdoc/>
		protected override void InitializeListeners()
		{
#region protected.panelToggles

			mEchoFXToggle.Initialize( ( value ) => { mEchoFXPanel.SetActive( value ); }, initialValue: false );
			mReverbFXToggle.Initialize( ( value ) => { mReverbFXPanel.SetActive( value ); }, initialValue: false );
			mFlangeFXToggle.Initialize( ( value ) => { mFlangeFXPanel.SetActive( value ); }, initialValue: false );
			mDistortionFXToggle.Initialize( ( value ) => { mDistortionFXPanel.SetActive( value ); }, initialValue: false );
			mChorusFXToggle.Initialize( ( value ) => { mChorusFXPanel.SetActive( value ); }, initialValue: false );
			mEQFXToggle.Initialize( ( value ) => { mEQFXPanel.SetActive( value ); }, initialValue: false );
			mLowpassFXToggle.Initialize( ( value ) => { mLowpassFXPanel.SetActive( value ); }, initialValue: false );
			mHighpassFXToggle.Initialize( ( value ) => { mHighpassFXPanel.SetActive( value ); }, initialValue: false );

#endregion protected.panelToggles

#region protected.reverb

			mReverb.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.Reverb = value;
				mReverb.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbName}", value );
			}, mMusicGenerator.ConfigurationData.Reverb, resetValue: MusicConstants.BaseReverb );

			mRoomSize.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.RoomSize = value;
				mRoomSize.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerRoomSizeName}", value );
			}, mMusicGenerator.ConfigurationData.RoomSize, resetValue: MusicConstants.BaseReverbRoom );

			mReverbDry.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbDry = value;
				mReverbDry.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbDryName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbDry, resetValue: MusicConstants.BaseReverbDry );

			mReverbDelay.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbDelay = value;
				mReverbDelay.Text.text = $"{value:0.00}s";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbDelayName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbDelay, resetValue: MusicConstants.BaseReverbDelay );

			mReverbRoomHF.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbRoomHF = value;
				mReverbRoomHF.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbRoomHFName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbRoomHF, resetValue: MusicConstants.BaseReverbRoomHF );

			mReverbDecay.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbDecay = value;
				mReverbDecay.Text.text = $"{value:0.00}s";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbDecayTimeName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbDecay, resetValue: MusicConstants.BaseReverbDecayTime );

			mReverbDecayHFRatio.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbDecayHFRatio = value;
				mReverbDecayHFRatio.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbDecayHFRatioName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbDecayHFRatio, resetValue: MusicConstants.BaseReverbDecayHFRatio );

			mReverbReflections.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbReflections = value;
				mReverbReflections.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbReflectionsName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbReflections, resetValue: MusicConstants.BaseReverbReflections );

			mReverbReflectDelay.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbReflectDelay = value;
				mReverbReflectDelay.Text.text = $"{value:0.00}s";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbReflectDelayName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbReflectDelay, resetValue: MusicConstants.BaseReverbReflectDelay );

			mReverbDiffusion.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbDiffusion = value;
				mReverbDiffusion.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbDiffusionName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbDiffusion, resetValue: MusicConstants.BaseReverbDiffusion );

			mReverbDensity.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbDensity = value;
				mReverbDensity.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbDensityName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbDensity, resetValue: MusicConstants.BaseReverbDensity );

			mReverbHFReference.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbHFReference = value;
				mReverbHFReference.Text.text = $"{value:0.00}Hz";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbHFReferenceName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbHFReference, resetValue: MusicConstants.BaseReverbHFReference );

			mReverbRoomLF.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbRoomLF = value;
				mReverbRoomLF.Text.text = $"{value:0.00}mB";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbRoomLFName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbRoomLF, resetValue: MusicConstants.BaseReverbRoomLF );


			mReverbLFReference.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ReverbLFReference = value;
				mReverbLFReference.Text.text = $"{value:0.00}Hz";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerReverbLFReferenceName}", value );
			}, mMusicGenerator.ConfigurationData.ReverbLFReference, resetValue: MusicConstants.BaseReverbLFReference );

#endregion protected.reverb

#region protected.echo

			mEchoWet.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.EchoWet = value / 100f;
				mEchoWet.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerEchoName}", value / 100f );
			}, mMusicGenerator.ConfigurationData.EchoWet * 100f, resetValue: MusicConstants.BaseEchoWet * 100f );

			mEchoDry.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.EchoDry = value / 100f;
				mEchoDry.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerEchoDryName}", value / 100f );
			}, mMusicGenerator.ConfigurationData.EchoDry * 100f, resetValue: MusicConstants.BaseEchoDry * 100f );

			mEchoDelay.Initialize( ( value ) =>
			{
				mEchoDelay.Text.text = $"{value:0.00}ms";
				mMusicGenerator.ConfigurationData.EchoDelay = value;
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerEchoDelayName}", value );
			}, mMusicGenerator.ConfigurationData.EchoDelay, resetValue: MusicConstants.BaseEchoDelay * 100f );

			mEchoDecay.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.EchoDecay = value / 100f;
				mEchoDecay.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerEchoDecayName}", value / 100f );
			}, mMusicGenerator.ConfigurationData.EchoDecay * 100f, resetValue: MusicConstants.BaseEchoDecay * 100f );

#endregion protected.echo

#region protected.flange

			mFlange.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.Flanger = value / 100f;
				mFlange.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerFlangeName}", value / 100f );
			}, mMusicGenerator.ConfigurationData.Flanger, resetValue: MusicConstants.BaseFlangeWet * 100f );

			mFlangeDry.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.FlangeDry = value / 100f;
				mFlangeDry.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerFlangeDryName}", value / 100f );
			}, mMusicGenerator.ConfigurationData.FlangeDry * 100f, resetValue: MusicConstants.BaseFlangeDry * 100f );

			mFlangeDepth.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.FlangeDepth = value;
				mFlangeDepth.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerFlangeDepthName}", value );
			}, mMusicGenerator.ConfigurationData.FlangeDepth, resetValue: MusicConstants.BaseFlangeDepth );

			mFlangeRate.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.FlangeRate = value;
				mFlangeRate.Text.text = $"{value:0.00}Hz";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerFlangeRateName}", value );
			}, mMusicGenerator.ConfigurationData.FlangeRate, resetValue: MusicConstants.BaseFlangeRate );

#endregion protected.flange

#region protected.distortion

			mDistortion.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.Distortion = value / 100f;
				mDistortion.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerDistortionName}", value / 100f );
			}, mMusicGenerator.ConfigurationData.Distortion * 100f, resetValue: MusicConstants.BaseDistortion * 100f );

#endregion protected.distortion

#region protected.chorus

			mChorusWetMixTap1.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.Chorus = value / 100f;
				mChorusWetMixTap1.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerChorusName}", value / 100f );
			}, mMusicGenerator.ConfigurationData.Chorus * 100f, resetValue: MusicConstants.BaseChorusWetMixTap1 * 100f );

			mChorusWetMixTap2.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.Chorus2 = value / 100f;
				mChorusWetMixTap2.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerChorus2Name}", value / 100f );
			}, mMusicGenerator.ConfigurationData.Chorus2 * 100f, resetValue: MusicConstants.BaseChorusWetMixTap2 * 100f );

			mChorusWetMixTap3.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.Chorus3 = value / 100f;
				mChorusWetMixTap3.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerChorus3Name}", value / 100f );
			}, mMusicGenerator.ConfigurationData.Chorus3 * 100f, resetValue: MusicConstants.BaseChorusWetMixTap3 * 100f );

			mChorusDry.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ChorusDry = value / 100f;
				mChorusDry.Text.text = $"{value:0.00}%";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerChorusDryName}", value / 100f );
			}, mMusicGenerator.ConfigurationData.ChorusDry * 100f, resetValue: MusicConstants.BaseChorusDry * 100f );

			mChorusDelay.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ChorusDelay = value;
				mChorusDelay.Text.text = $"{value:0.00}ms";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerChorusDelayName}", value );
			}, mMusicGenerator.ConfigurationData.ChorusDelay, resetValue: MusicConstants.BaseChorusDelay );

			mChorusRate.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ChorusRate = value;
				mChorusRate.Text.text = $"{value:0.00}Hz";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerChorusRateName}", value );
			}, mMusicGenerator.ConfigurationData.ChorusRate, resetValue: MusicConstants.BaseChorusRate );

			mChorusDepth.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ChorusDepth = value;
				mChorusDepth.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerChorusDepthName}", value );
			}, mMusicGenerator.ConfigurationData.ChorusDepth, resetValue: MusicConstants.BaseChorusDepth );

			mChorusFeedback.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ChorusFeedback = value;
				mChorusFeedback.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerChorusFeedbackName}", value );
			}, mMusicGenerator.ConfigurationData.ChorusFeedback, resetValue: MusicConstants.BaseChorusFeedback );

#endregion protected.chorus

#region protected.paramEQ

			mParamEQCenterFreq.Initialize( ( value ) =>
				{
					// This godawful...thing...below, is my clumsy attempt to smooth out unity's logarithmic scale for their audio mixer params.
					// If you know a better way, please let me know. Or, don't. Either/or, I'd almost prefer to never look at this method again :/
					value = Mathf.Pow( 10f, value ) * ( MusicConstants.MaxParamEQCenterFreq * .1f );
					mMusicGenerator.ConfigurationData.ParamEQCenterFreq = value;
					mParamEQCenterFreq.Text.text = $"{value:0.00}Hz";
					mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerCenterFreq}", value );
				}, Mathf.Log10( mMusicGenerator.ConfigurationData.ParamEQCenterFreq / ( MusicConstants.MaxParamEQCenterFreq * .1f ) ),
				resetValue: Mathf.Log10( MusicConstants.BaseParamEQCenterFreq / ( MusicConstants.MaxParamEQCenterFreq * .1f ) ) );

			mParamEQFreqGain.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ParamEQFreqGain = value;
				mParamEQFreqGain.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerFreqGain}", value );
			}, mMusicGenerator.ConfigurationData.ParamEQFreqGain, resetValue: MusicConstants.BaseParamEQFreqGain );

			mParamEQOctaveRange.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.ParamEQOctaveRange = value;
				mParamEQOctaveRange.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerOctaveRange}", value );
			}, mMusicGenerator.ConfigurationData.ParamEQOctaveRange, resetValue: MusicConstants.BaseParamEQOctaveRange );

#endregion protected.paramEQ

#region protected.lowpass

			mLowpassCutoffFreq.Initialize( ( value ) =>
				{
					value = Mathf.Pow( 10f, value ) * ( MusicConstants.MaxLowpassCutoffFreq * .1f );
					mMusicGenerator.ConfigurationData.LowpassCutoffFreq = value;
					mLowpassCutoffFreq.Text.text = $"{value:0.00}Hz";
					mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerLowpassCutoffFreq}", value );
				}, Mathf.Log10( mMusicGenerator.ConfigurationData.LowpassCutoffFreq / ( MusicConstants.MaxLowpassCutoffFreq * .1f ) ),
				resetValue: Mathf.Log10( MusicConstants.BaseLowpassCutoffFreq / ( MusicConstants.MaxLowpassCutoffFreq * .1f ) ) );

			mLowpassResonance.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.LowpassResonance = value;
				mLowpassResonance.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerLowpassResonance}", value );
			}, mMusicGenerator.ConfigurationData.LowpassResonance, resetValue: MusicConstants.BaseLowpassResonance );

#endregion protected.lowpass

#region protected.highpass

			mHighpassCutoffFreq.Initialize( ( value ) =>
				{
					value = Mathf.Pow( 10f, value ) * ( MusicConstants.MaxHighpassCutoffFreq * .1f );
					mMusicGenerator.ConfigurationData.HighpassCutoffFreq = value;
					mHighpassCutoffFreq.Text.text = $"{value:0.00}Hz";
					mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerHighpassCutoffFreq}", value );
				}, Mathf.Log10( mMusicGenerator.ConfigurationData.HighpassCutoffFreq / ( MusicConstants.MaxHighpassCutoffFreq * .1f ) ),
				resetValue: Mathf.Log10( MusicConstants.BaseHighpassCutoffFreq / ( MusicConstants.MaxHighpassCutoffFreq * .1f ) ) );

			mHighpassResonance.Initialize( ( value ) =>
			{
				mMusicGenerator.ConfigurationData.HighpassResonance = value;
				mHighpassResonance.Text.text = $"{value:0.00}";
				mMusicGenerator.UpdateEffectValue( $"Master{MusicConstants.MixerHighpassResonance}", value );
			}, mMusicGenerator.ConfigurationData.HighpassResonance, resetValue: MusicConstants.BaseHighpassResonance );

#endregion protected.highpass
		}

#endregion protected

#region private

#region private.panelToggles

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

#endregion private.panelToggles

		[Tooltip( "Slider to control global Lowpass Cutoff Frequency" )]
		[SerializeField] private UISlider mLowpassCutoffFreq;

		[Tooltip( "Slider to control global Lowpass Resonance" )]
		[SerializeField] private UISlider mLowpassResonance;

		[Tooltip( "Slider to control global Highpass Cutoff Frequency" )]
		[SerializeField] private UISlider mHighpassCutoffFreq;

		[Tooltip( "Slider to control global Highpass Resonance" )]
		[SerializeField] private UISlider mHighpassResonance;

		[Tooltip( "Slider to control global Echo Delay" )]
		[SerializeField] private UISlider mEchoDelay;

		[Tooltip( "Slider to control global Echo Decay" )]
		[SerializeField] private UISlider mEchoDecay;

		[Tooltip( "Slider to control global Echo Dry" )]
		[SerializeField] private UISlider mEchoDry;

		[Tooltip( "Slider to control global Echo Wet" )]
		[SerializeField] private UISlider mEchoWet;

		//reverb
		[Tooltip( "Slider to control global Reverb" )]
		[SerializeField] private UISlider mReverb;

		[Tooltip( "Slider to control global Reverb Decay" )]
		[SerializeField] private UISlider mReverbDecay;

		[Tooltip( "Slider to control global Room Size" )]
		[SerializeField] private UISlider mRoomSize;

		[SerializeField, Tooltip( "Reference to our ReverbDry slider" )]
		private UISlider mReverbDry;

		[SerializeField, Tooltip( "Reference to our ReverbRoomHF slider" )]
		private UISlider mReverbRoomHF;

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

		//flange
		[SerializeField, Tooltip( "Reference to our Flange slider" )]
		private UISlider mFlange;

		[SerializeField, Tooltip( "Reference to our FlangeDry slider" )]
		private UISlider mFlangeDry;

		[SerializeField, Tooltip( "Reference to our FlangeDepth slider" )]
		private UISlider mFlangeDepth;

		[SerializeField, Tooltip( "Reference to our FlangeRate slider" )]
		private UISlider mFlangeRate;

		//distortion 
		[SerializeField, Tooltip( "Reference to our distortion slider" )]
		private UISlider mDistortion;

		//chorus
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

		//param eq
		[SerializeField, Tooltip( "Reference to our ParamEQCenterFreq slider" )]
		private UISlider mParamEQCenterFreq;

		[SerializeField, Tooltip( "Reference to our ParamEQOctaveRange slider" )]
		private UISlider mParamEQOctaveRange;

		[SerializeField, Tooltip( "Reference to our ParamEQFreqGain slider" )]
		private UISlider mParamEQFreqGain;

#endregion private
	}
}
