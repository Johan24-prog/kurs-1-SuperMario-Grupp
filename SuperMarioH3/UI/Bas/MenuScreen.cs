using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    }
}
