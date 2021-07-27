using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#pragma warning disable 0649

namespace ProcGenMusic
{
	/// <summary>
	/// Manager for the generator's visual keyboard
	/// </summary>
	public class UIKeyboard : MonoBehaviour
	{
#region public

		/// <summary>
		/// Keyboard play mode
		/// </summary>
		public enum PlayMode
		{
			Normal,
			LeitmotifInstrument,
			LeitmotifPercussion,
			Percussion,
			Clip,
			ClipPercussion
		}

		/// <summary>
		/// Our current playmode
		/// </summary>
		public PlayMode CurrentPlayMode => mPlayMode;

		/// <summary>
		/// Reference to our emission pulse intensity to use for falling notes
		/// </summary>
		public float EmissionPulseIntensity => mEmissionPulseIntensity;

		/// <summary>
		/// Reference to our emission pulse intensity falloff to use for falling notes
		/// </summary>
		public float EmissionPulseIntensityFalloff => mEmissionPulseIntensityFalloff;

		public float EmissionMultiplier => mEmissionMultiplier;

		/// <summary>
		/// Mutes keyboard. This is mostly used when routing audio through another library
		/// </summary>
		/// <param name="isMuted"></param>
		public void ToggleKeyboardMute( bool isMuted )
		{
			mKeyboardIsMuted = isMuted;
		}

		/// <summary>
		/// Initialization
		/// </summary>
		/// <param name="uiManager"></param>
		public void Initialize( UIManager uiManager )
		{
			mIsInitialized = true;
			mUIManager = uiManager;
			mMusicGenerator = mUIManager.MusicGenerator;
			mMusicGenerator.NotePlayed += OnNotePlayed;
			mMusicGenerator.GroupsWereChosen.AddListener( OnGroupsWereChosen );
			mMusicGenerator.InstrumentAdded.AddListener( OnInstrumentAdded );
			mMusicGenerator.InstrumentWillBeRemoved.AddListener( OnInstrumentWillBeRemoved );
			InitializePercussionInstruments();
			CreateParticlePool();
			InitializeKeys();
			mKeyboardLight.FadeIn();
			mUIManager.UIEditorSettings.OnUseKeyboardLightsChanged.AddListener( OnUseKeyboardLightsChanged );
			mUIManager.UIEditorSettings.OnUseKeyboardParticlesChanged.AddListener( OnUseKeyboardParticlesChanged );
		}

		/// <summary>
		/// Stops the UIKeyboard
		/// </summary>
		public void Stop()
		{
			Stop( true );
		}

		/// <summary>
		/// Stops the UI Keyboard
		/// </summary>
		/// <param name="fadeLight"></param>
		public void Stop( bool fadeLight )
		{
			if ( mMusicGenerator == false )
			{
				return;
			}

			mAudioVisualizer.FadeOut();
			Reset();
			mPlayMode = PlayMode.Normal;
			if ( fadeLight )
			{
				mKeyboardLight.FadeIn();
			}
			else
			{
				mKeyboardLight.FadeOut();
			}
		}

		/// <summary>
		/// Pauses the UI Keyboard
		/// </summary>
		public void Pause()
		{
			mMusicGenerator.Pause();
			mKeyboardLight.FadeIn();
		}

		/// <summary>
		/// Plays the UI Keyboard (note: this will also play the generator)
		/// </summary>
		public void Play()
		{
			Play( PlayMode.Normal );
		}

		/// <summary>
		/// Plays the UI Keyboard (note: this will also play the generator for the given mode passed in)
		/// </summary>
		/// <param name="playMode"></param>
		public void Play( PlayMode playMode )
		{
			mMusicGenerator.Play();
			mKeyboardLight.FadeOut();
			mPlayMode = playMode;

			if ( playMode == PlayMode.Percussion ||
			     playMode == PlayMode.LeitmotifPercussion ||
			     playMode == PlayMode.ClipPercussion )
			{
				ForcePercussionGroupsPlaying();
				mAudioVisualizer.FadeOut();
			}
			else
			{
				mAudioVisualizer.FadeIn();
			}
		}

