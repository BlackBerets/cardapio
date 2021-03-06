﻿using HtmlAgilityPack;
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

                doc.DocumentNode.SelectNodes("//td[@class='verdana11']/span[@class='style2']")[0].Remove();
                doc.DocumentNode.SelectNodes("//td[@class='verdana11']/span[@class='style3']")[0].Remove();

                this.Info = doc.DocumentNode.SelectNodes("//td[@class='verdana11']")[0].InnerHtml;

                this.ParentPage.LoadInfo();

                this.SaveToIsolatedStorage();
            }
            else
            {
                ParentPage.Alert("Não foi possível carregar as informações. Por favor conecte-se à internet e tente novamente.");
                ParentPage.GoToStateDefault();
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

        /// <summary>
        /// Metodo auxiliar que salva os dados em disco
        /// </summary>
        public void SaveToIsolatedStorage()
        {
            using (IsolatedStorageFile Storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (StreamWriter PratosWriter = new StreamWriter(new IsolatedStorageFileStream("pratos.txt", FileMode.OpenOrCreate, Storage)))
                    PratosWriter.Write(this.Info);

                this.ParentPage.GoToStateDefault();
            }
        }

        /// <summary>
        /// Metodo auxiliar que carrega os dados salvos em disco
        /// </summary>
        public void Load()
        {
            using (IsolatedStorageFile Storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (StreamReader PratosReader = new StreamReader(new IsolatedStorageFileStream("pratos.txt", FileMode.OpenOrCreate, Storage)))
                    this.Info = PratosReader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(this.Info))
                ParentPage.Alert("Não foi possível carregar as informações. Por favor conecte-se à internet e tente novamente.");
            else
                this.ParentPage.LoadInfo();
            this.ParentPage.GoToStateDefault();
        }
    }
}
