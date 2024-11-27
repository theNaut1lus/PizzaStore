namespace PizzaStore.DB;

//class for the Pizza data model
public record Pizza
{
    public int Id { get; set; }
    public string? Name { get; set; }

}

//in memory db
public class PizzaDB
{
    //list of objects of class 'Pizza'
    private static List<Pizza> _pizzas = new List<Pizza>()
    {
        new Pizza{ Id=1, Name="Montemagno, Pizza shaped like a great mountain" },
        new Pizza{ Id=2, Name="The Galloway, Pizza shaped like a submarine, silent but deadly"},
        new Pizza{ Id=3, Name="The Noring, Pizza shaped like a Viking helmet, where's the mead"}
    };

    //get the pizza list
    public static List<Pizza> GetPizzas()
    {
        return _pizzas;
    }

    //gets a particular pizza based on it's id
    public static Pizza? GetPizza(int id)
    {
        return _pizzas.SingleOrDefault(pizza => pizza.Id == id);
    }

    //create a pizza and append to list
    public static Pizza CreatePizza(Pizza pizza)
    {
        _pizzas.Add(pizza);
        return pizza;
    }

    //update a particular pizza
    public static Pizza UpdatePizza(Pizza update)
    {
        _pizzas = _pizzas.Select(pizza =>
        {
            if (pizza.Id == update.Id)
            {
                pizza.Name = update.Name;
            }
            return pizza;
        }).ToList();
        return update;
    }

    //delete a particular pizza
    public static void RemovePizza(int id)
    {
        _pizzas = _pizzas.FindAll(pizza => pizza.Id != id).ToList();
    }
}