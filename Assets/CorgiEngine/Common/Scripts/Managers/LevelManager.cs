using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Spawns the player, handles checkpoints and respawn
	/// </summary>
	[AddComponentMenu("Corgi Engine/Managers/Level Manager")]
	public class LevelManager : Singleton<LevelManager>
	{
        /// the possible checkpoint axis
        /// 
        public GameObject failedLevel;
		public enum CheckpointsAxis { x, y, z}
        

		/// the prefab you want for your player
		[Header("Playable Characters")]
		[Information("The LevelManager is responsible for handling spawn/respawn, checkpoints management and level bounds. Here you can define one or more playable characters for your level..",InformationAttribute.InformationType.Info,false)]
		/// the list of player prefabs to instantiate
		public Character[] PlayerPrefabs ;
		/// should the player IDs be auto attributed (usually yes)
		public bool AutoAttributePlayerIDs = true;

		[Header("Checkpoints")]
		[Information("Here you can select a checkpoint attribution axis (if your level is horizontal go for X, Y if it's vertical), and a debug spawn where your player character will spawn from while in editor mode.",InformationAttribute.InformationType.Info,false)]
		/// Debug spawn	
		public CheckPoint DebugSpawn;
		/// the axis on which objects should be compared
		public CheckpointsAxis CheckpointAttributionAxis = CheckpointsAxis.x;
		[ReadOnly]
		/// the current checkpoint
		public CheckPoint CurrentCheckPoint;

		
		[Space(10)]
		[Header("Intro and Outro durations")]
		[Information("Here you can specify the length of the fade in and fade out at the start and end of your level.",InformationAttribute.InformationType.Info,false)]
		/// duration of the initial fade in (in seconds)
		public float IntroFadeDuration=1f;
		/// duration of the fade to black at the end of the level (in seconds)
		public float OutroFadeDuration=1f;

		[Space(10)]
		[Header("Level Bounds")]
		[Information("The level bounds are used to constrain the camera's movement, as well as the player character's. You can see it in real time in the scene view as you adjust its size (it's the yellow box).",InformationAttribute.InformationType.Info,false)]
		/// the level limits, camera and player won't go beyond this point.
		public Bounds LevelBounds = new Bounds(Vector3.zero,Vector3.one*10);

		/// the elapsed time since the start of the level
		public TimeSpan RunningTime { get { return DateTime.UtcNow - _started ;}}

	    // private stuff
		public List<Character> Players { get; protected set; }
	    public List<CheckPoint> Checkpoints { get; protected set; }
	    protected DateTime _started;
	    protected int _savedPoints;
	    protected CameraController _cameraController ;
		
		/// <summary>
		/// On awake, instantiates the player
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			InstantiatePlayableCharacters ();       
	    }

		/// <summary>
		/// Instantiate playable characters based on the ones specified in the PlayerPrefabs list in the LevelManager's inspector.
		/// </summary>
		protected virtual void InstantiatePlayableCharacters()
		{
			Players = new List<Character> ();

			if (PlayerPrefabs == null) { return; }

			// player instantiation
			if (PlayerPrefabs.Count() != 0)
			{ 
				foreach (Character playerPrefab in PlayerPrefabs)
				{
					Character newPlayer = (Character)Instantiate (playerPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
					newPlayer.name = playerPrefab.name;
					Players.Add(newPlayer);

					if (playerPrefab.CharacterType != Character.CharacterTypes.Player)
					{
						Debug.LogWarning ("LevelManager : The Character you've set in the LevelManager isn't a Player, which means it's probably not going to move. You can change that in the Character component of your prefab.");
					}
				}
			}
			else
			{
				//Debug.LogWarning ("LevelManager : The Level Manager doesn't have any Player prefab to spawn. You need to select a Player prefab from its inspector.");
				return;
			}	
		}
		
		/// <summary>
		/// Initialization
		/// </summary>
		public virtual void Start()
		{
			if (Players == null || Players.Count == 0) { return; }

			Initialization ();
			CheckpointAssignment ();
			LevelGUIStart ();

			// we handle the spawn of the character(s)
			if (Players.Count == 1)
			{
				SpawnSingleCharacter ();
			}
			else
			{
				SpawnMultipleCharacters ();
			}

			// we trigger a level start event
			MMEventManager.TriggerEvent(new CorgiEngineEvent(CorgiEngineEventTypes.LevelStart));
	        	
		}

		/// <summary>
		/// Gets current camera, points number, start time, etc.
		/// </summary>
		protected virtual void Initialization()
		{
			// storage
			_cameraController = FindObjectOfType<CameraController>();
			_savedPoints=GameManager.Instance.Points;
			_started = DateTime.UtcNow;

			// we store all the checkpoints present in the level, ordered by their x value
			Checkpoints = FindObjectsOfType<CheckPoint>().OrderBy(o => o.transform.position.x).ToList();
			// we assign the first checkpoint 
			CurrentCheckPoint = Checkpoints.Count > 0 ? Checkpoints[0] : null;
		}

		/// <summary>
		/// Assigns all respawnable objects in the scene to their checkpoint
		/// </summary>
		protected virtual void CheckpointAssignment()
		{
			// we get all respawnable objects in the scene and attribute them to their corresponding checkpoint
			IEnumerable<Respawnable> listeners = FindObjectsOfType<MonoBehaviour>().OfType<Respawnable>();
			foreach(Respawnable listener in listeners)
			{
				for (int i = Checkpoints.Count - 1; i>=0; i--)
				{
					Vector3 vectorDistance = ((MonoBehaviour) listener).transform.position - Checkpoints[i].transform.position;

					float distance = 0;
					if (CheckpointAttributionAxis == CheckpointsAxis.x)
					{
						distance = vectorDistance.x;
					}
					if (CheckpointAttributionAxis == CheckpointsAxis.y)
					{
						distance = vectorDistance.y;
					}
					if (CheckpointAttributionAxis == CheckpointsAxis.z)
					{
						distance = vectorDistance.z;
					}

					// if the object is behind the checkpoint (on the attribution axis), we move on to the next checkpoint
					if (distance < 0)
					{
						continue;
					}

					// if the object is further on the attribution axis compared to the checkpoint, we assign it to the checkpoint, and proceed to the next object										
					Checkpoints[i].AssignObjectToCheckPoint(listener);
					break;
				}
			}
		}

		/// <summary>
		/// Initializes GUI stuff
		/// </summary>
		protected virtual void LevelGUIStart()
		{
			// if we have a GUI manager
			if (GUIManager.Instance!= null)
			{ 
				// set the level name in the GUI
				GUIManager.Instance.SetLevelName(SceneManager.GetActiveScene().name);			
				// fade in
				GUIManager.Instance.FaderOn(false,IntroFadeDuration);
			}
		}

		/// <summary>
		/// Spawns a playable character into the scene
		/// </summary>
		protected virtual void SpawnSingleCharacter()
		{
			// in debug mode we spawn the player on the debug spawn point
			#if UNITY_EDITOR 
			if (DebugSpawn!= null)
			{
				DebugSpawn.SpawnPlayer(Players[0]);
			}
			else if (CurrentCheckPoint != null)
			{
				CurrentCheckPoint.SpawnPlayer(Players[0]);
			}
			#else
			if (CurrentCheckPoint != null)
			{			
			CurrentCheckPoint.SpawnPlayer(Players[0]);
			}
			#endif	
		}

		/// <summary>
		/// Spawns multiple playable characters into the scene
		/// </summary>
		protected virtual void SpawnMultipleCharacters()
		{
			int checkpointCounter = 0;
			int characterCounter = 1;
			bool spawned = false;
			foreach (Character player in Players)
			{
				spawned = false;

				if (AutoAttributePlayerIDs)
				{
					player.SetPlayerID("Player"+characterCounter);
				}

				player.name += " - " + player.PlayerID;

				if (Checkpoints.Count > checkpointCounter+1)
				{
					if (Checkpoints[checkpointCounter] != null)
					{
						Checkpoints[checkpointCounter].SpawnPlayer(player);
						characterCounter++;
						spawned = true;
						checkpointCounter++;
					}
				}
				if (!spawned)
				{
					Checkpoints[checkpointCounter].SpawnPlayer(player);
					characterCounter++;
				}
			}
		}

		/// <summary>
		/// Every frame we check for checkpoint reach
		/// </summary>
		public virtual void Update()
		{
			if (Players == null)
			{
				return;
			}
						
			_savedPoints = GameManager.Instance.Points;
			_started = DateTime.UtcNow;
		}

		/// <summary>
		/// Sets the current checkpoint.
		/// </summary>
		/// <param name="newCheckPoint">New check point.</param>
		public virtual void SetCurrentCheckpoint(CheckPoint newCheckPoint)
		{
			CurrentCheckPoint = newCheckPoint;
		}

		/// <summary>
		/// Gets the player to the specified level
		/// </summary>
		/// <param name="levelName">Level name.</param>
		public virtual void GotoLevel(string levelName)
		{		
			MMEventManager.TriggerEvent(new CorgiEngineEvent(CorgiEngineEventTypes.LevelEnd));

	        if (GUIManager.Instance!= null)
	        { 
	    		GUIManager.Instance.FaderOn(true,OutroFadeDuration);
	        }

            if (levelName != "MainMenu")
                PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 0) + 1);
            else
                PlayerPrefs.SetInt("CurrentLevel", 0);


            StartCoroutine(GotoLevelCo(levelName));
           
        }

	    /// <summary>
	    /// Waits for a short time and then loads the specified level
	    /// </summary>
	    /// <returns>The level co.</returns>
	    /// <param name="levelName">Level name.</param>
	    protected virtual IEnumerator GotoLevelCo(string levelName)
		{
            
            if (Players != null && Players.Count > 0)
	        { 
				foreach (Character player in Players)
				{
					player.Disable ();
                    
                }	    		
	        }

	        if (Time.timeScale > 0.0f)
	        { 
	            yield return new WaitForSeconds(OutroFadeDuration);
	        }
	        GameManager.Instance.UnPause();

	        if (string.IsNullOrEmpty(levelName))
	        {
				LoadingSceneManager.LoadScene("StartScreen");
			}
			else
			{
				LoadingSceneManager.LoadScene(levelName);
			}			
		}

		/// <summary>
		/// Kills the player.
		/// </summary>
		public virtual void KillPlayer(Character player)
		{
			Health characterHealth = player.GetComponent<Health>();
			if (characterHealth == null)
			{
				return;
			} 
			else
			{
				// we kill the character
				characterHealth.Kill ();
				MMEventManager.TriggerEvent(new CorgiEngineEvent(CorgiEngineEventTypes.PlayerDeath));

				// if we have only one player, we restart the level
				if (Players.Count < 2)
				{
                    failedLevel.SetActive(true);
                    GameManager.Instance.Pause2();
                }
			}
		}

	    /// <summary>
	    /// Coroutine that kills the player, stops the camera, resets the points.
	    /// </summary>
	    /// <returns>The player co.</returns>
	    protected virtual IEnumerator SoloModeRestart()
		{
			if (PlayerPrefabs.Count() <= 0)
			{
				yield break;
			}

			_cameraController.FollowsPlayer=false;
			yield return new WaitForSeconds(2f);			
			_cameraController.FollowsPlayer=true;
			if (CurrentCheckPoint != null)
			{
				CurrentCheckPoint.SpawnPlayer(Players[0]);
			}			
			_started = DateTime.UtcNow;
			GameManager.Instance.SetPoints(_savedPoints);
		}

		/// <summary>
		/// Freezes the character(s)
		/// </summary>
		public virtual void FreezeCharacters()
		{
			foreach (Character player in Players)
			{
				player.Freeze();	
			}
		}		

		/// <summary>
		/// Unfreezes the character(s)
		/// </summary>
		public virtual void UnFreezeCharacters()
		{
			foreach (Character player in Players)
			{
				player.UnFreeze();	
			}
		}

		/// <summary>
		/// Toggles Character Pause
		/// </summary>
		public virtual void ToggleCharacterPause()
		{
			foreach (Character player in Players)
			{

				CharacterPause characterPause = player.GetComponent<CharacterPause>();
				if (characterPause == null) 
				{
					break;
				}

				if (GameManager.Instance.Paused)
				{
					characterPause.PauseCharacter();
				}
				else
				{
					characterPause.UnPauseCharacter();
				}
			}
		} 
	}
}