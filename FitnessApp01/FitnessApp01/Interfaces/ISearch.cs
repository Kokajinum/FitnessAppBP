using FitnessApp01.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp01.Interfaces
{
    public interface ISearch<T>
    {
        Task<IEnumerable<T>> GetResultsAsync(string searchString);
    }
}
