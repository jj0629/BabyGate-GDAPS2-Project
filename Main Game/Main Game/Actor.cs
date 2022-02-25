using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Main_Game
{
    public class Actor
    {
		public enum MovementType { Idle, MovingLeft, MovingRight } //this is mostly just for drawing purposes

		#region Fields

		protected Rectangle pos;
        protected Animation idleTex;
		protected Animation movingTex;
		protected MovementType mt;

        #endregion

        #region Properties

        public Rectangle Bounds { get { return pos; } }

        public virtual int X
        {
            get { return pos.X; }
            set { pos.X = value; }
        }

        public virtual int Y
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        public int Width
        {
            get { return pos.Width; }
            set { pos.Width = value; }
        }

        public int Height
        {
            get { return pos.Height; }
            set { pos.Height = value; }
        }

		public MovementType MoveState
		{
			get
			{
				return mt;
			}
			set
			{
				mt = value;
			}
		}

        public Texture2D Texture
        {
            get
            {
                return idleTex.SpriteSheet;
            }
			//TODO this is icky... get rid of this and just use an animation property probs
            set
            {
                idleTex = new Animation(value, value.Height);
            }
        }

		public Animation IdleAnimation
		{
			get
			{
				return idleTex;
			}
			set
			{
				idleTex = value;
			}
		}

		public Animation WalkingAnimation
		{
			get
			{
				return movingTex;
			}
			set
			{
				movingTex = value;
			}
		}

        #endregion

        //Most basic constructor
        public Actor(Animation texture, Animation walking, Rectangle position, MovementType inmt = MovementType.Idle)
        {
            idleTex = texture;
			movingTex = walking;
            pos = position;
			mt = inmt;
        }

        //Basic, overridable draw method
        public virtual void Draw(SpriteBatch sb, Color color, bool debug)
        {
			switch(mt)
			{
				case MovementType.MovingLeft:
					movingTex.Draw(sb, pos);
					break;
				case MovementType.MovingRight:
					movingTex.Draw(sb, pos, SpriteEffects.FlipHorizontally);
					break;
				case MovementType.Idle:
					idleTex.Draw(sb, pos);
					break;
			}
		}

        //Non-overridable shortcut Draw method
        //Should be compatible with the overriden Draw() method in all cases
        public void Draw(SpriteBatch sb)
        {
            Draw(sb, Color.White, true);
        }
    }
}
