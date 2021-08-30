using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using FitnessApp01.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp01.Services
{
    public class SearchBase
    {
        public SearchBase(string api, string indexName)
        {
            Client = new SearchClient("PKXN76JWVJ", api);
            Index = Client.InitIndex(indexName);
        }

        public async Task<SearchResponse<Food>> GetResultAsync(string searchString)
        {
            var result = await Index.SearchAsync<Food>(new Query(searchString)
            {
                HitsPerPage = 5
            });
            return result;
        }

        public List<Food> GetHits(SearchResponse<Food> searchResponse)
        {
            return searchResponse.Hits;
        }

        private SearchClient Client { get; set; }
        private SearchIndex Index { get; set; }
    }
}
