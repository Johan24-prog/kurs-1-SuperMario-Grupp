using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioH3.UI.Menu
{
    public class TitleText
    {
        SpriteFont font;

        Vector2 textPos;
        Vector2 origin;

        string titleText;

        Color color;

        public float LineSpacing => font.LineSpacing;

        public TitleText(SpriteFont font, string titleText, Vector2 textPos, Color color)
        {
            this.font = font;
            this.textPos = textPos;
            this.titleText = titleText;
            this.color = color;

            Vector2 textSize = font.MeasureString(titleText);
            //Set origin to middle of string
            origin = textSize * 0.5f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                font, titleText, textPos, color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
