
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LevelFramework;
using Microsoft.Win32;

namespace _2DLevelEditor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Editor_Button lastbutton = new Editor_Button();

        List<Tile> l_tiles = new List<Tile>();

        List<Sprite> l_sprites = new List<Sprite>();

        List<Editor_Button> emptyButtonsAtStart = new List<Editor_Button>();

        List<Button> rightSideButtons = new List<Button>();

        List<Button> leftSideButtons = new List<Button>();

        List<string> l_materials = new List<string>();

        Level level = new Level();

        LevelManager manager = new LevelManager();

        string path
        {
            get
            {
                return Environment.CurrentDirectory + @"\Pictures\";
            }
        }
        

        public MainWindow()
        {
            InitializeComponent();

            level = manager.GenerateLevel(10, 10);

            // Fill List with Buttons and Init
            FillListsForButtons(l_tiles, l_sprites, emptyButtonsAtStart); 

            InitializeLevel(level);


        }

        //Fill List with Buttons and Init
        public void FillListsForButtons(List<Tile> _Tiles, List<Sprite> _Sprites, List<Editor_Button> _Buttons)
        {
            LevelManager lvlmanager = new LevelManager();
            
            l_materials.Add("Brick.png");
            l_materials.Add("Dirt.png");
            l_materials.Add("Grass.png");
            l_materials.Add("GrassAndDirt.png");
            l_materials.Add("Lava.png");

            for (int i = 0; i < l_materials.Count; i++)
            {
                Editor_Button button = new Editor_Button();

                button.SpriteID = i;

                button.TileID = i;

                button.Height = 64;

                _Buttons.Add(button);

                Tile tile = new Tile();

                tile.TileID = i;

                tile.SpriteID = i;

                _Tiles.Add(tile);

                Sprite sprite = new Sprite();

                sprite.ID = i;

                sprite.ImageData = lvlmanager.ImageToByte(path + l_materials[i]);

                _Sprites.Add(sprite);

                InitializeGameObjectListToWindow(_Buttons, _Sprites, _Tiles);
            }
        }

        public void SaveButton(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Möchtest du Speichern?", "Save Level", MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                LevelManager lvlmanager = new LevelManager();
                string path = Environment.CurrentDirectory;
                path += (@"\Saves\lastSave.xml");
                lvlmanager.SaveLevelXML(path, level);
            }
        }

        private void Save_As_Button(object sender, RoutedEventArgs e)
        {

            SaveFileDialog savefile = new SaveFileDialog();

            savefile.Filter = "Binary Format (*.lvl)|*.lvl|XML Format (*.xml)|*.xml";
            
            bool? result = savefile.ShowDialog();

            LevelManager lvlmanager = new LevelManager();


            if(result == true)
            {
                if(savefile.FilterIndex == 1)
                {
                    lvlmanager.SaveLevelBinary(savefile.FileName, level);
                }
                else if(savefile.FilterIndex == 2)
                {
                    lvlmanager.SaveLevelXML(savefile.FileName, level);   
                }
            }


        }

        private void Load_Button(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();

            openfile.Filter = "Binary Format (*.lvl)|*.lvl|XML Format (*.xml)|*.xml";

            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                if (openfile.FilterIndex == 1)
                {
                    level = manager.LoadLevelBinary(openfile.FileName);
                }
                else if (openfile.FilterIndex == 2)
                {
                    level = manager.LoadLevelXML(openfile.FileName);
                }
            }
            for (int i = 0; i < level.SizeX * level.SizeY; i++)
            {
                Image buttonImage = new Image();

                //Black
                if (level.Layers[0].Tiles[i].SpriteID == 0)
                {
                    buttonImage.Source = LoadImage(manager.ImageToByte(path + l_materials[0]));

                    leftSideButtons[i].Content = buttonImage;
                }
                //Green
                else if (level.Layers[0].Tiles[i].SpriteID == 1)
                {
                    buttonImage.Source = LoadImage(manager.ImageToByte(path + l_materials[1]));

                    leftSideButtons[i].Content = buttonImage;
                }
                //Blue
                else if (level.Layers[0].Tiles[i].SpriteID == 2)
                { 
                    buttonImage.Source = LoadImage(manager.ImageToByte(path + l_materials[2]));

                    leftSideButtons[i].Content = buttonImage;
                }
                //Yellow
                else if (level.Layers[0].Tiles[i].SpriteID == 3)
                {
                    buttonImage.Source = LoadImage(manager.ImageToByte(path + l_materials[3]));

                    leftSideButtons[i].Content = buttonImage;
                }
                //Brown
                else if (level.Layers[0].Tiles[i].SpriteID == 4)
                {
                    buttonImage.Source = LoadImage(manager.ImageToByte(path + l_materials[4]));

                    leftSideButtons[i].Content = buttonImage;
                }
            }
        }


        private void InitializeGameObjectListToWindow(List<Editor_Button> _Buttonlist, List<Sprite> _SpriteList, List<Tile> _TileList)
        {
            for (int y = 0; y < _Buttonlist.Count; y++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(64);
                Grid_GO_Area.RowDefinitions.Add(row);
            }

            
            foreach (Editor_Button _button in _Buttonlist)
            {
                Button button = new Button();


                Image ButtonImage = new Image();

                byte[] Image = new byte[0];

                foreach (Sprite sprite in _SpriteList)
                {
                    if (sprite.ID == _button.SpriteID)
                    {
                        Image = sprite.ImageData;
                        button.Tag = sprite.ID;
                        break;
                    }
                }
                button.Click += GoButtonsSetLastClicked;
                ButtonImage.Source = LoadImage(Image);
                button.Content = ButtonImage;
                Grid_GO_Area.Children.Add(button); 
                Grid.SetRow(button, _Buttonlist.Count-1);


                rightSideButtons.Add(button);
            }
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }


        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            //closes the window
            Application.Current.Shutdown();


        }

       

        private void OpenConfigWindowStart(object sender, RoutedEventArgs e,int _SizeX, int _SizeY)
        {
            //Open a window with config options
            LevelManager LManager = new LevelManager();
            Level MyLevel = LManager.GenerateLevel(_SizeX, _SizeY);
            InitializeLevel(MyLevel);
        }

        private void Add_A_Gameobject_Button_Struct(List<Editor_Button> _LButtons, List<Sprite> _LSprites, List<Tile> _LTiles, int _Height, bool _hasCollision, string _Path, string _Name)
        {
            LevelManager _LManager = new LevelManager();
            Editor_Button _button;
            _button.ID = _LButtons.Count;
            _button.Height = _Height;
            _button.SpriteID = _LSprites.Count;
            _button.TileID = _LTiles.Count;
            _button.Name = _Name;

            Tile _tile;
            _tile.SpriteID = _button.SpriteID;
            _tile.TileID = _button.TileID;
            _tile.HasCollision = _hasCollision;
            _tile.PosX = 0;
            _tile.PosY = 0;

            Sprite _sprite;
            _sprite.ID = _button.SpriteID;
            _sprite.ImageData = _LManager.ImageToByte(@_Path);
            _sprite.SizeX = 64;
            _sprite.SizeY = 64;

            _LButtons.Add(_button);
            _LTiles.Add(_tile);
            _LSprites.Add(_sprite);

            InitializeGameObjectListToWindow(_LButtons, _LSprites, _LTiles);
        }
         
        private void LevelButtonsGetInfoOnClick(object sender, RoutedEventArgs e)
        {
            Button ClickedButton = (Button)sender;
            Image ButtonImage = new Image();

            byte[] Image = l_sprites[lastbutton.SpriteID].ImageData;

            

            ButtonImage.Source = LoadImage(Image);
            ClickedButton.Content = ButtonImage;

            int tagAsInt;

            tagAsInt = int.Parse(ClickedButton.Tag.ToString());

            Tile newTile = level.Layers[0].Tiles[tagAsInt];

            newTile.SpriteID = lastbutton.SpriteID;

            level.Layers[0].Tiles[tagAsInt] = newTile;



        }

        private void GoButtonsSetLastClicked(object sender, RoutedEventArgs e)
        {
            Button ClickedButton = (Button)sender;
            Editor_Button GoButton = lastbutton;
           
            lastbutton = emptyButtonsAtStart[(int)ClickedButton.Tag];
        }

        void InitializeLevel(Level _level)
        {

            for (int y = 0; y < _level.SizeY; y++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(64);
                Grid_EditArea.RowDefinitions.Add(row);
            }

            for (int x = 0; x < _level.SizeX; x++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(64);
                Grid_EditArea.ColumnDefinitions.Add(col);
            }
            int count = 0;

            leftSideButtons.Clear();

            foreach (Tile tile in _level.Layers[0].Tiles)
            {
                Button NewButton = new Button();
                NewButton.Tag = count;
                count++;
                Image ButtonImage = new Image();

                byte[] Image = new byte[0];

                leftSideButtons.Add(NewButton);

                foreach (Sprite sprite in _level.Sprites)
                {
                    
                    if (sprite.ID == tile.SpriteID)
                    {
                        Image = sprite.ImageData;
                        break;
                    }
                }
                ButtonImage.Source = LoadImage(Image);
                NewButton.Content = ButtonImage;
                NewButton.Click += LevelButtonsGetInfoOnClick;
                Grid_EditArea.Children.Add(NewButton);
                Grid.SetColumn(NewButton, tile.PosX);
                Grid.SetRow(NewButton, tile.PosY);
            }
        }

        private void New_Button(object sender, RoutedEventArgs e)
        {
            


            level = manager.GenerateLevel(10, 10);

            InitializeLevel(level);
        }
    }
}
