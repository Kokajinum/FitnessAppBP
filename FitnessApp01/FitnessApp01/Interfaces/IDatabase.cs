using FitnessApp01.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FitnessApp01.Interfaces
{
    public interface IDatabase
    {
        Task CreateRegistrationSettingsAsync(RegistrationSettings registrationSettings);
        Task<RegistrationSettings> ReadRegistrationSettingsAsync();
        Task<List<Meal>> ReadMealDataAsync(string meal);
        /// <summary>
        /// Přečte data diáře z databáze
        /// </summary>
        /// <returns></returns>
        Task<Day> ReadDiaryDataAsync();
        Task CreateFoodDataAsync(Food food);
        Task CreateDayAsync(Day day);
        Task CreateMealAsync(Meal meal);
        Task UpdateMealAsync(Meal meal);
        Task UpdateDayAsync(Day day);
        Task DeleteMealAsync(Meal meal);
    }
}
