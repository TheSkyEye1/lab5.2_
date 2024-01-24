using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5._2_new
{
    public class tree
    {
        public double x;
        public double y;
        public double amount;
        Random rnd;
        public tree(double x, double y, Random rnd) 
        {
            this.x = x;
            this.y = y;
            this.rnd = rnd;
        }

        public void mutate()
        {
            if(rnd.Next(4) > 0)
            {
                if(rnd.Next(2) == 0)
                {
                    if(rnd.Next(2) == 0)
                    {
                        x += 1 + (5 - 1) * rnd.NextDouble();
                    }
                    else
                    {
                        x += 1 + (5 - 1) * rnd.NextDouble();
                    }
                }
                else
                {
                    if (rnd.Next(2) == 0)
                    {
                        y += 1 + (5 - 1) * rnd.NextDouble();
                    }
                    else
                    {
                        y += 1 + (5 - 1) * rnd.NextDouble();
                    }
                }
            }
        }



    }
}
