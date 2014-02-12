using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Reflection;

namespace CardapioWP7
{
    public partial class Sobre : PhoneApplicationPage
    {
        public Sobre()
        {
            InitializeComponent();

            Versao.Text = "Versão 1.0.1";

            NavigationInTransition navigateInTransition = new NavigationInTransition();
            navigateInTransition.Backward = new TurnstileTransition { Mode = TurnstileTransitionMode.BackwardIn };
            navigateInTransition.Forward = new TurnstileTransition { Mode = TurnstileTransitionMode.ForwardIn };

            NavigationOutTransition navigateOutTransition = new NavigationOutTransition();
            navigateOutTransition.Backward = new TurnstileTransition { Mode = TurnstileTransitionMode.BackwardOut };
            navigateOutTransition.Forward = new TurnstileTransition { Mode = TurnstileTransitionMode.ForwardOut };
            TransitionService.SetNavigationInTransition(this, navigateInTransition);
            TransitionService.SetNavigationOutTransition(this, navigateOutTransition);
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            wbt.Uri = new Uri("https://github.com/BlackBerets/Cardapio");
            wbt.Show();
        }

        private void SaeLink_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            wbt.Uri = new Uri("http://www.sae.ufrn.br/conteudo/servicos/ru/cardapio.php");
            wbt.Show();
        }
    }
}