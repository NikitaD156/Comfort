using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;

namespace Comfort
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ProductionViewModel();
        }

        
    }
}
