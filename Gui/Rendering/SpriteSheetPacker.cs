using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Ruminate.GUI.Framework {        

    public class SpriteSheetPacker : Source
    {
        public SpriteSheetPacker(string fileName) : base(fileName) {}

        internal override Dictionary<string, Rectangle> BuildAtlas()
        {
            var atlas = new Dictionary<string, Rectangle>();

            foreach (var line in FileName.Split(
                    new[] { Environment.NewLine }, 
                    StringSplitOptions.RemoveEmptyEntries))
            {

                if (string.IsNullOrWhiteSpace(line)) { continue; }

                var equalsSplit = line.Split('=');
                var optionSplit = equalsSplit[1].Split(' ');

                var rectangleSplit = (optionSplit[0]).Trim().Split(' ');
                var area = new Rectangle(
                    Int32.Parse(rectangleSplit[0]),
                    Int32.Parse(rectangleSplit[1]),
                    Int32.Parse(rectangleSplit[2]),
                    Int32.Parse(rectangleSplit[3]));

                atlas.Add((equalsSplit[0]).Trim(), area);
            }

            return atlas;
        }
    }
}
