/*
 * Author: Kelvin Lee
 *
 * 2016 03 15 derrived from solo project text handling class.
 * 2016 03 18 Refined function duplicants by creating new method allignmentSetActual which handles actual allignment process.
 * 2016 03 20 Rewrite code for dynamic and realtime allignments.
 * 2016 03 21 Further refinement of code for dynamic use.
 * 2016 03 21 Finalised the Class.
 * 2016 03 23 Modified Class and Variable Names and include usage of 'this'. Added additional comments.
 * 
 * Usage with Library:
    Download the Library from Shared area OR ask for it directly.
    Add the project library from your Soulution Explorer.
    Add a refernce from your project main soulution to this project.
    Add "using KL_TextHandlingLib".

    You now have "TextHandle" Class in your project.

 * Class Example Available
    See "KL_TextHandlingExample" Project Uploaded to "Team Grixis \\ ÅyT2: DEVELOPMENTÅz \\ ÅyImplementation & TestingÅz \\ ÅyCodesÅz \\ KL Text Handling Library and Example" on google shared drive.
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// This class allows creating conrtolled text objects that can be easily manipulated.
/// </summary>
namespace KL_TextHandlingLib
{
    public class KL_TextHandle
    {
        /// <summary>
        /// List of common anchor positions.
        /// </summary>
        public enum Anchor
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Left,
            Right,
            Top,
            Bottom,
            Center
        }

        //################## CLASS VARIABLES ##################
        #region Class Vars
        /// <summary>
        /// Spritefont used for the text to be displayed
        /// </summary>
        public SpriteFont spriteFont { get; set; }//SpriteFont used for this text
        /// <summary>
        /// Position of object, you should NOT have direct access to this in any circumstance!
        /// </summary>
        protected Vector2 pos, destPos;         //Position of the text (WARNING, Keep protected!, SEE "setPosition" and "modifyPosition" methods), destPos is thoretical position, used if this is attached to a moving anchor point.
        /// <summary>
        /// Actual Displayed Text!
        /// </summary>
        public String displayText { get; set; } //The ACTAUL string displayed
        /// <summary>
        /// Color of the Text
        /// </summary>
        public Color txtColor { get; set; }     //Color of text displayed
        /// <summary>
        /// Whether or not to Draw this object
        /// </summary>
        public bool isHidden { get; set; }      //If object is hidden

        //Variables for Anchoring
        private GraphicsDevice graphicsDevice;  //Used to find Window Width & Height (NOT USED FOR ANYTHING ELSE)
        private Vector2 lastOffsets, dynOffsets;//Last set of specified offsets
        private Rectangle lastAllignRect;       //Last specified Rectangle to anchor to
        private Vector2 lastAllignPos;          //Last specified position to anchor to
        private Anchor lastAllignment;          //Last specified anchor point from Allignment enum
        private bool isDynamic;                 //Used to determine if this moving along with an object
        private Vector2 fontMeasure;            //Width and Height of the String

        //^^^^^^^^^^^^^^^^^^ CLASS VARIABLES ^^^^^^^^^^^^^^^^^^
        #endregion

        //################## CLASS INITIALISATION Overload Methods ##################
        #region Setup Overload Methods

        /// <summary>
        /// Setup the Text Object with preset position.
        /// </summary>
        /// <param name="xPos">Preset X Position</param>
        /// <param name="yPos">Preset Y Position</param>
        /// <param name="txtColor">Color of the displayed Text</param>
        /// <param name="displayText">Actual string that will be displayed</param>
        /// <param name="spriteFont">Spritefont used for displayText</param>
        /// <param name="graphicsDevice">IMPORTANT: Put "GraphicsDevice" here! Anything else may result in strange behavior!</param>
        public KL_TextHandle(int xPos, int yPos, Color txtColor, String displayText, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {
            pos = new Vector2(xPos, yPos);
            this.displayText = displayText;
            this.spriteFont = spriteFont;
            this.txtColor = txtColor;

            //Setups for later use
            this.graphicsDevice = graphicsDevice;
            lastAllignPos = new Vector2(0, 0);
            lastOffsets = new Vector2(0, 0);
            dynOffsets = new Vector2(0, 0);
        }

        /// <summary>
        /// Setup the Text Object Anchored to the Screen
        /// </summary>
        /// <param name="anchorPos">Anchor Position Relative to the GAME WINDOW</param>
        /// <param name="offsetX">X offset from teh anchor point</param>
        /// <param name="offsetY">Y offset from teh anchor point</param>
        /// <param name="fontColor">Color of the displayed Text</param>
        /// <param name="displayText">Actual string that will be displayed</param>
        /// <param name="spriteFont">Spritefont used for displayText</param>
        /// <param name="graphicsDevice">IMPORTANT: Put "GraphicsDevice" here! Anything else may result in strange behavior!</param>
        public KL_TextHandle(Anchor anchorPos, int offsetX, int offsetY, Color fontColor, String displayText, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {
            pos = new Vector2(0, 0);
            this.displayText = displayText;
            this.spriteFont = spriteFont;
            txtColor = fontColor;

            //Setups for later use
            this.graphicsDevice = graphicsDevice;
            lastAllignPos = new Vector2(0, 0);
            dynOffsets = new Vector2(0, 0);

            //Addition Setup for this Overload Method
            lastOffsets = new Vector2(offsetX, offsetY);
            lastAllignment = anchorPos;
            setPosition(anchorPos, offsetX, offsetY);
        }

        /// <summary>
        /// Setup the Text Object Anchored to a RECTANGLE
        /// </summary>
        /// <param name="anchorPos">Anchor Position Relative to the RECTANGLE</param>
        /// <param name="anchorRect">RECTANGLE to anchor to</param>
        /// <param name="offsetX">X offset from teh anchor point</param>
        /// <param name="offsetY">Y offset from teh anchor point</param>
        /// <param name="fontColor">Color of the displayed Text</param>
        /// <param name="displayText">Actual string that will be displayed</param>
        /// <param name="spriteFont">Spritefont used for displayText</param>
        /// <param name="graphicsDevice">IMPORTANT: Put "GraphicsDevice" here! Anything else may result in strange behavior!</param>
        public KL_TextHandle(Anchor anchorPos, Rectangle anchorRect, int offsetX, int offsetY, Color fontColor, String displayText, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {
            pos = new Vector2(0, 0);
            this.displayText = displayText;
            this.spriteFont = spriteFont;
            txtColor = fontColor;

            //Setups for later use
            this.graphicsDevice = graphicsDevice;
            lastAllignPos = new Vector2(0, 0);
            dynOffsets = new Vector2(0, 0);

            //Addition Setup for this Overload Method
            lastOffsets = new Vector2(offsetX, offsetY);
            lastAllignment = anchorPos;
            lastAllignRect = anchorRect;
            setPosition(anchorRect, anchorPos, offsetX, offsetY);
        }

        /// <summary>
        /// Setup the Text Object Anchored to a POSITION (Vector2)
        /// </summary>
        /// <param name="anchorPos">Anchor Position Relative to the POSITION</param>
        /// <param name="anchorVPos">POSITION to anchor to</param>
        /// <param name="offsetX">X offset from teh anchor point</param>
        /// <param name="offsetY">Y offset from teh anchor point</param>
        /// <param name="fontColor">Color of the displayed Text</param>
        /// <param name="displayText">Actual string that will be displayed</param>
        /// <param name="spriteFont">Spritefont used for displayText</param>
        /// <param name="graphicsDevice">IMPORTANT: Put "GraphicsDevice" here! Anything else may result in strange behavior!</param>
        public KL_TextHandle(Anchor anchorPos, Vector2 anchorVPos, int offsetX, int offsetY, Color fontColor, String displayText, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {
            pos = new Vector2(0, 0);
            this.displayText = displayText;
            this.spriteFont = spriteFont;
            txtColor = fontColor;

            //Setups for later use
            this.graphicsDevice = graphicsDevice;
            lastAllignPos = new Vector2(0, 0);
            dynOffsets = new Vector2(0, 0);

            //Addition Setup for this Overload Method
            lastAllignPos = anchorVPos;
            lastOffsets = new Vector2(offsetX, offsetY);
            lastAllignment = anchorPos;
            setPosition(anchorVPos, anchorPos, offsetX, offsetY);
        }

        /// <summary>
        /// Setup a text to modify later.
        /// </summary>
        /// <param name="displayText">Actual string that will be displayed</param>
        /// <param name="spriteFont">Spritefont used for displayText</param>
        /// <param name="graphicsDevice">IMPORTANT: Put "GraphicsDevice" here! Anything else may result in strange behavior!</param>
        public KL_TextHandle(String displayText, SpriteFont spriteFont, GraphicsDevice graphicsDevice)
        {
            pos = new Vector2(0, 0);
            this.displayText = displayText;
            this.spriteFont = spriteFont;
            txtColor = Color.White;

            //Setups for later use
            this.graphicsDevice = graphicsDevice;
        }

        //^^^^^^^^^^^^^^^^^^ SETUP Overload Methods ^^^^^^^^^^^^^^^^^^
        #endregion

        //################## UPDATE OBJECT ##################
        #region UpdateObject

        /// <summary>
        /// ONLY NESSARY IF THE ANCHORED RECTANGLE HAS MOVED!
        /// </summary>
        /// <param name="anchorRect">Rectangle Initially Specified</param>
        public void updateObject(Rectangle anchorRect)
        {
            if (isDynamic)
            {
                lastAllignRect = anchorRect;
                setPositionDynamic();
            }
        }

        /// <summary>
        /// ONLY NESSARY IF THE ANCHORED POSITION HAS MOVED, Like the Player for Nametags
        /// </summary>
        /// <param name="anchorRect">Position of object to attach to</param>
        public void updateObject(Vector2 anchorVPos)
        {
            lastAllignPos = anchorVPos;
            setPosition((int)(anchorVPos.X + lastOffsets.X), (int)(anchorVPos.Y + lastOffsets.Y));
        }
        //^^^^^^^^^^^^^^^^^^ UPDATE OBJECT ^^^^^^^^^^^^^^^^^^
        #endregion

        //################## DRAW METHOD ##################
        #region DrawMethod

        /// <summary>
        /// Draws the text IF isHidden is FALSE
        /// </summary>
        /// <param name="DrawFor">spritebatch</param>
        public void DrawObject(SpriteBatch DrawFor)
        {
            if (!isHidden)
                DrawFor.DrawString(spriteFont, displayText, pos, txtColor);
        }
        //^^^^^^^^^^^^^^^^^^ DRAW METHOD ^^^^^^^^^^^^^^^^^^
        #endregion

        //################## Actual Allignment Function ##################
        #region Allignment Function Actual
        /// <summary>
        /// Actual Allignment Function. WARNING: This is NOT intended to be used directly! Using this directly MAY result in strange behavior!
        /// </summary>
        /// <param name="AnchorPos">Anchor enum</param>
        /// <param name="TWidth">Wdith of anchored object</param>
        /// <param name="THeight">Height of anchored object</param>
        /// <param name="fontMeasure">String's Width&Height Calculated by MeasureString().</param>
        /// <param name="offsets">Vector2 offsets of (X, Y)</param>
        private void allignmentSetActual(Anchor AnchorPos, int TWidth, int THeight, Vector2 fontMeasure, Vector2 offsets)
        {
            switch (AnchorPos)
            {
                case Anchor.Bottom:
                    pos.X = (TWidth / 2) - (fontMeasure.X / 2) + offsets.X;
                    pos.Y = THeight - fontMeasure.Y - offsets.Y;
                    break;
                case Anchor.BottomLeft:
                    pos.X = 0 + offsets.X;
                    pos.Y = THeight - fontMeasure.Y + offsets.Y;
                    break;
                case Anchor.BottomRight:
                    pos.X = TWidth - fontMeasure.X + offsets.X;
                    pos.Y = THeight - fontMeasure.Y + offsets.Y;
                    break;
                case Anchor.Center:
                    pos.X = (TWidth / 2) - (fontMeasure.X / 2) + offsets.X;
                    pos.Y = (THeight / 2) - (fontMeasure.Y / 2) + offsets.Y;
                    break;
                case Anchor.Left:
                    pos.X = 0 + offsets.X;
                    pos.Y = (THeight / 2) - (fontMeasure.Y / 2) + offsets.Y;
                    break;
                case Anchor.Right:
                    pos.X = TWidth - fontMeasure.X + offsets.X;
                    pos.Y = (THeight / 2) - (fontMeasure.Y / 2) + offsets.Y;
                    break;
                case Anchor.Top:
                    pos.X = (TWidth / 2) - (fontMeasure.X / 2) + offsets.X;
                    pos.Y = 0 + offsets.Y;
                    break;
                case Anchor.TopLeft:
                    pos.X = 0 + offsets.X;
                    pos.Y = 0 + offsets.Y;
                    break;
                case Anchor.TopRight:
                    pos.X = TWidth - fontMeasure.X + offsets.X;
                    pos.Y = 0 + offsets.Y;
                    break;
            }
        }
        //^^^^^^^^^^^^^^^^^^ Actual Allignment Function ^^^^^^^^^^^^^^^^^^
        #endregion

        //################## Set Position / Allignment ##################
        #region SetPosition Overload Methods

        /// <summary>
        /// Sets the position of the text. (STATIC)
        /// </summary>
        /// <param name="X">X Position</param>
        /// <param name="Y">Y Position</param>
        public void setPosition(int X, int Y)
        {
            isDynamic = false;
            pos.X = X;
            pos.Y = Y;
        }

        /// <summary>
        /// Sets the Position of the Text at an Anchor relative to the Game Window with an Offset. (STATIC)
        /// </summary>
        /// <param name="anchorPos">Anchor Position relative to GAME WINDOW</param>
        /// <param name="offsetX">X Offset form Anchor point</param>
        /// <param name="offsetY">Y Offset form Anchor point</param>
        public void setPosition(Anchor anchorPos, int offsetX, int offsetY)
        {
            isDynamic = false;
            fontMeasure = spriteFont.MeasureString(displayText);
            lastOffsets.X = offsetX;
            lastOffsets.Y = offsetY;
            lastAllignment = anchorPos;

            allignmentSetActual(anchorPos, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, fontMeasure, lastOffsets);
        }

        /// <summary>
        /// Sets the Position of the Text at an Anchor relative to specified Rectangle with an Offset. (STATIC/DYNAMIC, use updateObject() if rectangle moves)
        /// </summary>
        /// <param name="anchorRect">Rectangle to Attach to</param>
        /// <param name="anchorPos">Anchor Position relative to the RECTANGLE</param>
        /// <param name="offsetX">X Offset form Anchor point</param>
        /// <param name="offsetY">Y Offset form Anchor point</param>
        public void setPosition(Rectangle anchorRect, Anchor anchorPos, int offsetX, int offsetY)
        {
            isDynamic = true;
            fontMeasure = spriteFont.MeasureString(displayText);
            lastOffsets.X = offsetX;
            lastOffsets.Y = offsetY;
            lastAllignment = anchorPos;
            lastAllignRect = anchorRect;

            setPositionDynamic();

            //allignmentSetActual(anchorPos, gd.Viewport.Width, gd.Viewport.Height, fontMeasure, lastOffsets);
        }

        public void setPosition(Vector2 anchorVPos, Anchor anchorPos, int offsetX, int offsetY)
        {
            isDynamic = true;
            fontMeasure = spriteFont.MeasureString(displayText);
            lastOffsets.X = offsetX;
            lastOffsets.Y = offsetY;
            lastAllignment = anchorPos;
            lastAllignPos = anchorVPos;

            setPositionDynamic();

            //allignmentSetActual(anchorPos, gd.Viewport.Width, gd.Viewport.Height, fontMeasure, lastOffsets);
        }

        private void setPositionDynamic()
        {
            dynOffsets.X = lastAllignRect.X + lastOffsets.X;
            dynOffsets.Y = lastAllignRect.Y + lastOffsets.Y;

            allignmentSetActual(lastAllignment, lastAllignRect.Width, lastAllignRect.Height, fontMeasure, dynOffsets);
        }
        //^^^^^^^^^^^^^^^^^^ Set Position / Allignment ^^^^^^^^^^^^^^^^^^
        #endregion
        
        public void set_Offset(int x, int y)
        {
            lastOffsets.X = x;
            lastOffsets.Y = y;
        }
    }
}
