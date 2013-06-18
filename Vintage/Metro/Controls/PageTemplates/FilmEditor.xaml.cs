using Vintage.Films;
namespace Vintage.Metro.Controls.PageTemplates
{
    /// <summary>
    /// Interaction logic for FilmEditor.xaml
    /// </summary>
    public partial class FilmEditor : IPage
    {
        public FilmEditor(string filePath)
        {
            InitializeComponent();

            var film = new Halo4Film();
            film.Load(filePath);

            DataContext = film;
        }

        public bool Close()
        {
            return true;
        }
    }
}
