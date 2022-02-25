using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main_Game
{
    class playertest
    {
        double yVelocity;
        double xVelocity;
        double gravityTick = -0.1635;

        Rectangle position;
        Texture2D texture;
        int floor;

        public playertest(Texture2D texture, Rectangle position, int floor)
        {
            this.texture = texture;
            this.position = position;
            this.floor = floor;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        public void Update()
        {


            if (position.Y < floor)
                yVelocity = 0;
        }
    }
}
