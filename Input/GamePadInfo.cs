using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Input
{
	internal static class GamePadInfo
	{
		private struct GamePadStatePair(GamePadState prevState, GamePadState currState)
		{
			public GamePadState PrevState = prevState;
			public GamePadState CurrState = currState;
		}

		private static Dictionary<PlayerIndex, GamePadStatePair> states;
		private static readonly int MaxSupportedPlayers;

		static GamePadInfo()
		{
			states = [];
			MaxSupportedPlayers = Math.Min(GamePad.MaximumGamePadCount, (int)Enum.GetValues<PlayerIndex>().Last() + 1);

			for (int i = 0; i < MaxSupportedPlayers; i++)
			{
				var currState = GamePad.GetState(i);
				if (currState.IsConnected)
				{
					states.Add(Enum.GetValues<PlayerIndex>()[i], new(GamePadState.Default, currState));
				}
			}
		}

		static public void Update()
		{
			if (states.Count <= 0)
			{
				return;
			}

			for (PlayerIndex i = 0; i < states.Keys.Last() + 1; i++)
			{
				if (states.TryGetValue(i, out GamePadStatePair state))
				{
					var currState = GamePad.GetState(i);
					if (currState.IsConnected)
					{
						state.PrevState = state.CurrState;
						state.CurrState = currState;
						states[i] = state;
					}
				}
			}
		}

		static public bool IsButtonDown(Buttons button, PlayerIndex i = PlayerIndex.One)
		{
			if (states.TryGetValue(i, out GamePadStatePair state))
			{
				if (state.CurrState.IsConnected)
				{
					return state.CurrState.IsButtonDown(button);
				}
			}

			return false;
		}

		static public bool IsButtonUp(Buttons button, PlayerIndex i = PlayerIndex.One)
		{
			if (states.TryGetValue(i, out GamePadStatePair state))
			{
				if (state.CurrState.IsConnected)
				{
					return state.CurrState.IsButtonUp(button);
				}
			}

			return false;
		}

		static public bool WasButtonJustPressed(Buttons button, PlayerIndex i = PlayerIndex.One)
		{
			if (states.TryGetValue(i, out GamePadStatePair state))
			{
				if (state.CurrState.IsConnected)
				{
					return state.CurrState.IsButtonDown(button) && state.PrevState.IsButtonUp(button);
				}
			}

			return false;
		}

		static public bool WasButtonJustReleased(Buttons button, PlayerIndex i = PlayerIndex.One)
		{
			if (states.TryGetValue(i, out GamePadStatePair state))
			{
				if (state.CurrState.IsConnected)
				{
					return state.CurrState.IsButtonUp(button) && state.PrevState.IsButtonDown(button);
				}
			}

			return false;
		}

		static public Vector2 LeftThumbStick(PlayerIndex i = PlayerIndex.One)
		{
			if (states.TryGetValue(i, out GamePadStatePair state))
			{
				if (state.CurrState.IsConnected)
				{
					return state.CurrState.ThumbSticks.Left;
				}
			}
			
			return Vector2.Zero;
		}

		static public Vector2 RightThumbStick(PlayerIndex i = PlayerIndex.One)
		{
			if (states.TryGetValue(i, out GamePadStatePair state))
			{
				if (state.CurrState.IsConnected)
				{
					return state.CurrState.ThumbSticks.Right;
				}
			}

			return Vector2.Zero;
		}

		static public float LeftTrigger(PlayerIndex i = PlayerIndex.One)
		{
			if (states.TryGetValue(i, out GamePadStatePair state))
			{
				if (state.CurrState.IsConnected)
				{
					return state.CurrState.Triggers.Left;
				}
			}

			return 0.0f;
		}

		static public float RightTrigger(PlayerIndex i = PlayerIndex.One)
		{
			if (states.TryGetValue(i, out GamePadStatePair state))
			{
				if (state.CurrState.IsConnected)
				{
					return state.CurrState.Triggers.Right;
				}
			}

			return 0.0f;
		}
	}
}
