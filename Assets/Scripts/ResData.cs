using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Assets.Scripts
{
    public static class ResData
    {
        public static Dictionary<Type, Sprite> sprites;
        public static void Init()
        {
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlases/Units");

            sprites = new Dictionary<Type, Sprite>
            {
                { typeof(Redness), atlas.GetSprite("RedCardOverlay") },
                { typeof(Greenness), atlas.GetSprite("GreenCardOverlay") }
            };
        }
    }
}
