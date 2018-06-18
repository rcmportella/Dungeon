using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon
{
    public partial class Form1 : Form
    {
        Game game;
        bool p_gameOver = false;
        int p_startTime = 0;
        int p_currentTime = 0;
        int frameCount = 0;
        int frameTimer = 0;
        float frameRate = 0;
        int score = 0;
        Sprite dragon;
        Sprite zombie;
        Sprite spider;
        Sprite skeleton;
        Bitmap grass;
        Sprite archer;
        Sprite arrow;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Main();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Game_KeyPressed(e.KeyCode);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Shutdown();
        }

        public void Main()
        {
            Form1 form = (Form1)this;
            game = new Game(ref form, 800, 600);

            //load and initialize game assets
            Game_Init();

            while (!p_gameOver)
            {
                p_currentTime = Environment.TickCount;
                Game_Update(p_currentTime - p_startTime);
                if (p_currentTime > p_startTime + 16)
                {
                    p_startTime = p_currentTime;
                    Game_Draw();
                    Application.DoEvents();
                    game.Update();
                }
                frameCount += 1;
                if (p_currentTime > frameTimer + 1000)
                {
                    frameTimer = p_currentTime;
                    frameRate = frameCount;
                    frameCount = 0;
                }
            }
            //free memory and shut down
            Game_End();
            Application.Exit();

        }

        public void Shutdown()
        {
            p_gameOver = true;
        }

        public bool Game_Init()
        {
            this.Text = "Archery Shooting Game";

            //load the grassy background
//            grass = game.LoadBitmap("grass.bmp");
            grass = Properties.Resources.Tower1;
            //load the archer
            archer = new Sprite(ref game);
//            archer.Image = game.LoadBitmap("archer_attack.png");
            archer.Images = LoadBitmaps(".\\hunter 96x bitmaps\\shooting ",".bmp");
            archer.Size = new Size(96, 96);
            archer.Columns = 8;
            archer.TotalFrames = 64;
            archer.AnimationRate = 20;
            archer.Position = new PointF(360, 500);
            archer.AnimateDirection = Sprite.AnimateDir.NONE;

            //load the arrow
            arrow = new Sprite(ref game);
            //            arrow.Image = game.LoadBitmap("arrow.png");
            arrow.Image = Properties.Resources.pfeil_mit_schatten0000;
            arrow.Size = new Size(32, 64);
            arrow.TotalFrames = 1;
            arrow.Velocity = new PointF(0, -12.0f);
            arrow.Alive = false;

            //load the zombie
            zombie = new Sprite(ref game);
            zombie.Images = LoadBitmaps(".\\green zombie bitmaps\\walking ", ".bmp");
//            zombie.Image = game.LoadBitmap("zombie walk.png");
            zombie.Size = new Size(96, 96);
            zombie.Columns = 8;
            zombie.TotalFrames = 64;
            zombie.Position = new PointF(100, 10);
            zombie.Velocity = new PointF(-2.0f, 0);
            zombie.AnimationRate = 10;
/*
            //load the spider
            spider = new Sprite(ref game);
            spider.Image = game.LoadBitmap("redspiderwalking.png");
            spider.Size = new Size(96, 96);
            spider.Columns = 8;
            spider.TotalFrames = 64;
            spider.Position = new PointF(500, 80);
            spider.Velocity = new PointF(3.0f, 0);
            spider.AnimationRate = 20;
*/

            //load the dragon
            dragon = new Sprite(ref game);
            dragon.Images = LoadBitmaps(".\\T firedragon\\firedragon bitmaps flying\\firedragon fliegt ", ".png");
            //            dragon.Image = game.LoadBitmap("dragonflying.png");
            dragon.Size = new Size(128, 128);
            dragon.Columns = 8;
            dragon.TotalFrames = 64;
            dragon.AnimationRate = 20;
            dragon.Position = new PointF(300, 130);
            dragon.Velocity = new PointF(-4.0f, 0);

            //load the skeleton
            skeleton = new Sprite(ref game);
            skeleton.Images = LoadBitmaps(".\\swordskel bitmaps\\swordskel läuft ", ".bmp");
//            skeleton.Image = game.LoadBitmap("skeleton_walk.png");
            skeleton.Size = new Size(96, 96);
            skeleton.Columns = 8;
            skeleton.TotalFrames = 64;
            skeleton.Position = new PointF(400, 190);
            skeleton.Velocity = new PointF(5.0f, 0);
            skeleton.AnimationRate = 30;

            return true;
        }

        //not currently needed
        public void Game_Update(int time)
        {
            if (arrow.Alive)
            {
                /*
                //see if arrow hit spider
                if (arrow.IsColliding(ref spider))
                {
                    arrow.Alive = false;
                    score++;
                    spider.X = 800;
                }
                */

                //see if arrow hit dragon
                if (arrow.IsColliding(ref dragon))
                {
                    arrow.Alive = false;
                    score++;
                    dragon.X = 800;
                }
                //see if arrow hit zombie
                if (arrow.IsColliding(ref zombie))
                {
                    arrow.Alive = false;
                    score++;
                    zombie.X = 800;
                }

                //see if arrow hit skeleton
                if (arrow.IsColliding(ref skeleton))
                {
                    arrow.Alive = false;
                    score++;
                    skeleton.X = 800;
                }
            }
        }

        public void Game_Draw()
        {
            int row = 0;

            //draw background
            game.DrawBitmap(ref grass, 0, 0, 800, 600);

            //draw the arrow
            if (arrow.Alive)
            {
                arrow.Y += arrow.Velocity.Y;
                if (arrow.Y < -32)
                    arrow.Alive = false;
                arrow.Draw();
            }
            //draw the archer
            archer.Animate(10, 19);
            if (archer.CurrentFrame == 19)
            {
                archer.AnimateDirection = Sprite.AnimateDir.NONE;
                archer.CurrentFrame = 10;
                arrow.Alive = true;
                arrow.Position = new PointF(
                    archer.X + 32, archer.Y);
            }
            archer.DrawList();


            //draw the zombie
            zombie.X += zombie.Velocity.X;
            if (zombie.X < -96) zombie.X = 800;
            row = 6;
            zombie.Animate(row * 8 + 1, row * 8 + 7);
            zombie.DrawList();

            /*
            //draw the spider
            spider.X += spider.Velocity.X;
            if (spider.X > 800) spider.X = -96;
            row = 2;
            spider.Animate(row * 8 + 1, row * 8 + 7);
            spider.DrawList();
            */

            //draw the skeleton
            skeleton.X += skeleton.Velocity.X;
            if (skeleton.X > 800) skeleton.X = -96;
            row = 2;
            skeleton.Animate(row * 9 + 1, row * 9 + 8);
            skeleton.DrawList();

            //draw the dragon
            dragon.X += dragon.Velocity.X;
            if (dragon.X < -128) dragon.X = 800;
            row = 6;
            dragon.Animate(row * 8 + 1, row * 8 + 7);
            dragon.DrawList();
            game.Print(0, 0, "SCORE " + score.ToString());

        }

        public void Game_End()
        {
            dragon.ImagesDispose();
            dragon = null;
            archer.ImagesDispose();
            archer = null;
//            spider.Image.Dispose();
//            spider = null;
            zombie.ImagesDispose();
            zombie = null;
            grass = null;
        }
        public void Game_KeyPressed(System.Windows.Forms.Keys key)
        {
            switch (key)
            {
                case Keys.Escape: Shutdown(); break;
                case Keys.Space:
                    if (!arrow.Alive)
                    {
                        archer.AnimateDirection = Sprite.AnimateDir.FORWARD;
                        archer.CurrentFrame = 10;
                    }
                    break;
                case Keys.Right: break;
                case Keys.Down: break;
                case Keys.Left: break;
            }
        }

        public List<Bitmap> LoadBitmaps(string path, string imgend)
        {
            List<Bitmap> lb = new List<Bitmap>();
            for(int i = 0; i < 8; i++)
            {
                string temp = path + "n000" + Convert.ToString(i) + imgend;
//                temp = ".\\hunter 96x bitmaps\\Teste.bmp";
                Bitmap im = new Bitmap(temp);
                lb.Add(im);
            }
            for (int i = 0; i < 8; i++)
            {
                string temp = path + "ne000" + Convert.ToString(i) + imgend;
                Bitmap im = new Bitmap(temp);
                lb.Add(im);
            }
            for (int i = 0; i < 8; i++)
            {
                string temp = path + "e000" + Convert.ToString(i) + imgend;
                Bitmap im = new Bitmap(temp);
                lb.Add(im);
            }
            for (int i = 0; i < 8; i++)
            {
                string temp = path + "se000" + Convert.ToString(i) + imgend;
                Bitmap im = new Bitmap(temp);
                lb.Add(im);
            }
            for (int i = 0; i < 8; i++)
            {
                string temp = path + "s000" + Convert.ToString(i) + imgend;
                Bitmap im = new Bitmap(temp);
                lb.Add(im);
            }
            for (int i = 0; i < 8; i++)
            {
                string temp = path + "sw000" + Convert.ToString(i) + imgend;
                Bitmap im = new Bitmap(temp);
                lb.Add(im);
            }
            for (int i = 0; i < 8; i++)
            {
                string temp = path + "w000" + Convert.ToString(i) + imgend;
                Bitmap im = new Bitmap(temp);
                lb.Add(im);
            }
            for (int i = 0; i < 8; i++)
            {
                string temp = path + "nw000" + Convert.ToString(i) + imgend;
                Bitmap im = new Bitmap(temp);
                lb.Add(im);
            }
            return lb;
        }
    }
}
