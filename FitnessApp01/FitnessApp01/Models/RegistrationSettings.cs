using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace FitnessApp01.Models
{
    public class RegistrationSettings : NotifyModel
    {
        [MapTo("email")]
        public string Email { get; set; }

        [ServerTimestamp(CanReplace = false), MapTo("created")]
        public Timestamp Created { get; set; }

        private int _ageDB;
        [MapTo("age")]
        public int AgeDB
        {
            get { return _ageDB; }
            set { SetProperty(ref _ageDB, value); }
        }

        private double _weightDB;
        [MapTo("weight")]
        public double WeightDB
        {
            get { return _weightDB; }
            set { SetProperty(ref _weightDB, value); }
        }

        private double _heightDB;
        [MapTo("height")]
        public double HeightDB
        {
            get { return _heightDB; }
            set { SetProperty(ref _heightDB, value); }
        }

        [MapTo("gender")]
        public string GenderDB { get; set; }

        [MapTo("activity")]
        public double ActivityDB { get; set; }

        //hubnuti,pribirani,udrzeni
        [MapTo("goal")]
        public int GoalDB { get; set; }

        private double _desiredWeightDB;
        [MapTo("desiredWeight")]
        public double DesiredWeightDB
        {
            get { return _desiredWeightDB; }
            set { SetProperty(ref _desiredWeightDB, value); }
        }

        [MapTo("weightUnit")]
        public string WeightMeasureDB { get; set; }

        [MapTo("desiredWeightUnit")]
        public string DesiredWeightMeasureDB { get; set; }


        private int _caloriesGoal;
        [MapTo("caloriesGoal")]
        public int CaloriesGoal
        {
            get { return _caloriesGoal; }
            set { SetProperty(ref _caloriesGoal, value); }
        }

        private double _goalSpeed;
        [MapTo("speed")]
        public double GoalSpeed
        {
            get { return _goalSpeed; }
            set { SetProperty(ref _goalSpeed, value); }
        }

        [MapTo("macros")]
        public IDictionary<string, int> Macros { get; set; } = new Dictionary<string, int>();

        [Ignored]
        public int ProteinMacro
        {
            get 
            {
                return Macros.Where(x => x.Key.Equals("protein")).FirstOrDefault().Value;
            }
        }

        [Ignored]
        public int CarbohydratesMacro
        {
            get 
            {
                var macro = Macros.Where(x => x.Key.Equals("carbohydrates")).FirstOrDefault().Value;
                return macro;
            }
        }
        [Ignored]
        public int FatMacro
        {
            get
            {
                return Macros.Where(x => x.Key.Equals("fat")).FirstOrDefault().Value;
            }
        }
    }
}