		/// <summary>
		/// Plays a single clip (as opposed to the random generation)
		/// </summary>
		/// <param name="isPercussion"></param>
		public void PlayClip( bool isPercussion )
		{
			mKeyboardLight.FadeOut();
			mPlayMode = isPercussion ? PlayMode.ClipPercussion : PlayMode.Clip;
		}

		public void PlayKeyLight( int keyIndex, Color color )
		{
			mKeys[keyIndex].ShowLightHighlight( color );
		}

		/// <summary>
		/// Sets the display dirty. WIll update colors for the instruments 
		/// </summary>
		public void DirtyDisplay()
		{
			foreach ( var instrument in mQueuedNotes )
			{
				var instrumentColor = mUIManager.Colors[(int) instrument.Key.InstrumentData.StaffPlayerColor];
				foreach ( var note in instrument.Value )
				{
					note.UpdateColor( instrumentColor );
				}
			}

			foreach ( var percussionInstrument in mKeyboardPercussionInstruments )
			{
				var instrumentColor =
					mUIManager.Colors[(int) percussionInstrument.Key.InstrumentData.StaffPlayerColor];
				percussionInstrument.Value.UpdateColor( instrumentColor );
			}
		}

		/// <summary>
		/// Updates the editor style
		/// </summary>
		public void UpdateStyle()
		{
			Stop();
			mAudioVisualizer.DirtyVisuals();
			mGravity = cNormalGravity;
			mNoteEndPos = mBaseNoteEndPosition.position.y;
			mNoteStartPos = mBaseNoteStartPosition.position.y;
			switch ( mUIManager.FXSettings.UIStyle )
			{
				case UIEditorFXSettings.UIEditorStyle.None:
					mVisualizerGameObject.SetActive( false );
					break;
				case UIEditorFXSettings.UIEditorStyle.PianoRoll:
					mVisualizerGameObject.SetActive( false );
					break;
				case UIEditorFXSettings.UIEditorStyle.VisualizerOnly:
					mVisualizerGameObject.SetActive( true );
					break;
				case UIEditorFXSettings.UIEditorStyle.PianoRollAndVisualizer:
					mVisualizerGameObject.SetActive( true );
					break;
				case UIEditorFXSettings.UIEditorStyle.ReversePianoRoll:
					mGravity = cReverseGravity;
					mNoteEndPos = mBaseNoteStartPosition.position.y;
					mNoteStartPos = mBaseNoteEndPosition.position.y;
					mVisualizerGameObject.SetActive( false );
					break;
			}
		}

		/// <summary>
		/// Returns the horizontal offset for a given key
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public float GetKeyHorizontalOffset( int index )
		{
			return mKeys[index].Transform.position.x;
		}

		/// <summary>
		/// Returns the horizontal offset for a key given a position
		/// </summary>
		/// <param name="position"></param>
		/// <param name="keyIndex"></param>
		/// <returns></returns>
		public float GetKeyHorizontalOffset( Vector3 position, out int keyIndex )
		{
			keyIndex = 0;
			var closestDistance = float.MaxValue;
			var closestPosition = 0f;
			for ( var index = 0; index < mKeys.Length; index++ )
			{
				var distance = Mathf.Abs( position.x - mKeys[index].Transform.position.x );
				if ( distance < closestDistance )
				{
					closestDistance = distance;
					closestPosition = mKeys[index].Transform.position.x;
					keyIndex = index;
				}
			}

			return closestPosition;
		}

		/// <summary>
		/// Adds an instrument to our keyboard
		/// </summary>
		/// <param name="instrument"></param>
		public void AddInstrument( Instrument instrument )
		{
			if ( mQueuedNotes.ContainsKey( instrument ) == false )
			{
				mQueuedNotes.Add( instrument, new List<QueuedNote>() );
			}
		}

		/// <summary>
		/// Resets the keyboard display (will reset all notes)
		/// </summary>
		public void Reset()
		{
			if ( mMusicGenerator.GeneratorState != GeneratorState.Initializing )
			{
				mUIManager.MusicGenerator.Stop();
			}

			foreach ( var instrument in mQueuedNotes )
			{
				foreach ( var note in instrument.Value )
				{
					note.NoteDisplay.Stop();
				}

				instrument.Value.Clear();
			}

			foreach ( var key in mKeys )
			{
				key.Stop();
			}

			ClearPlayedNotes();
		}

