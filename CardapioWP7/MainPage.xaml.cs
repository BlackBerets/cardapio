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

namespace CardapioWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        public WebUpdater wupdater { get; private set; }
        private ProgressIndicator Pontinhos;

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
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            StoryboardIn.Begin();

            Pontinhos.IsVisible = true;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                if (isf.FileExists("pratos.txt"))
                    wupdater.Load();
                else
                    wupdater.GetUpdates();
            GoToStateDefault();
        }

        public void GoToStateDefault()
        {
            Pontinhos.IsVisible = false;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;     
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            StoryboardOut.Begin();
            StoryboardOut.Completed += new System.EventHandler(StoryboardOut_Completed);
        }

        private void StoryboardOut_Completed(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Sobre.xaml", UriKind.Relative));
        }

        private bool AntesDoAlmoco()
        {
            bool manha = DateTime.Now.TimeOfDay < new DateTime(2000, 1, 1, 14, 0, 0).TimeOfDay;
            bool noite = DateTime.Now.TimeOfDay < new DateTime(2000, 1, 1, 19, 0, 0).TimeOfDay;
            return manha || noite;

        }

        /// <summary>
        /// Funcao auxiliar para facilitar a apresentação de mensagens de alerta nessa página
        /// </summary>
        /// <param name="msg">Mensagem a ser mostrada</param>
        public void Alert(string msg)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(msg));
        }

        internal void LoadInfo()
        {

            ProcessHelper ph = new ProcessHelper();
            ph.ProcessInfo(wupdater.Info, wupdater.Periodo);

            pivot.Items.Clear();
            foreach (var Dia in ph.Semana)
            {
                PivotItem aba = new PivotItem();
                aba.Header = Dia.NomeDoDia();

                ScrollViewer sv_dia = new ScrollViewer();

                StackPanel stack_refeicoes = new StackPanel();
                sv_dia.Content = stack_refeicoes;

                TextBlock data = new TextBlock();
                data.Text = string.Format("Dia {0}/{1}", Dia.Data.Day, Dia.Data.Month);
                //data.FontSize = 12;
                stack_refeicoes.Children.Add(data);

                TextBlock title_almoco = new TextBlock();
                title_almoco.Text = "Almoço";
                title_almoco.FontSize = 32;
                stack_refeicoes.Children.Add(title_almoco);

                TextBlock almoco = new TextBlock();
                almoco.Text = Dia.Almoco;
                stack_refeicoes.Children.Add(almoco);

                TextBlock title_jantar = new TextBlock();
                title_jantar.Text = "Jantar";
                title_jantar.FontSize = 32;
                stack_refeicoes.Children.Add(title_jantar);

                TextBlock jantar = new TextBlock();
                jantar.Text = Dia.Jantar;
                stack_refeicoes.Children.Add(jantar);

                aba.Style = (Style)App.Current.Resources["AbaDiaDaSemana"];
                aba.Content = sv_dia;

                pivot.Items.Add(aba);

                if (Dia.Data.DayOfWeek == DateTime.Today.DayOfWeek)
                    pivot.SelectedItem = aba;
            }
        }

        private void BotaoAtualizar_Click(object sender, System.EventArgs e)
        {
            (sender as ApplicationBarIconButton).IsEnabled = false;
            Pontinhos.IsVisible = true;
            wupdater.GetUpdates();   
        }
    }
}