using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Input
{
	public static class InputInfo
	{
		static InputInfo()
		{
		}

		public static void Update()
		{
			KeyboardInfo.Update();
			GamePadInfo.Update();
		}

		public static bool IsKeyDown(Keys key)
		{
			return KeyboardInfo.IsKeyDown(key);
		}

		public static bool IsKeyUp(Keys key)
		{
			return KeyboardInfo.IsKeyUp(key);
		}

		public static bool WasKeyJustPressed(Keys key)
		{
			return KeyboardInfo.WasKeyJustPressed(key);
		}

		public static bool WasKeyJustReleased(Keys key)
		{
			return KeyboardInfo.WasKeyJustReleased(key);
		}



		public static bool IsButtonDown(Buttons button, PlayerIndex i = PlayerIndex.One)
		{
			return GamePadInfo.IsButtonDown(button, i);
		}

		public static bool IsButtonUp(Buttons button, PlayerIndex i = PlayerIndex.One)
		{
			return GamePadInfo.IsButtonUp(button, i);
		}

		public static bool WasButtonJustPressed(Buttons button, PlayerIndex i = PlayerIndex.One)
		{
			return GamePadInfo.WasButtonJustPressed(button, i);
		}

		public static bool WasButtonJustReleased(Buttons button, PlayerIndex i = PlayerIndex.One)
		{
			return GamePadInfo.WasButtonJustReleased(button, i);
		}

		static public Vector2 LeftThumbStick(PlayerIndex i = PlayerIndex.One)
		{
			return GamePadInfo.LeftThumbStick(i);
		}

		static public Vector2 RightThumbStick(PlayerIndex i = PlayerIndex.One)
		{
			return GamePadInfo.RightThumbStick(i);
		}

		static public float LeftTrigger(PlayerIndex i = PlayerIndex.One)
		{
			return GamePadInfo.LeftTrigger(i);
		}

		static public float RightTrigger(PlayerIndex i = PlayerIndex.One)
		{
			return GamePadInfo.RightTrigger();
		}
	}
}
