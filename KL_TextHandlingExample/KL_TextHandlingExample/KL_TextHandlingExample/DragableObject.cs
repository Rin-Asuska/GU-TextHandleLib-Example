using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KL_TextHandlingExample
{
    class DragableObject
    {
        //Class Vars
        public Texture2D texture;
        public Vector2 pos;
        public Rectangle rect;
        public Color ObjColor;
        public bool isDragged;

        public DragableObject(int x, int y, Color col, Texture2D img) //Setup
        {
            texture = img;
            ObjColor = col;
            rect = new Rectangle(x, y, texture.Width, texture.Height);
            isDragged = false;
            pos = new Vector2(x, y);
        }

        public void updateObject(MouseState ms, MouseState msOld)
        {
            //Pos is only to demonstrate NAMETAG example
            pos.X = rect.X;
            pos.Y = rect.Y;

            if (ms.LeftButton == ButtonState.Pressed && msOld.LeftButton != ButtonState.Pressed)
            {
                if (rect.Contains(ms.X, ms.Y) && !isDragged)
                {
                    isDragged = true;
                }
                else
                {
                    isDragged = false;
                }
            }

            if (isDragged)
            {
                rect.X = ms.X - (rect.Width / 2);
                rect.Y = ms.Y - (rect.Height / 2);
            }
        }

        public void DrawObject(SpriteBatch DrawFor) //Draw
        {
            DrawFor.Draw(texture, rect, ObjColor);
        }
    }
}
