using Microsoft.Xna.Framework.Input;

namespace Input
{
	internal static class KeyboardInfo
	{
		private static KeyboardState PrevState;
		private static KeyboardState CurrState;
		
		static KeyboardInfo()
		{
			PrevState = new KeyboardState();
			CurrState = Keyboard.GetState();
		}

		public static void Update()
		{
			PrevState = CurrState;
			CurrState = Keyboard.GetState();
		}

		public static bool IsKeyDown(Keys key)
		{
			return CurrState.IsKeyDown(key);
		}

		public static bool IsKeyUp(Keys key)
		{
			return CurrState.IsKeyUp(key);
		}

		public static bool WasKeyJustPressed(Keys key)
		{
			return CurrState.IsKeyDown(key) && PrevState.IsKeyUp(key);
		}

		public static bool WasKeyJustReleased(Keys key)
		{
			return CurrState.IsKeyUp(key) && PrevState.IsKeyDown(key);
		}
	}
}
