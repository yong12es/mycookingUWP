﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            this.InitializeComponent();
        }
        private void RecetasMundoButton_Click(object sender, RoutedEventArgs e)
        {
            // Navega hacia la página de recetas del mundo
            MainContentFrame.Navigate(typeof(RecetasMundoPag));
        }
        private void InicioButton_Click(object sender, RoutedEventArgs e)
        {
            // Navega hacia la página de recetas del mundo
            MainContentFrame.Navigate(typeof(InicioPage));
        }
        private void FiltrarIngredientesButton_Click(object sender, RoutedEventArgs e)
        {
            // Navega hacia la página de recetas del mundo
            MainContentFrame.Navigate(typeof(FiltrarIngredientesPage));
        }
        private void ListaTalleresButton_Click(object sender, RoutedEventArgs e)
        {
            // Navega hacia la página de recetas del mundo
            MainContentFrame.Navigate(typeof(ListaTalleresPage));
        }
        private void ListaCompraButton_Click(object sender, RoutedEventArgs e)
        {
            // Navega hacia la página de recetas del mundo
            MainContentFrame.Navigate(typeof(RecetasMundoPag));
        }
        private void CrearRecetaButton_Click(object sender, RoutedEventArgs e)
        {
            // Navega hacia la página de recetas del mundo
            MainContentFrame.Navigate(typeof(RecetasMundoPag));
        }

        
    }
}
