using UnityEngine;
using UnityEngine.AddressableAssets;

#pragma warning disable 0649

namespace ProcGenMusic
{
	[CreateAssetMenu( fileName = "DemoParameters", menuName = "ProcGenMusic/Demo/DemoParameters", order = 1 )]
	public class DemoParameters : ScriptableObject
	{
		public AssetReference mSpawnReference;
		public float mLightDecreaseMultiplier = 1f;
		public float mBounceLightIntensity = 1f;
		public float mBounceLightEmissiveIntensity = 1f;
		public ColorPalette mColorPalette;
		public float mBounceForce = 5f;
		public float mGravity = -20f;
		public float mMinLightIntensity = 1f;
	}
}
