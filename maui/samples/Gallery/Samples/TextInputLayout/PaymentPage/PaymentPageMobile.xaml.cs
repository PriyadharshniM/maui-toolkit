using core = Syncfusion.Maui.Toolkit.TextInputLayout;

namespace Syncfusion.Maui.ControlsGallery.TextInputLayout.SfTextInputLayout
{
	public partial class PaymentPageMobile : SampleView
	{
		public PaymentPageMobile()
		{
			InitializeComponent();
			PayNameField.InputViewPadding = new Thickness(16, 8, 16, 8);
			CardNumberField.InputViewPadding = new Thickness(16, 8, 16, 8);
			CardNumberField1.InputViewPadding = new Thickness(16, 8, 16, 8);
			CardNumberField2.InputViewPadding = new Thickness(16, 8, 16, 8);
			CardNumberField3.InputViewPadding = new Thickness(16, 8, 16, 8);
			CVVField.InputViewPadding = new Thickness(16, 8, 16, 8);
			MonthField.InputViewPadding = new Thickness(16, 8, 2, 8);
			YearField.InputViewPadding = new Thickness(16, 8, 2, 8);
		}
		private void SubmitButton_Clicked(object sender, EventArgs e)
		{
			PaymentPageMobile.FieldNullCheck(PayNameField);
			PaymentPageMobile.FieldNullCheck(CardNumberField);
			PaymentPageMobile.FieldNullCheck(CardNumberField1);
			PaymentPageMobile.FieldNullCheck(CardNumberField2);
			PaymentPageMobile.FieldNullCheck(CardNumberField3);
			PaymentPageMobile.FieldNullCheck(CVVField);
			PaymentPageMobile.FieldNullCheck(MonthField);
			PaymentPageMobile.FieldNullCheck(YearField);
			ValidateCardNumber();
		}

		private static void FieldNullCheck(core.SfTextInputLayout inputLayout)
		{
			if (string.IsNullOrEmpty(inputLayout.Text))
			{
				inputLayout.HasError = true;
			}
			else
			{
				inputLayout.HasError = false;
			}
		}


		private void ValidateCardNumber()
		{
			if (CardNumberField.Text == null || CardNumberField.Text.Length != 4 || !double.TryParse(CardNumberField.Text, out _))
			{
				CardNumberField.HasError = true;
			}
			else
			{
				CardNumberField.HasError = false;
			}

			if (CardNumberField1.Text == null || CardNumberField1.Text.Length != 4 || !double.TryParse(CardNumberField1.Text, out _))
			{
				CardNumberField1.HasError = true;
			}
			else
			{
				CardNumberField1.HasError = false;
			}

			if (CardNumberField2.Text == null || CardNumberField2.Text.Length != 4 || !double.TryParse(CardNumberField2.Text, out _))
			{
				CardNumberField2.HasError = true;
			}
			else
			{
				CardNumberField2.HasError = false;
			}

			if (CardNumberField3.Text == null || CardNumberField3.Text.Length != 4 || !double.TryParse(CardNumberField3.Text, out _))
			{
				CardNumberField3.HasError = true;
			}
			else
			{
				CardNumberField3.HasError = false;
			}

			if (CVVField.Text == null || CVVField.Text.Length != 3 || !double.TryParse(CVVField.Text, out _))
			{
				CVVField.HasError = true;
			}
			else
			{
				CVVField.HasError = false;
			}

		}

		private void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length == 4)
			{
				CardNumberField1.Focus();
			}
		}

		private void Entry2_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length == 4)
			{
				CardNumberField2.Focus();
			}
		}

		private void Entry3_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length == 4)
			{
				CardNumberField3.Focus();
			}
		}
	}
}