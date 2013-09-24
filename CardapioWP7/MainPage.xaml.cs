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

namespace CardapioWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        public WebUpdater updater { get; private set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            updater = new WebUpdater(this);

            // Set the data context of the listbox control to the sample data
            //DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!App.ViewModel.IsDataLoaded)
            //{
            //    App.ViewModel.LoadData();
            //}
			StoryboardIn.Begin();

            updater.GetUpdates();
            
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
            ph.ProcessInfo(updater.Info, updater.Periodo);

            foreach (var Dia in ph.Semana)
            {
                PivotItem aba = new PivotItem();
                aba.Header = Dia.Data.DayOfWeek;

                ScrollViewer sv_dia = new ScrollViewer();

                StackPanel stack_refeicoes = new StackPanel();
                sv_dia.Content = stack_refeicoes;

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
    }
}