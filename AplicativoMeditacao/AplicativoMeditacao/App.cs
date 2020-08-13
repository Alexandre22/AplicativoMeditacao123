using AplicativoMeditacao.Views;

using Xamarin.Forms;

namespace AplicativoMeditacao
{
    public class App : Application
    {
        
        public App()
        {
            
            var buttonStyle1 = new Style(typeof(Button))
            {
                Setters = {
                    new Setter { Property = Button.BackgroundColorProperty,   Value = Color.FromHex("#4729DA") }, //4729DA ... 84B6CA
                    new Setter { Property = Button.TextColorProperty,   Value = Color.FromHex("#FFFFFF") }, //84B6CA
                    new Setter { Property = Button.HorizontalOptionsProperty,   Value = LayoutOptions.CenterAndExpand },
                    new Setter { Property = Button.FontSizeProperty,   Value = 20.0 },
                    new Setter { Property = Button.FontAttributesProperty,   Value = FontAttributes.Bold },
                    new Setter { Property = Button.BorderRadiusProperty,   Value = 15 }
                }
            };
            
            var layoutStyle1 = new Style(typeof(StackLayout))
            {
                Setters =
                {
                    new Setter { Property = StackLayout.BackgroundColorProperty,   Value = Color.FromHex("#FFFFFF") },
                    new Setter { Property = StackLayout.PaddingProperty,   Value = 25 }
                }
            };

            //434638

            var labelStyle1 = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.TextColorProperty,   Value = Color.FromHex("#000000") }
                }
            };
            
            Resources = new ResourceDictionary();
            Resources.Add("buttonStyle1", buttonStyle1);
            Resources.Add("layoutStyle1", layoutStyle1);
            Resources.Add("labelStyle1", labelStyle1);

            
            MainPage = new NavigationPage(new TelaInicial());

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            if (SoundPlayer.SoundPlayerDemonstration.player != null)
            {
                SoundPlayer.SoundPlayerDemonstration.player.pararMeditacao();
                SoundPlayer.SoundPlayerDemonstration.player = null;
            }
        }

        protected override void OnResume()
        {
        }
        
    }
}
