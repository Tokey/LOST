using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Health))]

	/// <summary>
	/// Add this class to an enemy (or whatever you want), to be able to stomp on it
	/// </summary>
	[AddComponentMenu("Corgi Engine/Character/Damage/Stompable")] 
	public class Stompable : MonoBehaviour 
	{
		/// the possible ways to add knockback : noKnockback, which won't do nothing, set force, or add force
		public enum KnockbackStyles { NoKnockback, SetForce, AddForce }

		[Information("Add this component to an object (an enemy for example) you want the player to be able to stomp by jumping on it. You can define how many rays will be used to detect the collision (you can see them in debug mode), try and have a space between each ray smaller than your player's width), the force that will be applied to the stomper on impact, the mask used to detect the player, and how much damage each stomp should cause.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// The number of vertical rays cast to detect stomping
		public int NumberOfRays=5;
		/// The force the hit will apply to the stomper
		public Vector2 KnockbackForce = new Vector2(0f,15f);
		/// the type of knockback to apply when causing damage
		public KnockbackStyles DamageCausedKnockbackType = KnockbackStyles.SetForce;
		/// The layer the player is on
	    public LayerMask PlayerMask;
		/// The amount of damage each stomp causes to the stomped enemy
	    public int DamagePerStomp;

		// private stuff
	    protected BoxCollider2D _boxCollider;
	    protected Health _health;
	    protected const float _invincibilityDuration = 0.5f;
		protected CharacterJump _collidingCharacterJump;
		protected Vector2 _verticalRayCastStart;
		protected Vector2 _verticalRayCastEnd;

	    /// <summary>
	    /// On start, we get the various components
	    /// </summary>
	    protected virtual void Start ()
	    {
	        _boxCollider = (BoxCollider2D)GetComponent<BoxCollider2D>();
	        _health = (Health)GetComponent<Health>();	
		}

	    /// <summary>
	    /// Each update, we cast rays above
	    /// </summary>
	    protected virtual void Update () 
		{
	        CastRaysAbove();
		}

		/// <summary>
		/// Casts the rays above to detect stomping
		/// </summary>
	    protected virtual void CastRaysAbove()
	    {
	        float rayLength = 0.5f;

	        bool hitConnected = false;
	        int hitConnectedIndex = 0;

			_verticalRayCastStart.x = _boxCollider.bounds.min.x;
			_verticalRayCastStart.y = _boxCollider.bounds.max.y;
			_verticalRayCastEnd.x = _boxCollider.bounds.max.x;
			_verticalRayCastEnd.y = _boxCollider.bounds.max.y;

	        RaycastHit2D[] hitsStorage = new RaycastHit2D[NumberOfRays];

			// we cast rays above our object to check for anything trying to stomp it
	        for (int i = 0; i < NumberOfRays; i++)
	        {
	            Vector2 rayOriginPoint = Vector2.Lerp(_verticalRayCastStart, _verticalRayCastEnd, (float)i / (float)(NumberOfRays - 1));
				hitsStorage[i] = MMDebug.RayCast(rayOriginPoint, Vector2.up, rayLength, PlayerMask, Color.black, true);

	            if (hitsStorage[i])
	            {
	                hitConnected = true;
	                hitConnectedIndex = i;
	                break;
	            }
	        }

			// if we connect with something, we check to see if it's a corgicontroller, and if that's the case, we get stomped
	        if (hitConnected)
	        {
	        	// if the player is not hitting this enemy from above, we do nothing
				if (_boxCollider.bounds.max.y > hitsStorage[hitConnectedIndex].collider.bounds.min.y)
				{
					return;
				}
				CorgiController corgiController = hitsStorage[hitConnectedIndex].collider.gameObject.GetComponentNoAlloc<CorgiController>();
				if (corgiController!=null)
	            {
	            	// if the player is not going down, we do nothing and exit
					if (corgiController.Speed.y >= 0)
					{
						return;
					}

					if (DamageCausedKnockbackType == KnockbackStyles.SetForce)
					{
						corgiController.SetForce(KnockbackForce);	
					}
					if (DamageCausedKnockbackType == KnockbackStyles.AddForce)
					{
						corgiController.AddForce(KnockbackForce);	
					}

					if (_health != null)
					{
						_health.Damage(DamagePerStomp, gameObject,_invincibilityDuration,_invincibilityDuration);
					}

					// if what's colliding with us has a CharacterJump component, we reset its JumpButtonReleased flag so that the knockback effect is applied correctly.
					CharacterJump _collidingCharacterJump = hitsStorage[hitConnectedIndex].collider.gameObject.GetComponentNoAlloc<CharacterJump>();
					if (_collidingCharacterJump != null)
					{
						_collidingCharacterJump.ResetJumpButtonReleased ();
					}
	            }
	        }
	    }
	}
}