using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using System.Xml.Linq;

namespace lab5._2_new
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<tree> Trees = new List<tree>();
        int startPop = 10;
        Random rnd = new Random();
        int radius = 30;
        int maxMutChanse = 85;
        int minMutChanse = 25;
        int maxLifeTime = 5;
        int minLifeTime = 2;
        int radiusCoef = 2;
        int treesInRadiusMin = 3;
        int iteration = 0;
        int unchangedIteration = 0;
        int treeCount = 0;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
        }

        public void CreatePopulation()
        {
            for (int i = 0; i < startPop; i++)
            {
                double x = radius + (scene.Width - radius) * rnd.NextDouble();
                double y = radius + (scene.Height - radius) * rnd.NextDouble();

                List<tree> tr = new List<tree>();
                tr.AddRange(Trees);
                Trees.Add(new tree(x, y, rnd, rnd.Next(minMutChanse, maxMutChanse), rnd.Next(minLifeTime, maxLifeTime), radius, scene.Height, scene.Width));
                Trees[i].age = 0;
                foreach (tree t in tr)
                {
                    if (Trees[i].IsIntersecting(t))
                    {
                        Trees.Remove(Trees[i]);
                        i--;
                        break;
                    }
                }

            }

            treeCount = Trees.Count;
        }

        public void CalcilateFitness()
        {
            foreach (tree t in Trees)
            {
                t.fitness = int.MaxValue;
                t.TreesInRadius = new List<tree>();
            }

            for (int i = 0; i < Trees.Count; i++)
            {
                List<tree> treesinradius = new List<tree>();

                for (int j = 0; j < Trees.Count; j++)
                {
                    if (j != i && !Trees[i].TreesInRadius.Contains(Trees[j]))
                    {
                        double dist = Trees[i].DistanceTo(Trees[j].x, Trees[j].y);
                        if (dist <= radius * radiusCoef)
                        {
                            Trees[i].fitness += dist;
                            Trees[j].fitness += dist;
                            Trees[i].TreesInRadius.Add(Trees[j]);
                            Trees[j].TreesInRadius.Add(Trees[i]);
                        }
                    }
                }
            }
        }

        public void NextGeneration()
        {
            List<tree> parents = new List<tree>();

            for (int i = 0; i < Trees.Count / 2 + 2; i++)
            {
                int id = rnd.Next(Trees.Count);

                if (parents.Contains(Trees[id])) i--;
                else parents.Add(Trees[id]);
            }

            parents.Sort((x, y) => x.fitness.CompareTo(y.fitness));

            while (parents.Count > 1)
            {
                int index1 = rnd.Next(parents.Count);
                int index2 = index1;
                while (index1 == index2)
                {
                    index2 = rnd.Next(parents.Count);
                }
                tree parent1 = parents[index1];
                tree parent2 = parents[index2];

                tree children = parent1.crossover(parent2);
                children.TreesInRadius = new List<tree>();

                bool intersect = false;

                foreach (tree t in Trees)
                {
                    if (children.IsIntersecting(t))
                    {
                        intersect = true;
                        break;
                    }
                }

                if (!intersect) Trees.Add(children);

                parents.Remove(parent2);
                parents.Remove(parent1);
            }

            for (int i = 0; i < Trees.Count; i++)
            {
                Trees[i].mutate();

                if (Trees[i].TreesInRadius.Count < treesInRadiusMin)
                {
                    Trees[i].age += 1;
                }

                if (Trees[i].age == Trees[i].lifeTime)
                {
                    Trees.Remove(Trees[i]);
                }
            }

            if (Trees.Count < startPop)
            {
                for (int i = Trees.Count; i < startPop + 1; i++)
                {
                    double x = radius + (scene.Width - radius) * rnd.NextDouble();
                    double y = radius + (scene.Height - radius) * rnd.NextDouble();

                    List<tree> tr = new List<tree>();
                    tr.AddRange(Trees);
                    Trees.Add(new tree(x, y, rnd, rnd.Next(minMutChanse, maxMutChanse), rnd.Next(minLifeTime, maxLifeTime), radius, scene.Height, scene.Width));
                    Trees[i].age = 0;
                    foreach (tree t in tr)
                    {
                        if (Trees[i].IsIntersecting(t))
                        {
                            Trees.Remove(Trees[i]);
                            i--;
                            break;
                        }
                    }
                }
            }

        }

        public void DrawCircle(double x, double y)
        {
            Ellipse el = new Ellipse();
            SolidColorBrush cb = new SolidColorBrush();
            cb.Color = Color.FromArgb(255, 0, 255, 0);
            el.Fill = cb;
            el.StrokeThickness = 0;
            el.Stroke = Brushes.Black;
            el.Width = radius;
            el.Height = radius;
            el.RenderTransform = new TranslateTransform(x - radius / 2, y - radius / 2);
            scene.Children.Add(el);
        }

        public void DrawScene()
        {
            scene.Children.Clear();
            foreach (tree t in Trees)
            {
                DrawCircle(t.x, t.y);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Trees.Count == 0)
            {
                Trees = new List<tree>();
                CreatePopulation();
                DrawScene();
                timer.Start();
            }
            else
            {
                if (timer.IsEnabled) timer.Stop();
                else timer.Start();
            }
        }

        public void AddNewPop()
        {
            for (int i = Trees.Count; i < 5; i++)
            {
                double x = radius + (scene.Width - radius) * rnd.NextDouble();
                double y = radius + (scene.Height - radius) * rnd.NextDouble();

                List<tree> tr = new List<tree>();
                tr.AddRange(Trees);
                Trees.Add(new tree(x, y, rnd, rnd.Next(minMutChanse, maxMutChanse), rnd.Next(minLifeTime, maxLifeTime), radius, scene.Height, scene.Width));
                Trees[i].age = 0;
                foreach (tree t in tr)
                {
                    if (Trees[i].IsIntersecting(t))
                    {
                        Trees.Remove(Trees[i]);
                        break;
                    }
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            iteration++;

            CalcilateFitness();
            NextGeneration();

            if(treeCount == Trees.Count)
            {
                unchangedIteration++;
                if(unchangedIteration == 15)
                {
                    AddNewPop();
                }
            }
            else
            {
                treeCount = Trees.Count;
                unchangedIteration = 0;
            }

            DrawScene();

            treesLB.Content = "Trees: " + Trees.Count;
            iterLB.Content = "Iteration: " + iteration;
            unchitLB.Content = "Unchanged Iter: " + unchangedIteration;
        }
    }
}
