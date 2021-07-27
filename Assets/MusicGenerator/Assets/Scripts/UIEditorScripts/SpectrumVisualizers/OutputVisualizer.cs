using UnityEngine;

#pragma warning disable 0649

namespace ProcGenMusic
{
	public class OutputVisualizer : MonoBehaviour
	{
		/// <summary>
		/// Sets our visualizer line color
		/// </summary>
		/// <param name="color"></param>
		public void SetColor( Color color )
		{
			mAlphaVector.z = mOutputRenderer.startColor.a;
			mOutputRenderer.startColor = color * mAlphaVector;
			mOutputRenderer.endColor = color * mAlphaVector;
		}

		/// <summary>
		/// Toggles our visibility
		/// </summary>
		/// <param name="isOn"></param>
		public void ToggleFadeState( bool isOn )
		{
			if ( isOn )
			{
				mLineFader.FadeIn();
			}
			else
			{
				mLineFader.FadeOut();
			}
		}

		[SerializeField, Tooltip( "Reference to our output line renderer" )]
		private LineRenderer mOutputRenderer;

		[SerializeField, Range( .1f, 1f ), Tooltip( "Width multiplier of our line renderer" )]
		private float mOutputWidthScale = .29f;

		[SerializeField, Range( 1f, 100f ), Tooltip( "Height multiplier of our line renderer" )]
		private float mOutputHeightScale = 30f;

		private Vector4 mAlphaVector = Vector4.one;

		[SerializeField, Tooltip( "Reference to our line renderer" )]
		private LineFader mLineFader;

		/// <summary>
		/// Current position cache
		/// </summary>
		private readonly Vector3[] mPositions = new Vector3[AudioVisualizer.SAMPLE_SIZE];

		/// <summary>
		/// current scale cache
		/// </summary>
		private float[] mScaleValues;

		/// <summary>
		/// Sample size
		/// </summary>
		private readonly int mSampleSize = AudioVisualizer.SAMPLE_SIZE;

		/// <summary>
		/// Awake
		/// </summary>
		private void Awake()
		{
			mScaleValues = new float[mSampleSize];
		}

		/// <summary>
		/// Update
		/// </summary>
		private void Update()
		{
			var min = float.MaxValue;
			var max = 0f;
			for ( var index = 0; index < mPositions.Length; index++ )
			{
				var value = AudioVisualizer.OutputData[index];
				min = value < min ? value : min;
				max = value > max ? value : max;
				mScaleValues[index] = value;
			}

			min = Mathf.Max( min, float.MinValue );
			max = Mathf.Max( max, .0001f );
			for ( var index = 0; index < mSampleSize; index++ )
			{
				var value = ( mScaleValues[index] - min ) / max;
				mPositions[index] = new Vector3( index * mOutputWidthScale, value * mOutputHeightScale, 1 );
			}

			mOutputRenderer.positionCount = mPositions.Length;
			mOutputRenderer.SetPositions( mPositions );
		}
	}
}
