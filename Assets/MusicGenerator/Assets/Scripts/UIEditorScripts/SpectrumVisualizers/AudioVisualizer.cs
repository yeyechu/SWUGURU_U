using UnityEngine;

#pragma warning disable 0649

namespace ProcGenMusic
{
	public class AudioVisualizer : MonoBehaviour
	{
		/// <summary>
		///  Our sample size for the spectrum and output visualizers
		/// </summary>
		public static int SAMPLE_SIZE = 1024;

		public void DirtyVisuals()
		{
			mSpectrumVisualizer.SetColor( mUIManager.CurrentColors[(int) ColorFieldType.UI_1].Color );
			mOutputVisualizer.SetColor( mUIManager.CurrentColors[(int) ColorFieldType.UI_3].Color );
			mDBVisualizer.SetColor( mUIManager.CurrentColors[(int) ColorFieldType.UI_3].Color );
		}

		public void FadeOut()
		{
			mDBVisualizer.ToggleFadeState( false );
			mSpectrumVisualizer.ToggleFadeState( false );
			mOutputVisualizer.ToggleFadeState( false );
		}

		public void FadeIn()
		{
			mDBVisualizer.ToggleFadeState( true );
			mSpectrumVisualizer.ToggleFadeState( true );
			mOutputVisualizer.ToggleFadeState( true );
		}

		/// <summary>
		/// Array of our current spectrum data
		/// </summary>
		public static float[] SpectrumData { get; } = new float[SAMPLE_SIZE];

		/// <summary>
		/// Array of our current output data
		/// </summary>
		public static float[] OutputData { get; } = new float[SAMPLE_SIZE];

		/// <summary>
		/// Current RMS value
		/// </summary>
		public static float RMSValue { get; private set; }

		/// <summary>
		/// Current dB Value
		/// </summary>
		public static float DBValue { get; private set; }

		/// <summary>
		/// Current pitch value
		/// </summary>
		public static float PitchValue { get; private set; }

		private float mSampleRate;

		[SerializeField, Tooltip( "dB Scale" )]
		private float mDbScale = 0.1f;

		[SerializeField, Tooltip( "Reference to our ui manager" )]
		private UIManager mUIManager;

		[SerializeField, Tooltip( "Reference to our spectrum visualizer" )]
		private SpectrumVisualizer mSpectrumVisualizer;

		[SerializeField, Tooltip( "Reference to our dB Visualizer" )]
		private DBVisualizer mDBVisualizer;

		[SerializeField, Tooltip( "Reference to our output visualizer" )]
		private OutputVisualizer mOutputVisualizer;

		/// <summary>
		/// Awake.
		/// </summary>
		private void Awake()
		{
			mSampleRate = AudioSettings.outputSampleRate;
		}

		/// <summary>
		/// Update.
		/// </summary>
		private void Update()
		{
			AudioListener.GetSpectrumData( SpectrumData, 0, FFTWindow.Hamming );
			AudioListener.GetOutputData( OutputData, 0 );

			var sum = 0f;
			foreach ( var t in OutputData )
			{
				sum += t * t;
			}

			if ( OutputData.Length > 0 )
			{
				RMSValue = Mathf.Sqrt( sum / OutputData.Length );
				DBValue = 20f * Mathf.Log10( RMSValue / mDbScale );
				GetPitch();
			}
		}

		/// <summary>
		/// Returns current pitch
		/// </summary>
		private void GetPitch()
		{
			var maxV = 0f;
			var maxN = 0;
			for ( var index = 0; index < SpectrumData.Length; index++ )
			{
				if ( SpectrumData[index] > maxV && SpectrumData[index] > 0 )
				{
					maxV = SpectrumData[index];
					maxN = index;
				}
			}

			float freqN = maxN;
			if ( maxN > 0 && maxN < SpectrumData.Length - 1 )
			{
				var dL = SpectrumData[maxN - 1] / SpectrumData[maxN];
				var dR = SpectrumData[maxN + 1] / SpectrumData[maxN];
				freqN += .5f * ( dR * dR - dL * dL );
			}

			PitchValue = freqN * ( mSampleRate / 2 ) / SAMPLE_SIZE;
		}
	}
}
