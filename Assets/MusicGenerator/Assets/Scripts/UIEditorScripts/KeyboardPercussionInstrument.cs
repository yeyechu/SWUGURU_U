using UnityEngine;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// Handles visual representation of percussion instruments on the keyboard/piano roller
	/// </summary>
	public class KeyboardPercussionInstrument : MonoBehaviour, IKeyboardNoteDisplay
	{
#region public

		///<inheritdoc/>
		public Transform Transform { get; private set; }

		/// <summary>
		/// Initializes the KeyboardPercussionInstrument, setting color and injecting references
		/// </summary>
		/// <param name="uiKeyboard"></param>
		/// <param name="uiManager"></param>
		/// <param name="color"></param>
		public void Initialize( UIKeyboard uiKeyboard, UIManager uiManager, Color color )
		{
			mUIKeyboard = uiKeyboard;
			mUIManager = uiManager;
			UpdateColor( color );
			Transform = transform;
		}

		///<inheritdoc/>
		public void UpdateColor( Color color )
		{
			// we oversaturate our notes a bit
			Color.RGBToHSV( color, out var h, out var s, out var v );
			var saturation = color.Equals( Color.white ) ? 0f : mNoteSaturation;
			mColor = Color.HSVToRGB( h, saturation, v );
			mSpriteRenderer.material.SetColor( MaterialColor, mColor );
		}

		/// <summary>
		/// Updates the size and position of the Percussion Instrument
		/// </summary>
		/// <param name="position"></param>
		/// <param name="size"></param>
		public void UpdateSizeAndPosition( Vector3 position, Vector2 size )
		{
			Transform.localPosition = position;
			mSpriteRenderer.size = size;
		}

		///<inheritdoc/>
		public void Stop()
		{
			mEmissionMultiplier = mUIManager.FXSettings.FallingNoteEmissionIntensityFloor;
			mSpriteRenderer.material.SetColor( EmissionColor, mColor * Mathf.LinearToGammaSpace( mEmissionMultiplier ) );
		}

		///<inheritdoc/>
		public void Play( Vector3 position, Color color, bool particlesEnabled )
		{
			mEmissionMultiplier = mUIKeyboard.EmissionPulseIntensity;
		}

#endregion public

#region private

		[SerializeField, Tooltip( "Reference to our sprite renderer" )]
		private SpriteRenderer mSpriteRenderer;

		[SerializeField, Tooltip( "Amount of saturation for the notes" )]
		private float mNoteSaturation = .85f;

		private static readonly int EmissionColor = Shader.PropertyToID( "_EmissionColor" );

		/// <summary>
		/// Reference to the  UIKeyboard
		/// </summary>
		private UIKeyboard mUIKeyboard;

		/// <summary>
		/// Reference to our emission multiplier
		/// </summary>
		private float mEmissionMultiplier;

		/// <summary>
		/// Reference to the UIManager
		/// </summary>
		private UIManager mUIManager;

		/// <summary>
		/// The color of this percussion instrument
		/// </summary>
		private Color mColor;

		private static readonly int MaterialColor = Shader.PropertyToID( "_Color" );

		/// <summary>
		/// Update
		/// </summary>
		private void Update()
		{
			var emissionFloor = mUIManager.FXSettings.FallingNoteEmissionIntensityFloor;
			if ( mEmissionMultiplier > emissionFloor )
			{
				mEmissionMultiplier -= Time.deltaTime * mUIKeyboard.EmissionPulseIntensityFalloff;
				mSpriteRenderer.material.SetColor( EmissionColor, mColor * Mathf.LinearToGammaSpace( mEmissionMultiplier ) );
			}
			else
			{
				mEmissionMultiplier = emissionFloor;
			}
		}

#endregion private
	}
}
