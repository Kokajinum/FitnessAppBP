using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp01.Models
{
    public class RegistrationSettings
    {
        [MapTo("email")]
        public string Email { get; set; }
        [ServerTimestamp(CanReplace = false), MapTo("created")]
        public Timestamp Created { get; set; }
        [MapTo("age")]
        public int AgeDB { get; set; }
        [MapTo("weight")]
        public double WeightDB { get; set; }
        [MapTo("height")]
        public double HeightDB { get; set; }
        [MapTo("gender")]
        public string GenderDB { get; set; }
        [MapTo("activity")]
        public double ActivityDB { get; set; }
        [MapTo("goal")]
        public int GoalDB { get; set; }
        [MapTo("desiredWeight")]
        public double DesiredWeightDB { get; set; }
        [MapTo("weightUnit")]
        public string WeightMeasureDB { get; set; }
        [MapTo("desiredWeightUnit")]
        public string DesiredWeightMeasureDB { get; set; }
        [MapTo("caloriesGoal")]
        public int CaloriesGoal { get; set; }
        [MapTo("macros")]
        public IDictionary<string, double> Macros { get; set; } = new Dictionary<string, double>();
    }
}
