using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SignEditor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ScreenManager _sm;
        public MainPage()
        {
            this.InitializeComponent();

            var view = ApplicationView.GetForCurrentView();

            view.TitleBar.BackgroundColor = Color.FromArgb(255, 11, 140, 26);
            view.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 11, 140, 26);
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonPressedForegroundColor = Color.FromArgb(255, 11, 140, 26);
            view.TitleBar.ButtonPressedBackgroundColor = Colors.White;
            view.TitleBar.ButtonHoverBackgroundColor = Colors.White;
            view.TitleBar.ButtonHoverForegroundColor = Color.FromArgb(255, 11, 140, 26);

            Application.Current.Resources["SystemControlHighlightListAccentLowBrush"] = new SolidColorBrush(Color.FromArgb(255, 174, 232, 181));
            Application.Current.Resources["SystemControlHighlightListAccentMediumBrush"] = new SolidColorBrush(Color.FromArgb(255, 174, 232, 181));

        }
        

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            _sm = new ScreenManager(txt_serveruri.Text);
            Load();
        }

        private async void Load()
        {
            var val = await _sm.GetScreensAsync();
            lv_main.ItemsSource = val;
        }


        private void lv_main_ItemClick(object sender, ItemClickEventArgs e)
        {
            Screen s = (Screen)e.ClickedItem;

            if(s.name == "New Screen")
            {
                txt_name.Text ="";
                txt_feed.Text = "";
                txt_id.Text = "";
                txt_duration.Text = "";
                txt_order.Text = "";
                txt_sign_text.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                txt_sign_type.Text = "";
                txt_uri.Text = "";
            }
            else
            {
                txt_name.Text = s.name;
                txt_feed.Text = s.feed;
                txt_id.Text = s._id;
                txt_duration.Text = s.duration;
                txt_order.Text = s.order;
                txt_sign_text.Document.SetText(Windows.UI.Text.TextSetOptions.None, s.sign_text);
                txt_sign_type.Text = s.sign_type;
                txt_uri.Text = s.uri;
            }
            
        }

        private async void btn_update_Click(object sender, RoutedEventArgs e)
        {
            Screen s = new Screen();
            s.name = txt_name.Text;
            s.feed = txt_feed.Text;
            s.duration = txt_duration.Text;
            s.order = txt_order.Text;
            string enteredtext = "";
            txt_sign_text.Document.GetText(Windows.UI.Text.TextGetOptions.None, out enteredtext);
            s.sign_text = enteredtext;
            s.sign_type = txt_sign_type.Text;
            s.uri = txt_uri.Text;
            s._id = txt_id.Text;

            if(s._id != "")
            {
                await _sm.UpdateScreenAsync(s);
            }
            else
            {
                await _sm.CreateScreenAsync(s);
            }
            
            var val = await _sm.GetScreensAsync();
            lv_main.ItemsSource = val;
        }

        private async void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            Screen s = new Screen();
            s._id = txt_id.Text;

            try
            {
                await _sm.DeleteScreenAsync(s);
            }
            catch
            {

            }
            var val = await _sm.GetScreensAsync();
            lv_main.ItemsSource = val;
        }
    }
}
