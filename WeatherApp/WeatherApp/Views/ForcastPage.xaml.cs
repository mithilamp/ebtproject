using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForcastPage : ContentPage
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ForcastPage"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public ForcastPage (ForcastViewModel viewModel)
		{
			InitializeComponent();
            this.BindingContext = viewModel;
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = this.BindingContext as ForcastViewModel;
            viewModel.SubscribeToUpdates();
        }
    }
}