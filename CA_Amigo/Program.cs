using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_Amigo
{
    class Program
    {
        class Amigo
        {
            public string Nome { get; set; }
            public int latitude { get; set; }
            public int longitude { get; set; }
            public double distancia { get; set; }
        }

        class Visita
        {
            public Amigo Amigo { get; set; }
        }

        static void Main(string[] args)
        {
            List<Amigo> Amigos = new List<Amigo>();

            int opcao;
            do
            {
                Console.WriteLine("[ 1 ] Cadastrar amigo");
                Console.WriteLine("[ 2 ] Escolher Amigo para Visitar");
                Console.WriteLine("[ 9 ] Relatório de amigos próximos");
                Console.WriteLine("[ 0 ] Sair do Software");
                Console.WriteLine("-------------------------------------");
                Console.Write("Digite uma opção: ");
                opcao = Int32.Parse(Console.ReadLine());
                switch (opcao)
                {
                    case 1:
                        CadastrarAmigo(ref Amigos);
                        break;
                    case 2:
                        EscolherAmigoParaVisitar(ref Amigos);
                        break;
                    case 9:
                        RelatorioAmigos(Amigos);
                        break;
                    default:
                        Console.WriteLine("Escolher opção 1,2,9 ou 0 para sair");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }
            while (opcao != 0);
        }

        private static void RelatorioAmigos(List<Amigo> Amigos)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("***************|  AMIGOS PRÓXIMOS  |**************************");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("---------Amigo-----------------Latitude,Longitude,Distância-----");
          
            foreach (var item in Amigos)
            {
                Console.WriteLine(" {0}                                        {1},{2},{3}",item.Nome , item.latitude , item.longitude , item.distancia );
            }
    
            Console.WriteLine("----------------------------fim relatório----------------------");
        }

        private static void EscolherAmigoParaVisitar(ref List<Amigo> Amigos)
        {
            Console.Clear();
            
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("***************| ESCOLHER AMIGO PARA VISITAR |********************");
            Console.WriteLine("----------------------------------------------------------------");
            Console.Write("Digite o Nome do Amigo a visitar: ");
            string nome = Console.ReadLine();
            if (Amigos.Exists(x => x.Nome == nome))
            {
                Visita visita = new Visita();
                visita.Amigo  = Amigos.Find(x => x.Nome == nome);
                Console.WriteLine("Lista de 3 amigos próximos ao {0}", visita.Amigo.Nome);
                //Calcular distancias entre visita  e amigos
                Amigos = CalcularDistancia(visita, Amigos);
                
                List<Amigo> amigosProximos = new List<Amigo>();
                amigosProximos = Amigos.OrderBy(linha => linha.distancia).Take(4).ToList<Amigo>();
                RelatorioAmigos(amigosProximos);
                
                
            }
            else
            {
                Console.WriteLine("Amigo não cadastrado, favor cadastrar para poder visitar");
            }
            

          
        }

        private static void SaiPrograma()
        {
            Console.WriteLine();
            Console.WriteLine("Obrigado, vc saiu do Programa. Clique qq tecla para sair...");
        }

        private static void CadastrarAmigo(ref List<Amigo> amigos)
        {
            Console.Clear();
            bool jaExiste = false;
            Amigo amigo = new Amigo();
           
            do
            {
                jaExiste = false;
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("******************| CADASTRO DE AMIGOS |***********************");
                Console.WriteLine("----------------------------------------------------------------");
                Console.Write("Digite a latitude que deseja cadastrar: ");
                amigo.latitude = Int32.Parse(Console.ReadLine());
                Console.Write("Digite a longitute que deseja cadastrar: ");
                amigo.longitude = Int32.Parse(Console.ReadLine());
                
                jaExiste = VerificaCordenadaJaExiste(amigo,  amigos);

                if (jaExiste == false)
                {
                    Console.Write("Nome do amigo na Cordenada ({0},{1}): ", amigo.latitude, amigo.longitude);
                    amigo.Nome = Console.ReadLine();
                    amigos.Add(amigo);
                }
                else
                {
                    Console.WriteLine("Esta coordenada já está ocupada por outro amigo!");
                    amigo.Nome = "";
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            while (amigo.Nome.Length == 0);
            Console.WriteLine("Amigo cadastrado com Sucesso !");
        }

        private static bool VerificaCordenadaJaExiste(Amigo amigo, List<Amigo> amigos)
        {
            bool jaExiste = false;
            foreach (Amigo item in amigos)
            {
                if (item.latitude == amigo.latitude && item.longitude == amigo.longitude )
                {
                    jaExiste = true;
                    break;
                }
            }
            return jaExiste;
        }

        private static List<Amigo> CalcularDistancia(Visita Visita, List<Amigo> amigos)
        {
            foreach (Amigo item in amigos.ToList())
            {
                Amigo amigocalculado = new Amigo();
                var itemIndex = amigos.FindIndex(x => x.Nome == item.Nome && x.latitude == item.latitude && x.longitude == item.longitude);
                amigocalculado.Nome = item.Nome;
                amigocalculado.latitude = item.latitude;
                amigocalculado.longitude = item.longitude;
                amigocalculado.distancia = Math.Sqrt(Math.Pow(Visita.Amigo.latitude - amigos[itemIndex].latitude, 2) + Math.Pow(Visita.Amigo.longitude - amigos[itemIndex].longitude, 2));
                amigos.RemoveAt(itemIndex);
                amigos.Add(amigocalculado);
            }
            return amigos;
        }
    }

}
