using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SuperMarioH3.UI.Bas;

namespace SuperMarioH3.UI.Menu
{
    public class MenuManager
    {
        private Dictionary<string, Func<MenuScreen>> menus = new();
        MenuScreen activeMenu;

        public void RegisterMenu(string name, Func<MenuScreen> create)
        {
            menus[name] = create;
        }

        public void ShowMenu(string name)
        {
            activeMenu = menus[name]();
            activeMenu.ResetInput();
        }

        public void Update(GameTime gameTime)
        {
            activeMenu.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            activeMenu.Draw(gameTime);
        }
    }
}
