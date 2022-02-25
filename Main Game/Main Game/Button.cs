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
	public class Button
	{
		private Texture2D unpressed;
		private Texture2D mouseOver;
		private Texture2D pressed;
		private Rectangle pos;

		/// <summary>
		/// Cteates a button
		/// </summary>
		/// <param name="inUnpressed">The texture to be used when the button is unpressed</param>
		/// <param name="inMouseOver">The texture to be used when the button is moused over</param>
		/// <param name="inPressed">The texture to be used when the button is clicked</param>
		/// <param name="inPos">The position and size of the button</param>
		public Button(Texture2D inUnpressed, Texture2D inMouseOver, Texture2D inPressed, Rectangle inPos)
		{
			unpressed = inUnpressed;
			mouseOver = inMouseOver;
			pressed = inPressed;
			pos = inPos;
		}

		/// <summary>
		/// Gets and sets the position and dimensions of the button
		/// </summary>
		public Rectangle Location
		{
			get
			{
				return pos;
			}
			set
			{
				pos = value;
			}
		}

		/// <summary>
		/// gets and sets the x position of the button
		/// </summary>
		public int X
		{
			get
			{
				return pos.X;
			}
			set
			{
				pos = new Rectangle(value, pos.Y, pos.Width, pos.Height);
			}
		}

		/// <summary>
		/// Gets and sets the Y position of the button
		/// </summary>
		public int Y
		{
			get
			{
				return pos.Y;
			}
			set
			{
				pos = new Rectangle(pos.X, value, pos.Width, pos.Height);
			}
		}

		/// <summary>
		/// Draws the button's correct texture at its coordinate
		/// </summary>
		/// <param name="sb">The spritebatch</param>
		/// <param name="ms">The state of the mouse, which is used to check if the mouse is inside or clicking the button</param>
		public void Draw(SpriteBatch sb, MouseState ms)
		{
			//checks if the mouse is in the button
			if(ContainsMouse(ms))
			{
				//checks if the button is being clicked
				if (IsClicked(ms))
					sb.Draw(pressed, pos, Color.White);
				else
					sb.Draw(mouseOver, pos, Color.White);
			}
			else
			{
				sb.Draw(unpressed, pos, Color.White);
			}
		}

		/// <summary>
		/// Checks if the mouse is in the button
		/// </summary>
		/// <param name="ms">The mouse state used to check if the mouse is in the button</param>
		/// <returns>Whether or not the mouse is in the button dimensions</returns>
		private bool ContainsMouse(MouseState ms)
		{
			return (ms.Position.X > pos.X && ms.Position.X < pos.Width + pos.X && ms.Position.Y > pos.Y && ms.Position.Y < pos.Height + pos.Y);
		}
		
		/// <summary>
		/// Checks if the mouse is in the button and the button is being clicked
		/// </summary>
		/// <param name="ms">The mouse state to check if the mouse is in the button and clicking</param>
		/// <returns>Whether or not the button is being clicked</returns>
		public bool IsClicked(MouseState ms)
		{
			return ContainsMouse(ms) && ms.LeftButton == ButtonState.Pressed;
		}
	}
}
