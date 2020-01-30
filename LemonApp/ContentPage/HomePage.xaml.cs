﻿using System;
using System.Collections.Generic;
using System.Text;
using LemonLib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static LemonLib.InfoHelper;
using System.Threading.Tasks;

namespace LemonApp.ContentPage
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : UserControl
    {
        private MainWindow mw;
        public HomePage(MainWindow Context, ControlTemplate ct)
        {
            InitializeComponent();
            mw = Context;
            sv.Template=ct;
            SizeChanged += delegate {
                mw.WidthUI(HomePage_Gdtj);
                mw.WidthUI(HomePage_Nm);
            };
        }

        private async void LoadHomePage()
        {
            //---------加载主页HomePage----动画加持--
            mw.OpenLoading();
            HomePage_IFV.Visibility = Visibility.Hidden;
            GFGD.Visibility = Visibility.Hidden;
            DRGD.Visibility = Visibility.Hidden;
            GDTJ.Visibility = Visibility.Hidden;
            var data = await MusicLib.GetHomePageData();
            //--Top Focus--------
            HomePage_IFV.Updata(data.focus, mw);
            //--官方歌单----------
            foreach (var a in data.GFdata)
            {
                var k = new FLGDIndexItem(a.ID, a.Name, a.Photo, a.ListenCount) { Width = 175, Height = 175, Margin = new Thickness(12, 0, 12, 20) };
                k.StarEvent += (sx) =>
                {
                    MusicLib.AddGDILike(sx.id);
                    Toast.Send("收藏成功");
                };
                k.ImMouseDown += mw.FxGDMouseDown;
                HomePage_GFGD.Children.Add(k);
            }
            mw.WidthUI(HomePage_GFGD);
            //--达人歌单----------
            foreach (var a in data.DRdata)
            {
                var k = new FLGDIndexItem(a.ID, a.Name, a.Photo, a.ListenCount) { Width = 175, Height = 175, Margin = new Thickness(12, 0, 12, 20) };
                k.StarEvent += (sx) =>
                {
                    MusicLib.AddGDILike(sx.id);
                    Toast.Send("收藏成功");
                };
                k.ImMouseDown += mw.FxGDMouseDown;
                HomePage_DRGD.Children.Add(k);
            }
            mw.WidthUI(HomePage_DRGD);
            //--歌单推荐----------
            foreach (var a in data.Gdata)
            {
                var k = new FLGDIndexItem(a.ID, a.Name, a.Photo, a.ListenCount) { Width = 175, Height = 175, Margin = new Thickness(12, 0, 12, 20) };
                k.StarEvent += (sx) =>
                {
                    MusicLib.AddGDILike(sx.id);
                    Toast.Send("收藏成功");
                };
                k.ImMouseDown += mw.FxGDMouseDown;
                HomePage_Gdtj.Children.Add(k);
            }
            mw.WidthUI(HomePage_Gdtj);
            //--新歌首发----------
            foreach (var a in data.NewMusic)
            {
                var k = new FLGDIndexItem(a.MusicID, a.MusicName + " - " + a.SingerText, a.ImageUrl, 0) { Width = 175, Height = 175, Margin = new Thickness(10, 0, 10, 20) };
                k.Tag = a;
                k.ImMouseDown += (object s, MouseButtonEventArgs es) =>
                {
                    var sx = s as FLGDIndexItem;
                    Music dt = sx.Tag as Music;
                    mw.AddPlayDl_CR(new DataItem(dt));
                    mw.PlayMusic(dt.MusicID, dt.ImageUrl, dt.MusicName, dt.SingerText);
                };
                HomePage_Nm.Children.Add(k);
            }
            mw.WidthUI(HomePage_Nm);
            //------------------
            mw.CloseLoading();
            await Task.Delay(50);
            HomePage_IFV.Visibility = Visibility.Visible;
            mw.RunAnimation(HomePage_IFV);
            await Task.Delay(200);
            GFGD.Visibility = Visibility.Visible;
            mw.RunAnimation(GFGD);
            await Task.Delay(200);
            DRGD.Visibility = Visibility.Visible;
            mw.RunAnimation(DRGD);
            await Task.Delay(200);
            GDTJ.Visibility = Visibility.Visible;
            mw.RunAnimation(GDTJ);
            tb.Text = "新歌首发";
        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHomePage();
        }
    }
}