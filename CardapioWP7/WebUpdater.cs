using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO.IsolatedStorage;
using System.IO;

namespace CardapioWP7
{
    /// <summary>
    /// Helper para acessar e baixar a página da Web que contem as informações dos pratos
    /// </summary>
    public class WebUpdater
    {
        public string Info { get; private set; }
        public string Periodo { get; private set; }
        private MainPage ParentPage;


        /// <summary>
        /// Construtor da Classe. Recebe a página pai para poder chamar funções de maneira assíncrona
        /// </summary>
        /// <param name="_parentPage">Página pai</param>
        public WebUpdater(MainPage _parentPage)
        {
            Info = "";
            this.ParentPage = _parentPage;
        }


        public void GetUpdates()
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
            wc.DownloadStringAsync(new Uri("http://www.sae.ufrn.br/conteudo/servicos/ru/cardapio.php"));
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(e.Result);

                this.Periodo = doc.DocumentNode.SelectNodes("//td[@class='verdana11']/span[@class='style3']")[0].InnerHtml;

                doc.DocumentNode.SelectNodes("//td[@class='verdana11']/span[@class='style2']")[0].Remove();
                doc.DocumentNode.SelectNodes("//td[@class='verdana11']/span[@class='style3']")[0].Remove();

                this.Info = doc.DocumentNode.SelectNodes("//td[@class='verdana11']")[0].InnerHtml;

                this.SaveToIsolatedStorage();

                this.ParentPage.LoadInfo();
            }
            else
            {
                this.Load();
                if (string.IsNullOrEmpty(this.Info) || string.IsNullOrEmpty(this.Periodo))
                    ParentPage.Alert("Não foi possível carregar as informações. Por favor conecte-se à internet e tente novamente.");
                else
                {
                    this.ParentPage.LoadInfo();
                }
            }

        }

        /// <summary>
        /// Funcao auxiliar para quebrar a informação baixada do site para algo legível no app.
        /// </summary>
        /// <param name="_info">Html tag com as informações</param>
        public void ProcessInfo()
        {
            ProcessHelper ph = new ProcessHelper();
            ph.ProcessInfo(this.Info, this.Periodo);
        }

        public void SaveToIsolatedStorage()
        {
            using (IsolatedStorageFile Storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (StreamWriter PratosWriter = new StreamWriter(new IsolatedStorageFileStream("files\\pratos.txt", FileMode.Truncate, Storage)))
                    PratosWriter.Write(this.Info);

                using (StreamWriter PeriodoWriter = new StreamWriter(new IsolatedStorageFileStream("files\\periodo.txt", FileMode.Truncate, Storage)))
                    PeriodoWriter.Write(this.Periodo);
            }
        }


        internal void Load()
        {
            using (IsolatedStorageFile Storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (StreamReader PratosReader = new StreamReader(new IsolatedStorageFileStream("files\\pratos.txt", FileMode.OpenOrCreate, Storage)))
                    this.Info = PratosReader.ReadToEnd();
                using (StreamReader PeriodoReader = new StreamReader(new IsolatedStorageFileStream("files\\periodo.txt", FileMode.OpenOrCreate, Storage)))
                    this.Periodo = PeriodoReader.ReadToEnd();
            }
        }
    }
}
