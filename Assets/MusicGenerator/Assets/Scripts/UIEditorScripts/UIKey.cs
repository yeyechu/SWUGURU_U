using UnityEngine;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// Physical/visual key on the keyboard for the generator's editor
	/// </summary>
	public class UIKey : MonoBehaviour
	{
		/// <summary>
		/// Reference to the UIKey's transform
		/// </summary>
		public Transform Transform { get; private set; }

		/// <summary>
		/// Initialization
		/// </summary>
		/// <param name="uiManager"></param>
		public void Initialize( UIManager uiManager )
		{
			Transform = transform;
			mUIManager = uiManager;
		}

		/// <summary>
		/// Plays the UIKey's lights/particles
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="strength"></param>
		/// <param name="color"></param>
		public void Play( float duration, int strength, Color color )
		{
			mLight.enabled = mLightsAreEnabled;
			mLight.intensity = 0f;
			var mainModule = mParticle.main;
			mainModule.startSpeedMultiplier = mMinStrength + ( strength * mVerticalScaleMultiplier );
			mainModule.startColor = color;
			mIsPlaying = true;
			mTimer = 0;
			mPlayDuration = duration;
			mLight.color = color;
			if ( mParticlesAreEnabled )
			{
				mParticle.Play();
			}
		}

		public void Stop()
		{
			mParticle.Stop();
		}

		public void ShowLightHighlight( Color color )
		{
			mLight.color = color;
			mLight.enabled = mLightsAreEnabled;
			mLight.intensity = mMaxLightIntensity;
		}

		/// <summary>
		/// Toggle's whether lights are used for the ui key
		/// </summary>
		/// <param name="lightsAreEnabled"></param>
		public void ToggleLights( bool lightsAreEnabled )
		{
			mLightsAreEnabled = lightsAreEnabled;
		}

		/// <summary>
		/// Toggles whether particles are used for the ui key
		/// </summary>
		/// <param name="particlesAreEnabled"></param>
		public void ToggleParticles( bool particlesAreEnabled )
		{
			mParticlesAreEnabled = particlesAreEnabled;
		}

		/// <summary>
		/// Manual update loop
		/// </summary>
		/// <param name="deltaTime"></param>
		public void DoUpdate( float deltaTime )
		{
			if ( mTimer < mPlayDuration && mIsPlaying )
			{
				mTimer += deltaTime;
				if ( mTimer < mLightRampTime )
				{
					mLight.intensity = mMaxLightIntensity * mLightCurve.Evaluate( mTimer / mLightRampTime );
				}
				else
				{
					mLight.intensity = mMaxLightIntensity;
				}
			}
			else
			{
				if ( mLight.intensity > 0 )
				{
					mTimer -= deltaTime;
					mLight.intensity = mMaxLightIntensity * mLightCurve.Evaluate( mTimer / mLightRampTime );
				}
				else
				{
					mLight.enabled = false;
					mLight.intensity = 0f;
				}

				mIsPlaying = false;
			}
		}

		/// <summary>
		/// Reference to our ui manager
		/// </summary>
		private UIManager mUIManager;

		/// <summary>
		/// Current timer value
		/// </summary>
		private float mTimer;

		/// <summary>
		/// Current playing state of the ui key
		/// </summary>
		private bool mIsPlaying;

		/// <summary>
		/// Duration of the play effect
		/// </summary>
		private float mPlayDuration;

		/// <summary>
		/// Whether lights are enabled
		/// </summary>
		private bool mLightsAreEnabled = true;

		/// <summary>
		/// Whether particles are enabled
		/// </summary>
		private bool mParticlesAreEnabled = true;

		[SerializeField, Tooltip( "maximum light intensity for our key light" )]
		private float mMaxLightIntensity = 10f;

		[SerializeField, Tooltip( "Animation curve to use for lights" )]
		private AnimationCurve mLightCurve;

		[SerializeField, Tooltip( "Reference to our ui key's light" )]
		private Light mLight;

		[SerializeField, Tooltip( "Reference to our ui key's particle system" )]
		private ParticleSystem mParticle;

		[SerializeField, Tooltip( "Ramp up time for lights (sampled against our animation curve" )]
		private float mLightRampTime = .25f;

		[SerializeField, Tooltip( "Vertical scale multiplier for particle effects" )]
		private float mVerticalScaleMultiplier = 15f;

		[SerializeField, Tooltip( "Minimum strength used for the particle effect" )]
		private float mMinStrength = 3f;

		[SerializeField, Tooltip( "Note index to which this key corresponds" )]
		private int mNoteIndex;

		[SerializeField, Tooltip( "Duration for the light when pressed" )]
		private float mKeyPressLightDuration = .5f;

		[SerializeField, Tooltip( "Strength for the light when pressed" )]
		private int mKeypressLightStrength = 3;

		[SerializeField, Tooltip( "Color for the light when pressed" )]
		private Color mKeypressLightColor = Color.white;

		/// <summary>
		/// OnMouseDown (we handle the playing of the key/lights/particle manually
		/// </summary>
		private void OnMouseDown()
		{
			if ( mUIManager.MusicGenerator.GeneratorState == GeneratorState.Playing ||
			     mUIManager.MusicGenerator.GeneratorState == GeneratorState.Repeating ||
			     mUIManager.InstrumentListPanelUI.SelectedInstrument == null )
			{
				return;
			}

			Play( mKeyPressLightDuration, mKeypressLightStrength, mKeypressLightColor );
			var instrumentName = mUIManager.InstrumentListPanelUI.SelectedInstrument.InstrumentData.InstrumentType;
			var volume = mUIManager.InstrumentListPanelUI.SelectedInstrument.InstrumentData.Volume;
			var instrumentIndex = mUIManager.InstrumentListPanelUI.SelectedInstrument.InstrumentIndex;
			var noteIndex = mUIManager.InstrumentListPanelUI.SelectedInstrument.InstrumentData.IsPercussion ? 0 : mNoteIndex;
			mUIManager.MusicGenerator.PlayAudioClip( mUIManager.CurrentInstrumentSet, instrumentName, noteIndex, volume, instrumentIndex );
		}
	}
}
