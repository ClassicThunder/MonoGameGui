using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Ruminate.GUI.Framework 
{
    public abstract class Source
    {
        protected String FileName { get; private set; }

        public Source(string fileName)
        {
            FileName = fileName;
        }

        internal abstract Dictionary<string, Rectangle> BuildAtlas();
    }
}
