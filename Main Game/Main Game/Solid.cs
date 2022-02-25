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

    /// <summary>
    /// This Class serves to represent the various floors and walls that are static and solid.
    /// </summary>
    public class Solid
    {
        #region fields 

        private Rectangle pos;
        private Animation texture;
        private Boolean solid;
        private Boolean enabled = true;

        #endregion fields

        #region properties

        public Rectangle Bounds
        {
            get
            {
                return pos;
            }
        }

        public Animation Texture
        {
            get
            {
                return texture;
            }
        }

		public Animation Ani
		{
			get
			{
				return texture;
			}
			set
			{
				texture = value;
			}
		}

        public Boolean IsCollide
        {
            get
            {
                return solid;
            }
        }

        public Boolean Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                //it's quick, it's easy, and it's free: making the solid tiny af
                pos = new Rectangle(0, 0, 0, 0);
            }
        }

        #endregion properties

        /// <summary>
        /// initializes the object with a texture and a rectangle position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tex"></param>
        public Solid(Rectangle position, Animation tex, Boolean isSolid = true)
        {
            pos = position;
            texture = tex;
            solid = isSolid;
        }

        /// <summary>
        /// Returns true if the actor passed in is colliding with this solid, false otherwise.
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public virtual bool ActorIsCollide(Actor check)
        {
            if(pos.Intersects(check.Bounds) && enabled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Will draw the solid to the screen with its texture at its position
        /// </summary>
        public virtual void Draw(SpriteBatch sb)
        {
            if(enabled)
                texture.Draw(sb, pos);
        }

		public virtual void Update()
		{

		}

        public virtual void Trigger()
        {

        }
    }
}
