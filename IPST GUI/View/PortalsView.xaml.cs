﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using IPST_Engine;
using IPST_GUI.Message;
using IPST_GUI.ViewModel;
using NHibernate.Linq;
using NHibernate.Util;

namespace IPST_GUI.View
{
    /// <summary>
    /// Description for PortalsView.
    /// </summary>
    public partial class PortalsView : Window
    {
            PortalsViewModel _viewModel;
        /// <summary>
        /// Initializes a new instance of the PortalsView class.
        /// </summary>
        public PortalsView(IList<PortalSubmission> portalSubmissions)
        {
            InitializeComponent();
            
            _viewModel = DataContext as PortalsViewModel;
            ViewModelLocator locator = FindResource("Locator") as ViewModelLocator;
            _viewModel.PortalSubmissions = portalSubmissions.ToList();
            //portalSubmissions.ForEach(p => viewModel.PortalSubmissions.Add(new PortalViewModel(p, locator.PortalSubmissionRepository)));

            Messenger.Default.Register<MessageNavigatePortal>(this, NotificationMessageReceived);
            Messenger.Default.Register<MessageManualAcceptPortal>(this, NavigateManualAcceptPortal);
        }

        private void NavigateManualAcceptPortal(MessageManualAcceptPortal obj)
        {
            ViewModelLocator.RegisterInstance(obj.PortalSubmission);
            var manualAcceptView = new ManualAcceptView();
            manualAcceptView.ShowDialog();
            //ManualAcceptViewModel viewModel
        }

        private void NotificationMessageReceived(MessageNavigatePortal obj)
        {
            var portalView = new PortalView();
            portalView.DataContext = obj.ViewModel;
            portalView.ShowDialog();
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UIElement elem = (UIElement)PortalListView.InputHitTest(e.GetPosition(PortalListView));
            while (elem != PortalListView)
            {
                if (elem is ListViewItem)
                {
                    PortalViewModel selectedItem = ((ListViewItem)elem).Content as PortalViewModel;
                    selectedItem.OpenSubmissionCommand.Execute(selectedItem);
                    return;
                }
                elem = (UIElement)VisualTreeHelper.GetParent(elem);
            }
        }

        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.QueryCommand.Execute(((TextBox)sender).Text);
        }
    }
}