﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using HelloMaui.Common.Styles;


namespace HelloMaui
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage, IPage
	{
		public MainPage()
		{
			InitializeComponent();

            Application.Current.Resources = new LavaTheme();
		}

		public IView View
		{
			get => (IView)Content;
			set => Content = (View)value;
		}

        string currentEntry = "";
        int currentState = 1;
        string mathOperator;
        double firstNumber, secondNumber;
        string decimalFormat = "N0";
        bool isBlitzing = false;
        List<string> blitzWords = new List<string>
        {
            "01134",
            "993",
            "77345",
            "06",
            "040404",
            "30175",
            "733",
            "7735",
            "5907",
            "07734",
            "14",
            "5379909",
            "53177187714",
            "7714",
            "5317",
            "3080",
            "53045",
            "710",
            "5537",
            "618",
            "839",
            "707",
            "002",
            "30179",
            "09",
            "0375",
            "0637",
            "937",
            "7738"
        };

        void OnSelectNumber(object sender, EventArgs e)
        {
            if (isBlitzing) return;

            Button button = (Button)sender;
            string pressed = button.Text;

            currentEntry += pressed;

            if ((this.resultText.Text == "0" && pressed == "0")
                || (currentEntry.Length <= 1 && pressed != "0")
                || currentState < 0)
            {
                this.resultText.Text = "";
                if (currentState < 0)
                    currentState *= -1;
            }

            if (pressed == "." && decimalFormat != "N2")
            {
                decimalFormat = "N2";
            }

            this.resultText.Text += pressed;
            
            if (blitzWords.Contains(this.resultText.Text))
            {
                Blitz();
            }
        }

        void OnSelectOperator(object sender, EventArgs e)
        {
            if (isBlitzing) return;

            LockNumberValue(resultText.Text);

            currentState = -2;
            Button button = (Button)sender;
            string pressed = button.Text;
            mathOperator = pressed;            
        }

        private void LockNumberValue(string text)
        {
            double number;
            if (double.TryParse(text, out number))
            {
                if (currentState == 1)
                {
                    firstNumber = number;
                }
                else
                {
                    secondNumber = number;
                }

                currentEntry = string.Empty;
            }
        }

        void OnClear(object sender, EventArgs e)
        {
            if (isBlitzing) return;

            if(this.Container.Rotation != 0)
            {
                this.Container.RotateTo(0, 50);
                this.Resources["DisplayFont"] = "";
            }
            
            firstNumber = 0;
            secondNumber = 0;
            currentState = 1;
            decimalFormat = "N0";
            this.resultText.Text = "0";
            currentEntry = string.Empty;
        }

        void OnCalculate(object sender, EventArgs e)
        {
            if (currentState == 2)
            {
                if(secondNumber == 0)
                    LockNumberValue(resultText.Text);

                double result = Calculator.Calculate(firstNumber, secondNumber, mathOperator);

                this.CurrentCalculation.Text = $"{firstNumber} {mathOperator} {secondNumber}";

                this.resultText.Text = result.ToTrimmedString(decimalFormat);
                firstNumber = result;
                secondNumber = 0;
                currentState = -1;
                currentEntry = string.Empty;
            }
        }

        

        void OnNegative(object sender, EventArgs e)
        {
            if (isBlitzing) return;

            if (currentState == 1)
            {
                secondNumber = -1;
                mathOperator = "×";
                currentState = 2;
                OnCalculate(this, null);
            }
        }

        void OnPercentage(object sender, EventArgs e)
        {
            if (isBlitzing) return;

            if (currentState == 1)
            {
                LockNumberValue(resultText.Text);
                decimalFormat = "N2";
                secondNumber = 0.01;
                mathOperator = "×";
                currentState = 2;
                OnCalculate(this, null);
            }

        }

        private async void Blitz()
        {
            isBlitzing = true;
            await Task.Delay(200);

            await FlashBox.FadeTo(0.4, 200);
            await FlashBox.FadeTo(0.0, 100);
            await FlashBox.FadeTo(0.8, 200);
            await FlashBox.FadeTo(0.1, 100);
            await FlashBox.FadeTo(1, 210);
            await FlashBox.FadeTo(0.0, 25);
            await FlashBox.FadeTo(0.5, 50);
            await FlashBox.FadeTo(0.0, 25);
            await FlashBox.FadeTo(1, 80);
            await FlashBox.FadeTo(0.0, 100);

            this.Resources["DisplayFont"] = "DigitalDismay";

            await Task.Delay(1000);

            await FlashBox.FadeTo(0.2, 20);
            await FlashBox.FadeTo(0.0, 40);
            await FlashBox.FadeTo(0.8, 20);
            await FlashBox.FadeTo(0.1, 100);
            await FlashBox.FadeTo(1, 210);
            await FlashBox.FadeTo(0.0, 25);
            await FlashBox.FadeTo(0.5, 50);
            await FlashBox.FadeTo(0.0, 25);
            await FlashBox.FadeTo(1, 80);
            await FlashBox.FadeTo(0.0, 100);

            await this.Container.RotateTo(180, 50);
            isBlitzing = false;
            
        }
	}
}