		/// <summary>
		/// Manual Update loop
		/// </summary>
		/// <param name="deltaTime"></param>
		public void DoUpdate( float deltaTime )
		{
			if ( mIsInitialized == false )
			{
				return;
			}

			switch ( mPlayMode )
			{
				case PlayMode.Normal:
				case PlayMode.LeitmotifInstrument:
				case PlayMode.Clip:
					UpdateNormalMode( deltaTime );
					break;
			}
		}

		/// <summary>
		/// Clears and destroys all percussion instruments
		/// </summary>
		public void ClearPercussionInstruments()
		{
			foreach ( var percussionInstrument in mKeyboardPercussionInstruments )
			{
				Destroy( percussionInstrument.Value.gameObject );
			}

			mKeyboardPercussionInstruments.Clear();
		}

#endregion public

#region private

		/// <summary>
		/// Data for a queued note
		/// </summary>
		private class QueuedNote
		{
			public QueuedNote( Instrument instrument, NotePlayedArgs args, IKeyboardNoteDisplay noteDisplay,
				Vector3 position, Color color, bool useParticles = true, float? timeOffset = null )
			{
				Timer = timeOffset ?? 0f;
				InstrumentSet = args.InstrumentSet;
				Color = color;
				InstrumentName = args.InstrumentName;
				Note = args.Note;
				NoteDisplay = noteDisplay;
				Instrument = instrument;
				UseParticles = useParticles;

				// Percussion notes don't have the falling note display and the queued note is used mostly for data/timing.
				if ( instrument.InstrumentData.IsPercussion == false )
				{
					NoteDisplay.Play( position, color, useParticles );
				}
			}

			/// <summary>
			/// Updates the color of a queued note
			/// </summary>
			/// <param name="color"></param>
			public void UpdateColor( Color color )
			{
				Color = color;
				if ( Instrument.InstrumentData.IsPercussion == false )
				{
					NoteDisplay.UpdateColor( color );
				}
			}

			public float Timer;
			public Color Color { get; private set; }
			public readonly IKeyboardNoteDisplay NoteDisplay;
			public readonly InstrumentSet InstrumentSet;
			public readonly string InstrumentName;
			public readonly int Note;
			public readonly Instrument Instrument;
			public readonly bool UseParticles;
		}

		[SerializeField, Tooltip( "Reference to our audio visualizer" )]
		private AudioVisualizer mAudioVisualizer;

		[SerializeField, Tooltip( "Reference to our spectrum/output visualizer game object" )]
		private GameObject mVisualizerGameObject;

		[SerializeField, Tooltip( "Reference to the UI Keys" )]
		private UIKey[] mKeys;

		[SerializeField, Tooltip( "Reference to the background" )]
		private SpriteRenderer mBackground;

		[SerializeField, Tooltip( "Reference to our asset reference for falling notes" )]
		private AssetReference mFallingNote;

		[SerializeField, Tooltip( "Maximum number of particles to be used" )]
		private int mMaxNumParticles = 150;

		[SerializeField, Tooltip( "Reference to the transform to which we parent falling notes" )]
		private Transform mFallingNoteTransform;

		[SerializeField, Tooltip( "Reference to the transform where we will start falling notes" )]
		private Transform mBaseNoteStartPosition;

		private float mNoteStartPos;

		[SerializeField, Tooltip( "Reference to the transform where we will stop falling notes" )]
		private Transform mBaseNoteEndPosition;

		private float mNoteEndPos;

		[SerializeField, Tooltip( "Reference to the Fader for keyboard lights" )]
		private Fader mKeyboardLight;

		[SerializeField, Range( 1f, 10f ), Tooltip( "Intensity for pulsing lights on falling notes" )]
		private float mEmissionPulseIntensity = 5f;

		[SerializeField, Range( 1f, 50f ), Tooltip( "Falling for the pulsing lights on falling notes" )]
		private float mEmissionPulseIntensityFalloff = 5f;

		[SerializeField, Tooltip( "Reference to the transform to which we'll parent the keyboard percussion instruments" )]
		private Transform mKeyboardPercussionParent;

		[SerializeField, Tooltip( "Asset reference from which we'll spawn our percussion instruments" )]
		private AssetReference mKeyboardPercussionInstrumentBase;

