using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Main_Game
{
    // This'll be the main finite state machine that handles the overall game state.
    public enum MainGameState { StartMenu, HelpMenu, BattleTransition, Battle, GameOver, Goal, Game}
    public enum BattleState { Transition, Input, Combat, Show, Conclusion }

    /// <summary>
    /// This is Justin's attempt at a funny.
    /// Lars Puglise
    /// This is the main type for your game. YEEET
    /// Luke Roberge has no unique comment to make
    /// Paxton wishes for a swift death.
    /// </summary>
    public class Game1 : Game
    {
        // For the main finite state machine switching between menus, platforming, and combat
        public static MainGameState State;

        //Test test.
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		MouseState ms;

		Button startButton;
		Button backButton;
		Button helpButton;
		Button quitButton;

		Animation battleTransition;

        KeyboardManager debugCheck;

		Solid battlePlatforms;

        // Enemy textures
        // counterpoint: why here
        // just initialize them in LoadContent
        //Texture2D paper;
        //Texture2D rock;
        //Texture2D scissors;

        List<Texture2D> levelTextures;
		List<Texture2D> sparkleTextures;
		List<Texture2D> goalFireworks;

		Texture2D mouse;
		Texture2D mouseClickable;

        Level mainLevel;

        bool isDebugging;

        public bool IsDebugging
        {
            get
            {
                return isDebugging;
            }
            set
            {
                isDebugging = value;
            }
        }

        #region CombatFields

        //currentCombatEnemy contains the enemy combat started with
            //playerHealth has the player health for the level
        private Enemy currentCombatEnemy;
        
        public Enemy CurrentCombatEnemy
        {
            get
            {
                return currentCombatEnemy;
            }
            set
            {
                currentCombatEnemy = value;
            }
        }

        private Random enemyChoiceMaker = new Random();
        private int playerHealth;

        //used for loops in combat
        private bool combatDone = false;
        private bool validInput = false;
        private BattleState battleState = BattleState.Transition;

        //used to hold some frames so we wait when we need to
        private int frameBuffer;

        //used to draw what each side picked during combat
        private Texture2D rockResult;
        private Texture2D paperResult;
        private Texture2D scissorsResult;
        private Texture2D lizardResult;
        private Texture2D spockResult;

        //used for gathering user input
        private KeyboardState keyboard;
        private Keys[] input;
        
        //represent choices in combat
        private string enemyChoice = ""; //string holding enemy's play
        private string playerChoice = ""; //players play
        
        private string winner = ""; //see below
                                    //contains either:
                                    // "player" << player won
                                    // "enemy" << enemy won
                                    // "tie" << they tied



        #endregion

        #region CombatTransitionFields
        Rectangle worldTop;
        Rectangle worldBot;
        Rectangle viewTop;
        Rectangle viewBot;

        int pos;
        #endregion

        Button rockButton;
		Button paperButton;
		Button scissorsButton;
		Button spockButton;
		Button lizardButton;

		Button title;

		//Texture2D spark;
		//Sparkle yellow;
		public static List<Particle> sparkles = new List<Particle>();

		SpriteFont sf;

        //This is initialized to 1600x900 by default, but stretches to fit the window size
        //I'm putting this in exclusively so I can test my code on my bad laptop
        //This is NOT production ready - buttons do not draw or check for mouse in the correct places,
        RenderTarget2D renderTarget;   //and I'm pretty sure Actors and Solids are scaled wrongly too

        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;

            keyboard = Keyboard.GetState();
            input = keyboard.GetPressedKeys();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            State = MainGameState.StartMenu;

			IsMouseVisible = false;

            isDebugging = false;

            renderTarget = new RenderTarget2D(GraphicsDevice, 1600, 900);

            debugCheck = new KeyboardManager();

			//sparkles = new List<Particle>();
			sparkleTextures = new List<Texture2D>();

            //wait for 90 frames when used
            frameBuffer = 90;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
			// Create a new SpriteBatch, which can be used to draw textures.

			//NOTE: Button image sizes are 32px x 96px which is a 1:3 ratio. I can change this if y'all don't like it.

			player = new Player(new Animation(Content.Load<Texture2D>("enemy_rock_idle-Sheet"), Content.Load<Texture2D>("enemy_rock_idle-Sheet").Height), new Animation(Content.Load<Texture2D>("enemy_rock_walk-Sheet"), Content.Load<Texture2D>("enemy_rock_walk-Sheet").Height), new Rectangle(50, 50, 50, 50), new KeyboardManager(), graphics.GraphicsDevice);
			

            spriteBatch = new SpriteBatch(GraphicsDevice);
			startButton = new Button(Content.Load<Texture2D>("start_unpressed"), Content.Load<Texture2D>("start_mouseover"), Content.Load<Texture2D>("start_pressed"), new Rectangle(GraphicsDevice.Viewport.Width/2 - 128, GraphicsDevice.Viewport.Height/2, 256, 96));
			helpButton = new Button(Content.Load<Texture2D>("help_unpressed"), Content.Load<Texture2D>("help_mouseover"), Content.Load<Texture2D>("help_pressed"), new Rectangle(GraphicsDevice.Viewport.Width/2 - 128, startButton.Y + startButton.Location.Height + 48, 256, 96));
			quitButton = new Button(Content.Load<Texture2D>("quit_unpressed"), Content.Load<Texture2D>("quit_mouseover"), Content.Load<Texture2D>("quit_pressed"), new Rectangle(GraphicsDevice.Viewport.Width/2 - 128, helpButton.Y + helpButton.Location.Height + 48, 256, 96));
            backButton = new Button(Content.Load<Texture2D>("back_unpressed"), Content.Load<Texture2D>("back_mouseover"), Content.Load<Texture2D>("back_pressed"), new Rectangle(GraphicsDevice.Viewport.Width - 384, GraphicsDevice.Viewport.Height - 256, 256, 96));
			title = new Button(Content.Load<Texture2D>("title"), Content.Load<Texture2D>("title"), Content.Load<Texture2D>("title"), new Rectangle(GraphicsDevice.Viewport.Width / 2 - 512, 128, 1024, 288));

			sf = Content.Load<SpriteFont>("sf");


			//loads rps buttons
			rockButton = new Button(Content.Load<Texture2D>("rock"), Content.Load<Texture2D>("rock_mouseover"), Content.Load<Texture2D>("rock_mouseover"), new Rectangle(48, GraphicsDevice.Viewport.Height / 2, 64, 64));
			paperButton = new Button(Content.Load<Texture2D>("paper"), Content.Load<Texture2D>("paper_mouseover"), Content.Load<Texture2D>("paper_mouseover"), new Rectangle(128, GraphicsDevice.Viewport.Height/2 - 80, 64, 64));
			scissorsButton = new Button(Content.Load<Texture2D>("scissoros"), Content.Load<Texture2D>("scissoros_mouseover"), Content.Load<Texture2D>("scissoros_mouseover"), new Rectangle(208, GraphicsDevice.Viewport.Height / 2, 64, 64));
			lizardButton = new Button(Content.Load<Texture2D>("lizard"), Content.Load<Texture2D>("lizard_mouseover"), Content.Load<Texture2D>("lizard_mouseover"), new Rectangle(192, GraphicsDevice.Viewport.Height / 2 - 64, 64, 64));
			spockButton = new Button(Content.Load<Texture2D>("spock"), Content.Load<Texture2D>("spock_mouseover"), Content.Load<Texture2D>("spock_mouseover"), new Rectangle(64, GraphicsDevice.Viewport.Height / 2 - 64, 64, 64));

			sparkleTextures.Add(Content.Load<Texture2D>("sparkle_blue"));
			sparkleTextures.Add(Content.Load<Texture2D>("sparkle_yellow"));
			sparkleTextures.Add(Content.Load<Texture2D>("rock"));
			sparkleTextures.Add(Content.Load<Texture2D>("paper"));
			sparkleTextures.Add(Content.Load<Texture2D>("scissoros"));
            //these are just drawn to display what was chosen in combat
            rockResult = Content.Load<Texture2D>("rock");
            paperResult = Content.Load<Texture2D>("paper");
            scissorsResult = Content.Load<Texture2D>("scissoros");
            lizardResult = Content.Load<Texture2D>("lizard");
            spockResult = Content.Load<Texture2D>("spock");

           
			Texture2D dogDebug = Content.Load<Texture2D>("dog");
			Texture2D platformImg = Content.Load<Texture2D>("platform");

            // Load in the level's textures.
            levelTextures = new List<Texture2D>();
			levelTextures.Add(platformImg);
            levelTextures.Add(dogDebug);
			levelTextures.Add(Content.Load<Texture2D>("giant_fist"));
			levelTextures.Add(Content.Load<Texture2D>("giant_fist_left"));
			battleTransition = new Animation(Content.Load<Texture2D>("battle_transition-Sheet"), 128);

			mouse = Content.Load<Texture2D>("mouse");
			mouseClickable = Content.Load<Texture2D>("mouse_clickable");

			//backButton = new Button(Content.Load<Texture2D>("back_unpressed"), Content.Load<Texture2D>("back_mouseover"), Content.Load<Texture2D>("back_pressed"), new Rectangle(GraphicsDevice.Viewport.Width - 192, GraphicsDevice.Viewport.Height - 128, 128, 48));
			sf = Content.Load<SpriteFont>("sf");
		}

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

			ms = Mouse.GetState();

			for (int i = 0; i < sparkles.Count; i++)
			{
				sparkles[i].Update();
				if ((sparkles[i].X < -16 || sparkles[i].X > GraphicsDevice.Viewport.Width) && (sparkles[i].Y < -16 || sparkles[i].Y > GraphicsDevice.Viewport.Height))
				{
					sparkles.RemoveAt(i);
				}
			}
			// This will handle the overall state of the game for updating the logic of the game.
			switch (State)
            {

                case MainGameState.StartMenu:

					//resets button because it was moved during the gameover screen
					quitButton.Location = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 128, helpButton.Y + helpButton.Location.Height + 48, 256, 96);

					if (startButton.IsClicked(ms))
                    {
						State = MainGameState.Game;
                        mainLevel = new Level(GraphicsDevice, this);
                        mainLevel.LoadContent(Content, GraphicsDevice);

                        // Add in way to select different levels.
                        mainLevel.LoadLevel("Content/level1.level");
                        //mainLevel.BuildLevel();
						playerHealth = mainLevel.PlayerHealth;
						player = new Player(mainLevel.User.IdleAnimation, mainLevel.User.WalkingAnimation, new Rectangle(50, 50, 50, 50), new KeyboardManager(), graphics.GraphicsDevice);
                    }
					if (helpButton.IsClicked(ms))
						State = MainGameState.HelpMenu;
					if (quitButton.IsClicked(ms))
						Exit();
					if(title.IsClicked(ms))
					{
						sparkles.AddRange(Particle.GenerateParticles(new Point(-15, 0), new Point(15, 20), new Point(0, -1), ms.Position, sparkleTextures, Color.Cyan, Color.LightCyan, 2, enemyChoiceMaker));
					}
					break;
				case MainGameState.HelpMenu:
                    if(backButton.IsClicked(ms))
					{
						State = MainGameState.StartMenu;
					}
					break;
                case MainGameState.BattleTransition:
                    //Get the variables we need - a Vector2 of the player's position, that position transformed into viewport space using the scroll matrix, and the height of the bottom half of the split
                    Vector2 playerPos = new Vector2(mainLevel.User.X, mainLevel.User.Y); 
                    int splitY = (int)Vector2.Transform(playerPos, mainLevel.scrollMatrix).Y + (mainLevel.User.Height / 2);
                    int botHeight = 900 - splitY;

                    //Generate the world rectangles (these stay in place and define which part of the world/RenderTarget to draw
                    worldTop = new Rectangle(0, 0, 1600, splitY);
                    worldBot = new Rectangle(0, splitY, 1600, botHeight);

                    //Generate the view rectangles (these ease out of frame and define where the RenderTarget is drawn to)
                    viewTop = new Rectangle(0, (int)(-Math.Sin(MathHelper.ToRadians(pos)) * splitY), 1600, splitY);
                    viewBot = new Rectangle(0, (int)(Math.Sin(MathHelper.ToRadians(pos)) * botHeight) + splitY, 1600, botHeight); 

                    //Once the animation is over, reset the positions and switch into full battle
                    if(pos == 90)
                    {
                        State = MainGameState.Battle;
                        pos = 0;
                        break;
                    }

                    pos++;
                    break;
				#region combat
				case MainGameState.Battle:

                    //reset combat vars from last combat
                    combatDone = false;
                    validInput = false;
                    input = null;
                    keyboard = Keyboard.GetState();

                    input = keyboard.GetPressedKeys();
					player.MoveState = Actor.MovementType.Idle;
					currentCombatEnemy.MoveState = Actor.MovementType.Idle;

                    switch(battleState)
                    {
						case BattleState.Transition:
							if (battleTransition.isLastFrame())
							{
								battleState = BattleState.Input;
							}
							break;
                        case BattleState.Input:

                            keyboard = Keyboard.GetState();
                            input = keyboard.GetPressedKeys();
                            playerChoice = "";

                            //make sure button is valid input for the enemy
                            if (currentCombatEnemy.PlayerOptions.Contains("Rock") || currentCombatEnemy.PlayerOptions.Contains("rock")) //rock is a valid choice
                            {
                                //rock is the players choice
                                if (rockButton.IsClicked(ms) || (input.Length == 1 && input[0].Equals(Keys.A)))  //only one key is pressed at this point
                                {
                                    validInput = true; //we found that the input was valid
                                    playerChoice = "rock"; //set the players choice
                                    battleState = BattleState.Combat; 
                                    break;             //exit the valid input loop
                                }
                            }
                            if (currentCombatEnemy.PlayerOptions.Contains("Paper") || currentCombatEnemy.PlayerOptions.Contains("paper")) //etc
                            {
                                //paper is the player's choice
                                if (paperButton.IsClicked(ms) || (input.Length == 1 && input[0].Equals(Keys.W)))
                                {
                                    //etc
                                    validInput = true;
                                    playerChoice = "paper";
                                    battleState = BattleState.Combat;
                                    break;
                                }
                            }
                            if (currentCombatEnemy.PlayerOptions.Contains("Scissors") || currentCombatEnemy.PlayerOptions.Contains("scissors"))
                            {
                                //scissors
                                if (scissorsButton.IsClicked(ms) || (input.Length == 1 && input[0].Equals(Keys.D)))
                                {
                                    validInput = true;
                                    playerChoice = "scissors";
                                    battleState = BattleState.Combat;
                                    break;
                                }
                            }
                            if (currentCombatEnemy.PlayerOptions.Contains("Spock") || currentCombatEnemy.PlayerOptions.Contains("spock"))
                            {
                                //spock
                                if (spockButton.IsClicked(ms) || (input.Length == 1 && input[0].Equals(Keys.Q)))
                                {
                                    validInput = true;
                                    playerChoice = "spock";
                                    battleState = BattleState.Combat;
                                    break;
                                }
                            }
                            if (currentCombatEnemy.PlayerOptions.Contains("Lizard") || currentCombatEnemy.PlayerOptions.Contains("lizard"))
                            {
                                //Lizard
                                if (lizardButton.IsClicked(ms) || (input.Length == 1 && input[0].Equals(Keys.E)))
                                {
                                    validInput = true;
                                    playerChoice = "lizard";
                                    battleState = BattleState.Combat;
                                    break;
                                }
                            }

                            break;
						
						case BattleState.Combat:
                            //we have our valid input, now we get the enemies.
                            enemyChoice = currentCombatEnemy.EnemyOptions[enemyChoiceMaker.Next(0, currentCombatEnemy.EnemyOptions.Count)]; //picks a string of the enemies choice

                            //check to see if the player won or if the enemy won

                            if (playerChoice.Equals("rock")) //player chose rock, check enemy, see who won
                            {
                                if (enemyChoice.Equals("Rock"))
                                {
                                    winner = "tie";
                                }
                                else if (enemyChoice.Equals("Paper"))
                                {
                                    winner = "enemy";
                                }
                                else if (enemyChoice.Equals("Scissors"))
                                {
                                    winner = "player";
                                }
                                else if (enemyChoice.Equals("Lizard"))
                                {
                                    winner = "player";
                                }
                                else if (enemyChoice.Equals("Spock"))
                                {
                                    winner = "enemy";
                                }
                            }
                            //player chose paper
                            else if (playerChoice.Equals("paper"))
                            {
                                if (enemyChoice.Equals("Rock"))
                                {
                                    winner = "player";
                                }
                                else if (enemyChoice.Equals("Paper"))
                                {
                                    winner = "tie";
                                }
                                else if (enemyChoice.Equals("Scissors"))
                                {
                                    winner = "enemy";
                                }
                                else if (enemyChoice.Equals("Lizard"))
                                {
                                    winner = "enemy";
                                }
                                else if (enemyChoice.Equals("Spock"))
                                {
                                    winner = "player";
                                }
                            }
                            //player chose scissors
                            else if (playerChoice.Equals("scissors"))
                            {
                                if (enemyChoice.Equals("Rock"))
                                {
                                    winner = "enemy";
                                }
                                else if (enemyChoice.Equals("Paper"))
                                {
                                    winner = "player";
                                }
                                else if (enemyChoice.Equals("Scissors"))
                                {
                                    winner = "tie";
                                }
                                else if (enemyChoice.Equals("Lizard"))
                                {
                                    winner = "player";
                                }
                                else if (enemyChoice.Equals("Spock"))
                                {
                                    winner = "enemy";
                                }
                            }
                            //player chose lizard
                            else if (playerChoice.Equals("lizard"))
                            {
                                if (enemyChoice.Equals("Rock"))
                                {
                                    winner = "enemy";
                                }
                                else if (enemyChoice.Equals("Paper"))
                                {
                                    winner = "player";
                                }
                                else if (enemyChoice.Equals("Scissors"))
                                {
                                    winner = "enemy";
                                }
                                else if (enemyChoice.Equals("Lizard"))
                                {
                                    winner = "tie";
                                }
                                else if (enemyChoice.Equals("Spock"))
                                {
                                    winner = "player";
                                }
                            }
                            //player chose spock
                            else if (playerChoice.Equals("spock"))
                            {
                                if (enemyChoice.Equals("Rock"))
                                {
                                    winner = "player";
                                }
                                else if (enemyChoice.Equals("Paper"))
                                {
                                    winner = "enemy";
                                }
                                else if (enemyChoice.Equals("Scissors"))
                                {
                                    winner = "player";
                                }
                                else if (enemyChoice.Equals("Lizard"))
                                {
                                    winner = "enemy";
                                }
                                else if (enemyChoice.Equals("Spock"))
                                {
                                    winner = "tie";
                                }
                            }

                            battleState = BattleState.Show;

                            break;
                            //This is the phase we hold for about a second so that we can show the result of combat
                            //not necessarily who won, but who played what
                        case BattleState.Show:

                            //update for this state handles how long to hold drawing the results
                            frameBuffer--;

                            //after the amount of frames designated, switch past this phase
                            if(frameBuffer == 0)
                            {
                                battleState = BattleState.Conclusion;
                                frameBuffer = 90;
                            }

                            break;
                            //where we finalize and end combat to where it should be
                        case BattleState.Conclusion:

                            if (winner == "tie")
                            {
                                //combat tied, and must begin once more
                                battleState = BattleState.Transition;
                                winner = "";
                            }
                            else if (winner == "player")
                            {
                                //the player has won, so we disble the enemy and move back to normal gameplay
                                State = MainGameState.Game;
                                battleState = BattleState.Transition;

                                //this is where we would kill off the enemy
                                currentCombatEnemy.Dead = true;

                                winner = "";
                                //end combat
                                combatDone = true;
                            }
                            else if (winner == "enemy")
                            {
                                //the enemy has won, deplete player health.
                                playerHealth--;

                                //check to see if the player died, ending combat and the game
                                if (playerHealth < 1)
                                {
                                    combatDone = true;

                                    State = MainGameState.GameOver;
                                    battleState = BattleState.Transition;

                                }
                                //if they didnt die, combat is still going
                                winner = "";
                            }
                            break;
                    }   
                    break;
				#endregion combat
				case MainGameState.Game:
                    // If the J key is pressed, then the game will display the debugging tools like quad tree and collision boxes.
                    
                    if (debugCheck.GetKeyState(Keys.J) == KeyState.FirstTap)
                    {
                        isDebugging = !isDebugging;
                    }
                    mainLevel.Update();

                    //Keep these failsafes ready for if a transition begins with no valid world/view rectangles
                    worldTop = new Rectangle(0, 0, 1600, 900);
                    worldBot = new Rectangle(0, 0, 0, 0);
                    viewTop = new Rectangle(0, 0, 1600, 900);
                    viewBot = new Rectangle(0, 0, 0, 0);
                    break;
                case MainGameState.GameOver:
                    if (Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        State = MainGameState.StartMenu;
                    }
					if(quitButton.IsClicked(ms))
					{
						Exit();
					}
					if(backButton.IsClicked(ms))
					{
						//switches back to the actual game
						State = MainGameState.Game;

						//reloads the current level
						mainLevel.LoadContent(Content, GraphicsDevice);
						mainLevel.LoadLevel(mainLevel.LevelPath);
						playerHealth = mainLevel.PlayerHealth;
						battleTransition.Frame = 0;
					}

					break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if(State == MainGameState.BattleTransition)
            {
                GraphicsDevice.SetRenderTarget(renderTarget);
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
			// This will handle the overall state of the game for updating the images on the screen.

			
			switch (State)
			{
				case MainGameState.StartMenu:
					GraphicsDevice.Clear(Color.Beige);

					title.Draw(spriteBatch, ms);
					startButton.Draw(spriteBatch, ms);
					helpButton.Draw(spriteBatch, ms);
					quitButton.Draw(spriteBatch, ms);
					for (int i = 0; i < sparkles.Count; i++)
					{
						sparkles[i].Draw(spriteBatch);
					}
					break;
				case MainGameState.HelpMenu:
					GraphicsDevice.Clear(Color.Beige);

					spriteBatch.DrawString(sf, "Help menu", new Vector2(16, 16), Color.Black);
					spriteBatch.DrawString(sf, "Controls:", new Vector2(32, 64), Color.Black);
					//NOTE: controls can be changed (obviously), these are just placeholders (unless they aren't) for now
					spriteBatch.DrawString(sf, "Movement:", new Vector2(48, 96), Color.Black);
					spriteBatch.DrawString(sf, "Space - Jump\nA - Left\nD - Right", new Vector2(64, 128), Color.Black);
					spriteBatch.DrawString(sf, "During combat: ", new Vector2(48, 224), Color.Black);
					spriteBatch.DrawString(sf, "W - Paper\nA - Rock\nD - Scissors\nQ - Spock\nE - Lizard", new Vector2(64, 256), Color.Black);
					backButton.Draw(spriteBatch, ms);
					break;
                case MainGameState.BattleTransition:
                    GraphicsDevice.Clear(Color.LightGreen);

                    mainLevel.Draw(spriteBatch);

                    GraphicsDevice.SetRenderTarget(null);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

                    battleTransition.DrawSingleFrame(spriteBatch, GraphicsDevice.Viewport.Bounds, 0, SpriteEffects.None, GraphicsDevice.Viewport.Width / battleTransition.FrameWidth + 1);

                    spriteBatch.Draw(renderTarget, viewTop, worldTop, Color.White);
                    spriteBatch.Draw(renderTarget, viewBot, worldBot, Color.White);

                    //spriteBatch.DrawString(sf, viewTop.ToString(), new Vector2(10, 10), Color.White);


                    break;
				case MainGameState.Battle:
					GraphicsDevice.Clear(new Color(50, 50, 50));


					//puts player at left edge of screen
					player.X = 128;
					player.Y = GraphicsDevice.Viewport.Height / 2;

					//puts enemy at right edge of screen
					currentCombatEnemy.X = GraphicsDevice.Viewport.Width - currentCombatEnemy.Width - 128;
					currentCombatEnemy.Y = player.Y;//GraphicsDevice.Viewport.Height / 2;

					//draws platforms
					spriteBatch.Draw(levelTextures[3], new Rectangle(0, GraphicsDevice.Viewport.Height/2 + player.Height, 248, 251), Color.White);
					spriteBatch.Draw(levelTextures[2], new Rectangle(GraphicsDevice.Viewport.Width - 248, GraphicsDevice.Viewport.Height / 2 + player.Height, 248, 251), Color.White);

					List<string> choices = currentCombatEnemy.PlayerOptions;
					//draws options
					if(choices.Contains("Rock"))
						rockButton.Draw(spriteBatch, ms);
					if (choices.Contains("Paper"))
						paperButton.Draw(spriteBatch, ms);
					if (choices.Contains("Scissors"))
						scissorsButton.Draw(spriteBatch, ms);
					if (choices.Contains("Lizard"))
						lizardButton.Draw(spriteBatch, ms);
					if (choices.Contains("Spock"))
						spockButton.Draw(spriteBatch, ms);


					//draws player and enemy
					player.Draw(spriteBatch, Color.White, isDebugging);
					currentCombatEnemy.Draw(spriteBatch, Color.White, isDebugging);

					//draws the curtain if it is during the transition
					if (battleState == BattleState.Transition)
					{
						battleTransition.Draw(spriteBatch, GraphicsDevice.Viewport.Bounds, SpriteEffects.None, GraphicsDevice.Viewport.Width / battleTransition.FrameWidth + 1);
					}
                    else if(battleState == BattleState.Show)
                    {
                        //draw the player choice near them
                        if (playerChoice == "rock")
                        {
                            spriteBatch.Draw(rockResult, new Rectangle(player.X + 256, player.Y, 64, 64), Color.White);
                        }
                        if (playerChoice == "paper")
                        {
                            spriteBatch.Draw(paperResult, new Rectangle(player.X + 256, player.Y, 64, 64), Color.White);
                        }
                        if (playerChoice == "scissors")
                        {
                            spriteBatch.Draw(scissorsResult, new Rectangle(player.X + 256, player.Y, 64, 64), Color.White);
                        }
                        if (playerChoice == "lizard")
                        {
                            spriteBatch.Draw(lizardResult, new Rectangle(player.X + 256, player.Y, 64, 64), Color.White);
                        }
                        if (playerChoice == "spock")
                        {
                            spriteBatch.Draw(spockResult, new Rectangle(player.X + 256, player.Y, 64, 64), Color.White);
                        }

                        //draw the enemy choice near them

                        if (enemyChoice == "Rock")
                        {
                            spriteBatch.Draw(rockResult, new Rectangle(currentCombatEnemy.X - 256, player.Y, 64, 64), Color.White);
                        }   
                        if (enemyChoice == "Paper")
                        {
                            spriteBatch.Draw(paperResult, new Rectangle(currentCombatEnemy.X - 256, player.Y, 64, 64), Color.White);
                        }    
                        if (enemyChoice == "Scissors")
                        {
                            spriteBatch.Draw(scissorsResult, new Rectangle(currentCombatEnemy.X - 256, player.Y, 64, 64), Color.White);
                        }   
                        if (enemyChoice == "Lizard")
                        {
                            spriteBatch.Draw(lizardResult, new Rectangle(currentCombatEnemy.X - 256, player.Y, 64, 64), Color.White);
                        }   
                        if (enemyChoice == "Spock")
                        {
                            spriteBatch.Draw(spockResult, new Rectangle(currentCombatEnemy.X - 256, player.Y, 64, 64), Color.White);
                        }
                    }

					break;
                case MainGameState.Game:
                    GraphicsDevice.Clear(Color.LightGreen);

                    mainLevel.Draw(spriteBatch);
                    break;
				case MainGameState.GameOver:
					GraphicsDevice.Clear(Color.DarkRed);

					//draws the back button in the correct place
					//TODO adjust y pos
					backButton.X = startButton.X;
					backButton.Y = startButton.Y;
					backButton.Draw(spriteBatch, ms);

					//draws the quit button in the middle of the screen
					quitButton.X = backButton.X;
					quitButton.Y = backButton.Y + 128;
					quitButton.Draw(spriteBatch, ms);

					//draws gameover strin
					spriteBatch.DrawString(sf, "Game over!", new Vector2(backButton.X + 60, backButton.Y - 96), Color.Black);

					break;


			}

			spriteBatch.Draw(ms.LeftButton == ButtonState.Pressed ? mouseClickable : mouse, new Rectangle(ms.Position, new Point(32, 32)), Color.White); //set the second point in the rectangle to ms.position as well for fun times hehehe

			base.Draw(gameTime);
			spriteBatch.End();

        }
    }
}
