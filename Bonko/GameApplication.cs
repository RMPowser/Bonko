using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Logic;
using Input;
using Graphics;
using Logic.ComponentSystems;

namespace Bonko;

public class GameApplication : Core
{
	private World World;
	private Room CurrRoom;
	private Room? NextRoom;
	private RenderTarget2D NativeRenderTarget;
	private Rectangle GameClientArea;
	private bool UsePixelFiltering;
	private Effect PixelFilterShader;
	private int GameScale;
	private const int MinGameScale = 1;
	private int MaxGameScale = 10;
	private const int DefaultGameScale = 4;
	public const int NativeResolutionWidth = 320;
	public const int NativeResolutionHeight = 176;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	public GameApplication()
#pragma warning restore CS8618
		: base("Bonko", NativeResolutionWidth, NativeResolutionHeight, false, true)
	{
		IsFixedTimeStep = false;
		Window.AllowUserResizing = true;
		UsePixelFiltering = true;
		GameScale = 1;
	}

	protected override void Initialize()
	{
		base.Initialize();

		NativeRenderTarget = new RenderTarget2D(GraphicsDevice, NativeResolutionWidth, NativeResolutionHeight);

		DetermineMaxGameScale();
		SetGameScaleToDefault();

		Window.ClientSizeChanged += Window_ClientSizeChanged!;
		World = new World();
		CurrRoom = World.GetRoom("MainDeck_0");
		CurrRoom.Load();
	}

	protected override void LoadContent()
	{
		base.LoadContent();

		PixelFilterShader = Content.Load<Effect>("shaders/PixelFilter");
		PixelFilterShader.Parameters["SourceSize"].SetValue(new Vector2(NativeResolutionWidth, NativeResolutionHeight));
	}

	protected override void Update(GameTime gameTime)
	{
		InputInfo.Update();

		if (InputInfo.WasButtonJustPressed(Buttons.Back) || InputInfo.WasKeyJustPressed(Keys.Escape))
			Exit();
		
		if (InputInfo.WasKeyJustPressed(Keys.OemPlus) && !Graphics.IsFullScreen)
		{
			IncreaseGameScale();
		}

		if (InputInfo.WasKeyJustPressed(Keys.OemMinus) && !Graphics.IsFullScreen)
		{
			DecreaseGameScale();
		}

		if (InputInfo.WasKeyJustPressed(Keys.F11))
		{
			Graphics.IsFullScreen = !Graphics.IsFullScreen;
			if (!Graphics.IsFullScreen)
			{
				SetGameScaleToDefault();
				ApplyGameScale();
			}
			Graphics.ApplyChanges();
		}

		if (InputInfo.WasKeyJustPressed(Keys.F12))
		{
			UsePixelFiltering = !UsePixelFiltering;
		}

		if (InputInfo.WasKeyJustPressed(Keys.F5))
		{
			string currRoomName = CurrRoom.Name;
			World.Reload();
			CurrRoom = World.GetRoom(currRoomName);
			CurrRoom.Load();
		}

		if (NextRoom != null)
		{
			CurrRoom.Unload();
			CurrRoom = NextRoom;
			NextRoom = null;
			CurrRoom.Load();
		}

		UpdateableComponentSystem.Update(gameTime);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		// first render the game at native res
		GraphicsDevice.SetRenderTarget(NativeRenderTarget);
		GraphicsDevice.Clear(Color.CornflowerBlue);
		SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
		DrawableComponentSystem.Draw(SpriteBatch);
		SpriteBatch.End();

		// then draw it scaled up to the size of the backbuffer
		GraphicsDevice.SetRenderTarget(null);
		GraphicsDevice.Clear(Color.Black);
		SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: UsePixelFiltering ? PixelFilterShader : null);
		SpriteBatch.Draw(NativeRenderTarget, GameClientArea, Color.White);
		SpriteBatch.End();

		base.Draw(gameTime);
	}

	protected void IncreaseGameScale()
	{
		GameScale++;
		if (GameScale > MaxGameScale)
		{
			GameScale = MaxGameScale;
		}
		ApplyGameScale();
	}

	protected void DecreaseGameScale()
	{
		GameScale--;
		if (GameScale < MinGameScale)
		{
			GameScale = MinGameScale;
		}
		ApplyGameScale();
	}

	protected void DetermineMaxGameScale()
	{
		var screenRes = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
		int shortestDimension = MathHelper.Min(screenRes.Width, screenRes.Height);
		MaxGameScale = shortestDimension / (shortestDimension == screenRes.Width ? NativeResolutionWidth : NativeResolutionHeight);
	}

	protected void SetGameScaleToDefault()
	{
		GameScale = DefaultGameScale;
		ApplyGameScale();
	}

	protected void ApplyGameScale(Rectangle? newActualScreenRect = null)
	{
		GameClientArea = newActualScreenRect ?? new(0, 0, NativeResolutionWidth * GameScale, NativeResolutionHeight * GameScale);
		Graphics.PreferredBackBufferWidth = GameClientArea.Width;
		Graphics.PreferredBackBufferHeight = GameClientArea.Height;
		Graphics.ApplyChanges();

		PixelFilterShader.Parameters["OutputSize"].SetValue(new Vector2(GameClientArea.Width, GameClientArea.Height));
	}
	
	private void Window_ClientSizeChanged(object sender, System.EventArgs e)
	{
		Window.ClientSizeChanged -= Window_ClientSizeChanged!;

		var clientBounds = Window.ClientBounds;
		int shortestDimension = MathHelper.Min(clientBounds.Width, clientBounds.Height);
		GameScale = shortestDimension / (shortestDimension == GameClientArea.Width ? NativeResolutionWidth : NativeResolutionHeight);
		ApplyGameScale(new Rectangle(0, 0, clientBounds.Width, clientBounds.Height));

		Window.ClientSizeChanged += Window_ClientSizeChanged!;
	}
}
