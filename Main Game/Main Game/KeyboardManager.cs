using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main_Game
{

    // Input types used throughout the game; these are translated to the corresponding keys in this class
    // This is initialized at a value of 1 to keep the default value (0) free to handle malformed inputs.
    public enum Inputs { Jump = 1, Left, Right, Down, Up }

    // Three states:
    // FirstTap indicates the key has just gone down 
    // Down indicates it's been held for at least one frame 
    // Up indicates it's not down at all
    public enum KeyState { FirstTap, Down, Up }

    public class KeyboardManager
    {

        // This List should line up with the numbers of the Inputs enum
        private List<List<Keys>> bindings = new List<List<Keys>>(){ null, CombineKeys(Keys.Space, Keys.Up), CombineKeys(Keys.Left, Keys.A), CombineKeys(Keys.Right, Keys.D), CombineKeys(Keys.Down, Keys.S), CombineKeys(Keys.Up, Keys.W) };

        KeyboardState prevKb;
        KeyboardState kb;

        // Returns the appropriate KeyState for the given key based on the above description
        // This only works for ONE key - setting the prevKb at the end screws over any subsequent calls in the same frame
        public KeyState GetKeyState(Keys key)
        {
            KeyState returnState;
            kb = Keyboard.GetState();
            //Check if key is down at all
            if (kb.IsKeyDown(key))
            {
                //Check if it was previously down - if so, Down
                if (prevKb.IsKeyDown(key))
                    returnState = KeyState.Down;
                //If not, FirstTap
                else
                    returnState = KeyState.FirstTap;
            }
            //If not, return Up
            else
                returnState = KeyState.Up;
            //Set the "previous" keyboard state - this should likely be done in some sort of Update method if this method is used for more than one key
            prevKb = kb;
            //Return final verdict
            return returnState;
        }

        /// <summary>
        /// Returns any keys that are in a current input type.
        /// </summary>
        /// <param name="input">The type of input we are looking for.</param>
        /// <returns>The key being pressed in this method.</returns>
        public KeyState GetKeyState(Inputs input)
        {
            if (bindings[(int)input] != null)
                foreach (Keys key in bindings[(int)input])
                {
                    if (key != default(Keys))
                    {
                        KeyState ks = GetKeyState(key);
                        if(ks != KeyState.Up)
                        {
                            return ks;
                        }
                    }
                }
            return KeyState.Up;
        }

        private static List<Keys> CombineKeys(params Keys[] ks)
        {
            return ks.ToList();
        }
    }
}
