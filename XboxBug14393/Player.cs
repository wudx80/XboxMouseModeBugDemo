using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace XboxBug14393
{
    class Player
    {
        static Player playInstance;
        static UIElement saved = null;

        private Page rootPage;
        private Grid rootGrid;
        private Page widgetPage;
        private Grid wGrid;
        private Button closeButton;
        private Button ctaButton;
        private Button muteButton;
        private Page videoPage;
        private MediaElement media;

        public static UIElement CurrentContent
        {
            get
            {
                ContentControl ctrl = Window.Current.Content as ContentControl;
                if (ctrl == null)
                {
                    return Window.Current.Content;
                }
                else
                {
                    return (Window.Current.Content as ContentControl).Content as UIElement;
                }
            }
            private set
            {
                ContentControl ctrl = Window.Current.Content as ContentControl;
                if (ctrl == null)
                {
                    Window.Current.Content = value;
                }
                else
                {
                    (Window.Current.Content as ContentControl).Content = value;
                }
            }
        }

        public static void Play()
        {
            if (saved != null)
            {
                return;
            }

            playInstance = new Player();
            saved = CurrentContent;
            CurrentContent = playInstance.GetPage();
            playInstance.PlayVideo();
        }

        public Player()
        {
            createRootPage();
            createVideoPage();
            createWidgetPage();
            rootPage.RequiresPointer = RequiresPointer.WhenFocused;
        }

        private void createWidgetPage()
        {
            widgetPage = new Page();
            wGrid = new Grid() { Name = "widgetGrid" };
            Brush white = new SolidColorBrush(Colors.White);
            closeButton = new Button()
            {
                Name = "closeButton",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 5, 0, 0),
                Content = "Close",
                Foreground = white,
                BorderBrush = white,
            };
            closeButton.Click += CloseButton_Click; ;
            wGrid.Children.Add(closeButton);

            ctaButton = new Button()
            {
                Name = "ctaButton",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 5, 5, 0),
                Content = "CTA",
                Foreground = white,
                BorderBrush = white,
            };
            ctaButton.Click += CtaButton_Click;
            wGrid.Children.Add(ctaButton);

            muteButton = new Button()
            {
                Name = "muteButton",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 5, 5),
                Content = "Mute",
                Foreground = white,
                BorderBrush = white,
            };
            muteButton.Click += MuteButton_Clicked;
            wGrid.Children.Add(muteButton);
            widgetPage.Content = wGrid;
            rootGrid.Children.Add(widgetPage);
        }

        private async void CtaButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.vungle.com/privacy/"));
        }

        private void MuteButton_Clicked(object sender, RoutedEventArgs e)
        {
            media.IsMuted = !media.IsMuted;
            muteButton.Content = media.IsMuted ? "Unmute" : "Mute";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            EndPlay();
        }

        private void createVideoPage()
        {
            videoPage = new Page();
            Grid vGrid = new Grid() { Name = "videoGrid" };
            videoPage.Content = vGrid;
            WebView wv = new WebView();
            wv.Navigate(new Uri("http://www.google.com"));
            media = new MediaElement()
            {
                Name = "media",
                AutoPlay = false,
            };
            vGrid.Children.Add(wv);
            rootGrid.Children.Add(videoPage);
            //media.MediaOpened += Media_MediaOpened;
            //media.MediaFailed += Media_MediaFailed;
            //media.MediaEnded += Media_MediaEnded;
        }

        private void setVideoPath()
        {
            //Uri videoUri = new System.Uri("ms-appx:///resource/video1.mp4");
            //media.Source = videoUri;
        }

        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            // do nothing
        }

        private void Media_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            // do nothing
        }

        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            media.Play();
        }

        private void createRootPage()
        {
            rootPage = new Page();
            rootGrid = new Grid() { Name = "rootGrid" };
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            rootPage.Content = rootGrid;
        }

        private void EndPlay()
        {
            StopVideo();
            CurrentContent = saved;
            saved = null;
            playInstance = null;
        }

        private void StopVideo()
        {
            media.Stop();
        }

        private void PlayVideo()
        {
            setVideoPath();
        }

        private UIElement GetPage()
        {
            return rootPage;
        }
    }
}
