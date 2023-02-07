using mazeGenerator;
using mazeGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using mazeGame.Menu;
using mazeGame.GameElements;

namespace maze
{
    public class MazeGame : Game
    {
        //Members
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        int mazeSize = 1;  //single coord because we're assuming the maze size will be square *thumbs up*
        private SpriteFont font;
        //List<GameElement> elements; //this one may jsut become a base class
        //Menu...
        List<MenuElement> menuElements;
        bool isMenuSetUp = false;
        Menu menu;
        //...high scores...
        List<HighScoresElement> highScoresElements;       
        bool isHighScoresSetUp = false;
        HighScores highScores;
        //...credits...
        List<CreditsElement> creditsElements;
        bool isCreditsSetUp = false;
        Credits credits;


        //stuff from MazeGenerator
        IMazeStorage maze;
        IMazeCreation mazeCreator;
        Player player;
        IMazeSolver solver;
        //IMazeRenderer renderTarget;

        enum GameStates
        {
            Menu,
            HighScores,
            Credits,
            InitializingGame,
            PlayingGame,
            PostGame
        }
        GameStates gameState;

        //Constructor
        public MazeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Initialize stuff I declared
            gameState = GameStates.Menu;    //Let's start here!
            //elements = new();
            menuElements = new();           //menu
            menu = new();
            highScoresElements = new();     //high schore
            highScores = new();
            creditsElements = new();        //credits
            credits = new();
            //IMazeStorage maze;
            //IMazeCreation mazeCreator;
            //Player player;
            //IMazeSolver solver;
            //IMazeRenderer renderTarget;
        }

        //Monogame: Initialize
        protected override void Initialize()
        {
            //First we'll create the starting point for the maze
            maze = new MazeStorage_Dictionary(mazeSize, mazeSize);
                //Actually, don't generate until we're done with the menu

            //Next up, we'll create a maze using a specific routine            
            //IMazeCreation mazeCreator = new DepthFirst_Iterative();
            mazeCreator = new Prims();
            maze = maze.CreateMaze(mazeCreator);

            //We'll create a player object
            player = new(maze);

            //And a maze-solver object
            solver = new MazeSolver();
            solver = solver.Solve(maze, player);

            //..and then render it
            //IMazeRenderer renderTarget = new RenderDictionaryToConsole();
            //IMazeRenderer renderTarget = new RenderDictionaryToFile();
            //maze.Render(renderTarget, solver);
            //maze.Render(renderTarget);


            base.Initialize();
        }

        //Monogame: LoadContent
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Set up the font for the menu
            font = Content.Load<SpriteFont>("arial");

            // TODO: use this.Content to load your game content here
        }

        //Monogame: Update
        protected override void Update(GameTime gameTime)
        {
            switch (gameState) {
                case (GameStates.Menu):
                    //set up the menu
                    if (!isMenuSetUp)
                    {
                        menu.SetupMenu(menuElements);
                        isMenuSetUp = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.F1))
                    {
                        gameState = GameStates.InitializingGame;
                        mazeSize = 5;
                        menuElements.Clear();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.F2))
                    {
                        gameState = GameStates.InitializingGame;
                        mazeSize = 10;
                        menuElements.Clear();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.F3))
                    {
                        gameState = GameStates.InitializingGame;
                        mazeSize = 15;
                        menuElements.Clear();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.F4))
                    {
                        gameState = GameStates.InitializingGame;
                        mazeSize = 20;
                        menuElements.Clear();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.F5))
                    {
                        gameState = GameStates.HighScores;
                        isHighScoresSetUp = false;
                        menuElements.Clear();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.F6))
                    {
                        gameState = GameStates.Credits;
                        menuElements.Clear();
                    }/*else if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                               || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();*/

                    break;
                case GameStates.HighScores:
                    if (!isHighScoresSetUp)
                    {
                        highScores.SetupHighScores(highScoresElements);
                        isHighScoresSetUp = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.Menu;
                        isMenuSetUp = false;
                        highScoresElements.Clear();
                    }
                    break;
                case GameStates.Credits:
                    if (!isCreditsSetUp)
                    {
                        credits.SetupCredits(creditsElements);
                        isCreditsSetUp = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.Menu;
                        isMenuSetUp = false;
                        //creditsElements.Clear(); //only needs to be set up one time
                    }
                    break;
                case GameStates.InitializingGame:
                    break;
                case GameStates.PlayingGame:
                    break;
                case GameStates.PostGame:
                    break;
            }

            base.Update(gameTime);
        }

        //Monogame: Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            switch (gameState)
            {
                case (GameStates.Menu):
                    //render the menu
                    for(int i = 0; i < menuElements.Count; i++)
                    {
                        MenuElement el = menuElements[i];
                        spriteBatch.DrawString(font, el.text, el.coords, el.color);
                    }
                    break;
                case (GameStates.HighScores):
                    for (int i = 0; i < highScoresElements.Count; i++)
                    {
                        HighScoresElement el = highScoresElements[i];
                        spriteBatch.DrawString(font, el.text, el.coords, el.color);
                    }
                    break;
                case (GameStates.Credits):
                    for (int i = 0; i < creditsElements.Count; i++)
                    {
                        CreditsElement el = creditsElements[i];
                        spriteBatch.DrawString(font, el.text, el.coords, el.color);
                    }
                    break;
                case GameStates.InitializingGame:
                    // hourglass icon, maybe?  *shrug*
                    break;
                case GameStates.PlayingGame:
                    //Render the maze
                    break;
                case GameStates.PostGame:
                    // ummmm...TBD.  menu, but over the last maze, maybe?
                    break;
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}