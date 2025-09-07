using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Logic;
using Input;

namespace Bonko;

public class GameApplication : Core
{
	private Texture2D Logo;
	private RenderTarget2D NativeRenderTarget;
	private Rectangle ActualScreenRectangle;
	private int GameScale;
	private const int MinGameScale = 1;
	private const int MaxGameScale = 10;
	private const int DefaultGameScale = 4;
	public const int NativeResolutionWidth = 320;
	public const int NativeResolutionHeight = 176;

	public GameApplication()
		: base("Bonko", NativeResolutionWidth, NativeResolutionHeight, false, true)
	{
		IsFixedTimeStep = false;
		Window.AllowUserResizing = true;
		GameScale = 1;
	}

	protected override void Initialize()
	{
		// TODO: Add your initialization logic here
		base.Initialize();

		NativeRenderTarget = new RenderTarget2D(GraphicsDevice, NativeResolutionWidth, NativeResolutionHeight);
		SetDefaultGameScale();

		Window.ClientSizeChanged += Window_ClientSizeChanged;
	}

	protected override void LoadContent()
	{
		base.LoadContent();

		// TODO: use this.Content to load your game content here
		Logo = Content.Load<Texture2D>("images/logo");
	}

	protected override void Update(GameTime gameTime)
	{
		InputInfo.Update();

		if (InputInfo.WasButtonJustPressed(Buttons.Back) || InputInfo.WasKeyJustPressed(Keys.Escape))
			Exit();
		
		if (InputInfo.WasKeyJustPressed(Keys.OemPlus))
		{
			IncreaseGameScale();
		}

		if (InputInfo.WasKeyJustPressed(Keys.OemMinus))
		{
			DecreaseGameScale();
		}

		if (InputInfo.WasKeyJustPressed(Keys.F11))
		{
			Graphics.IsFullScreen = !Graphics.IsFullScreen;
			if (!Graphics.IsFullScreen)
			{
				ApplyGameScale();
			}
			Graphics.ApplyChanges();
		}

		// TODO: Add your update logic here

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		// first render the game at native res
		GraphicsDevice.SetRenderTarget(NativeRenderTarget);
		GraphicsDevice.Clear(Color.CornflowerBlue);
		SpriteBatch.Begin();
		SpriteBatch.Draw(Logo, Vector2.Zero, Color.White);
		SpriteBatch.End();

		// then draw it scaled up to the size of the backbuffer
		GraphicsDevice.SetRenderTarget(null);
		GraphicsDevice.Clear(Color.Black);
		SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
		SpriteBatch.Draw(NativeRenderTarget, ActualScreenRectangle, Color.White);
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

	protected void SetDefaultGameScale()
	{
		GameScale = DefaultGameScale;
		ApplyGameScale();
	}

	protected void ApplyGameScale()
	{
		ActualScreenRectangle = new Rectangle(0, 0, NativeResolutionWidth * GameScale, NativeResolutionHeight * GameScale);
		Graphics.PreferredBackBufferWidth = ActualScreenRectangle.Width;
		Graphics.PreferredBackBufferHeight = ActualScreenRectangle.Height;
		Graphics.ApplyChanges();
	}
	
	private void Window_ClientSizeChanged(object sender, System.EventArgs e)
	{
		Window.ClientSizeChanged -= Window_ClientSizeChanged;
		ActualScreenRectangle = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
		Graphics.PreferredBackBufferWidth = ActualScreenRectangle.Width;
		Graphics.PreferredBackBufferHeight = ActualScreenRectangle.Height;
		
		Graphics.ApplyChanges();
		Window.ClientSizeChanged += Window_ClientSizeChanged;
	}
}
