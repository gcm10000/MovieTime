using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTimeCore
{
    public class MovieInList
    {
        public string Name { get; }
        public string Image { get; }
        public string Year { get; }
        public MovieInList(string Name, string Image, string Year)
        {
            this.Name = Name;
            this.Image = Image;
            this.Year = Year;
        }
    }
}
