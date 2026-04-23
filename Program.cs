class Banqueiro
{
    private const int NUMBER_OF_CUSTOMERS = 5;
    private const int NUMBER_OF_RESOURCES = 3;
    int[] available = new int[NUMBER_OF_RESOURCES];
    int[,] maximum = new int[NUMBER_OF_CUSTOMERS, NUMBER_OF_RESOURCES];
    int[,] allocation = new int[NUMBER_OF_CUSTOMERS, NUMBER_OF_RESOURCES];
    int[,] need = new int[NUMBER_OF_CUSTOMERS, NUMBER_OF_RESOURCES];
    private readonly object lockBanqueiro = new object();

    // esse construtor preenche o available e já chama a função PreencheMaximum.
    public Banqueiro(int req1, int req2, int req3)
    {
        available[0] = req1;
        available[1] = req2;
        available[2] = req3;
        int[] limite = { req1, req2, req3 };
        PreencheMaximum(limite);
    }

    // o maximum diz respeito ao quanto máximo de recurso cada processo pode solicitar de cada recurso.
    public void PreencheMaximum(int[] limite)
    {
        Random aleatorio = new Random();
        for (int i = 0; i < maximum.GetLength(0); i++)
        {
            for (int j = 0; j < maximum.GetLength(1); j++)
            {
                maximum[i, j] = aleatorio.Next(0, limite[j] + 1);
            }
        }
    }

    // o que o processo precisa no momento
    // é: máximo - alocação
    // ele mostra quanto um processo ainda precisa para que seja executado
    public void Need()
    {
        for (int i = 0; i < maximum.GetLength(0); i++)
        {
            for (int j = 0; j < maximum.GetLength(1); j++)
            {
                need[i, j] = maximum[i, j] - allocation[i, j];
            }
        }
    }
    // O request solicita recursos, que devem ser conferidos se estão disponíveis, e, se estiverem, ele deve diminuir no available e no need, colocar no allocation.
    // O array request são os recursos que o cliente deseja.
    public int request_resources(int customer_num, int[] request)
    {
        lock (lockBanqueiro) // Lock que faz evitar que os clientes errem na contagem
        {
            // Verifica se a solicitação do cliente é válida.
            for (int i = 0; i < 3; i++)
            {
                if (request[i] > available[i] || request[i] > need[customer_num, i])
                    return -1;
            }
            //Faz uma espécie de "simulação", se a requisição é possível
            for (int i = 0; i < 3; i++)
            {
                available[i] -= request[i];
                allocation[customer_num, i] += request[i];
                Need();
            }

            // Confere se o resultado dessa simulação é válida, retornando 0, senão, devolve os recursos ("rollback") e retorna -1
            if (isSafe())
                return 0;
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    available[i] += request[i];
                    allocation[customer_num, i] -= request[i];
                    Need();
                }
                return -1;
            }
        }
    }
    public int release_resources(int customer_num, int[] release)
    {
        lock (lockBanqueiro) // Lock que faz evitar que os clientes errem na contagem
        {
            for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
            {
                available[i] += release[i]; // o cliente devolve o recurso
                allocation[customer_num, i] -= release[i]; // A alocação do cliente diminui
            }
            Need(); // Atualizamos a necessidade
            return 0;
        }
    }
    public bool isSafe()
    {
        int[] work = (int[])available.Clone(); // Cópia temporária dos recursos disponíveis, feita para que os recursos não sejam alterados no array correto.
        bool[] finish = new bool[NUMBER_OF_CUSTOMERS]; //Lista para marcar quem conseguiu terminar na simulação

        for (int i = 0; i < NUMBER_OF_CUSTOMERS; i++) // Nesse for tentamos encontrar uma sequência em que todos os consumidores conseguem terminar
        {
            bool achou = false; // Indica se algum cliente consegue ser atendido nessa rodada

            for (int j = 0; j < NUMBER_OF_CUSTOMERS; j++)
            {
                if (!finish[j]) // Para verificar se o cliente j ainda não terminou
                {
                    bool podeFinalizar = true;
                    for (int k = 0; k < NUMBER_OF_RESOURCES; k++) // Verificamos se o que o cliente j precisa é menor que o que temos disponível na cópia de available
                    {
                        if (need[j, k] > work[k])
                        {
                            podeFinalizar = false;
                            break;
                        }
                    }
                    //Se o cliente pode terminar significa que ele pegou os recursos, fez sua tarefa e devolveu tudo que ele tinha alocado
                    if (podeFinalizar)
                    {
                        for (int k = 0; k < NUMBER_OF_RESOURCES; k++)
                        {
                            work[k] += allocation[j, k];
                        }
                        finish[j] = true; // marcamos a simulação como concluída
                        achou = true; // achamos alguem que pode concluir sua tarefa, então marcamos como true;
                    }
                }
            }
            if (!achou) // Se durante o teste encontramos alguem que deixe o sistema inseguro isso significa que pode ter um deadlock, então quebramos quebramos
                break;
        }
        // verificamos se todos os clientes estão como true, se estiverem, então é seguro
        foreach (bool f in finish)
        {
            if (!f)
                return false; // alguém não era seguro, então retornamos como falso
        }
        return true; // o sistema é seguro
    }
}
class Program
{
    static void Main()
    {
        int req1, req2, req3;
        System.Console.WriteLine("Digite quanto do recurso 1 terá disponível");
        req1 = int.Parse(Console.ReadLine());
        System.Console.WriteLine("Digite quanto do recurso 2 terá disponível");
        req2 = int.Parse(Console.ReadLine());
        System.Console.WriteLine("Digite quanto do recurso 3 terá disponível");
        req3 = int.Parse(Console.ReadLine());

        Banqueiro banco = new Banqueiro(req1, req2, req3);

    }
}