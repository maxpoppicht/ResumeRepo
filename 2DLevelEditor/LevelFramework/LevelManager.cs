using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;

namespace LevelFramework
{
    public class LevelManager
    {
        public Level LoadLevelBinary(string _path)
        {
            StreamReader Reader = new StreamReader(_path);

            BinaryFormatter formatter = new BinaryFormatter();
            Level lvl = (Level)formatter.Deserialize(Reader.BaseStream);
            Reader.Close();
            return lvl;
        }

        public void SaveLevelBinary(string _path, Level _level)
        {
            StreamWriter Writer = new StreamWriter(_path);

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(Writer.BaseStream, _level);
            Writer.Close();
        }

        public Level LoadLevelXML(string _path)
        {
            StreamReader Reader = new StreamReader(_path);

            XmlSerializer Serializer = new XmlSerializer(typeof(Level));
            Level lvl = (Level)Serializer.Deserialize(Reader.BaseStream);
            Reader.Close();
            return lvl;
        }

        public void SaveLevelXML(string _path, Level _level)
        {
            StreamWriter Writer = new StreamWriter(_path);

            XmlSerializer Serializer = new XmlSerializer(typeof(Level));
            Serializer.Serialize(Writer.BaseStream, _level);
            Writer.Close();
        }

        public void LoadSprite(string _path, Level _level)
        {
            Sprite NewSprite = new Sprite();

            int HighestIDFound = 0;
            foreach (Sprite sprite in _level.Sprites)
            {
                if (sprite.ID > HighestIDFound)
                {
                    HighestIDFound = sprite.ID;
                }
            }

            NewSprite.ID = HighestIDFound + 1;

            _level.Sprites.Add(NewSprite);
        }

        public void GenerateRandomSprite(Level _level, Random _random)
        {
            Sprite RandomSprite = new Sprite();

            // Generate new ID
            int HighestIDFound = 0;
            foreach (Sprite sprite in _level.Sprites)
            {
                if (sprite.ID > HighestIDFound)
                {
                    HighestIDFound = sprite.ID;
                }
            }
            RandomSprite.ID = HighestIDFound + 1;

            // Set Size
            RandomSprite.SizeX = 1;
            RandomSprite.SizeY = 1;

            Bitmap RandomBitmap = new Bitmap(1, 1);
            int r = _random.Next(0, 256);
            int g = _random.Next(0, 256);
            int b = _random.Next(0, 256);
            RandomBitmap.SetPixel(0, 0, Color.FromArgb(255, r, g, b));
            MemoryStream BitmapStream = new MemoryStream();
            RandomBitmap.Save(BitmapStream, System.Drawing.Imaging.ImageFormat.Bmp);
            RandomSprite.ImageData = BitmapStream.ToArray();

            _level.Sprites.Add(RandomSprite);
        }

        public byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public byte[] ImageToByte(string _path)
        {
            return ImageToByte(Image.FromFile(_path));
        }

        public Level GenerateLevel(int _sizeX, int _sizeY)
        {
            // Init a level
            Level GeneratedLevel = new Level("RandomGeneratedLevel", _sizeX, _sizeY, new List<Sprite>(), new List<Layer>());

            // Lege Layer an
            Layer _layer = new Layer();
            Tile _tile = new Tile();

            _layer.ZOrder = 0;
            _layer.Tiles = new List<Tile>();
            
            for (int y = 0; y < _sizeY; y++)
            {
                for (int x = 0; x < _sizeX; x++)
                {
                    _tile.PosX = x;
                    _tile.PosY = y;
                    
                    _tile.HasCollision = false;

                    List<int> PossibleSpriteIDs = new List<int>();
                    _tile.SpriteID = -1;
                    foreach (Sprite sprite in GeneratedLevel.Sprites)
                    {
                        PossibleSpriteIDs.Add(sprite.ID);
                    }
                    _layer.Tiles.Add(_tile);
                }
            }
            GeneratedLevel.Layers.Add(_layer);
            return GeneratedLevel;
        }
    }
}
