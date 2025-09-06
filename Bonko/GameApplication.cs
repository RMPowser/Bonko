using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Logic;
using Input;

namespace Bonko;

public class GameApplication : Core
{
	private Texture2D _logo;
	private RenderTarget2D _nativeRenderTarget;
	private Rectangle _actualScreenRectangle;
	private int _gameScale;
	private const int _minGameScale = 1;
	private const int _maxGameScale = 10;
	private const int _defaultGameScale = 4;
	public const int NativeResolutionWidth = 320;
	public const int NativeResolutionHeight = 176;

	public GameApplication()
		: base("Bonko", NativeResolutionWidth, NativeResolutionHeight, false, true)
	{
		_gameScale = 1;
	}

	protected override void Initialize()
	{
		// TODO: Add your initialization logic here
		base.Initialize();

		_nativeRenderTarget = new RenderTarget2D(GraphicsDevice, NativeResolutionWidth, NativeResolutionHeight);
		SetDefaultGameScale();
	}

	protected override void LoadContent()
	{
		base.LoadContent();

		// TODO: use this.Content to load your game content here
		_logo = Content.Load<Texture2D>("images/logo");
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

		// TODO: Add your update logic here

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		// first render the game at native res
		GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
		GraphicsDevice.Clear(Color.CornflowerBlue);
		SpriteBatch.Begin();
		SpriteBatch.Draw(_logo, Vector2.Zero, Color.White);
		SpriteBatch.End();

		// then draw it scaled up to the size of the backbuffer
		GraphicsDevice.SetRenderTarget(null);
		GraphicsDevice.Clear(Color.Black);
		SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
		SpriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White);
		SpriteBatch.End();

		base.Draw(gameTime);
	}

	protected void IncreaseGameScale()
	{
		_gameScale++;
		if (_gameScale > _maxGameScale)
		{
			_gameScale = _maxGameScale;
		}
		ApplyGameScale();
	}

	protected void DecreaseGameScale()
	{
		_gameScale--;
		if (_gameScale < _minGameScale)
		{
			_gameScale = _minGameScale;
		}
		ApplyGameScale();
	}

	protected void SetDefaultGameScale()
	{
		_gameScale = _defaultGameScale;
		ApplyGameScale();
	}

	protected void ApplyGameScale()
	{
		_actualScreenRectangle = new Rectangle(0, 0, NativeResolutionWidth * _gameScale, NativeResolutionHeight * _gameScale);
		Graphics.PreferredBackBufferWidth = _actualScreenRectangle.Width;
		Graphics.PreferredBackBufferHeight = _actualScreenRectangle.Height;
		Graphics.ApplyChanges();
	}
}
