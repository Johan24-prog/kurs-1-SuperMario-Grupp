using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioH3.UI.Menu
{
    public class MenuScreen
    {
        private List<MenuButton> buttons = new();

        private Game1 game;
        private SpriteBatch spriteBatch;
        private Texture2D smallIcon;

        Vector2 iconOrigin;

        // Keyboard states for input handling
        private KeyboardState previousKey;
        private KeyboardState currentKey;

        //Input handling for gamepad
        private GamePadState currentGamePad;
        private GamePadState previousGamePad;

        int selectedIndex = 0;

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
            buttons.Add(new MenuButton(font, "Play Demo", topPosition, () =>
            {
                game.StartGame();
            }));

            topPosition.Y += spacing;
            buttons.Add(new MenuButton(font, "Options", topPosition, () =>
            {
                game.Exit();
            }));

            topPosition.Y += spacing;
            buttons.Add(new MenuButton(font, "Exit", topPosition, () =>
            {
                game.Exit();
            }));
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                UserInput(i);

                if (i == selectedIndex)
                {
                    buttons[i].IsHovering = true;
                    if (buttons[i].IsClicked)
                    {
                        buttons[i].TriggerClick();
                        buttons[i].IsClicked = false;
                    }
                }
                else
                {
                    buttons[i].IsHovering = false;
                }
            }
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
                    var iconPos = new Vector2(bounds.Left - spacing - iconOrigin.X, bounds.Y + bounds.Height * 0.5f - 5);
                    spriteBatch.Draw(
                        smallIcon, iconPos, null, Color.White, 0f, iconOrigin, 1f, SpriteEffects.None, 0f);

                    //After menu text
                    iconPos = new Vector2(bounds.Right + spacing + iconOrigin.X, bounds.Y + bounds.Height * 0.5f - 5);
                    spriteBatch.Draw(
                        smallIcon, iconPos, null, Color.White, 0f, iconOrigin, 1f, SpriteEffects.None, 0f);
                }
                button.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        private void UserInput(int i)
        {
            currentGamePad = GamePad.GetState(PlayerIndex.One);
            currentKey = Keyboard.GetState();

            MouseInput(i);

            if (InputUp())
            {
                MoveSelection(-1);
            }
            if (InputDown())
            {
                MoveSelection(1);
            }
            if (currentKey.IsKeyDown(Keys.Enter) ||
                currentGamePad.IsButtonDown(Buttons.A))
                buttons[i].IsClicked = true;

            previousGamePad = currentGamePad;
            previousKey = currentKey;
        }

        private void MoveSelection(int step)
        {
            selectedIndex = (selectedIndex + step + buttons.Count) % buttons.Count;
        }

        private void MouseInput(int index)
        {
            var mouse = Mouse.GetState();

            if (buttons[index].Bounds.Contains(mouse.Position))
                selectedIndex = index;

            if (buttons[index].IsHovering && mouse.LeftButton == ButtonState.Pressed)
                buttons[index].IsClicked = true;
        }

        private bool InputUp() =>
            currentKey.IsKeyDown(Keys.W) && !previousKey.IsKeyDown(Keys.W) ||
            currentKey.IsKeyDown(Keys.Up) && !previousKey.IsKeyDown(Keys.Up) ||
            currentGamePad.IsButtonDown(Buttons.DPadUp) && !previousGamePad.IsButtonDown(Buttons.DPadUp);

        private bool InputDown() =>
            currentKey.IsKeyDown(Keys.S) && !previousKey.IsKeyDown(Keys.S) ||
            currentKey.IsKeyDown(Keys.Down) && !previousKey.IsKeyDown(Keys.Down) ||
            currentGamePad.IsButtonDown(Buttons.DPadDown) && !previousGamePad.IsButtonDown(Buttons.DPadDown);
    }
}
