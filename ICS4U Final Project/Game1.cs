using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace ICS4U_Final_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int asteroidSize, playerHealth;
        float seconds;

        KeyboardState keyboardState;
        MouseState mouseState;

        Rectangle window;

        Texture2D shipTexture;
        Texture2D asteroidTexture;
        Texture2D starBackgroundTexture;

        SpriteFont menuFont;

        Player ship;
        List<Asteroid> asteroids = new List<Asteroid>();

        Random generator = new Random();

        enum Screen
        {
            Menu,
            Game,
            GameOver
        }

        Screen screen;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            seconds = 0;
            asteroidSize = 50;
            playerHealth = 100;

            screen = Screen.Menu;

            base.Initialize();
            ship = new Player(shipTexture, 350);
            asteroids.Add(new Asteroid(asteroidTexture, generator.Next(0, 800 - asteroidSize), asteroidSize));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            shipTexture = Content.Load<Texture2D>("rocketship");
            asteroidTexture = Content.Load<Texture2D>("asteroid");
            starBackgroundTexture = Content.Load<Texture2D>("star_background");
            menuFont = Content.Load<SpriteFont>("menu font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (screen == Screen.Menu)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    screen = Screen.Game;
                }
            }
            else if (screen == Screen.Game)
            {
                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (seconds > 0.7)
                {
                    seconds = 0f;
                    asteroidSize = generator.Next(25, 100);
                    asteroids.Add(new Asteroid(asteroidTexture, generator.Next(0, 800 - asteroidSize), asteroidSize));
                }

                ship.HSpeed = 0;
                foreach (Asteroid asteroid in asteroids)
                {
                    asteroid.VSpeed = 3;
                    asteroid.Move();
                }

                if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                    ship.HSpeed = 3;
                else if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                    ship.HSpeed = -3;

                ship.Offscreen(window);

                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (asteroids[i].Bounds.Top >= window.Height)
                    {
                        asteroids.RemoveAt(i);
                        i--;
                    }
                }

                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (ship.Collide(asteroids[i].Bounds))
                    {
                        playerHealth -= 1;
                    }
                }

                if (playerHealth <= 0)
                    screen = Screen.GameOver;

                ship.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (screen == Screen.Menu)
            {
                _spriteBatch.Draw(starBackgroundTexture, new Vector2(0, 0), Color.White);
                _spriteBatch.DrawString(menuFont, "Spaceship Game", new Vector2(260, 100), Color.White);
            }
            else if (screen == Screen.Game)
            {
                ship.Draw(_spriteBatch);
                foreach (Asteroid asteroid in asteroids)
                    asteroid.Draw(_spriteBatch);
                _spriteBatch.DrawString(menuFont, "Health: " + playerHealth.ToString(), new Vector2(0, 0), Color.White);
            }
            else if (screen == Screen.GameOver)
            {
                _spriteBatch.DrawString(menuFont, "Game Over!", new Vector2(260, 100), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