		private readonly Dictionary<Instrument, KeyboardPercussionInstrument> mKeyboardPercussionInstruments =
			new Dictionary<Instrument, KeyboardPercussionInstrument>();

		[SerializeField, Range( .01f, 1f ), Tooltip( "Height of the percussion instruments display" )]
		private float mPercussionDisplayHeight = .2f;

		/// <summary>
		/// Current gravity multiplier (this is ultimately based on fx settings
		/// </summary>
		private float mGravityMultiplier = 1f;

		/// <summary>
		/// Reference to our music generator
		/// </summary>
		private MusicGenerator mMusicGenerator;

		/// <summary>
		/// Container for notes to be removed (we need to temporarily cache them)
		/// </summary>
		private readonly List<QueuedNote> mNotesToRemove = new List<QueuedNote>();

		/// <summary>
		/// Container for our queued notes to play
		/// </summary>
		private readonly Dictionary<Instrument, List<QueuedNote>> mQueuedNotes = new Dictionary<Instrument, List<QueuedNote>>();

		/// <summary>
		/// Pool of falling notes.
		/// </summary>
		private UIFallingNote[] mParticlePool;

		/// <summary>
		/// Current index of our particle pool
		/// </summary>
		private int mParticlePoolIndex;

		/// <summary>
		/// Reference to our ui manager
		/// </summary>
		private UIManager mUIManager;

		/// <summary>
		/// Current play mode
		/// </summary>
		private PlayMode mPlayMode;

		/// <summary>
		/// Multiplier to use for light pulse on falling notes
		/// </summary>
		private float mEmissionMultiplier = 1f;

		/// <summary>
		/// Rate at which falling notes will fall (applied to gravity multiplier)
		/// </summary>
		private const float cFallingNoteSpeedRate = 50f;

		/// <summary>
		/// Gravity. We ignore unity's setting, to avoid touching the end user's physics settings
		/// </summary>
		private float mGravity = 9.8f;

		private const float cReverseGravity = 9.8f;
		private const float cNormalGravity = -9.8f;

		/// <summary>
		/// Whether we've been intiialized
		/// </summary>
		private bool mIsInitialized;

		/// <summary>
		/// Whether keyboard is muted. (this is mostly for routing audio through another library).
		/// </summary>
		private bool mKeyboardIsMuted;

		/// <summary>
		/// Creates our particle pool
		/// </summary>
		private void CreateParticlePool()
		{
			mParticlePool = new UIFallingNote[mMaxNumParticles];

			for ( var index = 0; index < mMaxNumParticles; index++ )
			{
				var particleIndex = index; //< not redundant. Caching for the callback value
				CreateFallingNote( ( instantiatedObject ) =>
				{
					if ( instantiatedObject != null )
					{
						mParticlePool[particleIndex] = instantiatedObject.gameObject.GetComponent<UIFallingNote>();
						mParticlePool[particleIndex].Stop();
					}
				} );
			}
		}

		/// <summary>
		/// Creates a falling note
		/// </summary>
		/// <param name="onCreate"></param>
		private void CreateFallingNote( Action<GameObject> onCreate )
		{
			mMusicGenerator.AddressableManager.SpawnAddressableInstance( mFallingNote, new AddressableSpawnRequest(
				Vector3.zero, Quaternion.identity, onCreate.Invoke, mFallingNoteTransform
			) );
		}

