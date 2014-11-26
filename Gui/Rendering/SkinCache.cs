using System.Collections.Generic;

namespace Ruminate.GUI.Framework 
{
    public class SkinCache
    {
        private string CurrentSkin { get; set; }

        private Dictionary<string, Skin> SkinMap { get; set; }

        public void AddSkin(string name, Skin skin)
        {
            if (CurrentSkin == null) 
            {
                CurrentSkin = name;
            }

            SkinMap.Add(name, skin);
        }

        public void SetCurrentSkin(string name) 
        {
            CurrentSkin = name;
        }

        public Skin GetSkin()
        {
            return SkinMap[CurrentSkin];
        }
    }
}
