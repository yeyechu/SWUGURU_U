﻿using UnityEngine;
using System;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// scriptable object to for fx settings for the editor
	/// </summary>
	[CreateAssetMenu( fileName = "FXSettings", menuName = "ProcGenMusic/FX/FXSettings", order = 1 )]
	public class UIEditorFXSettings : ScriptableObject
	{
		[Serializable]
		public enum UIEditorStyle
		{
			None = 0,
			PianoRoll = 1,
			VisualizerOnly = 2,
			PianoRollAndVisualizer = 3,
			ReversePianoRoll = 4,
		}

		[SerializeField]
		public UIEditorStyle UIStyle = UIEditorStyle.PianoRoll;

		[SerializeField, Tooltip( "Emissive intensity for visualizers" )]
		public float VisualizerEmissiveIntensity = 3f;

		[SerializeField, HideInInspector]
		public int ColorPaletteIndex;

		[SerializeField, Tooltip( "Whether the UI Keyboard will use falling note particles" )]
		public bool UseFallingNoteParticles = true;

		[SerializeField, Tooltip( "Whether the keyboard will play particles when notes hit it" )]
		public bool UseKeyboardParticles = true;

		[SerializeField, Tooltip( "Whether the keyboard keys will light up when notes hit it" )]
		public bool UseKeyboardLights = true;

		[SerializeField, Tooltip( "Whether falling notes will 'pulse' with the beat" )]
		public bool UseFallingNotePulse = true;

		[SerializeField, Tooltip( "Min value for the falling note emission intensity" )]
		public float FallingNoteEmissionIntensityFloor = 0f;

		[SerializeField, Tooltip( "Our falling note spin value" )]
		public Vector3 FallingNoteSpinValue = new Vector3( 1f, 1f, 1f );

		[SerializeField, Range( 1f, 100f ), Tooltip( "How fast the notes in the editor will fall" )]
		public float FallingNoteSpeed = 10f;

		[SerializeField, Tooltip( "Whether bloom is enabled in the ui editor" )]
		public bool BloomIsEnabled = true;

		[SerializeField, Range( 1, 5 ), Tooltip( "Multiplier for note spin" )]
		public int NoteSpinMultiplier = 1;
	}
}