		/// <summary>
		/// Note played event
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private bool OnNotePlayed( object source, NotePlayedArgs args )
		{
			if ( args.InstrumentIndex >= args.InstrumentSet.Instruments.Count )
			{
				return false;
			}

			var instrument = args.InstrumentSet.Instruments[args.InstrumentIndex];
			var reverse = mUIManager.FXSettings.UIStyle == UIEditorFXSettings.UIEditorStyle.ReversePianoRoll;
			var playImmediate = mUIManager.FXSettings.UIStyle == UIEditorFXSettings.UIEditorStyle.None ||
			                    mUIManager.FXSettings.UIStyle == UIEditorFXSettings.UIEditorStyle.VisualizerOnly ||
			                    reverse;

			if ( mMusicGenerator.GeneratorState != GeneratorState.Playing &&
			     mMusicGenerator.GeneratorState != GeneratorState.Repeating &&
			     mUIManager.UIMeasureEditor.State != DisplayEditorState.Playing )
			{
				return true;
			}


			var instrumentData = instrument.InstrumentData;

			switch ( mPlayMode )
			{
				case PlayMode.Normal:
				case PlayMode.LeitmotifInstrument:
				case PlayMode.Clip:
					if ( playImmediate )
					{
						var color = mUIManager.Colors[(int) instrument.InstrumentData.StaffPlayerColor];
						if ( instrumentData.IsPercussion &&
						     mKeyboardPercussionInstruments.TryGetValue( instrument, out var percussionInstrument ) )
						{
							{
								var eColor = instrument.InstrumentData.StaffPlayerColor;
								percussionInstrument.Play( Vector3.zero, mUIManager.Colors[(int) eColor], mUIManager.FXSettings.UseFallingNoteParticles );
							}
						}
						else
						{
							mKeys[args.Note].Play( .5f, UnityEngine.Random.Range( 1, 5 ), color );
							if ( reverse && mQueuedNotes.TryGetValue( instrument, out var queuedNotes ) )
							{
								QueueNote( args, instrument, queuedNotes );
							}
						}

						return true;
					}
					else if ( mQueuedNotes.TryGetValue( instrument, out var queuedNotes ) )
					{
						QueueNote( args, instrument, queuedNotes );
						return false;
					}

					break;
				case PlayMode.ClipPercussion:
					if ( instrumentData.IsPercussion )
					{
						mUIManager.MeasureEditor.PercussionEditor.PlayNote( args );
					}

					return true;

				case PlayMode.Percussion:
					if ( instrumentData.UseForcedPercussion )
					{
						mUIManager.PercussionEditor.PlayNote( args );
						return true;
					}

					break;
				case PlayMode.LeitmotifPercussion:
				{
					if ( instrumentData.Leitmotif.IsEnabled )
					{
						if ( instrumentData.IsPercussion && mUIManager.InstrumentListPanelUI.PercussionIsSelected )
						{
							mUIManager.LeitmotifEditor.PercussionEditor.PlayNote( args );
						}

						return true;
					}

					break;
				}
			}

			return false;
		}

		/// <summary>
		/// Queues a note for playing
		/// </summary>
		/// <param name="args"></param>
		/// <param name="instrument"></param>
		/// <param name="queuedNotes"></param>
		private void QueueNote( NotePlayedArgs args, Instrument instrument, List<QueuedNote> queuedNotes )
		{
			var position = mKeys[args.Note].transform.position;
			position.y = mNoteStartPos;
			var eColor = instrument.InstrumentData.StaffPlayerColor;
			var color = mUIManager.Colors[(int) eColor];

			IKeyboardNoteDisplay noteDisplay;
			if ( instrument.InstrumentData.IsPercussion &&
			     mKeyboardPercussionInstruments.TryGetValue( instrument, out var percussionInstrument ) )
			{
				noteDisplay = percussionInstrument;
			}
			else
			{
				noteDisplay = mParticlePool[mParticlePoolIndex];
			}

			queuedNotes.Add( new QueuedNote( instrument, args, noteDisplay, position, color,
				mUIManager.FXSettings.UseFallingNoteParticles ) );
			mParticlePoolIndex++;

			if ( mParticlePoolIndex >= mMaxNumParticles )
			{
				mParticlePoolIndex = 0;
			}
		}

		/// <summary>
		/// Clears played notes
		/// </summary>
		private void ClearPlayedNotes()
		{
			var isReverse = mUIManager.FXSettings.UIStyle == UIEditorFXSettings.UIEditorStyle.ReversePianoRoll;
			foreach ( var noteToRemove in mNotesToRemove )
			{
				// animate before removing. We're doing here as we have access to the number of simultaneously played notes.
				if ( noteToRemove.Instrument.InstrumentData.IsPercussion )
				{
					noteToRemove.NoteDisplay.Play( Vector3.zero, noteToRemove.Color, noteToRemove.UseParticles );
				}
				else if ( isReverse )
				{
					noteToRemove.NoteDisplay.Stop();
				}
				else
				{
					mKeys[noteToRemove.Note].Play( .5f, mNotesToRemove.Count, noteToRemove.Color );
					noteToRemove.NoteDisplay.Stop();
				}

				mQueuedNotes[noteToRemove.Instrument].Remove( noteToRemove );
			}

			if ( mPlayMode == PlayMode.Clip &&
			     mUIManager.UIMeasureEditor.State == DisplayEditorState.Playing &&
			     mUIManager.UIMeasureEditor.ClipState == ClipState.Finished && HasQueuedNotes() == false )
			{
				mUIManager.UIMeasureEditor.Stop();
			}
		}

