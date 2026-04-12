using System.Diagnostics;

class Banqueiro
{
    private const int NUMBER_OF_CUSTOMERS = 5;
    private const int NUMBER_OF_RESOURCES = 10;
    int[] available = new int[NUMBER_OF_RESOURCES];
    int[,] maximum = new int[NUMBER_OF_CUSTOMERS,NUMBER_OF_RESOURCES];
    int[,] allocation = new int[NUMBER_OF_CUSTOMERS,NUMBER_OF_RESOURCES];
    int[,] need = new int[NUMBER_OF_CUSTOMERS,NUMBER_OF_RESOURCES];

}
class Program
{
    static void Main()
    {

    }
}