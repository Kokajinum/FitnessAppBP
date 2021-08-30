using FitnessApp01.Services;
using Plugin.CloudFirestore.Attributes;
using System.ComponentModel;

namespace FitnessApp01.Models
{
    public class Food
    {
        public Food(string name, int kcal, double carbs, double sugar,
            double protein, double fat, string who, string measure, double saturated = 0,
            double fiber = 0, double salt = 0, string brand = "", double portionSize = 0)
        {
            Name = name;
            Brand = brand;
            Kcal = kcal;
            Carbohydrates = carbs;
            Sugar = sugar;
            Protein = protein;
            Fat = fat;
            SaturatedFat = saturated;
            Fiber = fiber;
            Salt = salt;
            PortionSize = portionSize;
            Measure = measure;
            Who = who;
        }

        public Food()
        {

        }

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
        [MapTo("portionSize")]
        public double PortionSize { get; set; }
        [MapTo("measure")]
        public string Measure { get; set; }
        [MapTo("who")]
        public string Who { get; set; }

    }
}