using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CardapioWP7
{
    /// <summary>
    /// Helper para acessar e baixar a página da Web que contem as informações dos pratos
    /// </summary>
    public class WebUpdater
    {
        public string Info {get; private set;}
        private MainPage ParentPage;
        private string Periodo;

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

                this.Info = doc.DocumentNode.SelectNodes("//td[@class='verdana11']")[0].InnerText;
                this.Periodo = doc.DocumentNode.SelectNodes("//td[@class='verdana11']/span[@class='style3']")[0].InnerHtml;
            }
            else
            {
                ParentPage.Alert(e.Error.Message);
            }

        }

        /// <summary>
        /// Funcao auxiliar para quebrar a informação baixada do site para algo legível no app.
        /// </summary>
        /// <param name="_info">Html tag com as informações</param>
        public void ProcessInfo(string _info)
        {
            
        }

    }
}
