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
using mazeGame.GameElements.Base_classes;
using mazeGame.GameElements.Derived_classes.Maze_stuff;

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
        public Texture2D black1x1;
        public Texture2D purple1x1;
        public Texture2D red1x1;
        public Texture2D goal_marker;
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
        //...post-game...
        List<PostGameElement> postGameElements;
        bool isPostGameSetUp = false;
        PostGame postGame;

        //more bools!
        internal bool showBreadcrumbTrail = false;
        internal bool showShortestPath = false;
        internal bool showHint = false;

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
            gameState = GameStates.Menu;    //Menu is the initial state, apparently

            menuElements = new();           //menu
            menu = new();
            highScoresElements = new();     //high schore
            highScores = new();
            creditsElements = new();        //credits
            credits = new();
            mazeElements = new();           //maze
            maze = null;    //<---NOTE: different
            postGameElements = new();       //post-game
            postGame = new();
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
            black1x1 = Content.Load<Texture2D>("black1x1");
            purple1x1 = Content.Load<Texture2D>("purple1x1");
            red1x1 = Content.Load<Texture2D>("red1x1");
            goal_marker = Content.Load<Texture2D>("goal-marker");
            //cell_sprite = Content.Load<Texture2D>("cell-placeholder"); ;

            // ORIGINAL MESSAGE: use this.Content to load your game content here
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
                    //render the high scores screen
                    for (int i = 0; i < highScoresElements.Count; i++)
                    {
                        HighScoresElement el = highScoresElements[i];
                        spriteBatch.DrawString(font, el.text, el.coords, el.color);
                    }
                    break;
                case (GameStates.Credits):
                    //render the credits
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
                    // MAYBE: have multiple background sprites and randomly choose one
                    spriteBatch.Draw(background, new Rectangle(0, 0, windowSize.X, windowSize.Y), Color.White);

                    //...then on to the lists
                     for (int i = 0; i < mazeElements[MazeElement.ElementType.Wall].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Wall][i];
                        if(el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.ShortestPath].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.ShortestPath][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.BreadcrumbTrail].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.BreadcrumbTrail][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Hint].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Hint][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Goal].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Goal][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Player].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Player][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }

                    //TODO Also, render the score, the timer, the keypress legend...and w/e else


                    break;
                case GameStates.PostGame:
                    //First, render the maze
                    spriteBatch.Draw(background, new Rectangle(0, 0, windowSize.X, windowSize.Y), Color.White);
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Wall].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Wall][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.BreadcrumbTrail].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.BreadcrumbTrail][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.ShortestPath].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.ShortestPath][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Hint].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Hint][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Goal].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Goal][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }
                    for (int i = 0; i < mazeElements[MazeElement.ElementType.Player].Count; i++)
                    {
                        MazeElement el = mazeElements[MazeElement.ElementType.Player][i];
                        if (el.callType == CallType.Vector2)
                            spriteBatch.Draw(el.texture, el.coords, el.color);
                        else //el.callType == Rectangle
                            spriteBatch.Draw(el.texture, el.rect, el.color);
                    }

                    //...then render the post-game stuff over it
                    for (int i = 0; i < postGameElements.Count; i++)
                    {
                        PostGameElement el = postGameElements[i];
                        if(el.renderType == RenderType.Text)
                            spriteBatch.DrawString(font, el.text, el.coords, el.color);
                        else //el.renderType == UI
                        {
                            if (el.callType == CallType.Vector2)
                                spriteBatch.Draw(el.texture, el.coords, el.color);
                            else //el.callType == Rectangle
                                spriteBatch.Draw(el.texture, el.rect, el.color);
                        }
                    }
                    break;

            }//END switch(gameState)

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
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.F1))             //TODO: Update all(/most) KeyDowns to the (prev==down, cur==up) type
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
                    if (prevState.IsKeyUp(Keys.Escape) && currentState.IsKeyDown(Keys.Escape))
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
                    if (prevState.IsKeyUp(Keys.Escape) && currentState.IsKeyDown(Keys.Escape))
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

                    if (maze.player.IsAtGoal())  //check to see if we're done
                        gameState = GameStates.PostGame;

                    //...then handle keypresses
                    if (prevState.IsKeyUp(Keys.Up) && currentState.IsKeyDown(Keys.Up))                          //TODO: wasd, ijkl
                    {
                        //up!
                        (int row, int col) = maze.player.GetPosition();
                        if (Player.IsMoveAllowed(maze.mazeStorage, maze.player, row - 1, col))
                        {
                            maze.player.SetPosition(row - 1, col);
                            maze.player.SetCellAsVisited(row - 1, col);
                            showHint = false;
                        }                 
                            
                     }
                    else if (prevState.IsKeyUp(Keys.Down) && currentState.IsKeyDown(Keys.Down))
                    {
                        //down!
                        (int row, int col) = maze.player.GetPosition();
                        if (Player.IsMoveAllowed(maze.mazeStorage, maze.player, row + 1, col))
                        {
                            maze.player.SetPosition(row + 1, col);
                            maze.player.SetCellAsVisited(row + 1, col);
                            showHint = false;
                        }
                    }
                    else if (prevState.IsKeyUp(Keys.Left) && currentState.IsKeyDown(Keys.Left))
                    {
                        //left!
                         (int row, int col) = maze.player.GetPosition();
                        if (Player.IsMoveAllowed(maze.mazeStorage, maze.player, row, col - 1))
                        {
                            maze.player.SetPosition(row, col - 1);
                            maze.player.SetCellAsVisited(row, col - 1);
                            showHint = false;
                        }                            
                    }
                    else if (prevState.IsKeyUp(Keys.Right) && currentState.IsKeyDown(Keys.Right))
                    {
                        //right!
                        (int row, int col) = maze.player.GetPosition();
                        if (Player.IsMoveAllowed(maze.mazeStorage, maze.player, row, col + 1))
                        {
                            maze.player.SetPosition(row, col + 1);
                            maze.player.SetCellAsVisited(row, col + 1);
                            showHint = false;
                        }
                    }
                    else if (prevState.IsKeyUp(Keys.B) && currentState.IsKeyDown(Keys.B))
                    {
                        if (!showBreadcrumbTrail)
                            showBreadcrumbTrail = true;
                        else
                            showBreadcrumbTrail = false;
                    }
                    else if (prevState.IsKeyUp(Keys.P) && currentState.IsKeyDown(Keys.P))
                    {
                        if (!showShortestPath)
                            showShortestPath = true;
                        else
                            showShortestPath = false;
                    }
                    else if (prevState.IsKeyUp(Keys.H) && currentState.IsKeyDown(Keys.H))
                    {
                        if (!showHint)
                            showHint = true;
                        else
                            showHint = false;
                    }
                    else if (prevState.IsKeyUp(Keys.Escape) && currentState.IsKeyDown(Keys.Escape))
                    {
                        //exit to menu
                        gameState = GameStates.Menu;
                        isMenuSetUp = false;
                        mazeElements.Clear();
                        isMazeSetUp = false;
                        showBreadcrumbTrail= false;
                        showHint = false;
                        showShortestPath = false;
                    }
                    break;
                case GameStates.PostGame:
                    // Write some 'congrats' text, wait for ESC to be pressed, return to menu
                    EmptyMazeElementsLists();
                    maze.MakeMaze(mazeSize, mazeElements, this);
                    if (!isPostGameSetUp)
                    {
                        postGame.SetUpPostGame(postGameElements, this);
                        isPostGameSetUp = true;
                    }

                    if (prevState.IsKeyUp(Keys.Escape) && currentState.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.Menu;
                        isMenuSetUp = false;
                        mazeElements.Clear();
                        isMazeSetUp = false;
                        showBreadcrumbTrail = false;
                        showHint = false;
                        showShortestPath = false;
                    }

                    break;

            }//END switch (gameState)

            prevState = currentState;   //keyboard state, btw

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