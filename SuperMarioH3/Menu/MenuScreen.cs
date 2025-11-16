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
        private Texture2D smallIcon;

        Vector2 iconOrigin;

        public MenuScreen(Game1 game, SpriteFont font, SpriteBatch spriteBatch, Texture2D smallIcon)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.smallIcon = smallIcon;

            //center origin of icon
            iconOrigin = new Vector2(smallIcon.Width * 0.5f, smallIcon.Height * 0.5f);

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
            MouseState mouse = Mouse.GetState();

            foreach (var button in buttons)
                button.Update(mouse);

            if (buttons[0].IsClicked)
                game.StartGame();

            if (buttons[1].IsClicked) { }
            //ExitGame();
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (var button in buttons)
            {
                if (button.IsHovering)
                {
                    var spacing = button.LineSpacing * 0.5f;
                    var bounds = button.Bounds;
                    //Before menu text
                    var iconPos = new Vector2(bounds.Left - spacing - iconOrigin.X, bounds.Y + bounds.Height * 0.5f);
                    spriteBatch.Draw(
                        smallIcon, iconPos, null, Color.White, 0f, iconOrigin, 1f, SpriteEffects.None, 0f);
                    //After menu text
                    iconPos = new Vector2(bounds.Right + spacing + iconOrigin.X, bounds.Y + bounds.Height * 0.5f);
                    spriteBatch.Draw(
                        smallIcon, iconPos, null, Color.White, 0f, iconOrigin, 1f, SpriteEffects.None, 0f);
                }
                button.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
