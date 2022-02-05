using Plugin.CloudFirestore.Attributes;

namespace FitnessApp01.Models
{
    public class Meal
    {
        [MapTo("name")]
        public string Name { get; set; }
        [MapTo("brand")]
        public string Brand { get; set; }
        [MapTo("kcal")]
        public int Kcal { get; set; }
        [MapTo("carbohydrates")]
        public double Carbohydrates { get; set; }
        [MapTo("sugar")]
        public double Sugar { get; set; }
        [MapTo("protein")]
        public double Protein { get; set; }
        [MapTo("fat")]
        public double Fat { get; set; }
        [MapTo("saturatedFat")]
        public double SaturatedFat { get; set; }
        [MapTo("fiber")]
        public double Fiber { get; set; }
        [MapTo("salt")]
        public double Salt { get; set; }
        [MapTo("weight")]
        public double Weight { get; set; }
        [MapTo("mealType")]
        public string MealType { get; set; }
        [MapTo("measure")]
        public string Measure { get; set; }
        [MapTo("kcalOrig")]
        public int KcalOrig { get; set; }
        [MapTo("carbohydratesOrig")]
        public double CarbohydratesOrig { get; set; }
        [MapTo("sugarOrig")]
        public double SugarOrig { get; set; }
        [MapTo("proteinOrig")]
        public double ProteinOrig { get; set; }
        [MapTo("fatOrig")]
        public double FatOrig { get; set; }
        [MapTo("saturatedFatOrig")]
        public double SaturatedFatOrig { get; set; }
        [MapTo("fiberOrig")]
        public double FiberOrig { get; set; }
        [MapTo("saltOrig")]
        public double SaltOrig { get; set; }

        public Meal()
        {

        }
        public Meal(string name, string brand, double weight, int kcal, double carbs, double sugar,
            double protein, double fat, double saturated, double fiber, double salt, string mealType,
            int kcalOrig, double carbohydratesOrig, double sugarOrig, double proteinOrig, double fatOrig,
            double saturatedFatOrig, double fiberOrig, double saltOrig, string measure)
        {
            Name = name;
            Weight = weight;
            Kcal = kcal;
            Brand = brand;
            Carbohydrates = carbs;
            Sugar = sugar;
            Protein = protein;
            Fat = fat;
            SaturatedFat = saturated;
            Fiber = fiber;
            Salt = salt;
            MealType = mealType;
            KcalOrig = kcalOrig;
            CarbohydratesOrig = carbohydratesOrig;
            SugarOrig = sugarOrig;
            ProteinOrig = proteinOrig;
            FatOrig = fatOrig;
            SaturatedFatOrig = saturatedFatOrig;
            FiberOrig = fiberOrig;
            SaltOrig = saltOrig;
            Measure = measure;
        }
    }   
}
