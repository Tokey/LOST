using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{
	/// <summary>
	/// A simple component meant to be added to the pause button
	/// </summary>
	public class PauseButton : MonoBehaviour
	{
		/// Puts the game on pause
	    public virtual void PauseButtonAction()
	    {
	        GameManager.Instance.Pause();
	    }	
	}
}