using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Core;
using homework01.ViewModels;
using homework01.Models;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace homework01
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {

        public NewPage()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
        }

        ViewModels.TodoItemViewModel ViewModel { get; set; }

        private void createupclick(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            /*
            if (date.Date < DateTime.Today)//检查日期
            {
                //弹出对话框错误信息
                flag = false;
                var messageDialog = new MessageDialog("date is wrong!");
                messageDialog.ShowAsync();

            }
            */
            if (title.Text == "" && detail.Text == "")//文本框空
            {
                //弹出对话框错误信息
                flag = false;
                var messageDialog = new MessageDialog("地点和备注栏为空!");
#pragma warning disable CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。
                messageDialog.ShowAsync();
#pragma warning restore CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。

                return;
            }
            else if (detail.Text == "")
            {
                flag = false;
                var messageDialog = new MessageDialog("备注栏为空!");
#pragma warning disable CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。
                messageDialog.ShowAsync();
#pragma warning restore CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。
            }
            else if (title.Text == "")
            {
                flag = false;
                var messageDialog = new MessageDialog("地点为空!");
#pragma warning disable CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。
                messageDialog.ShowAsync();
#pragma warning restore CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。
            }
            if (flag == true)
            {
                if (createup.Content.ToString() == "Create")
                {
                    this.ViewModel.AddTodoTtem("", title.Text, place.Text, detail.Text, false, date.Date.DateTime);
                    ViewModel.SelectedItem = null;
                    var messageDialog = new MessageDialog("创建成功!");
#pragma warning disable CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。
                    messageDialog.ShowAsync();
#pragma warning restore CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。
                    Frame.Navigate(typeof(MainPage), ViewModel);


                }
                else
                {
                    /*this.ViewModel.SelectedItem.update(title.Text, place.Text, detail.Text, date.Date.DateTime);*/
                    this.ViewModel.AddTodoTtem("", title.Text, place.Text, detail.Text, false, date.Date.DateTime);
                    ViewModel.AllItems.Remove(ViewModel.SelectedItem);
                    ViewModel.SelectedItem = null;
                    var messageDialog = new MessageDialog("更新成功!");
#pragma warning disable CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。
                    messageDialog.ShowAsync();
#pragma warning restore CS4014 // 由于此调用不会等待，因此在此调用完成之前将会继续执行当前方法。请考虑将 "await" 运算符应用于调用结果。
                    ViewModel.SelectedItem = null;
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }

            }

        }

        private void cancelitem(object sender, RoutedEventArgs e)
        {
            if (createup.Content.ToString() == "Update")
            {
                title.Text = ViewModel.SelectedItem.title;
                date.Date = ViewModel.SelectedItem.date;
                place.Text = ViewModel.SelectedItem.place;
                detail.Text = ViewModel.SelectedItem.description;
            }
            else
            {
                title.Text = "";
                detail.Text = "";
                date.Date = DateTime.Today;
            }
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            delete.Visibility = Visibility.Collapsed;
            createup.Content = "Create";
            ViewModel.AllItems.Remove(ViewModel.SelectedItem);
            ViewModel.SelectedItem = null;
            var messageDialog = new MessageDialog("删除成功！").ShowAsync();
            Frame.Navigate(typeof(MainPage), ViewModel);
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


        private async void pic1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            await Common.selectPic(pic1);
        }
        private async void pic2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            await Common.selectPic(pic2);
        }
        private async void pic3_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            await Common.selectPic(pic3);
        }
    }
}
