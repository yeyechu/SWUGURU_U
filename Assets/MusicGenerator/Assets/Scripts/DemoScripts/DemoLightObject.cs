using UnityEngine;

#pragma warning disable 0649

namespace ProcGenMusic
{
	public class DemoLightObject : MonoBehaviour
	{
		public Transform Transform => mTransform;

		protected DemoParameters mDemoParameters;

		[SerializeField] protected Light mLight;
		private Material mMaterial;

		private float mEmissiveIntensity;
		private Color mColor;
		private bool mIsInitialized;
		private Transform mTransform;
		private static readonly int EmissionColor = Shader.PropertyToID( "_EmissionColor" );

		public void Initialize( Color color, DemoParameters demoParameters )
		{
			mTransform = transform;
			mDemoParameters = demoParameters;
			mColor = color;
			mMaterial = GetComponent<MeshRenderer>().material;
			mLight.intensity = 0f;
			mLight.color = mColor;
			mIsInitialized = true;
		}

		public virtual void Select()
		{
			mLight.intensity = mDemoParameters.mBounceLightIntensity;
			mEmissiveIntensity = mDemoParameters.mBounceLightEmissiveIntensity;
		}

		private void Update()
		{
			if ( mIsInitialized )
			{
				DoUpdate();
			}
		}

		protected virtual void DoUpdate()
		{
			if ( mLight.intensity > mDemoParameters.mMinLightIntensity )
			{
				mLight.intensity -= mDemoParameters.mLightDecreaseMultiplier * Time.deltaTime;
			}
			else
			{
				mLight.intensity = mDemoParameters.mMinLightIntensity;
			}

			if ( mEmissiveIntensity > 0 )
			{
				mEmissiveIntensity -= mDemoParameters.mLightDecreaseMultiplier * Time.deltaTime;
				mMaterial.SetColor( EmissionColor, mColor * Mathf.LinearToGammaSpace( mEmissiveIntensity ) );
			}
			else
			{
				mEmissiveIntensity = 0;
				mMaterial.SetColor( EmissionColor, mColor * Mathf.LinearToGammaSpace( mEmissiveIntensity ) );
			}
		}
	}
}
