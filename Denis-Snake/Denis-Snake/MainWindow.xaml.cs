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

namespace Denis_Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dt;
        List<Snake> Snakey;
        List<Food> Foods;
        Random rd = new Random();
        double x = 200;
        double y = 200;
        int direction = 0;
        int left = 4;
        int right = 6;
        int up = 8;
        int down = 2;
        int Score = 0;
        int Count = 0;
        public MainWindow()
        {
            InitializeComponent();
            dt = new DispatcherTimer();
            Snakey = new List<Snake>();
            Foods = new List<Food>();
            Snakey.Add(new Snake(x, y));
            Foods.Add(new Food(rd.Next(0, 39) * 10, rd.Next(0, 39) * 10));
            dt.Interval = new TimeSpan(0, 0, 0, 0, 40);
            dt.Tick += time_Tick;
            
        }

        void addFoodToCanvas()
        {
            Foods[0].setfoodposition();
            Canvas.Children.Insert(0, Foods[0].ell);
        }

        void addSnakeToCanvas()
        {
            foreach(Snake snake in Snakey)
            {
                snake.setsnakeposition();
                Canvas.Children.Add(snake.rec);
            }
        }

        void time_Tick(object sender, EventArgs e)
        {
            if (direction != 0)
            {
                for(int i=Snakey.Count-1;i>0;i--)
                {
                    Snakey[i] = Snakey[i - 1];
                }
            }

            if (direction == up)
                y -= 10;
            if (direction == down)
                y += 10;
            if (direction == left)
                x -= 10;
            if (direction == right)
                x += 10;

            if(Snakey[0].x==Foods[0].x && Snakey[0].y == Foods[0].y)
            {
                Snakey.Add(new Snake(Foods[0].x, Foods[0].y));
                Foods[0]=new Food(rd.Next(0, 39) * 10, rd.Next(0, 39) * 10);
                Canvas.Children.RemoveAt(0);
                addFoodToCanvas();
                Score++;
                txtScore.Text = Score.ToString();
            }

            Snakey[0] = new Snake(x, y);

            if(Snakey[0].x>390|| Snakey[0].y > 390|| Snakey[0].x < 0|| Snakey[0].y < 0)
            {
                MessageBoxResult GameOver = MessageBox.Show("Game over\nYour score is: " + Score);
                if (GameOver == MessageBoxResult.OK)
                    this.Close();
            }

            for(int i = 1; i < Snakey.Count; i++)
            {
                if (Snakey[0].x == Snakey[i].x && Snakey[0].y == Snakey[i].y)
                {
                    MessageBoxResult GameOver = MessageBox.Show("Game over\nYour score is: " + Score);
                    if (GameOver == MessageBoxResult.OK)
                        this.Close();
                }
            }

            for(int i = 0; i < Canvas.Children.Count; i++)
            {
                if (Canvas.Children[i] is Rectangle)
                    Count++;
            }
            Canvas.Children.RemoveRange(1, Count);
            Count = 0;
            addSnakeToCanvas();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && direction != down)
                direction = up;
            if (e.Key == Key.Down && direction != up)
                direction = down;
            if (e.Key == Key.Left && direction != right)
                direction = left;
            if (e.Key == Key.Right && direction != left)
                direction = right;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            addSnakeToCanvas();
            addFoodToCanvas();
            dt.Start();
        }


    }
}
