using System.Collections.Generic; 
namespace JSONFormatter
{ 

    public class Item
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public double ppu { get; set; }
        public Batters batters { get; set; }
        public List<Topping> topping { get; set; }
    }

}