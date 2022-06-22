using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Widget;
using Java.Lang;
using mARkIt.Droid.Helpers;
using mARkIt.Models;
using mARkIt.Utils;
using Xamarin.Essentials;

namespace mARkIt.Droid.Activities
{
    [Activity(Label = "AddAMarkActivity")]
    public class AddAMarkActivity : AppCompatActivity,ITextWatcher
    {
        private EditText m_MessageEditText;
        private TextView m_NumOfLettersTextView;
        private RadioButton m_GeneralRadioButton;
        private RadioButton m_FoodRadioButton;
        private RadioButton m_SportRadioButton;
        private RadioButton m_HistoryRadioButton;
        private RadioButton m_NatureRadioButton;
        private RadioButton m_WoodRadioButton;
        private RadioButton m_MetalRadioButton;
        private RadioButton m_SchoolRadioButton;
        private const int k_MaxLetters = 40;
        private int m_LettersCount = 0;

        public void AfterTextChanged(IEditable s)
        {
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            m_LettersCount = s.Count();
            int remainingLetters = k_MaxLetters - m_LettersCount;
            m_NumOfLettersTextView.SetTextColor(remainingLetters < 0 ? Android.Graphics.Color.Red : Android.Graphics.Color.White);
            m_NumOfLettersTextView.Text = "Letters: " + remainingLetters;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            SetContentView(Resource.Layout.AddAMark);
            Button saveButton = FindViewById<Button>(Resource.Id.SaveButton);
            saveButton.Click += SaveButton_Click;
            findComponents();
            m_NumOfLettersTextView.Text = "Letters: " + k_MaxLetters;
            m_MessageEditText.AddTextChangedListener(this);
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            bool inputIsValid = validateInput();
            Button button = sender as Button;
            button.Clickable = false;

            if (inputIsValid)
            {
                try
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    Mark mark = new Mark()
                    {
                        Message = m_MessageEditText.Text,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Altitude = location.Altitude.HasValue ? location.Altitude.Value : 1,
                        CategoriesCode = getCategoriesCode(),
                        Style = getStyle()
                    };
                    bool uploadSuccessful = await Mark.Insert(mark);

                    if (uploadSuccessful)
                    {
                        Alert.Show("Success", "Upload successfull.", this, Finish);
                    }
                    else
                    {
                        Alert.Show("Faliure", "Upload failed.", this);
                        button.Clickable = true;
                    }
                }
                catch (FeatureNotEnabledException)
                {
                    Toast.MakeText(this, "Please activate location services.", ToastLength.Long).Show();
                    button.Clickable = true;
                }
            }
        }

        private string getStyle()
        {
            string style = null;
            if (m_WoodRadioButton.Checked == true)
            {
                style = "Wood";
            }
            else if(m_MetalRadioButton.Checked == true)
            {
                style = "Metal";
            }
            else if(m_SchoolRadioButton.Checked == true)
            {
                style = "School";
            }
            return style;
        }

        private bool validateInput()
        {
            bool isInputValid = false;

            if (m_LettersCount == 0)
            {
                Toast.MakeText(this, "Please insert a message.", ToastLength.Long).Show();
            }
            else if (m_LettersCount > k_MaxLetters)
            {
                Toast.MakeText(this, "Please delete some letters.", ToastLength.Long).Show();
            }
            else
            {
                isInputValid = true;
            }

            return isInputValid;
        }

        private int getCategoriesCode()
        {
            int catagories = 0;
            if (m_GeneralRadioButton.Checked)
            {
                catagories = (int)eCategories.General;
            }
            else if (m_FoodRadioButton.Checked)
            {
                catagories = (int)eCategories.Food;
            }
            else if (m_SportRadioButton.Checked)
            {
                catagories = (int)eCategories.Sport;
            }
            else if (m_HistoryRadioButton.Checked)
            {
                catagories = (int)eCategories.History;
            }
            else if (m_NatureRadioButton.Checked)
            {
                catagories = (int)eCategories.Nature;
            }
            return catagories;
        }

        private void findComponents()
        {
            m_MessageEditText = FindViewById<EditText>(Resource.Id.MarkMessageEditText);
            m_NumOfLettersTextView = FindViewById<TextView>(Resource.Id.LettersTextView);
            m_GeneralRadioButton = FindViewById<RadioButton>(Resource.Id.GeneralRadioButton);
            m_FoodRadioButton = FindViewById<RadioButton>(Resource.Id.FoodRadioButton);
            m_HistoryRadioButton = FindViewById<RadioButton>(Resource.Id.HistoryRadioButton);
            m_SportRadioButton = FindViewById<RadioButton>(Resource.Id.SportRadioButton);
            m_NatureRadioButton = FindViewById<RadioButton>(Resource.Id.NatureRadioButton);
            m_WoodRadioButton= FindViewById<RadioButton>(Resource.Id.WoodRadioButton);
            m_MetalRadioButton = FindViewById<RadioButton>(Resource.Id.MetalRadioButton);
            m_SchoolRadioButton=FindViewById<RadioButton>(Resource.Id.SchoolRadioButton);
        }
    }
}
