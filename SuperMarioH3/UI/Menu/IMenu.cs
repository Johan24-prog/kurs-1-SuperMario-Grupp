using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioH3.UI.Menu
{
    public interface IMenu
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
