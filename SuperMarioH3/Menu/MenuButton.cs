using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public MenuButton(SpriteFont font, string buttonText, Vector2 textPos)
        {
            this.font = font;
            this.buttonText = buttonText;
            this.textPos = textPos;

            Vector2 textSize = font.MeasureString(buttonText);
            //Set origin to middle of string
            origin = textSize * 0.5f;

            buttonBox = new Rectangle(
                (int)(textPos.X - origin.X - 5),
                (int)(textPos.Y - origin.Y),
                (int)textSize.X,
                (int)textSize.Y
            );
        }
        public void Update(MouseState mouse)
        {
            IsHovering = buttonBox.Contains(mouse.Position);
            IsClicked = IsHovering && mouse.LeftButton == ButtonState.Pressed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = IsHovering ? Color.White : Color.GhostWhite;

            spriteBatch.DrawString(
                font, buttonText, textPos, color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
