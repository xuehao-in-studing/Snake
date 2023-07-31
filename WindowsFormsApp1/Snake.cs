using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    #region 贪吃蛇类，用户操作对象
    public class Snake
    {
        #region 属性
        //定义身体
        private List<Point> body = new List<Point>();
        //定义每个小块的大小
        private int radious = 5;
        //定义运动方向enum
        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        private Direction dir;

        public List<Point> Body { get => body;private set => body = value; }
        public int Radious 
        { 
            get => radious;
            set 
            {
                if (value > 0)
                    radious = value;
                else
                    throw new Exception("radious value error,must be int and above 0");
            } 
        }

        public Direction Dir { get => dir; set => dir = value; }
        #endregion
        // 🐍🐍初始化，位置位于(0,0),方向是向右
        public Snake()
        {
            Body.Add(new Point(10, 10));
            Dir = Direction.Right;
        }

        public Snake(Point StartPoint, Direction Direction, int BodyRadious)
        {
            Body.Add(StartPoint);
            Dir = Direction;
            Radious = BodyRadious;
        }

        public void ChangeDirection(Direction Direction)
        {
            Dir = Direction;
        }

        /// <summary>
        /// 移动控制，键盘操作
        /// </summary>
        public void Move()
        {
            int next_x, next_y;
            switch (Dir)
            {
                case Direction.Left:
                    next_x = -Radious; next_y = 0; break;
                case Direction.Right:
                    next_x = Radious; next_y = 0; break;
                case Direction.Up:
                    next_x = 0; next_y = -Radious; break;
                case Direction.Down:
                    next_x = 0; next_y = Radious; break;
                default:
                    next_x = 0; next_y = 0; break;
            }
            Point NewPoint = new Point(Body[0].X + next_x, Body[0].Y + next_y);
            // 更新🐍🐍的身体，加头去尾来更新，头部引领身体的运动
            Body.Insert(0, NewPoint);
            Body.RemoveAt(Body.Count - 1);
        }

        /// <summary>
        /// 吃到食物，尾部加点，得分
        /// </summary>
        public void AddTail()
        {
            // 在尾部添加一个新的点
            // 这个点的圆心在上一个点的边上
            int next_x, next_y;
            switch (Dir)
            {
                case Direction.Left:
                    next_x = -Radious; next_y = 0; break;
                case Direction.Right:
                    next_x = Radious; next_y = 0; break;
                case Direction.Up:
                    next_x = 0; next_y = -Radious; break;
                case Direction.Down:
                    next_x = 0; next_y = Radious; break;
                default:
                    next_x = 0; next_y = 0; break;
            }
            Point NewPoint = new Point(Body[Body.Count - 1].X + next_x, Body[Body.Count - 1].Y + next_y);
            Body.Add(NewPoint);
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <returns>bool 是否碰撞</returns>
        public bool Iscollision()
        {
            Point head = Body[0];
            //检测是否头部碰撞到身体
            for (int i = 2; i < Body.Count; i++)
            {
                double distance = Math.Sqrt(Math.Pow(head.X - Body[i].X, 2) + Math.Pow(head.Y - Body[i].Y, 2));
                if (distance + 0.01 < Math.Sqrt(Math.Pow(Radious, 2) * 2))
                {
                    return true;
                }
            }
            return false;
        }
    }
    #endregion
}
