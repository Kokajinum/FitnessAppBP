using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FitnessApp01.Services
{
    public class RegistrationDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AgeInputTemplate { get; set; }
        public DataTemplate WeightInputTemplate { get; set; }
        public DataTemplate HeightInputTemplate { get; set; }
        public DataTemplate GenderInputTemplate { get; set; }
        public DataTemplate ActivityInputTemplate { get; set; }
        public DataTemplate GoalInputTemplate { get; set; }
        public DataTemplate DesiredWeightInputTemplate { get; set; }
        public int Counter { get; set; } = 0;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch ((string)item)
            {
                case "1":
                    return AgeInputTemplate;
                case "2":
                    return WeightInputTemplate;
                case "3":
                    return HeightInputTemplate;
                case "4":
                    return GenderInputTemplate;
                case "5":
                    return ActivityInputTemplate;
                case "6":
                    return GoalInputTemplate;
                case "7":
                    return DesiredWeightInputTemplate;
                default:
                    return AgeInputTemplate;
            }
        }
    }
}
