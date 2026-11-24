namespace PizzaFutar

{

    internal class Program
    {

        static void Main(string[] args)

        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Feladatok feladatok = new Feladatok();

            feladatok.Futtat();


            Console.WriteLine("\nA program futása befejeződött.");

            Console.ReadKey();

        }

    }

}