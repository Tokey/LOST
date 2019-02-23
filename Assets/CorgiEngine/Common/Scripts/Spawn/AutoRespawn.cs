using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this script to an object and it will automatically be reactivated and revived when the player respawns.
	/// </summary>
	[AddComponentMenu("Corgi Engine/Spawn/Auto Respawn")]
	public class AutoRespawn : MonoBehaviour, Respawnable 
	{
		/// <summary>
	    /// When the player respawns, we reinstate this agent.
	    /// </summary>
	    /// <param name="checkpoint">Checkpoint.</param>
	    /// <param name="player">Player.</param>
		public virtual void OnPlayerRespawn (CheckPoint checkpoint, Character player)
		{
			if (GetComponent<Health>() != null)
			{
				GetComponent<Health>().Revive();
			}
			gameObject.SetActive(true);
		}
	}
}