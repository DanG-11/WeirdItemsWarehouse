namespace WeirdItemsWarehouse
{
    class Item
    {
        public string name { get; set; }
        public double weight_kg { get; set; }
        public int weirdness_level { get; set; }
        public bool is_fragile { get; set; }

        public Item(string Name, double Weight_kg, int Weirdness_level, bool Is_fragile)
        {
            name = Name;
            weight_kg = Weight_kg;
            weirdness_level = Weirdness_level;
            is_fragile = Is_fragile;
        }

        public string Description()
        {
            return "{\n\t Item description:" +
                   $"\t\"name\": \"{name}\",\n" +
                   $"\t\"weight_kg\": \"{weight_kg}\",\n" +
                   $"\t\"weirdness_level\": \"{weirdness_level}\",\n" +
                   $"\t\"is_fragile\": \"{(is_fragile ? "YES" : "NO")}\",\n" +
                   "}";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
