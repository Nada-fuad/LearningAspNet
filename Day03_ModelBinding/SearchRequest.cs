using Microsoft.AspNetCore.Mvc;

namespace Day03_ModelBinding
{
    public class SearchRequest 
    {
        public string Id { get; set; }
        public String Name { get; set; }
        public string Description { get; set; }
    }
}
