using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Main_Game
{
    class Goal<T> : Solid
    {
        private Action<T> action;
		private T actionParam;
		private Animation triggeredAni;
		private Animation finishedAni;
		private List<Texture2D> fireworks;
		private bool triggering;
		private bool triggered;


		/// <summary>
		/// The constructor for a goal object.
		/// </summary>
		/// <param name="position">This goal's position in the game world.</param>
		/// <param name="tex">This goal's animation (textures included).</param>
		/// <param name="action">The event tied to this goal.</param>
		/// <param name="nextLevel">The parameter tied to this goal's event.</param>
		public Goal(Rectangle position, Animation tex, Action<T> action, T actionParam, Animation triggerAni, Animation claimedAni, List<Texture2D> fireworkTextures) : base(position, tex, false)
        {
            this.action = action;
            this.actionParam = actionParam;
			fireworks = fireworkTextures;
			triggeredAni = triggerAni;
			finishedAni = claimedAni;
			triggered = false;
        }

		public override void Trigger()
		{
			//Game1.State = MainGameState.Goal;
            if (this.Texture != null)
            {
                if (!triggering)
                {
                    Game1.sparkles.AddRange(Particle.GenerateParticles(new Point(-10, -10), new Point(10, 10), new Point(0, -1), new Point(Bounds.X, Bounds.Y), fireworks, new Color(255, 255, 0), Color.Red, 30, new Random()));
                    this.Ani = triggeredAni;
                    triggering = true;
                }
                if (this.triggeredAni.isLastFrame())
                {
                    Triggered();
                }
            }
            else
            {
                Triggered();
            }
			
		}

		public override void Update()
		{
			if (triggering && !triggered)
			{
				Trigger();
			}
		}

		public override void Draw(SpriteBatch sb)
		{
            if (this.Texture != null)
            {
                if (triggering)
                {
                    if (triggeredAni.isLastFrame())
                        triggeredAni = finishedAni;
                    triggeredAni.Draw(sb, Bounds);
                }
                else
                {
                    Ani.Draw(sb, Bounds);
                }
            }
            
		}

		/// <summary>
		/// Activates the constructor-specified method with the constructor-specified parameter.
		/// </summary>
		public void Triggered()
        {
			triggered = true;
			action(actionParam);
        }
    }
}
