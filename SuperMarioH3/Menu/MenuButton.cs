using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioH3.Menu
{
    public class MenuButton
    {
        SpriteFont font;
        Rectangle buttonBox;

        string buttonText;
        Vector2 textPos;

        public MenuButton(SpriteFont font, string buttonText, Vector2 messagePos)
        {
            this.font = font;
            this.buttonText = buttonText;
            this.textPos = messagePos;

            Vector2 textSize = font.MeasureString(buttonText);
            buttonBox = new Rectangle(
                (int)messagePos.X,
                (int)messagePos.Y,
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
