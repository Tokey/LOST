using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// This base class, meant to be extended (see ProjectileWeapon.cs for an example of that) handles rate of fire (rate of use actually), and ammo reloading
	/// </summary>
	public class Weapon : MonoBehaviour 
	{
		/// the possible use modes for the trigger
		public enum TriggerModes { SemiAuto, Auto }
		/// the possible states the weapon can be in
		public enum WeaponStates { WeaponIdle, WeaponStart, WeaponSingleUse, WeaponInUse, WeaponStop, WeaponReloadStart, WeaponReload, WeaponReloadStop }

		/// is this weapon on semi or full auto ?
		public TriggerModes TriggerMode = TriggerModes.Auto;
		// the delay before use, that will be applied for every shot
		public float DelayBeforeUse = 0f;
		// the time (in seconds) between two shots		
		public float TimeBetweenUses = 1f;

		[Header("Position")]
		/// an offset that will be applied to the weapon once attached to the center of the WeaponAttachment transform.
		public Vector3 WeaponAttachmentOffset = Vector3.zero;
		/// should that weapon be flipped when the character flips ?
		public bool FlipWeaponOnCharacterFlip = true;
		/// the FlipValue will be used to multiply the model's transform's localscale on flip. Usually it's -1,1,1, but feel free to change it to suit your model's specs
		public Vector3 FlipValue = new Vector3(-1,1,1);

		[Header("Effects")]
		/// a list of effects to trigger when the weapon is used
		public List<ParticleSystem> ParticleEffects;

		[Header("Animation Parameters Names")]
		/// the name of the weapon's idle animation parameter : this will be true all the time except when the weapon is being used
		public string IdleAnimationParameter;
		/// the name of the weapon's start animation parameter : true at the frame where the weapon starts being used
		public string StartAnimationParameter;
		/// the name of the weapon's single use animation parameter : true at each frame the weapon activates (shoots)
		public string SingleUseAnimationParameter;
		/// the name of the weapon's in use animation parameter : true when the weapon is in use
		public string InUseAnimationParameter;
		/// the name of the weapon stop animation parameter : true at the frame where the weapon stops being used
		public string StopAnimationParameter;
		/// the name of the weapon reload start animation parameter
		public string ReloadStartAnimationParameter;
		/// the name of the weapon reload animation parameter
		public string ReloadAnimationParameter;
		/// the name of the weapon reload end animation parameter
		public string ReloadStopAnimationParameter;
		/// the name of the weapon's angle animation parameter
		public string WeaponAngleAnimationParameter;
		/// the name of the weapon's angle animation parameter, adjusted so it's always relative to the direction the character is currently facing
		public string WeaponAngleRelativeAnimationParameter;


		[Header("Sounds")]
		/// the sound to play when the weapon starts being used
		public AudioClip WeaponStartSfx;
		/// the sound to play while the weapon is in use
		public AudioClip WeaponUsedSfx;
		/// the sound to play when the weapon stops being used
		public AudioClip WeaponStopSfx;
		/// the sound to play when the weapon gets reloaded
		public AudioClip WeaponReloadSfx; 

		[Header("Hands Position")]
		/// the transform to which the character's left hand should be attached to
		public Transform LeftHandHandle;
		/// the transform to which the character's right hand should be attached to
		public Transform RightHandHandle;

		/// the weapon's owner
		public Character Owner { get; protected set; }
		/// if true, the weapon is flipped
	    public bool Flipped { get; protected set; }
		/// the weapon's state machine
		public MMStateMachine<WeaponStates> WeaponState;

		protected SpriteRenderer _spriteRenderer;
		protected float _lastUseTimeStamp = 0;
	    protected bool _usable;
	    protected bool _usedOnce = false;

	    /// <summary>
	    /// Initialize this weapon.
	    /// </summary>
		public virtual void Initialize()
		{
			Flipped = false;
			_lastUseTimeStamp = 0;
			_spriteRenderer = GetComponent<SpriteRenderer> ();
			SetParticleEffects (false);
			WeaponState = new MMStateMachine<WeaponStates>(gameObject,false);
			WeaponState.ChangeState(WeaponStates.WeaponIdle);
		}

		/// <summary>
		/// Sets the weapon's owner
		/// </summary>
		/// <param name="newOwner">New owner.</param>
		public virtual void SetOwner(Character newOwner)
		{
			Owner = newOwner;
		}

		/// <summary>
		/// On Update, we check if the weapon is or should be used
		/// </summary>
		protected virtual void Update()
		{
			WeaponUseCheck();
		}

		/// <summary>
		/// On LateUpdate, processes the weapon state
		/// </summary>
		protected virtual void LateUpdate()
		{
			if (WeaponState == null) { return; }

			if ( (WeaponState.CurrentState == WeaponStates.WeaponStop)
				|| (WeaponState.CurrentState == WeaponStates.WeaponReloadStop))
			{
				WeaponState.ChangeState(WeaponStates.WeaponIdle);
			}

			if (WeaponState.CurrentState == WeaponStates.WeaponStart) 
			{
				WeaponState.ChangeState(WeaponStates.WeaponInUse);
			}
		}

		/// <summary>
		/// Returns true if the weapon should be used at this frame
		/// </summary>
		protected virtual bool WeaponUseCheck()
		{
			// if we're not supposed to fire, we do nothing and exit
			if (!_usable)	{ return false; }

			// if we're in semi auto mode and have already used the weapon once, we do nothing and exit
			if ((TriggerMode == TriggerModes.SemiAuto) && (_usedOnce))
			{
				return false;
			}

			if (WeaponState.CurrentState == WeaponStates.WeaponSingleUse)
			{
				WeaponState.ChangeState(WeaponStates.WeaponInUse);
			}

			// if enough time has passed since the weapon's last use (based on the TimeBetweenUses), we use the weapon again
			if(Time.time - _lastUseTimeStamp > TimeBetweenUses + DelayBeforeUse)
			{

				WeaponState.ChangeState(WeaponStates.WeaponSingleUse);

				_lastUseTimeStamp = Time.time; // reset timer for fire rate
				_usedOnce = true;

				if (DelayBeforeUse > 0)
				{
					StartCoroutine (WeaponUseDelay (DelayBeforeUse));
				}
				else
				{
					WeaponUse ();
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Adds a delay before the actual weapon use
		/// </summary>
		/// <returns>The use delay.</returns>
		/// <param name="initialDelay">Initial delay.</param>
		protected virtual IEnumerator WeaponUseDelay(float initialDelay)
		{
			yield return new WaitForSeconds (initialDelay);
			WeaponUse ();
		}

		/// <summary>
		/// Called when the weapon starts, plays the sound, changes the state
		/// </summary>
		public virtual void WeaponStart()
		{
			SfxPlayWeaponStartSound();
			_usable = true;
			if (WeaponUseCheck())
			{
				WeaponState.ChangeState(WeaponStates.WeaponStart);
			}
		}

		/// <summary>
		/// Called when the weapon stops, plays the sound, changes the state
		/// </summary>
		public virtual void WeaponStop()
		{
			SfxPlayWeaponStopSound();
			_usable = false;
			_usedOnce = false;
			WeaponState.ChangeState(WeaponStates.WeaponStop);
		}	

		/// <summary>
		/// When the weapon is used, plays the corresponding sound
		/// </summary>
		protected virtual void WeaponUse()
		{	
			SetParticleEffects (true);		
			_lastUseTimeStamp = Time.time; // reset timer for fire rate
			_usedOnce = true;
			SfxPlayWeaponUsedSound();
		}

		/// <summary>
		/// Sets the particle effects on or off
		/// </summary>
		/// <param name="status">If set to <c>true</c> status.</param>
		protected virtual void SetParticleEffects(bool status)
		{
			foreach (ParticleSystem system in ParticleEffects)
			{
				if (system == null) { return; }

				if (status)
				{
					system.Play();
				}
				else
				{
					system.Pause();
				}
			}
		}

		/// <summary>
		/// Reloads the weapon
		/// </summary>
		/// <param name="ammo">Ammo.</param>
		public virtual void ReloadWeapon(int ammo)
		{
			SfxPlayWeaponReloadSound();
			//TODO WeaponState.ChangeState(WeaponStates.WeaponReloadStart);
		}

		/// <summary>
		/// Flips the weapon.
		/// </summary>
		public virtual void FlipWeapon()
		{			
			Flipped = !Flipped;
		}

		/// <summary>
		/// Flips the weapon model.
		/// </summary>
		public virtual void FlipWeaponModel()
		{			
			if (_spriteRenderer != null)
			{
				_spriteRenderer.flipX = !_spriteRenderer.flipX;
			} 
			else
			{
				transform.localScale = Vector3.Scale (transform.localScale, FlipValue);		
			}			
		}

		/// <summary>
		/// Plays the weapon's start sound
		/// </summary>
		protected virtual void SfxPlayWeaponStartSound()
		{
			if (WeaponStartSfx!=null) {	SoundManager.Instance.PlaySound(WeaponStartSfx,transform.position);	}
		}	

		/// <summary>
		/// Plays the weapon's used sound
		/// </summary>
		protected virtual void SfxPlayWeaponUsedSound()
		{
			if (WeaponUsedSfx!=null) {	SoundManager.Instance.PlaySound(WeaponUsedSfx,transform.position);	}
		}	

		/// <summary>
		/// Plays the weapon's stop sound
		/// </summary>
		protected virtual void SfxPlayWeaponStopSound()
		{
			if (WeaponStopSfx!=null) {	SoundManager.Instance.PlaySound(WeaponStopSfx,transform.position);	}
		}	

		/// <summary>
		/// Plays the weapon's reload sound
		/// </summary>
		protected virtual void SfxPlayWeaponReloadSound()
		{
			if (WeaponReloadSfx!=null) {	SoundManager.Instance.PlaySound(WeaponReloadSfx,transform.position); }
		}		
	}
}