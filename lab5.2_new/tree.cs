using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab5._2_new
{
    public class tree
    {
        public double x;
        public double y;
        Random rnd;
        public List<tree> TreesInRadius;
        public int mutationChanse;
        public int lifeTime;
        public int age;
        public double fitness = int.MaxValue;
        public int radius;
        double H;
        double W;

        public tree(double x, double y, Random rnd, int mutationChanse, int lifeTime, int radius, double H, double W) 
        {
            this.x = x;
            this.y = y;
            this.rnd = rnd;
            this.mutationChanse = mutationChanse;
            this.lifeTime = lifeTime;
            this.radius = radius;
            this.H = H;
            this.W = W;
        }
        public void mutate()
        {
            bool intersecting = false;
            foreach (tree tree in TreesInRadius)
            {
                if (IsIntersecting(tree))
                {
                    intersecting = true;
                    break;
                }
            }
            if (!intersecting)
            {
                if (rnd.Next(100) < mutationChanse)
                {
                    if (rnd.Next(2) == 0)
                    {
                        if (rnd.Next(2) == 0)
                        {
                            double coef = 1 + (5 - 1) * rnd.NextDouble();
                            if (x + coef + radius > W) x = W - radius;
                            else x += 1 + (5 - 1) * rnd.NextDouble();
                        }
                        else
                        {
                            double coef = 1 + (5 - 1) * rnd.NextDouble();
                            if (x - coef < radius) x = radius;
                            else x -= 1 + (5 - 1) * rnd.NextDouble();
                        }
                    }
                    else
                    {
                        if (rnd.Next(2) == 0)
                        {
                            double coef = 1 + (5 - 1) * rnd.NextDouble();
                            if (y + coef + radius > H) y = H - radius;
                            else y += 1 + (5 - 1) * rnd.NextDouble();
                        }
                        else
                        {
                            double coef = 1 + (5 - 1) * rnd.NextDouble();
                            if (y - coef < radius) x = radius;
                            else y -= 1 + (5 - 1) * rnd.NextDouble();
                        }
                    }
                }
            }
        }

        public tree crossover(tree parent2)
        {
            double newX;
            double newY;

            if (rnd.Next(2) == 0) newX = x;
            else newX = parent2.x;
            if (rnd.Next(2) == 0) newY = y;
            else newY = parent2.y;

            int newlifetime;

            if (lifeTime > parent2.lifeTime) newlifetime = rnd.Next(parent2.lifeTime, lifeTime + 1);
            else if (lifeTime < parent2.lifeTime) newlifetime = rnd.Next(lifeTime, parent2.lifeTime + 1);
            else newlifetime = rnd.Next(lifeTime + 1);

            int mut;
            if (rnd.Next(2) == 0) mut = mutationChanse;
            else mut = parent2.mutationChanse;

            return new tree(newX, newY, rnd, mut, newlifetime, radius, H, W);
        }

        public bool IsIntersecting(tree t)
        {
            double distance = DistanceTo(t.x, t.y);
            if (distance == 0)
            {
                return true;
            }
            else if (distance < (radius * 2) + 5)
            {
                return true;
            }
            return false;

        }

        public double DistanceTo(double x1, double y1)
        {
            return Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2));
        }


    }
}
