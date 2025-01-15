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
        Texture2D rectangleTexture;

        Rectangle playButton;

        Vector2 backgroundScroll1, backgroundScroll2;

        SpriteFont menuFont;

        Player ship;
        List<Asteroid> asteroids;

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

            playButton = new Rectangle(350, 300, 100, 50);

            backgroundScroll1 = new Vector2(0, 0);
            backgroundScroll2 = new Vector2(0, 0 - window.Height);

            base.Initialize();
            ship = new Player(shipTexture, 350);

            asteroids = new List<Asteroid>();
            asteroids.Add(new Asteroid(asteroidTexture, generator.Next(0, 800 - asteroidSize), asteroidSize));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            shipTexture = Content.Load<Texture2D>("rocketship");
            asteroidTexture = Content.Load<Texture2D>("asteroid");
            starBackgroundTexture = Content.Load<Texture2D>("star_background");
            rectangleTexture = Content.Load<Texture2D>("rectangle");
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
                if (mouseState.LeftButton == ButtonState.Pressed && playButton.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1)))
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
                    asteroidSize = generator.Next(20, 100);
                    asteroids.Add(new Asteroid(asteroidTexture, generator.Next(0, window.Width - asteroidSize), asteroidSize));
                }

                ship.HSpeed = 0;

                backgroundScroll1.Y += 3;
                backgroundScroll2.Y += 3;
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

                if (backgroundScroll1.Y >= window.Height)
                {
                    backgroundScroll1.Y = 0 - window.Height;
                }
                if (backgroundScroll2.Y >= window.Height)
                {
                    backgroundScroll2.Y = 0 - window.Height;
                }

                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (ship.Collide(asteroids[i].Bounds))
                    {
                        playerHealth -= (int)Math.Round((asteroids[i].Bounds.Width / 4.0), 0);
                        asteroids.Remove(asteroids[i]);
                        i--;
                    }
                }

                if (playerHealth <= 0)
                    screen = Screen.GameOver;

                ship.Update();
            }
            else if (screen == Screen.GameOver)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && playButton.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1)))
                {
                    playerHealth = 100;
                    ship = new Player(shipTexture, 350);

                    asteroids.Clear();
                    asteroids.Add(new Asteroid(asteroidTexture, generator.Next(0, 800 - asteroidSize), asteroidSize));

                    screen = Screen.Game;
                }
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
                _spriteBatch.Draw(rectangleTexture, playButton, Color.Gray);
                _spriteBatch.DrawString(menuFont, "Play", new Vector2(372, 306), Color.White);
                _spriteBatch.DrawString(menuFont, "Spaceship Game", new Vector2(260, 100), Color.White);
            }
            else if (screen == Screen.Game)
            {
                _spriteBatch.Draw(starBackgroundTexture, backgroundScroll1, Color.White);
                _spriteBatch.Draw(starBackgroundTexture, backgroundScroll2, Color.White);
                ship.Draw(_spriteBatch);
                foreach (Asteroid asteroid in asteroids)
                    asteroid.Draw(_spriteBatch);
                _spriteBatch.DrawString(menuFont, "Health: " + playerHealth.ToString(), new Vector2(0, 0), Color.White);
            }
            else if (screen == Screen.GameOver)
            {
                _spriteBatch.Draw(starBackgroundTexture, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(rectangleTexture, playButton, Color.Gray);
                _spriteBatch.DrawString(menuFont, "Retry", new Vector2(363, 306), Color.White);
                _spriteBatch.DrawString(menuFont, "Game Over!", new Vector2(300, 100), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
