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
        public Texture2D background;
        public Texture2D player_sprite;
        public Texture2D yellow1x1;
        //Texture2D cell_sprite;
        public Point windowSize = new(1200, 1200);     //NOTE: Setting the window size to 1200x1200
        private KeyboardState prevState;

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
        Dictionary<MazeElement.ElementType, List<MazeElement>> mazeElements;
        bool isMazeSetUp = false;
        Maze maze;

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

            //Change the resolution
            _graphics.PreferredBackBufferWidth = windowSize.X; //1200;
            _graphics.PreferredBackBufferHeight = windowSize.Y; // 1200;
            _graphics.ApplyChanges();

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

            //Now let's load us some sprites
            background = Content.Load<Texture2D>("galaxy-background");
            player_sprite = Content.Load<Texture2D>("player");
            yellow1x1 = Content.Load<Texture2D>("yellow1x1");
            //cell_sprite = Content.Load<Texture2D>("cell-placeholder"); ;

            // TODO: use this.Content to load your game content here
        }

        //Monogame: Update
        protected override void Update(GameTime gameTime)
        {
            ProcessInput(gameTime);

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
                    //  TODO, MAYBE: have multiple background sprites and randomly choose one
                    spriteBatch.Draw(background, new Rectangle(0, 0, windowSize.X, windowSize.Y), Color.White);

                    //...then on to the lists
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Cell].Count; i++)
                    {
                        //skipping this; cells will simply be transparent
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Wall].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Wall][i];
                        if(el.callType == MazeElement.CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.BreadcrumbTrail].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.BreadcrumbTrail][i];
                        if (el.callType == MazeElement.CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.ShortestPath].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.ShortestPath][i];
                        if (el.callType == MazeElement.CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }                   
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Hint].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Hint][i];
                        if (el.callType == MazeElement.CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Goal].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Goal][i];
                        if (el.callType == MazeElement.CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Player].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Player][i];
                        if (el.callType == MazeElement.CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
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
        
        }//END Draw()

        private void ProcessInput(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            switch (gameState)
            {
                case (GameStates.Menu):
                    if (!isMenuSetUp)
                    {
                        menu.SetupMenu(menuElements);
                        isMenuSetUp = true;
                    }   //A thought: Could do this with another state (eg. MenuSetup)
                    if (Keyboard.GetState().IsKeyDown(Keys.F1))                     //TODO: Update all KeyDowns to the (prev==down, cur==up) type
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
                    else if(prevState.IsKeyUp(Keys.Escape) && currentState.IsKeyDown(Keys.Escape))
                    {
                        Exit();
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
                        maze = new(mazeSize, mazeElements);
                        maze.SetupMaze(mazeSize, mazeElements, this);
                        isMazeSetUp = true;                                       
                        gameState = GameStates.PlayingGame;
                    }
                    break;
                case GameStates.PlayingGame:
                    //Empty out the maze dict's lists then re-make the current maze
                    EmptyMazeElementsLists();
                    maze.MakeMaze(mazeSize, mazeElements, this);

                    if (maze.player.IsAtGoal())     //check to see if we're done
                        gameState = GameStates.PostGame;

                    //...then handle keypresses
                    if (prevState.IsKeyUp(Keys.Up) && currentState.IsKeyDown(Keys.Up))              //TODO: wasd, ijkl
                    {
                        //up!
                        //TODO in a little bit: check for walls & out-of-bound conditions
                        //...but for the moment:
                        (int row, int col) = maze.player.GetPosition();
                        maze.player.SetPosition(row - 1, col);

                    }
                    else if (prevState.IsKeyUp(Keys.Down) && currentState.IsKeyDown(Keys.Down))
                    {
                        //down!
                        //TODO in a little bit: (see above)
 
                        (int row, int col) = maze.player.GetPosition();
                        maze.player.SetPosition(row + 1, col);
                    }
                    else if (prevState.IsKeyUp(Keys.Left) && currentState.IsKeyDown(Keys.Left))
                    {
                        //left!
                        //TODO in a little bit: (see above)

                        (int row, int col) = maze.player.GetPosition();
                        maze.player.SetPosition(row, col - 1);
                    }
                    else if (prevState.IsKeyUp(Keys.Right) && currentState.IsKeyDown(Keys.Right))
                    {
                        //right!
                        //TODO in a little bit: (see above)

                        (int row, int col) = maze.player.GetPosition();
                        maze.player.SetPosition(row, col + 1);
                    }
                    else if (prevState.IsKeyUp(Keys.Escape) && currentState.IsKeyDown(Keys.Escape))
                    {
                        //exit to menu
                        gameState = GameStates.Menu;
                        isMenuSetUp = false;
                        mazeElements.Clear();
                        isMazeSetUp = false;
                    }
                    break;
                case GameStates.PostGame:
                    //TODO: Figure out what we're going to do here
                    // eg. Write some 'congrats' text, wait for ESC to be pressed, return to menu

                     //TMP-to-perm
                    //if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    if(prevState.IsKeyUp(Keys.Escape) && currentState.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.Menu;
                        isMenuSetUp = false;
                        mazeElements.Clear();
                        isMazeSetUp = false;
                    }
                    //END TMP
                    break;

            }//END switch (gameState)

            prevState = currentState;   //set prevState to the current keyboard state

        }//END ProcessInput()

        internal void EmptyMazeElementsLists()
        {
            mazeElements[MazeElement.ElementType.Cell].Clear();
            mazeElements[MazeElement.ElementType.Wall].Clear();
            mazeElements[MazeElement.ElementType.Player].Clear();
            mazeElements[MazeElement.ElementType.ShortestPath].Clear();
            mazeElements[MazeElement.ElementType.BreadcrumbTrail].Clear();
            mazeElements[MazeElement.ElementType.Hint].Clear();
            mazeElements[MazeElement.ElementType.Goal].Clear();
        }

    }//END class MazeGame

}//END namespace