using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.Snake;


namespace WindowsFormsApp1
{
    #region 第一版可以运行的贪吃蛇
    public partial class Form1 : Form
    {
        // 游戏容器
        private PictureBox gameBoard = new PictureBox();
        //食物随机生成器
        private Random random = new Random();
        //食物
        private Point food = new Point(50, 50);

        private Timer timer = new Timer();

        private bool gameOver = false;
        private int timerInterval = 1000;
        private int speed = 10;
        public Snake snake = new Snake();

        private int score = 0;
        private Snake.Direction dir;
        public Form1()
        {
            InitializeComponent();
            InitGameBorad();
            TimerStart();
        }

        public void InitGameBorad()
        {
            //初始化画板的属性
            gameBoard.Width = 100;
            gameBoard.Height = 100;
            gameBoard.Dock = DockStyle.Fill;
            gameBoard.BackColor = Color.LightCoral;

            // 把画板刷新事件进行绑定
            this.Controls.Add(gameBoard);
            gameBoard.Paint += this.GameBoardPaint;
            gameBoard.Focus();
            // 键盘事件绑定
            this.KeyDown += this.MoveControl;
        }

        private void GameBoardPaint(object sender, PaintEventArgs e)
        {
            int radius = snake.Radious;  // 定义点的半径
            int diameter = 2 * radius;  // 计算点的直径
            string score_text;
            // 显示得分和游戏进程信息
            if (!gameOver)
            {
                score_text = "Score: " + score.ToString() + " Game running";
                score_text += "\n W:go up A:go left S:go down D: go right";
            }
            else
                score_text = "Score: " + score.ToString() + " Game over";

            Brush brush = new SolidBrush(Color.White);  // 定义一个白色的画刷
            Font font = new Font("Arial", 12, FontStyle.Bold);
            // 添加文字信息描述得分
            e.Graphics.DrawString(score_text, font, brush, score_window.Location);
            //绘制一个圆形点
            e.Graphics.FillEllipse(brush, food.X - radius, food.Y - radius, diameter, diameter);
            // 绘制蛇的身体
            foreach (var body in snake.Body)
            {
                e.Graphics.FillEllipse(brush, body.X, body.Y, diameter, diameter);
            }
        }
        public void TimerStart()
        {
            timer.Interval = (int)timerInterval / speed;
            // 时钟滴答事件绑定游戏逻辑更新事件
            timer.Tick += this.Update;
            timer.Start();
        }

        private void GenerateFood()
        {
            int x = random.Next(0, gameBoard.Width / 10) * 10;
            int y = random.Next(0, gameBoard.Height / 10) * 10;
            food = new Point(x, y);
        }

        private void GenerateSnake()
        {
            snake.AddTail();
        }

        private void Update(object sender, EventArgs e)
        {
            var head = snake.Body[0];
            //吃到食物再刷新食物
            double distance = Math.Sqrt(Math.Pow(head.X - food.X, 2) + Math.Pow(head.Y - food.Y, 2));
            if (distance < 10)
            {
                score += 10;
                GenerateFood();
                GenerateSnake();
            }
            // 游戏没有结束时，画板更新
            if (!gameOver)
            {
                snake.Move();
                gameBoard.Invalidate();
            }
            if (GameOver())
            {
                gameOver = true;
            }
        }

        public bool GameOver()
        {
            Point head = snake.Body[0];

            // 实时计算蛇与四围墙壁之间的距离，并取最小值来判断
            double wall_left = Math.Sqrt(Math.Pow(head.X, 2));
            double wall_right = Math.Sqrt(Math.Pow(head.X - gameBoard.Width, 2));
            double wall_up = Math.Sqrt(Math.Pow(head.Y - gameBoard.Height, 2));
            double wall_down = Math.Sqrt(Math.Pow(head.Y, 2));
            double[] Distance_Array = { wall_left, wall_right, wall_up, wall_down };

            //身体碰撞检测
            if (snake.Iscollision())
            {
                Console.WriteLine("game over");
                return true;
            }
            //墙壁碰撞检测
            if (Distance_Array.Min() <= snake.Radious)
                return true;
            return false;
        }

