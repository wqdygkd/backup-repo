﻿using System.Windows;
using System.Windows.Media.Animation;

namespace Lemon_App
{
    /// <summary>
    /// Msg.xaml 的交互逻辑
    /// </summary>
    public partial class Msg : Window
    {
        public Msg(string tx)
        {
            InitializeComponent();
            tb.Text = tx;
            Left = SystemParameters.WorkArea.Width - Width;
            Top = SystemParameters.WorkArea.Height - Height + 10;
        }
        public void tbclose()
        {
            var t = Resources["Closed"] as Storyboard;
            t.Completed += delegate { Close(); };
            t.Begin();
        }
    }
}