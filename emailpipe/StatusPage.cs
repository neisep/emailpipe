﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace emailpipe
{
    public class StatusPage : IPage
    {
        public ListView EmailListView { get; set; }

        public StatusPage()
        {
            EmailListView = new ListView();
        }

        public FrameworkElement GenerateContent()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "Subject", Width = 250, DisplayMemberBinding = new Binding("Subject") });
            gridView.Columns.Add(new GridViewColumn { Header = "Date", Width = 80, DisplayMemberBinding = new Binding("Date") });

            EmailListView.View = gridView;
            EmailListView.Opacity = 0.7;

            return EmailListView;
        }
    }
}