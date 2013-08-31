using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Reflection;

namespace CardapioWP7
{
    public partial class Sobre : PhoneApplicationPage
    {
        public Sobre()
        {
            InitializeComponent();

            Versao.Text = "Versão " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}