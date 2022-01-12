using FitnessApp01.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp01.Helpers
{
    public static class CalorieGoalCalculator
    {
        //The Mifflin St Jeor equation
        //zdroj: https://blog.nasm.org/nutrition/resting-metabolic-rate-how-to-calculate-and-improve-yours
        //konstanty pro úroveň aktivity (ActivityDB)
        //zdroj: https://www.healthline.com/health/what-is-basal-metabolic-rate#How-many-calories-you-need-everyday
        public static int Calculate(RegistrationSettings rs)
        {
            int genderConstant = rs.GenderDB == "male" ? 5 : -161;
            double result = ((10 * rs.WeightDB)
                            + (6.25 * rs.HeightDB)
                            - (5 * rs.AgeDB) + genderConstant) * rs.ActivityDB;

            double speedConstant = 1;
            //Pokud uživatel mění DesiredWeightDB nebo WeightDB, může dojít k změně cíle
            //hubnutí
            if (rs.DesiredWeightDB < rs.WeightDB)
            {
                rs.GoalDB = 1;
                //zrychlit nabírání/hubnutí lze o 0.1(10%), 0.2(20%), 0.3(30%)
                speedConstant = 1 - rs.GoalSpeed;
            }
            //nabírání
            else if (rs.DesiredWeightDB > rs.WeightDB)
            {
                rs.GoalDB = 2;
                speedConstant = 1 + rs.GoalSpeed + 0.05;
            }
            //udržení
            else
            {
                rs.GoalDB = 3;
                //u udržení zrychlení neexistuje
            }

            result *= speedConstant;
            int actualRes = Convert.ToInt32(result);
            return actualRes;
        }
    }
}
