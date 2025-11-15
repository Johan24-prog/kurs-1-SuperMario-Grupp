using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioH3.Menu
{
    public class MenuScreen
    {
        private List<MenuButton> buttons = new();

        private Game1 game;
        private SpriteBatch spriteBatch;

        public MenuScreen(Game1 game, SpriteFont font, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            //offset space between buttons
            var spacing = font.LineSpacing * 1.7f;

            //Create position of first button
            Vector2 topPosition = new Vector2(
               game.GraphicsDevice.PresentationParameters.BackBufferWidth,
               game.GraphicsDevice.PresentationParameters.BackBufferHeight * 0.7f) * 0.5f;

            //Create buttons
            buttons.Add(new MenuButton(font, "Play Demo", topPosition));

            topPosition.Y += spacing;
            buttons.Add(new MenuButton(font, "Options", topPosition));

            topPosition.Y += spacing;
            buttons.Add(new MenuButton(font, "Exit", topPosition));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
