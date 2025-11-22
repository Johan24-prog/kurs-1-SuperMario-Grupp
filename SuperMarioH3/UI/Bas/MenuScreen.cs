using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperMarioH3.UI.Menu;

namespace SuperMarioH3.UI.Bas
{
    public abstract class MenuScreen
    {
        protected List<MenuButton> buttons = new();

        protected Game1 game;
        protected SpriteBatch spriteBatch;
        protected Texture2D smallIcon;
        protected MenuManager menuManager;

        protected Vector2 iconOrigin;

        protected int selectedIndex = 0;

        // Keyboard states for input handling
        private KeyboardState previousKey;
        private KeyboardState currentKey;

        //Input handling for gamepad
        private GamePadState currentGamePad;
        private GamePadState previousGamePad;

        public virtual void Update(GameTime gameTime)
        {
            UserInput();
            for (int i = 0; i < buttons.Count; i++)
            {
                MouseInput(i);
                if (i == selectedIndex)
                {
                    buttons[i].IsHovering = true;
                    if (buttons[i].IsClicked || InputClicked())
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
        public virtual void Draw(GameTime gameTime)
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

        private void UserInput()
        {
            currentGamePad = GamePad.GetState(PlayerIndex.One);
            currentKey = Keyboard.GetState();

            if (InputUp())
            {
                MoveSelection(-1);
            }
            if (InputDown())
            {
                MoveSelection(1);
            }

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

        private bool InputClicked() =>
            currentKey.IsKeyDown(Keys.Enter) ||
            currentGamePad.IsButtonDown(Buttons.A);

        private bool InputUp() =>
            currentKey.IsKeyDown(Keys.W) && !previousKey.IsKeyDown(Keys.W) ||
            currentKey.IsKeyDown(Keys.Up) && !previousKey.IsKeyDown(Keys.Up) ||
            currentGamePad.IsButtonDown(Buttons.DPadUp) && !previousGamePad.IsButtonDown(Buttons.DPadUp);

        private bool InputDown() =>
            currentKey.IsKeyDown(Keys.S) && !previousKey.IsKeyDown(Keys.S) ||
            currentKey.IsKeyDown(Keys.Down) && !previousKey.IsKeyDown(Keys.Down) ||
            currentGamePad.IsButtonDown(Buttons.DPadDown) && !previousGamePad.IsButtonDown(Buttons.DPadDown);

        private void ResetInput()
        {
            currentGamePad = GamePad.GetState(PlayerIndex.One);
            previousGamePad = currentGamePad;

            currentKey = Keyboard.GetState();
            previousKey = currentKey;
        }
    }
}

