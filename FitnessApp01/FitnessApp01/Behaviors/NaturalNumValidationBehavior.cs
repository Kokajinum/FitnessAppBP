using Xamarin.Forms;

namespace FitnessApp01.Behaviors
{
    public static class NaturalNumValidationBehavior
    {
        public static readonly BindableProperty AttachBehaviorProperty = BindableProperty.CreateAttached(
            "AttachBehavior",
            typeof(bool),
            typeof(NaturalNumValidationBehavior),
            false,
            propertyChanged: OnAttachBehaviorChanged);

        public static void OnAttachBehaviorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var entry = bindable as Entry;
            if (entry == null)
            {
                return;
            }

            bool attachBehavior = (bool)newValue;
            if (attachBehavior)
            {
                entry.TextChanged += OnEntryTextChanged;
            }
            else
            {
                entry.TextChanged -= OnEntryTextChanged;
            }
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = int.TryParse(e.NewTextValue, out int num) && num != 0;
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }

        public static bool GetAttachBehavior(BindableObject view)
        {
            return (bool)view.GetValue(AttachBehaviorProperty);
        }

        public static void SetAttachBehavior(BindableObject view, bool value)
        {
            view.SetValue(AttachBehaviorProperty, value);
        }


    }
}
