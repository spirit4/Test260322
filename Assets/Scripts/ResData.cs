using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Assets.Scripts
{
    public static class ResData
    {
        public static Dictionary<Type, Sprite> sprites;//TODO readonly? find better way
        public static void Init()
        {
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlases/Units");

            sprites = new Dictionary<Type, Sprite>();
            sprites.Add(typeof(Redness), atlas.GetSprite("RedCardOverlay"));
            sprites.Add(typeof(Greenness), atlas.GetSprite("GreenCardOverlay"));
        }
    }
}
