using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.ComponentModel;
using Microsoft.Phone.Shell;
using Microsoft.Advertising.Mobile.UI;

namespace CardapioWP7
{


    public partial class MainPage : PhoneApplicationPage
    {
        public WebUpdater wupdater { get; private set; }
        private ProgressIndicator Pontinhos;
        public bool FirstLoad;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            wupdater = new WebUpdater(this);
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            Pontinhos = new ProgressIndicator();
            Pontinhos.Text = "Carregando...";
            Pontinhos.IsIndeterminate = true;
            SystemTray.SetProgressIndicator(this, Pontinhos);

            FirstLoad = true;

            NavigationInTransition navigateInTransition = new NavigationInTransition();
            navigateInTransition.Backward = new TurnstileTransition { Mode = TurnstileTransitionMode.BackwardIn };
            navigateInTransition.Forward = new TurnstileTransition { Mode = TurnstileTransitionMode.ForwardIn };

            NavigationOutTransition navigateOutTransition = new NavigationOutTransition();
            navigateOutTransition.Backward = new TurnstileTransition { Mode = TurnstileTransitionMode.BackwardOut };
            navigateOutTransition.Forward = new TurnstileTransition { Mode = TurnstileTransitionMode.ForwardOut };
            TransitionService.SetNavigationInTransition(this, navigateInTransition);
            TransitionService.SetNavigationOutTransition(this, navigateOutTransition);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (FirstLoad)
            {
                Pontinhos.IsVisible = true;
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                    if (isf.FileExists("pratos.txt"))
                        wupdater.Load();
                    else
                        wupdater.GetUpdates();
                FirstLoad = false;
            }
            GoToStateDefault();
        }

        public void GoToStateDefault()
        {
            Pontinhos.IsVisible = false;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
        }

        private void BotaoAtualizar_Click(object sender, System.EventArgs e)
        {
            (sender as ApplicationBarIconButton).IsEnabled = false;
            Pontinhos.IsVisible = true;
            wupdater.GetUpdates();
        }

        private void MenuItemSobre_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Sobre.xaml", UriKind.Relative));
        }

        private void MenuItemHorarios_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Horarios.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Funcao auxiliar para facilitar a apresentação de mensagens de alerta nessa página
        /// </summary>
        /// <param name="msg">Mensagem a ser mostrada</param>
        public void Alert(string msg)
        {
            MessageBox.Show(msg);
        }

        internal void LoadInfo()
        {

            ProcessHelper ph = new ProcessHelper();
            ph.ProcessInfo(wupdater.Info, wupdater.Periodo);

            pivot.Items.Clear();
            foreach (var Dia in ph.Semana)
            {
                PivotItem aba = new PivotItem
                {
                    Header = Dia.NomeDoDia()
                };

                ScrollViewer sv_dia = new ScrollViewer();

                StackPanel stack_refeicoes = new StackPanel();
                sv_dia.Content = stack_refeicoes;

                TextBlock data = new TextBlock
                {
                    Text = string.Format("Dia {0}/{1}", Dia.Data.Day, Dia.Data.Month)
                };
                stack_refeicoes.Children.Add(data);

                if (!String.IsNullOrEmpty(Dia.Desjejum))
                    AdicionaRefeicao(Dia, stack_refeicoes, Refeicao.Desjejum);

                if (!String.IsNullOrEmpty(Dia.Almoco))
                    AdicionaRefeicao(Dia, stack_refeicoes, Refeicao.Almoço);

                if (!String.IsNullOrEmpty(Dia.Jantar))
                    AdicionaRefeicao(Dia, stack_refeicoes, Refeicao.Jantar);


                sv_dia.Style = (Style)App.Current.Resources["AbaDiaDaSemana"];
                aba.Content = sv_dia;

                pivot.Items.Add(aba);

                if (Dia.Data.DayOfWeek == DateTime.Today.DayOfWeek)
                {
                    pivot.SelectedItem = aba;

                    AdControl ad = new AdControl("74e1a034-5680-4210-808c-52463e8bbcf1", "162701", true)
                    {
                        IsAutoCollapseEnabled = true,
                        Margin = new Thickness(12),
                        Width = 300,
                        Height = 50
                    };

#if DEBUG
                    ad.ErrorOccurred += ad_ErrorOccurred;
#endif

                    stack_refeicoes.Children.Add(ad);
                }
            }
        }

#if DEBUG
        void ad_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {

            MessageBox.Show(e.Error.Message);
        }
#endif

        private static void AdicionaRefeicao(CardapioWP7.Dia Dia, StackPanel stack_refeicoes, Refeicao refeicao)
        {
            TextBlock title = new TextBlock();
            title.FontSize = 32;

            TextBlock pratos = new TextBlock();

            switch (refeicao)
            {
                case Refeicao.Desjejum:
                    title.Text = "Desjejum";
                    pratos.Text = Dia.Desjejum;
                    break;
                case Refeicao.Almoço:
                    title.Text = "Almoço";
                    pratos.Text = Dia.Almoco;
                    break;
                case Refeicao.Jantar:
                    title.Text = "Jantar";
                    pratos.Text = Dia.Jantar;
                    break;
                default:
                    break;
            }

            stack_refeicoes.Children.Add(title);
            stack_refeicoes.Children.Add(pratos);
        }


    }
}