namespace WeirdItemsWarehouse
{
    class Item
    {
        public string name { get; set; }
        public decimal weight_kg { get; set; }
        public int weirdness_level { get; set; }
        public bool is_fragile { get; set; }

        public Item(string Name, decimal Weight_kg, int Weirdness_level, bool Is_fragile)
        {
            name = Name;
            weight_kg = Weight_kg;
            weirdness_level = Weirdness_level;
            is_fragile = Is_fragile;
        }

        public string Description()
        {
            return "{\n\t Item description:\n" +
                   $"\t\"name\": \"{name}\",\n" +
                   $"\t\"weight_kg\": \"{weight_kg}\",\n" +
                   $"\t\"weirdness_level\": \"{weirdness_level}\",\n" +
                   $"\t\"is_fragile\": \"{(is_fragile ? "YES" : "NO")}\",\n" +
                   "}";
        }
    }

    class Warehouse
    {
        private List<Item> items;
        public int maximum_capacity { get; set; }
        public decimal maximum_total_weight { get; set; }

        public int current_amount_of_items => items.Count;

        public Warehouse(int Maximum_capacity, decimal Maximum_total_weight)
        {
            maximum_capacity = Maximum_capacity;
            maximum_total_weight = Maximum_total_weight;
            items = new List<Item>();
        }

        public (bool, string) add_item_to_warehouse(Item item)
        {
            decimal current_weight = 0;

            if (current_amount_of_items >= maximum_capacity)
            {
                return (false, "Error: The warehouse's full.");
            }

            foreach (var i in items)
            {
                current_weight += i.weight_kg;
            }

            if(current_weight + item.weight_kg > maximum_total_weight)
            {
                return (false, "Error: Exceeded the maximum weight for this warehouse.");
            }

            if (item.weirdness_level == 7 && item.is_fragile && current_amount_of_items >= maximum_capacity / 2)
            {
                return (false, "Error: The item is too risky with current warehouse capacity. (It's fragile and it's weirdness is level 7)");
            }

            items.Add(item);
            return (true, "Item added succesfully!");
        }

        public (bool, string) remove_item_from_warehouse(string item_name)
        {
            //Ten ziutek przeszukuje liste, zwraza pierwszy element który określił warunek, porównuje co ja chce usunąć z tym co jest w warehouse i ignoruje różnicę w małuch i dużych literach.
            var item = items.FirstOrDefault(i => i.name.Equals(item_name, StringComparison.OrdinalIgnoreCase));
            if (item == null)
                return (false, "Error: The item has not been found.");

            items.Remove(item);
            return (true, "The item has been removed successfully!");
        }

        public void print_all_items()
        {
            if (items.Count == 0)
            {
                Console.WriteLine("The warehouse's empty.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine(item.Description());
            }
        }

        public void print_fragile_or_heavy_items(decimal weight_threshold)
        {
            var filtered_items = items.Where(i => i.is_fragile || i.weight_kg > weight_threshold).ToList();
            if (filtered_items.Count == 0)
            {
                Console.WriteLine("There've been no fragile or heavy items found.");
                return;
            }

            foreach (var item in filtered_items)
            {
                Console.WriteLine(item.Description());
            }
        }

        public decimal calculate_average_weirdness()
        {
            if (items.Count != 0)
            {
                return Convert.ToDecimal(items.Average(i => i.weirdness_level));
            }
            else
            {
                return 0;
            }
        }
    }

    class WarehouseController
    {
        private List<Warehouse> warehouses;

        public WarehouseController()
        {
            warehouses = new List<Warehouse>();
        }

        public void create_new_warehouse()
        {
            Console.WriteLine("Enter the warehouse capacity (Integer): ");
            int capacity = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the maximum total weight of the warehouse (Kg): ");
            decimal maxWeight = Convert.ToDecimal(Console.ReadLine());

            var warehouse = new Warehouse(capacity, maxWeight);
            warehouses.Add(warehouse);

            Console.WriteLine("The new warehouse has been created successfully.");
        }

        public Warehouse select_warehouse()
        {
            Console.WriteLine("Select a warehouse (Number equal or more than 0): ");
            for (int i = 0; i < warehouses.Count; i++)
            {
                Console.WriteLine($"{i}. Warehouse {i + 1} - Capacity: {warehouses[i].maximum_capacity}, Max Weight: {warehouses[i].maximum_total_weight}");
            }

            int index = Convert.ToInt32(Console.ReadLine());

            if (index >= 0 && index < warehouses.Count)
            {
                return warehouses[index];
            }
            else
            {
                Console.WriteLine("Invalid selection.");
                return null;
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Collectible company Kuriozum Sp. z o. o.");

            WarehouseController warehouse_manager = new WarehouseController();

            while (true)
            {
                Console.Write("""
                              Choose an option:
                              1. Create new warehouse
                              2. Work with existing warehouse
                              3. Exit
                              """ + "\n");

                string option = Console.ReadLine();

                if (option == "1")
                {
                    warehouse_manager.create_new_warehouse();
                }
                else if (option == "2")
                {
                    Warehouse selected_warehouse = warehouse_manager.select_warehouse();

                    if (selected_warehouse != null)
                    {
                        while (true)
                        {
                            Console.Write("""
                                          Choose an option:
                                          1. Add item
                                          2. Remove item
                                          3. List all items
                                          4. Calculate average weirdness
                                          5. List fragile or heavy items
                                          6. Return to main menu
                                          """ + "\n");

                            string warehouse_option = Console.ReadLine();

                            if (warehouse_option == "1")
                            {
                                Console.WriteLine("Item name: ");
                                string name = Console.ReadLine();

                                Console.WriteLine("Weight (Kg): ");
                                decimal weight = Convert.ToDecimal(Console.ReadLine());

                                Console.WriteLine("Weirdness level (1-10): ");
                                int weirdness = Convert.ToInt32(Console.ReadLine());

                                Console.WriteLine("Is it fragile? (YES/NO): ");
                                bool isFragile = Console.ReadLine().Trim().ToLower() == "yes";

                                Item item = new Item(name, weight, weirdness, isFragile);
                                var (success, message) = selected_warehouse.add_item_to_warehouse(item);
                                Console.WriteLine(message);
                            }
                            else if (warehouse_option == "2")
                            {
                                Console.WriteLine("Enter the item name to remove: ");
                                string item_name = Console.ReadLine();

                                var (success, message) = selected_warehouse.remove_item_from_warehouse(item_name);
                                Console.WriteLine(message);
                            }
                            else if (warehouse_option == "3")
                            {
                                selected_warehouse.print_all_items();
                            }
                            else if (warehouse_option == "4")
                            {
                                decimal average_weirdness = selected_warehouse.calculate_average_weirdness();
                                Console.WriteLine($"Average weirdness in the warehouse: {average_weirdness}");
                            }
                            else if (warehouse_option == "5")
                            {
                                Console.WriteLine("Enter the weight threshold for heavy items: ");
                                decimal weight_threshold = Convert.ToDecimal(Console.ReadLine());

                                selected_warehouse.print_fragile_or_heavy_items(weight_threshold);
                            }
                            else if (warehouse_option == "6")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("This option doesn't exist.");
                            }
                        }
                    }
                }
                else if (option == "3")
                {
                    Console.WriteLine("Shutting down the program...");
                    break;
                }
                else
                {
                    Console.WriteLine("This option doesn't exist.");
                }
            }
        }
    }
}
