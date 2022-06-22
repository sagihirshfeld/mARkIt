using mARkIt.iOS.Helpers;
using mARkIt.Models;
using mARkIt.Utils;
using Syncfusion.iOS.Buttons;
using System;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;

namespace mARkIt.iOS
{
    public partial class AddAMarkViewController : UIViewController
    {
        private SfRadioButton m_WoodMarkStyleRadioButton, m_MetalMarkStyleRadioButton, m_SchoolMarkStyleRadioButton;
        private SfRadioButton m_GeneralCategoryRadioButton, m_FoodCategoryRadioButton, m_HistoryCategoryRadioButton, m_SportCategoryRadioButton, m_NatureCategoryRadioButton;
        private const int m_MaxLettersAllowed = 40;

        public AddAMarkViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true)));
            doneBarButton.Clicked += DoneBarButton_Clicked;
            addMarkStyleRadioButtons();
            addCategoriesRadioButtons();
            markTextField.EditingChanged += MarkTextField_EditingChanged;
            letterCounterLabel.Text = m_MaxLettersAllowed.ToString();
            markTextField.ShouldReturn = delegate
            {
                markTextField.ResignFirstResponder();
                return true;
            };
        }


        private async Task uploadMarkAsync()
        {
            bool markUploaded = false;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Mark mark = new Mark()
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Altitude = location.Altitude.HasValue ? location.Altitude.Value : 1,
                        Message = markTextField.Text,
                        Style = getMarkStyle(),
                        CategoriesCode = getCategory()
                    };
                    markUploaded = await Mark.Insert(mark);
                }
            }
            catch (Exception) { }

            if (markUploaded)
            {
                Alert.Display("Success", "The mARk uploaded", this, new Action<UIAlertAction>((a) => NavigationController.PopViewController(true)));
            }
            else
            {
                Alert.Display("Error", "There was a problem uploading your mARk", this);
            }
        }

        private string getMarkStyle()
        {
            string markStyle = null;
            if ((bool)m_WoodMarkStyleRadioButton.IsChecked)
            {
                markStyle = "Wood";
            }
            else if ((bool)m_SchoolMarkStyleRadioButton.IsChecked)
            {
                markStyle = "School";
            }
            else if ((bool)m_MetalMarkStyleRadioButton.IsChecked)
            {
                markStyle = "Metal";
            }
            return markStyle;
        }

        private int getCategory()
        {
            int category = 0;
            if ((bool)m_GeneralCategoryRadioButton.IsChecked)
            {
                category = (int)eCategories.General;
            }
            else if ((bool)m_FoodCategoryRadioButton.IsChecked)
            {
                category = (int)eCategories.Food;
            }
            else if ((bool)m_SportCategoryRadioButton.IsChecked)
            {
                category = (int)eCategories.Sport;
            }
            else if ((bool)m_HistoryCategoryRadioButton.IsChecked)
            {
                category = (int)eCategories.History;
            }
            else if ((bool)m_NatureCategoryRadioButton.IsChecked)
            {
                category = (int)eCategories.Nature;
            }

            return category;
        }

        private async void DoneBarButton_Clicked(object sender, EventArgs e)
        {
            doneBarButton.Enabled = false;
            markTextField.UserInteractionEnabled = false;
            if (markTextField.Text.Count() > m_MaxLettersAllowed)
            {
                Alert.Display("Error", "There are too many letters!", this);
            }
            else if (string.IsNullOrEmpty(markTextField.Text))
            {
                Alert.Display("Error", "Please fill mARk text", this);
            }
            else
            {
                await uploadMarkAsync();
            }

            doneBarButton.Enabled = true;
            markTextField.UserInteractionEnabled = true;
        }

        partial void CancleBarButton_Activated(UIBarButtonItem sender)
        {
            NavigationController.PopViewController(true);
        }

        private void MarkTextField_EditingChanged(object sender, EventArgs e)
        {
            int remainingLetters = m_MaxLettersAllowed - markTextField.Text.Count<char>();
            letterCounterLabel.Text = (remainingLetters).ToString();
            if (remainingLetters < 0)
            {
                letterCounterLabel.TextColor = UIColor.Red;
            }
            else if (remainingLetters < 10)
            {
                letterCounterLabel.TextColor = UIColor.Yellow;
            }
            else
            {
                letterCounterLabel.TextColor = UIColor.White;
            }
        }

        private void addCategoriesRadioButtons()
        {
            m_GeneralCategoryRadioButton = new SfRadioButton();
            m_FoodCategoryRadioButton = new SfRadioButton();
            m_HistoryCategoryRadioButton = new SfRadioButton();
            m_SportCategoryRadioButton = new SfRadioButton();
            m_NatureCategoryRadioButton = new SfRadioButton();
            m_GeneralCategoryRadioButton.IsChecked = true;
            m_GeneralCategoryRadioButton.SetTitle("General", UIControlState.Normal);
            m_FoodCategoryRadioButton.SetTitle("Food", UIControlState.Normal);
            m_HistoryCategoryRadioButton.SetTitle("History", UIControlState.Normal);
            m_SportCategoryRadioButton.SetTitle("Sport", UIControlState.Normal);
            m_NatureCategoryRadioButton.SetTitle("Nature", UIControlState.Normal);
            m_GeneralCategoryRadioButton.Font = UIFont.FromName("Arial", 15);
            m_FoodCategoryRadioButton.Font = UIFont.FromName("Arial", 15);
            m_HistoryCategoryRadioButton.Font = UIFont.FromName("Arial", 15);
            m_SportCategoryRadioButton.Font = UIFont.FromName("Arial", 15);
            m_NatureCategoryRadioButton.Font = UIFont.FromName("Arial", 15);

            categoriesRadioGroup.AddArrangedSubview(m_GeneralCategoryRadioButton);
            categoriesRadioGroup.AddArrangedSubview(m_FoodCategoryRadioButton);
            categoriesRadioGroup.AddArrangedSubview(m_HistoryCategoryRadioButton);
            categoriesRadioGroup.AddArrangedSubview(m_SportCategoryRadioButton);
            categoriesRadioGroup.AddArrangedSubview(m_NatureCategoryRadioButton);
        }

        private void addMarkStyleRadioButtons()
        {
            m_WoodMarkStyleRadioButton = new SfRadioButton();
            m_MetalMarkStyleRadioButton = new SfRadioButton();
            m_SchoolMarkStyleRadioButton = new SfRadioButton();
            m_WoodMarkStyleRadioButton.IsChecked = true;
            markStyleRadioGroup.AddArrangedSubview(m_WoodMarkStyleRadioButton);
            markStyleRadioGroup.AddArrangedSubview(m_MetalMarkStyleRadioButton);
            markStyleRadioGroup.AddArrangedSubview(m_SchoolMarkStyleRadioButton);
        }
    }
}
