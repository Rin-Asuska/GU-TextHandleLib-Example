/* Author: Kelvin Lee
 * 
 * Example use of the TextHandle Library

 * How to include in your project:
    1: Download the project soulution "KL_TextHandlingLib". (Should be ALREADY DOWNLOADED along with THIS)
    2: Right click on your project SOLUTION, in this cause its "Soulution 'KL_TextHandlingExample' (3 Projects)" in the Solution Explorer
       then select "Add > Existing Project" and navigate to "KL_TextHandlingLib.csproj" form the soulution you've downloaded.
    3: Right click your project, in this case its "KL_TextHandlingExample"
       then select "Add > Refernce..." then TICK "KL_TextHandlingLib" under "Projects"
    4: Now add "using KL_TextHandlingLib" (It will now show up as suggested fixes if you use "TextHandle".
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KL_TextHandlingLib; //This will include the library

namespace KL_TextHandlingExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Class Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MouseState mouseState;
        MouseState mouseStateOld;

        //TextHandle Class Example Variables
        SpriteFont sampleFont;
        DragableObject sampleRectangle;

        KL_TextHandle myTextStatic1, myTextAnchored1, myTextAnchoredToRectangle1; //This is an example setting up everything from class constructor.

        KL_TextHandle myTextStatic2, myTextAnchored2, myTextDragMe; //This is examples of setting up using other methods.

        KL_TextHandle myNameTag; //Example of a Nametag?

        //End class Variables

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            this.IsMouseVisible = true;
            mouseState = Mouse.GetState();
            mouseStateOld = Mouse.GetState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            sampleFont = Content.Load<SpriteFont>("SpriteFont1");
            sampleRectangle = new DragableObject(300, 200, Color.Aquamarine, Content.Load<Texture2D>("Untitled-2"));

            myTextStatic1 = new KL_TextHandle(100, 50, Color.Bisque, "myTextStatic1", sampleFont, GraphicsDevice);
            myTextAnchored1 = new KL_TextHandle(KL_TextHandle.Anchor.Bottom, 0, 10, Color.Crimson, "myTextAnchored1", sampleFont, GraphicsDevice);
            myTextAnchoredToRectangle1 = new KL_TextHandle(KL_TextHandle.Anchor.Right, sampleRectangle.rect, -5, 5, Color.DarkOrange, "myAnchoredTextR1", sampleFont, GraphicsDevice);

            myTextStatic2 = new KL_TextHandle("StaticText2", sampleFont, GraphicsDevice);
            //Lets modify myTextStatic2
            myTextStatic2.setPosition(350, 100);
            myTextStatic2.txtColor = Color.Sienna;

            myTextAnchored2 = new KL_TextHandle(100, 100, Color.Red, "Anchored2", sampleFont, GraphicsDevice);
            //Lets anchor this to the bottom left shall we?
            myTextAnchored2.setPosition(KL_TextHandle.Anchor.BottomLeft, 0, 0);

            myTextDragMe = new KL_TextHandle("DRAG ME", sampleFont, GraphicsDevice);
            //Lets attach this to the Top left of the sampleRectangle, CHECK THE UPDATE method for the state toggle.
            myTextDragMe.setPosition(sampleRectangle.rect, KL_TextHandle.Anchor.TopLeft, 5, 5);
            myTextDragMe.txtColor = Color.Black;

            myNameTag = new KL_TextHandle("MY NAMETAG", sampleFont, GraphicsDevice); //SEE UPDATE METHOD
            myNameTag.set_Offset(0, -30); //Puts nametag ABOVE the box
            myNameTag.txtColor = Color.Red;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            //Make the dragablebox, well, dragable. This is to demonstrate the TextHandle Dynamic attach.
            mouseState = Mouse.GetState();
            sampleRectangle.updateObject(mouseState, mouseStateOld);

            //MAKE DRAGME text visible or hidden:
            if (sampleRectangle.isDragged)
            {
                myTextDragMe.isHidden = true;
            }
            else
            {
                myTextDragMe.isHidden = false;
            }


            //Update Text to be on the rectangle specified.
            myTextAnchoredToRectangle1.updateObject(sampleRectangle.rect);
            myTextDragMe.updateObject(sampleRectangle.rect);

            //Update Text to be at a namepag like position
            myNameTag.updateObject(sampleRectangle.pos);


            base.Update(gameTime);
            mouseStateOld = mouseState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            //Draw EVERYTHING

            //Draw The sampleBox
            sampleRectangle.DrawObject(spriteBatch);

            //Draw Text
            myTextStatic1.DrawObject(spriteBatch);
            myTextAnchored1.DrawObject(spriteBatch);
            myTextAnchoredToRectangle1.DrawObject(spriteBatch);

            myTextStatic2.DrawObject(spriteBatch);
            myTextAnchored2.DrawObject(spriteBatch);
            myTextDragMe.DrawObject(spriteBatch);

            myNameTag.DrawObject(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