        private void MoveControl(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                dir = Snake.Direction.Up;
                snake.ChangeDirection(dir);
            }
            if (e.KeyCode == Keys.A)
            {
                dir = Snake.Direction.Left;
                snake.ChangeDirection(dir);
            }
            if (e.KeyCode == Keys.S)
            {
                dir = Snake.Direction.Down;
                snake.ChangeDirection(dir);
            }
            if (e.KeyCode == Keys.D)
            {
                dir = Snake.Direction.Right;
                snake.ChangeDirection(dir);
            }
        }

    }
    #endregion


    #region 不再采用的game类
    //public class Game
    //{
    //    // 游戏容器
    //    private PictureBox gameBoard = new PictureBox();
    //    //食物随机生成器
    //    private Random random = new Random();
    //    //食物
    //    private Point food= new Point(50, 50);

    //    private Timer timer = new Timer(); 

    //    private bool gameOver = false;
    //    private int timerInterval = 1000;
    //    private int speed;
    //    public Snake snake = new Snake();

    //    private int score=0;

    //    public Game(int speed) {
    //        speed = speed;
    //    }

    //    public void UpdateScore(int score)
    //    {
    //        score = score;
    //    }

    //    public void Init(Form1 form) {
    //        gameBoard.Width = 100;
    //        gameBoard.Height = 100;
    //        gameBoard.Dock = DockStyle.Fill;
    //        gameBoard.BackColor = Color.LightCoral;
    //        form.Controls.Add(gameBoard);
    //        gameBoard.Paint += this.GameBoardPaint;
    //        gameBoard.Focus();
    //    }

    //    private void GameBoardPaint(object sender, PaintEventArgs e)
    //    {
    //        int radius = snake.GetRaduious();  // 定义点的半径
    //        int diameter = 2 * radius;  // 计算点的直径
    //        string score_text = "Score: " + score.ToString();

    //        Brush brush = new SolidBrush(Color.White);  // 定义一个白色的画刷

    //        //绘制一个圆形点
    //        e.Graphics.FillEllipse(brush, food.X - radius, food.Y - radius, diameter, diameter);

    //        foreach (var body in snake.GetBody())
    //        {
    //            e.Graphics.FillEllipse(brush, body.X, body.Y, diameter, diameter);
    //        }
    //    }

    //    private void GenerateFood()
    //    {
    //        int x = random.Next(0, gameBoard.Width / 10) * 10;
    //        int y = random.Next(0, gameBoard.Height / 10) * 10;
    //        food = new Point(x, y);
    //    }

    //    private void GenerateSnake()
    //    {
    //        snake.AddTail();
    //    }


    //    public void Start()
    //    {
    //        timer.Interval = (int)timerInterval / speed;
    //        timer.Tick += this.Update;
    //        timer.Start();
    //    }

    //    private void Update(object sender, EventArgs e)
    //    {
    //        var head = snake.GetBody()[0];
    //        //吃到食物再刷新食物
    //        double distance = Math.Sqrt(Math.Pow(head.X - food.X, 2)+ Math.Pow(head.Y - food.Y, 2));
    //        if(distance < 10)
    //        {
    //            score += 10;
    //            GenerateFood();
    //            GenerateSnake();
    //        }
    //        if(!gameOver)
    //        {
    //            snake.Move();
    //            gameBoard.Invalidate();
    //        }
    //        if (Stop())
    //        {
    //            gameOver = true;
    //        }
    //    }

    //    public bool Stop() {
    //        Point head = snake.GetBody()[0];
    //        double distance_wall = Math.Sqrt(Math.Pow(head.X, 2) + Math.Pow(head.Y, 2));
    //        //身体碰撞检测
    //        if (snake.Iscollision())
    //        {
    //            Console.WriteLine("game over");
    //            return true;
    //        }
    //        //墙壁碰撞检测
    //        if (distance_wall < snake.GetRaduious())
    //            return true;
    //        return false;
    //    }
    //}
    #endregion


}
