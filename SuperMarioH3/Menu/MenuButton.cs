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

        public bool IsHovering { get; set; }
        public bool IsClicked { get; set; }

        public MenuButton(SpriteFont font, string buttonText, Vector2 textPos)
        {
            this.font = font;
            this.buttonText = buttonText;
            this.textPos = textPos;

            Vector2 textSize = font.MeasureString(buttonText);
            //Set origin to middle of string
            origin = textSize * 0.5f;

            buttonBox = new Rectangle(
                (int)(textPos.X - origin.X),
                (int)(textPos.Y - origin.Y),
                (int)textSize.X,
                (int)textSize.Y
            );
        }
        public void Update(MouseState mouse)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, buttonText, textPos, Color.White);
        }
    }
}
