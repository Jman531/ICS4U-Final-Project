using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS4U_Final_Project
{
    internal class Asteroid
    {
        private Texture2D _texture;
        private Rectangle _rectangle;
        private Vector2 _speed;

        public Asteroid(Texture2D texture, int x, int size)
        {
            _texture = texture;
            _rectangle = new Rectangle(x, 0 - size, size, size);
            _speed = new Vector2();
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public Rectangle Bounds
        {
            get { return _rectangle; }
            set { _rectangle = value; }
        }

        public float VSpeed
        {
            get { return _speed.Y; }
            set { _speed.Y = value; }
        }

        public void Move()
        {
            _rectangle.Y += (int)_speed.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
        }
    }
}