		/// <summary>
		/// Returns whether there are any currently queued notes
		/// </summary>
		/// <returns></returns>
		private bool HasQueuedNotes()
		{
			foreach ( var instrument in mQueuedNotes )
			{
				if ( instrument.Value.Count > 0 )
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Updates the note position
		/// </summary>
		/// <param name="note"></param>
		private void UpdateNotePosition( QueuedNote note )
		{
			if ( note.Instrument.InstrumentData.IsPercussion )
			{
				return;
			}

			var position = note.NoteDisplay.Transform.position;
			var fallPosition = FallPosition( note.Timer );
			if ( mUIManager.FXSettings.UIStyle == UIEditorFXSettings.UIEditorStyle.ReversePianoRoll )
			{
				if ( fallPosition > mNoteEndPos )
				{
					mNotesToRemove.Add( note );
					return;
				}
			}

			note.NoteDisplay.Transform.position = new Vector3( position.x, fallPosition, position.z );
		}

		/// <summary>
		/// Plays a note
		/// </summary>
		/// <param name="note"></param>
		private void PlayNote( QueuedNote note )
		{
			// Setting Volume here rather than from note args, as it avoids a delay when adjusting Volume in the editor
			var data = note.Instrument.InstrumentData;
			var shortestTimestep =
				data.ForceBeat == false && data.TimeStep >= Timestep.Half &&
				data.SuccessionType == SuccessionType.Rhythm; // note.InstrumentSet.GetShortestSuccessionType( SuccessionType.Rhythm );
			var emissionFloor = mUIManager.FXSettings.FallingNoteEmissionIntensityFloor;
			if ( shortestTimestep &&
			     data.UseForcedPercussion == false &&
			     mUIManager.FXSettings.UseFallingNotePulse && mEmissionMultiplier == emissionFloor )
			{
				PulseLights();
			}

			var volume = note.Instrument.InstrumentData.Volume;
			if ( mKeyboardIsMuted == false )
			{
				mMusicGenerator.PlayNote( note.InstrumentSet, volume, note.InstrumentName, note.Note,
					note.Instrument.InstrumentIndex );
			}

			mNotesToRemove.Add( note );
		}

		/// <summary>
		/// Calculates fall time given current gravity settings and fall distance
		/// </summary>
		/// <returns></returns>
		private float FallTime()
		{
			var fallHeight = mNoteStartPos - mNoteEndPos;
			return Mathf.Sqrt( fallHeight * 2f / ( cReverseGravity * mGravityMultiplier ) );
		}

		/// <summary>
		/// Fall position given time
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		private float FallPosition( float time )
		{
			return 0.5f * mGravity * mGravityMultiplier * Mathf.Pow( time, 2 ) + mNoteStartPos;
		}

		/// <summary>
		/// Initializes our keys
		/// </summary>
		private void InitializeKeys()
		{
			foreach ( var key in mKeys )
			{
				key.Initialize( mUIManager );
			}
		}

		/// <summary>
		/// Keyboard lights use changed event
		/// </summary>
		/// <param name="value"></param>
		private void OnUseKeyboardLightsChanged( bool value )
		{
			foreach ( var key in mKeys )
			{
				key.ToggleLights( value );
			}

			if ( value == false )
			{
				mKeyboardLight.FadeIn();
			}
			else
			{
				mKeyboardLight.FadeOut();
			}
		}

		/// <summary>
		/// Keyboard particles use state changed event
		/// </summary>
		/// <param name="value"></param>
		private void OnUseKeyboardParticlesChanged( bool value )
		{
			foreach ( var key in mKeys )
			{
				key.ToggleParticles( value );
			}
		}

		/// <summary>
		/// Pulses our lights
		/// </summary>
		private void PulseLights()
		{
			mEmissionMultiplier = mEmissionPulseIntensity;
		}

		/// <summary>
		/// Initializes the percussion instruments
		/// </summary>
		private void InitializePercussionInstruments()
		{
			ClearPercussionInstruments();

			foreach ( var instrument in mMusicGenerator.InstrumentSet.Instruments )
			{
				if ( instrument.InstrumentData.IsPercussion )
				{
					OnInstrumentAdded( instrument );
				}
			}
		}

		/// <summary>
		/// Instrument added event
		/// </summary>
		/// <param name="instrument"></param>
		private void OnInstrumentAdded( Instrument instrument )
		{
			if ( mKeyboardPercussionInstruments.ContainsKey( instrument ) ||
			     instrument.InstrumentData.IsPercussion == false )
			{
				return;
			}

			mMusicGenerator.AddressableManager.SpawnAddressableInstance( mKeyboardPercussionInstrumentBase,
				new AddressableSpawnRequest(
					mKeyboardPercussionParent.position, Quaternion.identity, ( result ) =>
					{
						if ( result != null )
						{
							result.transform.localScale = Vector3.one;

							var percussionInstrument = result.GetComponentInChildren<KeyboardPercussionInstrument>();
							mKeyboardPercussionInstruments.Add( instrument, percussionInstrument );
							percussionInstrument.Initialize( this, mUIManager,
								mUIManager.Colors[(int) instrument.InstrumentData.StaffPlayerColor] );
							ResizePercussionInstruments();
						}
					},
					mKeyboardPercussionParent
				) );
		}

		/// <summary>
		/// Instrument will be removed event
		/// </summary>
		/// <param name="instrument"></param>
		private void OnInstrumentWillBeRemoved( Instrument instrument )
		{
			if ( mQueuedNotes.TryGetValue( instrument, out var queuedNotes ) )
			{
				foreach ( var note in queuedNotes )
				{
					note.NoteDisplay.Stop();
				}

				queuedNotes.Clear();
				mQueuedNotes.Remove( instrument );
			}

			if ( mKeyboardPercussionInstruments.ContainsKey( instrument ) )
			{
				Destroy( mKeyboardPercussionInstruments[instrument].gameObject );
				mKeyboardPercussionInstruments.Remove( instrument );
			}

			for ( var index = mNotesToRemove.Count - 1; index >= 0; index-- )
			{
				if ( mNotesToRemove[index].Instrument == instrument )
				{
					mNotesToRemove.RemoveAt( index );
				}
			}

			ResizePercussionInstruments();
		}

		/// <summary>
		/// Resizes the percussion instruments. When new instruments are added we fit them all within the allowed space
		/// </summary>
		private void ResizePercussionInstruments()
		{
			var index = 0;
			var spacing = mBackground.size.x / mKeyboardPercussionInstruments.Count;
			var halfSpace = mKeyboardPercussionInstruments.Count > 1 ? spacing / 2f : 0;
			foreach ( var instrument in mKeyboardPercussionInstruments )
			{
				var position = Vector3.zero;
				position.x = mKeyboardPercussionInstruments.Count > 1 ? -mBackground.size.x / 2f : 0f;
				position.x += index * spacing;
				position.x += halfSpace;
				var size = new Vector2( spacing, mPercussionDisplayHeight );
				instrument.Value.UpdateSizeAndPosition( position, size );
				index++;
			}
		}

		/// <summary>
		/// Adjusts gravity. (if FX falling note speed changes, we try to accomodate it. This can result in some funky behavior.
		/// </summary>
		private void AdjustGravity()
		{
			if ( mGravityMultiplier < mUIManager.FXSettings.FallingNoteSpeed )
			{
				mGravityMultiplier = Mathf.Clamp( mGravityMultiplier + cFallingNoteSpeedRate * Time.deltaTime, 1,
					mUIManager.FXSettings.FallingNoteSpeed );
			}
			else if ( mGravityMultiplier > mUIManager.FXSettings.FallingNoteSpeed )
			{
				mGravityMultiplier = Mathf.Clamp( mGravityMultiplier - cFallingNoteSpeedRate * Time.deltaTime,
					mUIManager.FXSettings.FallingNoteSpeed, 100f );
			}
		}

		/// <summary>
		/// Update our falling note objects
		/// </summary>
		/// <param name="deltaTime"></param>
		private void UpdateNoteObjects( float deltaTime )
		{
			if ( mMusicGenerator )
			{
				//var rotationSpeed = mUIManager.FXSettings.FallingNoteSpinValue *
				//                    ( mUIManager.FXSettings.NoteSpinMultiplier * deltaTime *
				//                      ( 360f / ( mUIManager.MusicGenerator.InstrumentSet.BeatLength *
				//                                 mUIManager.MusicGenerator.InstrumentSet.TimeSignature.StepsPerMeasure ) ) );
				//mNoteStartPos.Rotate( rotationSpeed );

				foreach ( var note in mParticlePool )
				{
					if ( note.IsEnabled )
					{
						note.DoUpdate( mEmissionMultiplier );
					}
				}

				var emissionFloor = mUIManager.FXSettings.FallingNoteEmissionIntensityFloor;
				if ( mEmissionMultiplier > emissionFloor )
				{
					mEmissionMultiplier -= deltaTime * mEmissionPulseIntensityFalloff;
				}
				else
				{
					mEmissionMultiplier = emissionFloor;
				}
			}
		}

		/// <summary>
		/// Updates the keyboard for normal play mode
		/// </summary>
		/// <param name="deltaTime"></param>
		private void UpdateNormalMode( float deltaTime )
		{
			if ( mMusicGenerator &&
			     ( mMusicGenerator.GeneratorState == GeneratorState.Playing ||
			       mMusicGenerator.GeneratorState == GeneratorState.Repeating ||
			       mUIManager.UIMeasureEditor.State == DisplayEditorState.Playing ) )
			{
				AdjustGravity();
				UpdateNoteObjects( deltaTime );
				mNotesToRemove.Clear();
				var fallTime = FallTime();
				var isReversed = mUIManager.FXSettings.UIStyle == UIEditorFXSettings.UIEditorStyle.ReversePianoRoll;
				foreach ( var queuedNotes in mQueuedNotes )
				{
					foreach ( var note in queuedNotes.Value )
					{
						note.Timer += Time.deltaTime;

						if ( isReversed == false && note.Timer > fallTime )
						{
							PlayNote( note );
						}
						else
						{
							UpdateNotePosition( note );
						}
					}
				}
			}

			foreach ( var key in mKeys )
			{
				key.DoUpdate( deltaTime );
			}

			ClearPlayedNotes();
		}

		/// <summary>
		/// Forces groups playing for percussion.
		/// Note: technically, we should be setting Configuration.ManualGroups to true and setting this once,
		/// however, in this case, I don't want to handle deciding how to serialize that value if they save while the
		/// percussion editor is open. This is dirty, but functional.
		/// </summary>
		private void ForcePercussionGroupsPlaying()
		{
			for ( var index = 0; index < MusicConstants.NumGroups; index++ )
			{
				mMusicGenerator.InstrumentSet.GroupIsPlaying[index] = true;
			}
		}

		/// <summary>
		/// On Music Generator groups were chosen event.
		/// </summary>
		private void OnGroupsWereChosen()
		{
			if ( mPlayMode == PlayMode.Percussion ||
			     mPlayMode == PlayMode.LeitmotifPercussion ||
			     mPlayMode == PlayMode.ClipPercussion )
			{
				ForcePercussionGroupsPlaying();
			}
		}

		/// <summary>
		/// OnDestroy
		/// </summary>
		private void OnDestroy()
		{
			if ( mMusicGenerator != false )
			{
				mMusicGenerator.InstrumentAdded.RemoveListener( OnInstrumentAdded );
				mMusicGenerator.InstrumentWillBeRemoved.RemoveListener( OnInstrumentWillBeRemoved );
				mMusicGenerator.GroupsWereChosen.RemoveListener( OnGroupsWereChosen );
			}

			if ( mUIManager != false )
			{
				mUIManager.UIEditorSettings.OnUseKeyboardLightsChanged.RemoveListener( OnUseKeyboardLightsChanged );
				mUIManager.UIEditorSettings.OnUseKeyboardParticlesChanged.RemoveListener( OnUseKeyboardParticlesChanged );
			}
		}

#endregion private
	}
}
