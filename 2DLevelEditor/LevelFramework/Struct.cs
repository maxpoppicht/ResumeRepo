using System;
using System.Collections.Generic;

namespace LevelFramework
{
    [Serializable]
    public struct Level
    {
        public string Name;
        public int SizeX;
        public int SizeY;
        public List<Sprite> Sprites;
        public List<Layer> Layers;

        public Level(string _name, int _sizeX, int _sizeY, List<Sprite> _sprites, List<Layer> _layers)
        {
            Name = _name;
            SizeX = _sizeX;
            SizeY = _sizeY;
            Sprites = _sprites;
            Layers = _layers;
        }
    }

    [Serializable]
    public struct Layer
    {
        public int ZOrder;
        public List<Tile> Tiles;
    }

    [Serializable]
    public struct Tile
    {
        public int TileID;
        public int SpriteID;
        public int PosX;
        public int PosY;
        public bool HasCollision;
    }

    [Serializable]
    public struct Sprite
    {
        public int ID;
        public int SizeX;
        public int SizeY;
        public byte[] ImageData;
    }

    [Serializable]
    public struct Editor_Button
    {
        public string Name;
        public int ID;
        public int Height;
        public int TileID;
        public int SpriteID;
    }

}
