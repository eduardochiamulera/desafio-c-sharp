
namespace Algorithm.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Program
    {
        /// <summary>
        /// PROBLEMA:
        /// 
        /// Implementar um algoritmo para o controle de posição de um drone em um plano cartesiano (X, Y).
        /// 
        /// O ponto inicial do drone é "(0, 0)" para cada execução do método Evaluate ao ser executado cada teste unitário.
        /// 
        /// A string de entrada pode conter os seguintes caracteres N, S, L, e O representando Norte, Sul, Leste e Oeste 
        /// respectivamente.
        /// Estes catacteres podem estar presentes aleatóriamente na string de entrada.
        /// Uma string de entrada "NNNLLL" irá resultar em uma posição final "(3, 3)", assim como uma string "NLNLNL" irá 
        /// resultar em "(3, 3)".
        /// 
        /// Caso o caracter X esteja presente, o mesmo irá cancelar a operação anterior. 
        /// Caso houver mais de um caracter X consecutivo, o mesmo cancelará mais de uma ação na quantidade em que o X 
        /// estiver presente.
        /// Uma string de entrada "NNNXLLLXX" irá resultar em uma posição final "(1, 2)" pois a string poderia ser simplificada 
        /// para "NNL".
        /// 
        /// Além disso, um número pode estar presente após o caracter da operação, representando o "passo" que a operação deve 
        /// acumular.
        /// Este número deve estar compreendido entre 1 e 2147483647.
        /// Deve-se observar que a operação 'X' não suporta opção de "passo" e deve ser considerado inválido. Uma string de entrada 
        /// "NNX2" deve ser considerada inválida.
        /// Uma string de entrada "N123LSX" irá resultar em uma posição final "(1, 123)" pois a string pode ser simplificada para 
        /// "N123L"
        /// Uma string de entrada "NLS3X" irá resultar em uma posição final "(1, 1)" pois a string pode ser siplificada para "NL".
        /// 
        /// Caso a string de entrada seja inválida ou tenha algum outro problema, o resultado deve ser "(999, 999)".
        /// 
        /// OBSERVAÇÕES:
        /// Realizar uma implementação com padrões de código para ambiente de "produção". 
        /// Comentar o código explicando o que for relevânte para a solução do problema.
        /// Adicionar testes unitários para alcançar uma cobertura de testes relevânte.
        /// </summary>
        /// <param name="input">String no padrão "N1N2S3S4L5L6O7O8X"</param>
        /// <returns>String representando o ponto cartesiano após a execução dos comandos (X, Y)</returns>
        
        public static string Evaluate(string input)
        {

            if (ValidacaoString(input))
                return "(999, 999)";

            input = input.Trim();
            var antiga = input;
            var x = 0;
            var y = 0;
            var listaComNumero = new List<string>();
            var listaLetras = new List<char>();

            input = RemoverX(input, antiga);

            SepararString(input, listaComNumero, listaLetras);

            foreach (var item in listaComNumero)
            {
                char letra;
                int numero;
                SepararLetrasNumero(item, out letra, out numero);

                if (numero < 1 || numero > int.MaxValue - 1)
                    return "(999, 999)";

                MontarPlanoCartesiano(ref x, ref y, letra, numero);
            }

            foreach (var item in listaLetras)
            {
                MontarPlanoCartesiano(ref x, ref y, item, null);
            }

            return $"({x}, {y})";
        }

        private static string RemoverX(string input, string antiga)
        {
            while (input.IndexOf('X') != -1)
            {
                var posicao = input.IndexOf("X");
                if ((posicao != input.Length - 1))
                {
                    if (Char.IsDigit(input[posicao + 1])) //valida se tem numero depois
                        return "";
                }

                input = input.Remove(posicao, 1); //removendo X
                if (Char.IsDigit(antiga[posicao - 1]))
                {
                    input = Regex.Replace(input, "[NSLO]{1}[0-9]+", "");
                }
                else
                {
                    input = input.Remove((posicao - 1), 1); //removendo anterior
                }
            }
            return input;
        }

        private static bool ValidacaoString(string input)
        {
            return string.IsNullOrEmpty(input) || input.Length == 0 || input.StartsWith("X") || input.IndexOf("N") < 0 && input.IndexOf("L") < 0 && input.IndexOf("O") < 0
                            && input.IndexOf("S") < 0 || Regex.IsMatch(input, "[X,x]+[0123456789]+") ||
                            string.IsNullOrWhiteSpace(input) || !Regex.IsMatch(input, "S|L|O|N+[0-9]") || Char.IsDigit(input[0]);
        }

        private static void MontarPlanoCartesiano(ref int x, ref int y, char letra, int? numero = null)
        {
            switch (letra)
            {
                case 'N':
                    if (numero.HasValue)
                        y += numero.Value;
                    else
                        y++;
                    break;
                case 'L':
                    if (numero.HasValue)
                        x += numero.Value;
                    else
                        x++;
                    break;
                case 'S':
                    if (numero.HasValue)
                        y += -numero.Value;
                    else
                        y--;
                    break;
                case 'O':
                    if (numero.HasValue)
                        x += -numero.Value;
                    else
                        x--;
                    break;
                default:
                    break;
            }
        }

        private static void SepararLetrasNumero(string item, out char letra, out int numero)
        {
            letra = item[0];
            var numString = item.Remove(0, 1);
            numero = int.Parse(numString);
        }

        private static void SepararString(string input, List<string> listaComNumero, List<char> listaLetras)
        {
            var j = 0;
            while (input.Length > 0)
            {
                var a = Regex.Match(input, "[NSLO]{1}[0-9]+").Value;
                if (a != "")
                {
                    listaComNumero.Add(a);
                    input = input.Replace(a, "");
                }
                else
                {
                    listaLetras.Add(input[0]);
                    input = input.Remove(0, 1);
                }
                j++;
            }
        }
    }
}
