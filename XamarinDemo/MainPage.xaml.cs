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
        private  int _tries = 0;

        public MainPage()
        {
            InitializeComponent();

            StartNewGame();
            NewGame.IsEnabled = false;
        }

        private void StartNewGame()
        {
            _tries = 0;
            _number = 1 + rnd.Next(99);

            Analytics.TrackEvent("New Game", new Dictionary<string, string>()
            {
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

        private void NewGame_Button_Clicked(object sender, EventArgs e)
        {
            StartNewGame();
        }
    }
}
