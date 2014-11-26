using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ruminate.GUI.Framework {

    public sealed class Skin 
    {
        public Texture2D Texture { get; private set; }       
        public Dictionary<string, Rectangle> Map { get; private set; }

        public Skin(Texture2D texture, Source source) 
        {
            Texture = texture;
            Map = source.BuildAtlas();
        }
    }
}
