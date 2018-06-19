﻿using ProMama.ViewModels.Home.Paginas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProMama.Views.Home.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostosSaudeView : ContentPage
    {
        public PostosSaudeView()
        {
            InitializeComponent();

            BindingContext = new PostosSaudeViewModel();
        }
    }
}