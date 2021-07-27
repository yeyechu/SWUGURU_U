using UnityEngine;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// The falling note object in the generator keyboard display
	/// </summary>
	public class UIFallingNote : MonoBehaviour, IKeyboardNoteDisplay
	{
#region public

		/// <summary>
		/// Reference to the falling note's transform
		/// </summary>
		public Transform Transform => mTransform;

		/// <summary>
		/// Falling note enabled state
		/// </summary>
		public bool IsEnabled { get; private set; }

		/// <summary>
		/// Plays the falling note. This will InstrumentSet the position, color, etc.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="color"></param>
		/// <param name="particlesEnabled"></param>
		public void Play( Vector3 position, Color color, bool particlesEnabled )
		{
			mBaseObject.SetActive( true );
			mTransform.position = position;
			mMeshRenderer.enabled = true;
			SetColor( color );

			IsEnabled = true;
		}

		/// <summary>
		/// Updates the color of the falling note
		/// </summary>
		/// <param name="color"></param>
		public void UpdateColor( Color color )
		{
			SetColor( color );
		}

		/// <summary>
		/// Stops the falling note (this will InstrumentSet the entire game object and particles inactive)
		/// </summary>
		public void Stop()
		{
			mMeshRenderer.enabled = false;
			mBaseObject.SetActive( false );
			IsEnabled = false;
		}

		/// <summary>
		/// Manual update loop. For the falling notes, this handles rotation/color
		/// </summary>
		/// <param name="emissionMultiplier"></param>
		public void DoUpdate( float emissionMultiplier )
		{
			mMeshRenderer.material.SetColor( EmissionColor, mColor * Mathf.LinearToGammaSpace( emissionMultiplier ) );
		}

#endregion public

#region private

		[SerializeField, Tooltip( "Reference to our Mesh renderer for the falling note" )]
		private MeshRenderer mMeshRenderer;

		[SerializeField, Tooltip( "Reference to our base game object for the falling note" )]
		private GameObject mBaseObject;

		[SerializeField, Tooltip( "Reference to our Transform for the falling note" )]
		private Transform mTransform;

		[SerializeField, Tooltip( "Reference to our note color saturation" )]
		private float mNoteSaturation = .85f;

		/// <summary>
		/// static reference to emission color id
		/// </summary>
		private static readonly int EmissionColor = Shader.PropertyToID( "_EmissionColor" );

		/// <summary>
		/// Note color
		/// </summary>
		private Color mColor;

		/// <summary>
		/// Sets the falling note color
		/// </summary>
		/// <param name="color"></param>
		private void SetColor( Color color )
		{
			// we oversaturate our notes a bit
			Color.RGBToHSV( color, out var h, out var s, out var v );
			var saturation = color.Equals( Color.white ) ? 0f : mNoteSaturation;
			mColor = Color.HSVToRGB( h, saturation, v );
			mMeshRenderer.material.SetColor( "_Color", mColor );
		}

#endregion private
	}
}
