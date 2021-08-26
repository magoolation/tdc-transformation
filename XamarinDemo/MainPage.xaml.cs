using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinDemo
{
    public partial class MainPage : ContentPage
    {
        private Random rnd = new Random();
        private int _number;
        private int _max = 9;
        private  int _tries = 0;

        public MainPage()
        {
            InitializeComponent();            
            NewGame.IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            StartNewGame();
        }

        private void StartNewGame()
        {
            _tries = 0;
            _number = 1 + rnd.Next(_max);

            Analytics.TrackEvent("New Game", new Dictionary<string, string>()
            {
                { "Level", (_max + 1).ToString() },
                { "Value", _number.ToString() }
            });

            Submit.IsEnabled = true;
            Guess.Text = "";
            Guess.Focus();
            NewGame.IsEnabled = false;
        }

        private void Crash_Button_Clicked(object sender, EventArgs e)
        {
            Crashes.GenerateTestCrash();
        }

        private void Guess_Button_Clicked(object sender, EventArgs e)
        {
            _tries++;

            try
            {
                int guess = int.Parse(Guess.Text);

                if (guess == _number)
                {
                    Analytics.TrackEvent("Right guess", new Dictionary<string, string>()
                    {
                        { "Guess", Guess.Text },
                        {                         "Tries", _tries.ToString() },
                    });

                    DisplayAlert("Success", "You got it!", "OK");
                    Submit.IsEnabled = false;
                    NewGame.IsEnabled = true;
                }
                else
                { 
                    Analytics.TrackEvent("Wrong Guess", new Dictionary<string, string>()
                    {
                        { "Guess", Guess.Text },
                        { "Tries", _tries.ToString() },
                    });

                    DisplayAlert("Oops", "Not this time! Please try again!", "OK");
                    Guess.Text = "";
                    Guess.Focus();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>()
                {
                    { "Value", Guess.Text },
                });
                DisplayAlert("Error", "Invalid input. Please try again!", "OK");
                Guess.Text = "";
                Guess.Focus();
            }
        }

        private async void NewGame_Button_Clicked(object sender, EventArgs e)
        {
            string option = await DisplayActionSheet("Choose a Level", "Cancel", null, new string[] { "10", "50", "100" });

            _max = option switch
            {
                "10" => 9,
            "50" => 49,
            "100" => 99,
            _ => 0
        };

        if (_max == 0)
            {
            return;
            }

            StartNewGame();
        }
    }
}
