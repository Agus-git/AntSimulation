using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSimulation
{
    class World
    {
        private Random rnd = new Random();

        private const int width = 125;
        private const int height = 125;
        private Size size = new Size(width, height);
        private List<GameObject> objects = new List<GameObject>();
        private List<GameObject> comida = new List<GameObject>();
        
        public int GameObjects { get { return objects.Count + comida.Count; } }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public PointF Center { get { return new PointF(width / 2, height / 2); } }

        public bool IsInside(PointF p)
        {
            return p.X >= 0 && p.X < width
                && p.Y >= 0 && p.Y < height;
        }
        
        public PointF RandomPoint()
        {
            return new PointF(rnd.Next(width), rnd.Next(height));
        }

        public float Random()
        {
            return (float)rnd.NextDouble();
        }

        public float Random(float min, float max)
        {
            return (float)rnd.NextDouble() * (max - min) + min;
        }

        public void Add(Food obj)
        {
            comida.Add(obj);
        }

        public void Remove(Food obj)
        {
            comida.Remove(obj);
        }

        public void Add(Pheromone obj)
        {
            comida.Add(obj);
        }

        public void Remove(Pheromone obj)
        {
            comida.Remove(obj);
        }

        public void Add(GameObject obj)
        {
            objects.Add(obj);
        }

        public void Remove(GameObject obj)
        {
            objects.Remove(obj);
        }

        public void Update()
        {
            foreach (var item in comida.ToArray())
            {
                item.InternalUpdateOn(this);
                item.Position = Mod(item.Position, size);
            }
            foreach (GameObject obj in objects.ToArray())
            {
                obj.InternalUpdateOn(this);
                obj.Position = Mod(obj.Position, size);
            }
        }

        public void DrawOn(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.White, 0, 0, width, height);
            foreach (GameObject obj in objects.ToArray())
            {
                graphics.FillRectangle(new Pen(obj.Color).Brush, obj.Bounds);
            }
            foreach (GameObject item in comida)
            {
                graphics.FillRectangle(new Pen(item.Color).Brush, item.Bounds);
            }
        }

        public double Dist(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public double Dist(float x1, float y1, float x2, float y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        // http://stackoverflow.com/a/10065670/4357302
        private static float Mod(float a, float n)
        {
            float result = a % n;
            if ((a < 0 && n > 0) || (a > 0 && n < 0))
                result += n;
            return result;
        }
        private static PointF Mod(PointF p, SizeF s)
        {
            return new PointF(Mod(p.X, s.Width), Mod(p.Y, s.Height));
        }
        
        public GameObject[] GameObjectsNear(PointF pos,int radio)
        {
            List<GameObject> temp = new List<GameObject>();
            Point ubicacion = new Point(0, 0);
            ubicacion.X = int.Parse(pos.X.ToString());
            ubicacion.Y = int.Parse(pos.Y.ToString());
            byte contador = 0;
            GameObject[] miComida = comida.ToArray();

            for (int x = ubicacion.X - radio; x < ubicacion.X + radio; x++)
            {
                for (int y = ubicacion.Y - radio; y < ubicacion.Y + radio; y++)
                {
                    foreach (var item in miComida)
                    {
                        if (pos == item.Position)
                        {
                            temp.Add(item);
                        }
                    }
                    contador++;
                }
            }
            return temp.ToArray();
        }

    }
}
