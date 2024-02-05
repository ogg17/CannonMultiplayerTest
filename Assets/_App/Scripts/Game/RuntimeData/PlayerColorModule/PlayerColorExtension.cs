using System;
using System.Collections.Generic;

namespace VgGames.Game.RuntimeData.PlayerColorModule
{
    public static class PlayerColorExtension
    {
        private static readonly Dictionary<string, PlayerColor> Colors = new()
        {
            { "Red", PlayerColor.Red },
            { "Green", PlayerColor.Green },
            { "Blue", PlayerColor.Blue },
            { "Yellow", PlayerColor.Yellow }
        };
        
        private static readonly Dictionary<PlayerColor, string> Tags = new()
        {
            { PlayerColor.Red, "Red" },
            { PlayerColor.Green, "Green" },
            { PlayerColor.Blue, "Blue" },
            { PlayerColor.Yellow, "Yellow" }
        };

        public static PlayerColor ToColor(this string tag)
        {
            if (Colors.TryGetValue(tag, out var c))
                return c;
            throw new IndexOutOfRangeException("Color Not Found!");
        }

        public static bool TryToColor(this string tag, out PlayerColor color)
        {
            if (Colors.TryGetValue(tag, out var c))
            {
                color = c;
                return true;
            }

            color = default;
            return false;
        }

        public static string ToTag(this PlayerColor color)
        {
            if (Tags.TryGetValue(color, out var t))
                return t;
            throw new IndexOutOfRangeException("Color Tag Not Found!");
        }

        public static bool TryToTag(this PlayerColor color, out string tag)
        {
            if (Tags.TryGetValue(color, out var t))
            {
                tag = t;
                return true;
            }

            tag = default;
            return false;
        }
    }
}