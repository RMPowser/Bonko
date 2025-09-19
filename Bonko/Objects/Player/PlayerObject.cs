using CppNet;
using Logic;
using Logic.Components;
using Logic.ComponentSystems;
using Logic.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Bonko.Objects.Player
{
	public class PlayerObject : Entity, IUpdateableComponent
	{
		const int TILE_SIZE = 16;

		enum AnimIndex
		{
			idle,
			sit,
			bonk,
			crouch,
			jump,
			ledge_grab,
			ledge_climb,
			morph_intro,
			morph,
			spin,
			spin_fast,
			spin_attack,
			walk
		}
		
		Random random;
		
		private AnimatedSpriteComponent animation;
		private TransformComponent transform;
		private CollisionRectComponent collisionRect;

		TimeSpan blinkTimer;

		// states
		bool idle;
		bool elevator;
		bool walking;
		bool jumping;
		bool spinning;
		bool facingRight;
		bool grounded;
		bool crouching;
		bool ledgeGrab;
		bool climbingUpLedge;
		bool wallJumping;
		bool morphed;
		bool ledgeGrabFacingRight;
		bool bonking;
		bool bombJumping;
		bool speedBoosting;
		bool sliding;
		bool shinesparking;

		// bools
		bool canJump;
		bool canWallJump;
		bool canMove;
		TimeSpan canMoveTimer;
		bool canShoot;
		TimeSpan canShootTimer;
		bool showGun;
		bool angledDown;
		bool aimingDown;
		bool aimingUp;
		bool inLiquid;
		bool inLava;

		// physics
		float hsp;
		float vsp;
		float jumpSpeed;
		float jumpHeightCounter;
		float maxJumpHeightWater;
		float maxJumpHeightNormal;
		float maxJumpHeightHigh;
		float maxBombJumpHeight;
		float maxJumpHeight;
		float grav;
		float acceleration;
		float maxFallingSpeed;
		float maxWalkingSpeed;
		float maxWalkingSpeedAir;
		float climbingUpLedgeCounter;
		float crouchingTimer;
		float wallJumpFrames;
		float bombsPlaced;
		float superBombsPlaced;
		float chargeCounter;
		float chargeMax;
		float chargeImageIndex;
		float speedBoostCounter;
		TimeSpan speedBoostEngageAt;
		float speedBoostSpeed;
		float slidingFrames;
		bool shinesparkStored;
		float storedShineSparkFrames;
		float shinesparkCounter;
		float shinesparkCounterFrames;
		float shinesparkSpeed;
		object? shinesparkInstance; // keeps track of the shinespark obj;

		// animation
		float gunX;
		float gunY;
		int gunFrame;
		int wallJumpAnimCounter;
		int unmorphAnimCounter;
		int bonkingAnimCounter;
		int lastFramePlayedFootstep;
		int hue;
		int camShake;
		int shinesparkEndShakeFrames;
		int superRocketShakeFrames;

		// camera
		enum TransitionTypes { left, right, up, down };
		object obj_camera;
		float lerpAmountTowardsVolume;
		float lerpAmountTowardsPlayer;
		float screenFadeAlpha;
		bool roomChangeFinished;
		bool transitionNow;
		float lerpAmountTowardsHatchTargetX;
		float lerpAmountTowardsHatchTargetY;

		struct AcquiredAbilities
		{
			public bool ledgeGrab = true;
			public bool gun = false;
			public bool morph = false;
			public bool bomb = false;
			public bool rocket = false;
			public bool charge = false;
			public bool superRocket = false;
			public bool highJump = false;
			public bool moonJump = false;
			public bool superBomb = false;
			public bool spinAttack = false;
			public bool spacer = false;
			public bool peircer = false;
			public bool wave = false;
			public bool speedBoost = false;
			public bool resistFric = false;
			public bool resistTemp = false;

			public AcquiredAbilities()
			{
			}
		} 
		AcquiredAbilities acquiredAbilities;

		struct LocksAcquired
		{
			bool gray = false;
			bool blue = false;
			bool green = false;
			bool yellow = false;
			bool red = false;

			public LocksAcquired()
			{
			}
		}
		LocksAcquired locksAcquired;

		const int maxHealthTanks = 20;
		const int maxRocketTanks = 50;
		const int maxSuperBombTanks = 30;
		bool[] collectedHealthTanks;
		bool[] collectedRocketTanks;
		bool[] collectedSuperBombTanks;

		int currentTankHP;
		int numHealthTanks;
		int numFullHealthTanks;
		int maxRocketAmmo;
		int currentRocketAmmo;
		int maxSuperBombAmmo;
		int currentSuperBombAmmo;

		// room transition
		Vector2 offset;
		Vector2 transitionHatchPos;
		TransformComponent? targetDoorTransform;
		string? targetRoomName;
		TransitionTypes? transitionDirection;

		public PlayerObject(string name) 
			: base(name)
		{
			random = new();

			transform = new(this);
			transform.Origin = new Vector2(24, 48);
			AddComponent(transform);

			animation = new(this, "sprites/Player/Bonko");
			AddComponent(animation);

			collisionRect = new(this, 19, 20, 10, 28);
			AddComponent(collisionRect);
			
			UpdateableComponentSystem.Register(this);

			idle = true;
			walking = false;
			jumping = false;
			spinning = false;
			facingRight = true;
			grounded = false;
			crouching = false;
			ledgeGrab = false;
			climbingUpLedge = false;
			wallJumping = false;
			morphed = false;
			ledgeGrabFacingRight = false;
			bonking = false;
			bombJumping = false;
			speedBoosting = false;
			sliding = false;
			shinesparking = false;
			elevator = false;

			canJump = false;
			canWallJump = false;
			canMove = true;
			canMoveTimer = TimeSpan.FromSeconds(0);
			canShoot = true;
			canShootTimer = TimeSpan.FromSeconds(0);
			showGun = true;
			angledDown = false;
			aimingDown = false;
			aimingUp = false;
			inLiquid = false;
			inLava = false;

			hsp = 0;
			vsp = 0;
			jumpSpeed = 4;
			jumpHeightCounter = 0;
			maxJumpHeightWater = (TILE_SIZE * 3) + (TILE_SIZE / 2);
			maxJumpHeightNormal = (TILE_SIZE * 4) + (TILE_SIZE / 2);
			maxJumpHeightHigh = (TILE_SIZE * 6) + (TILE_SIZE / 2);
			maxBombJumpHeight = (TILE_SIZE * 2) + (TILE_SIZE / 2);
			maxJumpHeight = maxJumpHeightNormal;
			grav = 0.3f;
			acceleration = 0.2f;
			maxFallingSpeed = 4;
			maxWalkingSpeed = 3;
			maxWalkingSpeedAir = 1.5f;
			climbingUpLedgeCounter = 0;
			crouchingTimer = 8;
			wallJumpFrames = 3;
			bombsPlaced = 0;
			superBombsPlaced = 0;
			chargeCounter = 0;
			chargeMax = 60;
			chargeImageIndex = 0;
			speedBoostCounter = 0;
			speedBoostEngageAt = TimeSpan.FromSeconds(2); // in seconds
			speedBoostSpeed = 6;
			slidingFrames = 20;
			shinesparkStored = false;
			storedShineSparkFrames = 60 * 3;
			shinesparkCounter = 0;
			shinesparkCounterFrames = TILE_SIZE * 4;
			shinesparkSpeed = 10;
			shinesparkInstance = null; // keeps track of the shinespark obj used for collision

			gunX = 0;
			gunY = 0;
			gunFrame = 0;
			wallJumpAnimCounter = 0;
			unmorphAnimCounter = -1;
			bonkingAnimCounter = -1;
			lastFramePlayedFootstep = 0;
			hue = 255;
			camShake = 0;
			shinesparkEndShakeFrames = 30;
			superRocketShakeFrames = 10;

			obj_camera = new();
			lerpAmountTowardsVolume = 0;
			lerpAmountTowardsPlayer = 0;
			screenFadeAlpha = 0;
			roomChangeFinished = false;
			transitionNow = false;
			lerpAmountTowardsHatchTargetX = 0;
			lerpAmountTowardsHatchTargetY = 0;

			collectedHealthTanks = new bool[maxHealthTanks];
			collectedRocketTanks = new bool[maxRocketTanks];
			collectedSuperBombTanks = new bool[maxSuperBombTanks];

			currentTankHP = 99;
			numHealthTanks = 0;
			numFullHealthTanks = 0;
			maxRocketAmmo = 0;
			currentRocketAmmo = 0;
			maxSuperBombAmmo = 0;
			currentSuperBombAmmo = 0;

			offset = Vector2.Zero;
			transitionHatchPos = Vector2.Zero;
			targetDoorTransform = null;
			targetRoomName = null;
			transitionDirection = null;
		}

		public override void Unload()
		{
			UpdateableComponentSystem.Deregister(this);
			base.Unload();
		}

		public void Update(GameTime gameTime)
		{
			BeginStep(gameTime);
			TickAlarms(gameTime);
			Step(gameTime);
			UpdateSprite(gameTime);
		}

		private void BeginStep(GameTime gameTime)
		{
			CheckForRoomTransition();
		}

		private void CheckForRoomTransition()
		{
			if (Globals.roomTransition)
			{
				return;
			}

			var X = transform.Position.X;
			var Y = transform.Position.Y;

#pragma warning disable CS8602 // i know the out vars are non-null if the functions returns true
			if (CollisionRectComponentSystem.IsColliding(X + 1, Y, "teleportRight", out var r))
			{
				Globals.roomTransition = true;
				offset.Y = r.Y - Y;
				transitionHatchPos.X = r.X - Globals.viewPosX;
				transitionHatchPos.Y = r.Y - Globals.viewPosY;

			}
#pragma warning restore CS8602
		}

		private void TickAlarms(GameTime gameTime)
		{
		}

		private void Step(GameTime gameTime)
		{
			if (true)
			{

			}
		}

		private void UpdateSprite(GameTime gameTime)
		{
			AnimIndex sprite_index = (AnimIndex)animation.sprite_index;
			if (!elevator)
			{
				if (idle)
				{
					if (sprite_index == AnimIndex.crouch)
					{
						if (animation.image_index > 0)
						{
							animation.image_speed = -1;
						}
						else
						{
							animation.ChangeAnimation((int)AnimIndex.idle);
							animation.image_index = 0;
						}
					}
					else if (sprite_index != AnimIndex.idle)
					{
						animation.ChangeAnimation((int)AnimIndex.idle);
						animation.image_index = 0;
					}
					else
					{
						blinkTimer -= gameTime.ElapsedGameTime;
						if (blinkTimer.TotalSeconds <= 0)
						{
							animation.image_speed = 1;
							animation.image_index = 1;

							blinkTimer += TimeSpan.FromSeconds((double)random.Next(0, 3) + random.NextDouble());
						}

						if (animation.image_index == 0)
						{
							animation.image_speed = 0;
						}
					}
				}
			}
		}
	}
}
