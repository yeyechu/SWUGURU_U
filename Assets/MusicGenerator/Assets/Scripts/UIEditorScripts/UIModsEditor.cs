using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#pragma warning disable 0649

namespace ProcGenMusic
{
	public class UIModsEditor : UIPanel
	{
		///<inheritdoc/>
		public override void UpdateUIElementValues()
		{
			UpdateEnabledMods();
		}

		///<inheritdoc/>
		protected override void InitializeListeners()
		{
			foreach ( var mod in mMusicGenerator.Mods )
			{
				mMusicGenerator.AddressableManager.SpawnAddressableInstance( m_modUIObject, new AddressableSpawnRequest( Vector3.zero, Quaternion.identity, ( result ) =>
				{
					var modObject = result.GetComponent<ModUIObject>();
					if ( modObject != false )
					{
						modObject.Initialize( mod, mUIManager );
						modObject.transform.localScale = Vector3.one;
						mInstantiatedModObjects.Add( modObject );
					}
				}, mModParentTransform ) );
			}
		}

		private void UpdateEnabledMods()
		{
			foreach ( var mod in mInstantiatedModObjects )
			{
				mod.Toggle( mMusicGenerator.ConfigurationData.Mods.Contains( mod.ModName ) );
			}
		}

		[SerializeField, Tooltip( "Reference to the mod parent transform" )]
		private Transform mModParentTransform;

		[SerializeField, Tooltip( "Asset Reference to our mod ui object" )]
		private AssetReference m_modUIObject;

		/// <summary>
		/// Our instantiated objects
		/// </summary>
		private List<ModUIObject> mInstantiatedModObjects = new List<ModUIObject>();
	}
}
