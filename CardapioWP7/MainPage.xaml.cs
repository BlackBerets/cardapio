﻿using System;
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
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            if (AntesDoAlmoco())
            {
                Jantar.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                Almoco.Visibility = Visibility.Collapsed;
            }
			StoryboardIn.Begin();
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

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Jantar.Visibility == System.Windows.Visibility.Collapsed)
            {
                HideAlmoco.Begin();
                ShowJantar.Begin();
            }
        }

        private void TextBlock_Tap_1(object sender, GestureEventArgs e)
        {
            if (Almoco.Visibility == Visibility.Collapsed)
            {
                HideJantar.Begin();
                ShowAlmoco.Begin();
            }
        }

        private bool AntesDoAlmoco()
        {
            bool manha = DateTime.Now.TimeOfDay < new DateTime(2000, 1, 1, 14, 0, 0).TimeOfDay;
            bool noite = DateTime.Now.TimeOfDay < new DateTime(2000, 1, 1, 19, 0, 0).TimeOfDay;
            return manha || noite;
                
        }
    }
}