using System.Windows;
using Vintage.Backend;

namespace Vintage.Metro.Controls.PageTemplates
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
	public partial class StartPage : IPage
    {
        public StartPage()
        {
            InitializeComponent();

            // Save the Start Page
            Settings.StartPage = this;

            // Setup the UI
            mainContent.Visibility = Visibility.Visible;
        }
        public bool Close() { return true; }

        private void btnOpenLocalFilm_Click(object sender, RoutedEventArgs e)
        {
            Settings.HomeWindow.AddFilm();
        }
    }
}
