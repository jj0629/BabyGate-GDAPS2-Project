using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Main_Game
{
    public class Enemy : Actor
    {
		#region fields

        private List<String> playerOptions;
        private List<String> enemyOptions;

        private string enemyType;

        #region PhysicsVars

        //Unless these are replaced (as floor should be), all of these should initially be included alongside the physics code
        //Constants can be changed to modify the properties of the jump

        //Gravity-related variables
        private int yVelocity = 0;
        private const int gravity = 1;
        //Instances of floor should be replaced by the Y of whatever Solid is below the Player
        private int floor = 500;
        private bool grounded = true;

        //Left-right movement related variables
        private int xVelocity = 0;

        //General movement related variables
        private Vector2 initialDirection;

        //Landing-related objects
        Rectangle yCollider;

        #endregion

        private bool dead;

		#endregion fields

		#region properties

		/// <summary>
		/// Gets the options the player has when in combat with this enemy.
		/// </summary>
		public List<String> PlayerOptions
        {
            get
            {
                return playerOptions;
            }
        }


        /// <summary>
        /// Gets the options this enemy has when in combat.
        /// </summary>
        public List<String> EnemyOptions
        {
            get
            {
                return enemyOptions;
            }
        }

        public string EnemyType
        {
            get
            {
                return enemyType;
            }
            set
            {
                enemyType = value;
            }
        }

        public bool Dead
        {
            get
            {
                return dead;
            }
            set
            {
                dead = value;
            }
        }

        //Make yCollider available to other classes
        public Rectangle YCollider
        {
            get { return yCollider; }
        }

        //Set floor correctly
        public int Floor
        {
            get { return floor + pos.Height; }
            set { floor = value - pos.Height; }
        }

        /// <summary>
        /// Returns the smallest possible Rectangle containing both the Enemy bounds and the YCollider
        /// <para>Useful for determining large areas to check against the enemy at once</para>
        /// </summary>
        public Rectangle ColliderCheck
        {
            get
            {
                return new Rectangle(Math.Min(pos.X, YCollider.X),
                                     Math.Min(pos.Y, YCollider.Y),
                                     Math.Max(pos.Width, YCollider.Width),
                                     Math.Max(pos.Height, YCollider.Height));
            }
        }

        #endregion properties


        /// <summary>
        /// initializes the enemy with a texture and a position
        /// </summary>
        /// <param name="texture">The texture of the enemy</param>
        /// <param name="rectanglePos">The position of the enemy</param>
        /// <param name="startingMovement">The speed and direction the enemy will move in at the beginning. Leave as 0,0 if you don't want the enemy to move.</param>
        /// <param name="startingMovement">The amount of space you want the enemy to wander around in. Leave as 0 if you don't want it to cycle movement. Make it negative if you want the enemy's cycle to be symmetrical.</param>
        public Enemy(Animation t, Animation walk, Rectangle r, Vector2 startingMovement) : base(t, walk, r)
        {
            enemyOptions = new List<string>();
            playerOptions = new List<string>();

            initialDirection = startingMovement;
            xVelocity = (int)startingMovement.X;
            yCollider = new Rectangle(pos.X + pos.Width / 8, pos.Y + pos.Height, (int)(pos.Width * 0.75), 300);

            dead = false;
        }

		public void Update()
		{
            if (Dead)
                return;
            //If the actor isn't grounded, subtract gravity from the velocity
            if (!grounded)
            {
                yVelocity += gravity;
            }
            //Modify the actor's position by the calculated velocity
            Y += yVelocity;

            //If the actor's FALLING through the floor and not grounded:
            // - Set their Y value to the floor
            // - Set their Y velocity to 0
            // - Make them grounded
            if (pos.Y >= floor && !grounded /* && yVelocity < 0 */)
            {
                pos.Y = floor;
                yVelocity = 0;
                grounded = true;
            }
            if(pos.Y >= floor)
            {
                if(!grounded)
                {
                    if(yVelocity < 0)
                    {

                    }
                }
            }
            //If the actor is above the floor, they're no longer grounded
            else if (pos.Y < floor)
            {
                grounded = false;
            }
            //Otherwise, keep the current grounded value

            yCollider = new Rectangle(pos.X + pos.Width / 8, pos.Y + pos.Height, (int)(pos.Width * 0.75), 300);

            X += xVelocity;


			if (xVelocity < 0)
				mt = MovementType.MovingLeft;
			else if (xVelocity > 0)
				mt = MovementType.MovingRight;
		}

        public void CollideWithCeiling(int ceilingY)
        {
            //Make sure the enemy isn't *already* falling
            //This prevents "hitching" when the player is falling and pushing against the side of a block
            if (yVelocity >= 0)
            {
                yVelocity = 0;
                Y = ceilingY;
            }
        }

        public override void Draw(SpriteBatch sb, Color color, bool debug)
        {
            if(!dead)
            {
                base.Draw(sb, color, debug);
            }
        }

        public void ReverseDirection()
        {
            xVelocity = -xVelocity;

			if (mt == MovementType.MovingRight)
				mt = MovementType.MovingLeft;
			else
				mt = MovementType.MovingRight;
		}
    }
}
