using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;
using System.Text;
using Windows.Storage.Pickers;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Media.Playback;
using Mid_Pro;
using Windows.UI.Popups;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace homework01
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class thirdpage : Page
    {
        //播放器
        MediaPlayer _mediaPlayer = new MediaPlayer();
        ViewModels.TodoItemViewModel ViewModel { get; set; }
        public thirdpage()
        {
            
            this.InitializeComponent();
            // 设置播放源，初始化
             var mediaSource = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/first.mp4"));
            _mediaPlayer.Source = mediaSource;
             myplayer.SetMediaPlayer(_mediaPlayer);
            ViewModel = new ViewModels.TodoItemViewModel();
        }
        private void togglePaneButton_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }
        private void appbutton_click(object sender, RoutedEventArgs e)
        {
                /*ViewModel.SelectedItem = null;*/
                Frame.Navigate(typeof(MainPage), this.ViewModel);
        }
        private void audioclk(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(thirdpage));
        }
        private async void openfileclk(object sender, RoutedEventArgs e)
        {
            
            var openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".mp3");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null && (file.FileType == ".mp3" || file.FileType == ".mp4"))
            {
                var mediaSource = MediaSource.CreateFromStorageFile(file);
              //  mediaSource.OpenOperationCompleted += setTimelinelen;
                _mediaPlayer.Source = mediaSource;
            }
            

        }
        string City;
#pragma warning disable CS0169 // 从不使用字段“thirdpage.y”
        string y;
#pragma warning restore CS0169 // 从不使用字段“thirdpage.y”
        private async void query_Click(object sender, RoutedEventArgs e)
        {
            City = city.Text;
            RootObject weather = await OpenWeatherMapProxy.getweather(City);
            if (weather.code != 200 && City=="")
            {
                var dialog = new MessageDialog("您没有输入城市哟，亲~", "消息提示");
                var result = await dialog.ShowAsync();
                return;
            }
            else if (weather.code != 200)
            {
                var dialog = new MessageDialog("您没有输入正确的城市名哟，亲~", "消息提示");
                var result = await dialog.ShowAsync();
                return;
            }
            text1.Visibility = text2.Visibility = text3.Visibility = Visibility.Visible;
#pragma warning disable CS0219 // 变量“outt”已被赋值，但从未使用过它的值
            string outt = "";
#pragma warning restore CS0219 // 变量“outt”已被赋值，但从未使用过它的值
            string youtt = "";
            youtt = "日期：" + weather.data.forecast[0].date + "\n" + "最高气温: " + weather.data.forecast[0].high + "\n" + "风向: " + weather.data.forecast[0].fengxiang
                 + "\n" + "最低气温: " + weather.data.forecast[0].low + "\n" + "天气情况: "
        + weather.data.forecast[0].type + "\n" + "\n" + "日期：" + weather.data.forecast[1].date + "\n" + "最高气温: " + weather.data.forecast[1].high + "\n" + "风向: " + weather.data.forecast[1].fengxiang
                 + "\n" + "最低气温: " + weather.data.forecast[1].low + "\n" + "天气情况: "
        + weather.data.forecast[1].type + "\n" + "\n" + "日期：" + weather.data.forecast[2].date + "\n" + "最高气温: " + weather.data.forecast[2].high + "\n" + "风向: " + weather.data.forecast[2].fengxiang
                 + "\n" + "最低气温: " + weather.data.forecast[2].low + "\n" + "天气情况: "
        + weather.data.forecast[2].type + "\n" + "\n" + "日期：" + weather.data.forecast[3].date + "\n" + "最高气温: " + weather.data.forecast[3].high + "\n" + "风向: " + weather.data.forecast[3].fengxiang
                 + "\n" + "最低气温: " + weather.data.forecast[3].low + "\n" + "天气情况: "
        + weather.data.forecast[3].type + "\n" + "\n" + "日期：" + weather.data.forecast[4].date + "\n" + "最高气温: " + weather.data.forecast[4].high + "\n" + "风向: " + weather.data.forecast[4].fengxiang
                 + "\n" + "最低气温: " + weather.data.forecast[4].low + "\n" + "天气情况: "
        + weather.data.forecast[4].type;
             resulttext.Text = youtt;
            teml.Text = weather.data.wendu+"°C";
            zhishu.Text = weather.data.ganmao;
        }
    }
}
