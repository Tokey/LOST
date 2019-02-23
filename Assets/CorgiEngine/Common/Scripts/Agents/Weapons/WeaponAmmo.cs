using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	public class WeaponAmmo : MonoBehaviour 
	{
		//TODO Coming soon to a Corgi Engine near you
		[Header("Ammo")]
		public bool Reloadable = true;
		public int CurrentAmmo { get; protected set; }
		public int MagazineSize;
	}
}