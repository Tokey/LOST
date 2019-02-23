using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// A list of the possible Corgi Engine base events
    /// </summary>
    /// 

   
	public enum CorgiEngineEventTypes
	{
		LevelStart,
		LevelEnd,
		PlayerDeath
	}

	/// <summary>
	/// A type of events used to signal level start and end (for now)
	/// </summary>
	public struct CorgiEngineEvent
	{
		public CorgiEngineEventTypes EventType;
		/// <summary>
		/// Initializes a new instance of the <see cref="MoreMountains.CorgiEngine.CorgiEngineEvent"/> struct.
		/// </summary>
		/// <param name="eventType">Event type.</param>
		public CorgiEngineEvent(CorgiEngineEventTypes eventType)
		{
			EventType = eventType;
		}
	} 

	/// <summary>
	/// The game manager is a persistent singleton that handles points and time
	/// </summary>
	[AddComponentMenu("Corgi Engine/Managers/Game Manager")]
	public class GameManager : PersistentSingleton<GameManager>
	{		
		/// the target frame rate for the game
		public int TargetFrameRate=300;
		/// the current number of game points
		public int Points { get; private set; }
		/// the current time scale
		public float TimeScale { get; private set; }
		/// true if the game is currently paused
		public bool Paused { get; set; } 
		// true if we've stored a map position at least once
		public bool StoredLevelMapPosition{ get; set; }
		/// the current player
		public Vector2 LevelMapPosition { get; set; }

	    // storage
	    protected float _savedTimeScale=1f;

	    /// <summary>
	    /// On Start(), sets the target framerate to whatever's been specified
	    /// </summary>
	    protected virtual void Start()
	    {
			Application.targetFrameRate = TargetFrameRate;
            PlayerPrefs.SetInt("Slevel" + PlayerPrefs.GetInt("CurrentLevel",0), 0);
            PlayerPrefs.SetInt("LevelLock" + PlayerPrefs.GetInt("CurrentLevel", 0), 10);
            Points = PlayerPrefs.GetInt("Points", 0);
        }
					
		/// <summary>
		/// this method resets the whole game manager
		/// </summary>
		public virtual void Reset()
		{
			Points = PlayerPrefs.GetInt("Points", 0);
            TimeScale = 1f;
			Paused = false;
			GUIManager.Instance.RefreshPoints ();
		}	
			
		/// <summary>
		/// Adds the points in parameters to the current game points.
		/// </summary>
		/// <param name="pointsToAdd">Points to add.</param>
		public virtual void AddPoints(int pointsToAdd)
		{
			Points += pointsToAdd;
            PlayerPrefs.SetInt("Points", Points);
			GUIManager.Instance.RefreshPoints ();
		}
		
		/// <summary>
		/// use this to set the current points to the one you pass as a parameter
		/// </summary>
		/// <param name="points">Points.</param>
		public virtual void SetPoints(int points)
		{
			Points = points;
			GUIManager.Instance.RefreshPoints ();
		}
		
		/// <summary>
		/// sets the timescale to the one in parameters
		/// </summary>
		/// <param name="newTimeScale">New time scale.</param>
		public virtual void SetTimeScale(float newTimeScale)
		{
			_savedTimeScale = Time.timeScale;
			Time.timeScale = newTimeScale;
		}
		
		/// <summary>
		/// Resets the time scale to the last saved time scale.
		/// </summary>
		public virtual void ResetTimeScale()
		{
			Time.timeScale = _savedTimeScale;
		}
		
		/// <summary>
		/// Pauses the game or unpauses it depending on the current state
		/// </summary>
		public virtual void Pause()
		{	
			// if time is not already stopped		
			if (Time.timeScale>0.0f)
			{
				Instance.SetTimeScale(0.0f);
				Instance.Paused=true;
				GUIManager.Instance.SetPause(true);
			}
			else
			{
				UnPause();
			}		
			LevelManager.Instance.ToggleCharacterPause();
		}
        public virtual void Pause2()
        {
            // if time is not already stopped		
            if (Time.timeScale > 0.0f)
            {
                Instance.SetTimeScale(0.0f);
                Instance.Paused = true;
                //GUIManager.Instance.SetPause(true);
            }
            else
            {
                UnPause();
            }
            LevelManager.Instance.ToggleCharacterPause();
        }

        /// <summary>
        /// Unpauses the game
        /// </summary>
        public virtual void UnPause()
	    {
	        Instance.ResetTimeScale();
	        Instance.Paused = false;
	        if (GUIManager.Instance!= null)
	        { 
	            GUIManager.Instance.SetPause(false);
	        }
	    }
	}
}