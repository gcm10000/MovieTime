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

namespace MovieTimeCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListViewProduct.ItemsSource = GetMovies();
        }
        public List<MovieInList> GetMovies()
        {
            return new List<MovieInList>()
            {
                new MovieInList("John Wick 1", "", "2020"),
                new MovieInList("John Wick 2", "", "2020"),
                new MovieInList("John Wick 3", "", "2020"),
                new MovieInList("Vingadores", "", "2019"),
                new MovieInList("Batman: Cavaleiro das Trevas", "", "2008")
            };
        }
    }
}
