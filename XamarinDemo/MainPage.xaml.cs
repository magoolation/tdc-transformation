using Microsoft.AppCenter;
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

        public MainPage()
        {
            InitializeComponent();

            _number = 1 + rnd.Next(99);
            NewGame.IsEnabled = false;
        }

        private void Crash_Button_Clicked(object sender, EventArgs e)
        {
            Crashes.GenerateTestCrash();
        }

        private void Guess_Button_Clicked(object sender, EventArgs e)
        {
            int guess = int.Parse(Guess.Text);

            if (guess == _number)
            {
                DisplayAlert("Success", "You got it!", "OK");
                Submit.IsEnabled = false;
                NewGame.IsEnabled = true;
            }
            else
            {
                DisplayAlert("Oops", "Not this time! Please try again!", "OK");
                Guess.Text = "";
                Guess.Focus();
            }
        }

        private void NewGame_Button_Clicked(object sender, EventArgs e)
        {
            _number = 1 + rnd.Next(99);
            Submit.IsEnabled = true;
            Guess.Text = "";
            Guess.Focus();
            NewGame.IsEnabled = false;
        }
    }
}
