using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioH3.Menu
{
    public class MenuButton
    {
        SpriteFont font;

        Rectangle buttonBox;

        Vector2 textPos;
        Vector2 origin;

        string buttonText;

        public Rectangle Bounds => buttonBox;
        public bool IsHovering { get; set; }
        public bool IsClicked { get; set; }
        public float LineSpacing => font.LineSpacing;

        public Action OnClick;

        public MenuButton(SpriteFont font, string buttonText, Vector2 textPos, Action onClick)
        {
            this.font = font;
            this.buttonText = buttonText;
            this.textPos = textPos;
            OnClick = onClick;

            Vector2 textSize = font.MeasureString(buttonText);
            //Set origin to middle of string
            origin = textSize * 0.5f;

            buttonBox = new Rectangle(
                (int)(textPos.X - origin.X - 3),
                (int)(textPos.Y - origin.Y),
                (int)textSize.X,
                (int)textSize.Y
            );
        }
        public void TriggerClick()
        {
            OnClick?.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = IsHovering ? Color.White : Color.GhostWhite;

            spriteBatch.DrawString(
                font, buttonText, textPos, color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
