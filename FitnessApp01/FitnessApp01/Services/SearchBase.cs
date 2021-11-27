using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp01.Services
{
    public class SearchBase : ISearch<Food>
    {
        public SearchBase(string api, string indexName)
        {
            Client = new SearchClient("PKXN76JWVJ", api);
            Index = Client.InitIndex(indexName);
        }

        public async Task<IEnumerable<Food>> GetResultsAsync(string searchString)
        {
            var response = await GetResponseAsync(searchString);
            return GetHits(response);
        }

        private async Task<SearchResponse<Food>> GetResponseAsync(string searchString)
        {
            var result = await Index.SearchAsync<Food>(new Query(searchString)
            {
                HitsPerPage = 8
            });
            return result;
        }

        private IEnumerable<Food> GetHits(SearchResponse<Food> searchResponse)
        {
            return searchResponse.Hits;
        }

        private SearchClient Client { get; set; }
        private SearchIndex Index { get; set; }
    }
}
