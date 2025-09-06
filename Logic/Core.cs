using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Logic;

public class Core : Game
{
	internal static Core instance;

	public static Core Instance => instance;

	public static GraphicsDeviceManager Graphics { get; private set; }

	// new intentionally shadows base.GraphicsDevice. so we can do Core.GraphicsDevice statically instead of Core.Instance.GraphicsDevice.
	public static new GraphicsDevice GraphicsDevice { get; private set; }

	public static SpriteBatch SpriteBatch { get; private set; }

	// new intentionally shadows base.Content. so we can do Core.GraphicsDevice statically instead of Core.Instance.GraphicsDevice.
	public static new ContentManager Content { get; private set; }


	public Core(string title, int width, int height, bool fullScreen, bool vsync)
	{
		// Ensure that multiple cores are not created.
		if (instance != null)
		{
			throw new InvalidOperationException($"Only a single {nameof(Core)} instance can be created");
		}

		// Store reference to engine for global member access.
		instance = this;

		// Create a new graphics device manager.
		Graphics = new GraphicsDeviceManager(this);

		// Set the graphics defaults.
		Graphics.PreferredBackBufferWidth = width;
		Graphics.PreferredBackBufferHeight = height;
		Graphics.IsFullScreen = fullScreen;
		Graphics.SynchronizeWithVerticalRetrace = vsync;
		Graphics.HardwareModeSwitch = false;

		// Apply the graphic presentation changes.
		Graphics.ApplyChanges();

		// Set the window title.
		Window.Title = title;
		Window.AllowAltF4 = true;
		Window.AllowUserResizing = false;

		// Set the core's content manager to a reference of the base Game's
		// content manager.
		Content = base.Content;

		// Set the root directory for content.
		Content.RootDirectory = "Content";

		// Mouse is invisible by default.
		IsMouseVisible = false;
	}

	protected override void Initialize()
	{
		base.Initialize();

		// Set the core's graphics device to a reference of the base Game's
		// graphics device.
		GraphicsDevice = base.GraphicsDevice;

		// Create the sprite batch instance.
		SpriteBatch = new SpriteBatch(GraphicsDevice);
	}
}
