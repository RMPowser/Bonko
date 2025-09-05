using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Logic;

namespace Bonko;

public class GameApplication : Core
{
	private Texture2D _logo;
	private int _gameScale;
	private int _minGameScale;
	private int _maxGameScale;
	public GameApplication()
		: base("Bonko", 320, 176, false, true)
	{
		_gameScale = 1;
		_minGameScale = 1;
		_maxGameScale = 10;
	}

	protected override void Initialize()
	{
		// TODO: Add your initialization logic here

		base.Initialize();
	}

	protected override void LoadContent()
	{
		base.LoadContent();

		// TODO: use this.Content to load your game content here
		_logo = Content.Load<Texture2D>("images/logo");
	}

	protected override void Update(GameTime gameTime)
	{
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();
		if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
		{
			IncreaseGameScale();
		}
		if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
		{
			DecreaseGameScale();
		}

		// TODO: Add your update logic here

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		// TODO: Add your drawing code here
		SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

		SpriteBatch.Draw(_logo, Vector2.Zero, Color.White);

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
		Graphics.PreferredBackBufferWidth = (int)NativeResolution.X * _gameScale;
		Graphics.PreferredBackBufferHeight = (int)NativeResolution.Y * _gameScale;
		Graphics.ApplyChanges();
	}
	protected void DecreaseGameScale()
	{
		_gameScale--;
		if (_gameScale < _minGameScale)
		{
			_gameScale = _minGameScale;
		}
		Graphics.PreferredBackBufferWidth = (int)NativeResolution.X * _gameScale;
		Graphics.PreferredBackBufferHeight = (int)NativeResolution.Y * _gameScale;
		Graphics.ApplyChanges();
	}
}
