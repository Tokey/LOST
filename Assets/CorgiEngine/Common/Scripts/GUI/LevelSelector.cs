using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace MoreMountains.CorgiEngine
{
	/// <summary>
	/// This component allows the definition of a level that can then be accessed and loaded. Used mostly in the level map scene.
	/// </summary>
	public class LevelSelector : MonoBehaviour
	{
		/// the exact name of the target level
	    public string LevelName;

		/// <summary>
		/// Loads the level specified in the inspector
		/// </summary>
	    public virtual void GoToLevel()
	    {
	        LevelManager.Instance.GotoLevel(LevelName);
	    }


        //overriden by tokey , not a corgi default
        public virtual void GoToLevel(string lvlName)
        {
            
            PlayerPrefs.SetInt("Slevel" + PlayerPrefs.GetInt("CurrentLevel", 0), 0);
            LevelManager.Instance.GotoLevel(lvlName);
        }

        //another one :3 cuz, bugs :3 
        public virtual void ToLevel(int toLevel)
        {
            toLevel--;
            PlayerPrefs.SetInt("CurrentLevel", toLevel);
        }

        /// <summary>
        /// Restarts the current level
        /// </summary>
        public virtual void RestartLevel()
	    {
            int level = PlayerPrefs.GetInt("CurrentLevel", 0);
            GameManager.Instance.UnPause();
            PlayerPrefs.SetInt("Slevel" + level, 0);
            LoadingSceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    }
		
	}
}