using Windows.UI.Xaml;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Core;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Text;
using Windows.UI.Popups;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Collections.Generic;
using Newtonsoft.Json;
using Windows.UI.Notifications;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel;
using System.Diagnostics;
using homework01.ViewModels;
using Windows.UI.Xaml.Input;
using Windows.Storage.Pickers;
using NotificationsExtensions.Tiles;


// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace homework01
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ViewModels.TodoItemViewModel ViewModel { get; set; }
        public TodoItemViewModel ViewModels = ((App)App.Current).ViewModels;
        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ViewModel = new ViewModels.TodoItemViewModel();
        }

        private void togglePaneButton_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }
        private void appbutton_click(object sender, RoutedEventArgs e)
        {
            if (rightcontent.Visibility.ToString() == "Collapsed")
            {
                ViewModel.SelectedItem = null;
                Frame.Navigate(typeof(NewPage), this.ViewModel);

            }
            else
            {
                createup.Content = "Create";
                delete.Visibility = Visibility.Collapsed;
                title.Text = place.Text = detail.Text = "";
                date.Date = DateTime.Now;
                title.Focus(FocusState.Programmatic);//???
            }

        }

        private void uncheck(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            Line line = VisualTreeHelper.GetChild(parent, 2) as Line;
            line.Visibility = Visibility.Collapsed;
        }

        private void oncheck(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            Line line = VisualTreeHelper.GetChild(parent, 2) as Line;
            line.Visibility = Visibility.Visible;
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            delete.Visibility = Visibility.Collapsed;
            createup.Content = "Create";
            ViewModel.AllItems.Remove(ViewModel.SelectedItem);
            ViewModel.SelectedItem = null;
            var messageDialog = new MessageDialog("删除成功！").ShowAsync();
        }

        private async void Share_click(object sender, RoutedEventArgs e)
        {
            var title = ViewModel.SelectedItem.title + "(" + ViewModel.SelectedItem.place + ")";
            var summary = ViewModel.SelectedItem.description;
            Uri uri = new Uri("https://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url=1.1&sharesource=qzone&title=" + title + "&summary=" + summary);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }


        private async void createupclick(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (title.Text == "" && detail.Text == "")//文本框空
            {
                //弹出对话框错误信息
                flag = false;
                var messageDialog = new MessageDialog("地点和备注栏为空!");
                await messageDialog.ShowAsync();

                return;
            }
            else if (detail.Text == "")
            {
                flag = false;
                var messageDialog = new MessageDialog("备注栏为空!");
                await messageDialog.ShowAsync();
            }
            else if (title.Text == "")
            {
                flag = false;
                var messageDialog = new MessageDialog("地点为空!");
                await messageDialog.ShowAsync();
            }
            if (flag == true)
            {
                if (createup.Content.ToString() == "Create")
                {
                    this.ViewModel.AddTodoTtem("", title.Text, place.Text, detail.Text, false, date.Date.DateTime);
                    ViewModel.SelectedItem = null;
                    var messageDialog = new MessageDialog("创建成功!");
                    await messageDialog.ShowAsync();
                }
                else
                {
                    /*this.ViewModel.SelectedItem.update(title.Text, place.Text, detail.Text, date.Date.DateTime);*/
                    this.ViewModel.AddTodoTtem("", title.Text, place.Text, detail.Text, false, date.Date.DateTime);
                    ViewModel.AllItems.Remove(ViewModel.SelectedItem);
                    ViewModel.SelectedItem = null;
                    var messageDialog = new MessageDialog("更新成功!");
                    await messageDialog.ShowAsync();
                }

                var line1 = title.Text;
                var line2 = place.Text;
                var line3 = date.Date.DateTime.ToShortDateString();

                TileContent content = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileLarge = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                            {
                                new TileText()
                                {
                                    Text = line1,
                                    Style = TileTextStyle.Subheader
                                },
                                new TileText()
                                {
                                    Text = line2,
                                    Style = TileTextStyle.Title
                                },
                                new TileText()
                                {
                                    Text = line3,
                                    Style = TileTextStyle.Body
                                }
                            }
                            }
                        },
                        TileMedium = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                            {
                                new TileText()
                                {
                                    Text = line1,
                                    Style = TileTextStyle.Subtitle
                                },
                                new TileText()
                                {
                                    Text = line2,
                                    Style = TileTextStyle.Subtitle
                                },
                                new TileText()
                                {
                                    Text = line3,
                                    Style = TileTextStyle.Body
                                }
                            }
                            }
                        },
                        TileWide = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                            {
                                new TileText()
                                {
                                    Text = line1,
                                    Style = TileTextStyle.Subtitle
                                },
                                new TileText()
                                {
                                    Text = line2 + "  " + line3,
                                    Style = TileTextStyle.Subtitle
                                },
                            }
                            }
                        }
                    }
                };

                var notification = new TileNotification(content.GetXml());
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            }

        }

        private void cancelitem(object sender, RoutedEventArgs e)
        {
            if (createup.Content.ToString() == "Update")
            {
                title.Text = ViewModel.SelectedItem.title;
                place.Text = ViewModel.SelectedItem.place;
                date.Date = ViewModel.SelectedItem.date;
                detail.Text = ViewModel.SelectedItem.description;
            }
            else
            {
                title.Text = "";
                detail.Text = "";
                date.Date = DateTime.Today;
            }
        }

        private void itemclk(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = e.ClickedItem as Models.TodoItem;
            if (rightcontent.Visibility.ToString() == "Collapsed")
            {
                //   ViewModel.SelectedItem = e.ClickedItem as Models.TodoItem;
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
            else
            {
                delete.Visibility = Visibility.Visible;
                createup.Content = "Update";
                title.Text = ViewModel.SelectedItem.title;
                place.Text = ViewModel.SelectedItem.place;
                detail.Text = ViewModel.SelectedItem.description;
                date.Date = ViewModel.SelectedItem.date;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            if (e.NavigationMode == NavigationMode.New)
            {
                // If this is a new navigation, this is a fresh launch so we can discard any saved state
                ApplicationData.Current.LocalSettings.Values.Remove("Item");
                ApplicationData.Current.LocalSettings.Values.Remove("allitems");
                ApplicationData.Current.LocalSettings.Values.Remove("selectitem");
            }
            else
            {
                // Try to restore state if any, in case we were terminated
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("allitems"))
                {
                    ViewModel.AllItems.Clear();
                    List<string> L = JsonConvert.DeserializeObject<List<string>>(
                      (string)ApplicationData.Current.LocalSettings.Values["allitems"]);
                    foreach (var l in L)
                    {
                        myItem a = JsonConvert.DeserializeObject<myItem>(l);
                        Models.TodoItem item = new Models.TodoItem(a.id, a.title, a.place, a.description, a.completed, a.date);
                        ViewModel.AllItems.Add(item);
                    }
                }
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("selectitem"))
                {
                    ViewModel.SelectedItem = ViewModel.AllItems[(int)(ApplicationData.Current.LocalSettings.Values["selectitem"])];
                }

                if (ApplicationData.Current.LocalSettings.Containers.ContainsKey("Item"))
                {
                    ApplicationDataContainer Item = ApplicationData.Current.LocalSettings.Containers["Item"];
                    createup.Content = Item.Values["btn"] as string;

                    title.Text = Item.Values["title"] as string;
                    place.Text = Item.Values["place"] as string;
                    detail.Text = Item.Values["detail"] as string;
                    date.Date = (DateTimeOffset)(Item.Values["date"]);
                    Common.selectName = Item.Values["imgname"] as string;

                    if (Common.selectName == "")
                    {
                        pic1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Cool.jpg"));
                    }
                    else
                    {
                        var file = await ApplicationData.Current.LocalFolder.GetFileAsync(Common.selectName);
                        IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                        BitmapImage bitmapImage = new BitmapImage();
                        await bitmapImage.SetSourceAsync(fileStream);
                        pic1.Source = bitmapImage;
                    }
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).issuspend)
            {
                // Save volatile state in case we get terminated later on
                // then we can restore as if we'd never been gone
                ApplicationDataContainer Item = ApplicationData.Current.LocalSettings.CreateContainer("Item", ApplicationDataCreateDisposition.Always);
                if (ApplicationData.Current.LocalSettings.Containers.ContainsKey("Item"))
                {
                    Item.Values["btn"] = createup.Content;

                    Item.Values["title"] = title.Text;
                    Item.Values["place"] = place.Text;
                    Item.Values["detail"] = detail.Text;
                    Item.Values["date"] = date.Date;
                    Item.Values["imgname"] = Common.selectName;
                }
                if (ViewModel.SelectedItem != null)
                {
                    ApplicationData.Current.LocalSettings.Values["selectitem"] = ViewModel.AllItems.IndexOf(ViewModel.SelectedItem);
                }
                List<string> L = new List<string>();
                var allitems = ViewModel.AllItems;
                foreach (var a in allitems)
                {
                    var item = new myItem(a.id, a.title, a.place, a.description, a.completed, a.date);
                    L.Add(JsonConvert.SerializeObject(item));
                }
                ApplicationData.Current.LocalSettings.Values["allitems"] = JsonConvert.SerializeObject(L);
            }
        }


        private void audioclk(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(thirdpage));
        }


        private void lvVerses_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            menuFlyout.ShowAt(leftcontent, e.GetPosition(this.leftcontent));
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


    class Common
    {
        public static ViewModels.TodoItemViewModel ViewModel = new ViewModels.TodoItemViewModel();
        public static string selectName = "";
        public static async System.Threading.Tasks.Task selectPic(Image pic)
        {
            var fop = new FileOpenPicker();
            fop.ViewMode = PickerViewMode.Thumbnail;
            fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".jpeg");
            fop.FileTypeFilter.Add(".png");
            fop.FileTypeFilter.Add(".gif");

            Windows.Storage.StorageFile file = await fop.PickSingleFileAsync();
            try
            {
                IRandomAccessStream fileStream;
                using (fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    pic.Source = bitmapImage;

                    var name = file.Path.Substring(file.Path.LastIndexOf('\\') + 1);
                    selectName = name;

                    await file.CopyAsync(Windows.Storage.ApplicationData.Current.LocalFolder, name, Windows.Storage.NameCollisionOption.ReplaceExisting);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return;
            }
        }
    }

}
