using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ICS4U_Final_Project
{
    internal class Player
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _speed;

        public Player(Texture2D texture, int x)
        {
            _texture = texture;
            _location = new Rectangle(x, 390, 50, 105);
            _speed = new Vector2();
        }

        public float HSpeed
        {
            get { return _speed.X; }
            set { _speed.X = value; }
        }

        private void Move()
        {
            _location.X += (int)_speed.X;
        }

        public void Offscreen(Rectangle window)
        {
            if (_location.Right >= window.Width)
            {
                _location.X = window.Width - _location.Width;
            }
            else if (_location.Left <= 0)
            {
                _location.X = 0;
            }
        }

        public bool Collide(Rectangle item)
        {
            return _location.Intersects(item);
        }

        public void Update()
        {
            Move();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
