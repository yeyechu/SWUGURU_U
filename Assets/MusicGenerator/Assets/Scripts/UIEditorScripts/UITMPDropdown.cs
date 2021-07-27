using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable 0649

namespace ProcGenMusic
{
	[Serializable]
	public class TMPDropdownOption : PanelOption<UITMPDropdownWrapper>
	{
	}

	/// <summary>
	/// UI Element for dropdown panels
	/// </summary>
	public class UITMPDropdown : MonoBehaviour, IPanelOption<UITMPDropdownWrapper>
	{
		[SerializeField]
		private bool mShouldUpdatePosition = true;

		///<inheritdoc/>
		public UITMPDropdownWrapper Option => mOptions.Option;

		///<inheritdoc/>
		public TMP_Text Text => mOptions.Text;

		///<inheritdoc/>
		public TMP_Text Title => mOptions.Title;

		///<inheritdoc/>
		public GameObject VisibleObject => mOptions.VisibleObject;

		///<inheritdoc/>
		public Tooltip Tooltip => mOptions.Tooltip;

		/// <summary>
		/// Initializes the UI Dropdown
		/// </summary>
		/// <param name="action"></param>
		/// <param name="initialValue"></param>
		public void Initialize( UnityAction<int> action, int? initialValue = null )
		{
			Option.ToggleShouldUpdatePosition( mShouldUpdatePosition );
			Option.onValueChanged.RemoveAllListeners();
			Option.onValueChanged.AddListener( action );
			if ( initialValue.HasValue )
			{
				Option.value = initialValue.Value;
				action.Invoke( initialValue.Value );
			}
		}

		[SerializeField]
		private TMPDropdownOption mOptions;

		/// <summary>
		/// OnDestroy
		/// </summary>
		private void OnDestroy()
		{
			Option.onValueChanged.RemoveAllListeners();
		}
	}
}
