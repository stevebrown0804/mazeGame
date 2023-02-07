using mazeGenerator;
using mazeGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using mazeGame.Menu;
using mazeGame.GameElements;
using mazeGame.Setup.Maze;
using mazeGame.GameElements.Derived_classes;

namespace maze
{
    public class MazeGame : Game
    {
        //Members
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        int mazeSize = 0;  //single coord because we're assuming the maze size will be square. *thumbs up*
        private SpriteFont font;
        Texture2D background;

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
        //...maze...
        Dictionary<MazeElement.ElementType, List<MazeElement>> mazeElements;        //<--notice: different!
        bool isMazeSetUp = false;
        Maze maze;

        //stuff from MazeGenerator
        /*IMazeStorage mazeStorage;
        IMazeCreation mazeCreator;
        Player player;
        IMazeSolver solver;*/
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

            menuElements = new();           //menu
            menu = new();
            highScoresElements = new();     //high schore
            highScores = new();
            creditsElements = new();        //credits
            credits = new();
            mazeElements = new();           //maze
            maze = null; 
        }

        //Monogame: Initialize
        protected override void Initialize()
        {
            base.Initialize();
        }

        //Monogame: LoadContent
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Set up the font for the menu
            font = Content.Load<SpriteFont>("arial");

            background = Content.Load<Texture2D>("galaxy-background");

            // TODO: use this.Content to load your game content here
        }

        //Monogame: Update
        protected override void Update(GameTime gameTime)
        {
            switch (gameState) {
                case (GameStates.Menu):
                    if (!isMenuSetUp)
                    {
                        menu.SetupMenu(menuElements);
                        isMenuSetUp = true;
                    }   //A thought: Could do this with another state (eg. MenuSetup)
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
                    }

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
                    }
                    break;
                case GameStates.InitializingGame:
                    if (!isMazeSetUp)
                    {
                        maze = new(mazeSize, mazeElements);   //mazeSize is non-zero at this point, right?  tbd
                        maze.SetupMaze(mazeSize, mazeElements);
                        isMazeSetUp = true;
                        gameState = GameStates.PlayingGame;
                    }
                    break;
                case GameStates.PlayingGame:
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))     //TODO: wasd, ijkl
                    {
                        //TODO: up!

                        if (maze.player.IsAtGoal())
                            gameState = GameStates.PostGame;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        //TODO: down!

                        if (maze.player.IsAtGoal())
                            gameState = GameStates.PostGame;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        //TODO: left!

                        if (maze.player.IsAtGoal())
                            gameState = GameStates.PostGame;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        //TODO: right!

                        if (maze.player.IsAtGoal())
                            gameState = GameStates.PostGame;
                    }
                    break;
                case GameStates.PostGame:
                    //TODO: Figure out what we're going to do here
                    // eg. Write some 'congrats' text, wait for ESC to be pressed, return to menu

                    //TMP-to-perm
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.Menu;
                        isMenuSetUp = false;
                    }
                    //END TMP
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
                    //..starting with the background:
                    spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);  //default window size? TBD
                    //...then on to the lists
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Cell].Count; i++)
                    {
                        //TODO
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Wall].Count; i++)
                    {
                        //TODO
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.BreadcrumbTrail].Count; i++)
                    {
                        //TODO
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.ShortestPath].Count; i++)
                    {
                        //TODO
                    }                   
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Hint].Count; i++)
                    {
                        //TODO
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Player].Count; i++)
                    {
                        //TODO
                    }

                    //Also, render the score, the timer, the keypresses...and w/e else
                    // TODO

                    break;
                case GameStates.PostGame:
                    // ummmm...TBD
                    //TODO
                    break;
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}