using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioH3.UI.Bas;

namespace SuperMarioH3.UI.Menu
{
    public class StartScreen : MenuScreen
    {
        public StartScreen(Game1 game, MenuManager menuManager, SpriteFont font, SpriteBatch spriteBatch, Texture2D smallIcon)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.smallIcon = smallIcon;
            this.menuManager = menuManager;

            //center origin of icon
            iconOrigin = new Vector2(smallIcon.Width * 0.5f, smallIcon.Height * 0.5f);

            //offset space between buttons
            var spacing = font.LineSpacing * 1.7f;

            //Create position of first button
            Vector2 topPosition = new Vector2(
               game.GraphicsDevice.PresentationParameters.BackBufferWidth,
               game.GraphicsDevice.PresentationParameters.BackBufferHeight * 0.7f) * 0.5f;

            //Create buttons
            buttons.Add(new MenuButton(font, "Play Demo", topPosition, () =>
            {
                game.StartGame();
            }));

            topPosition.Y += spacing;
            buttons.Add(new MenuButton(font, "Options", topPosition, () =>
            {
                menuManager.ShowMenu("Options");
            }));

            topPosition.Y += spacing;
            buttons.Add(new MenuButton(font, "Exit", topPosition, () =>
            {
                game.Exit();
            }));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}