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
        public static int Calculate(RegistrationSettings rs)
        {
            double speedConstant = 0;
            int s = rs.GenderDB == "male" ? 5 : -161;
            double result = ((10 * rs.WeightDB)
                            + (6.25 * rs.HeightDB)
                            - (5 * rs.AgeDB) + s) * rs.ActivityDB;

            if (rs.DesiredWeightDB < rs.WeightDB)
            {
                rs.GoalDB = 1;
                speedConstant = 1 - rs.GoalSpeed;
            } 
            else if (rs.DesiredWeightDB > rs.WeightDB)
            {
                rs.GoalDB = 2;
                speedConstant = 1 + rs.GoalSpeed + 0.05;
            }
            else
            {
                rs.GoalDB = 3;
                speedConstant = 1;
            }

            result *= speedConstant;
            return (int)result;
        }
    }
}
