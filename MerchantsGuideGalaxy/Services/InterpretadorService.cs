using MerchantsGuideGalaxy.Classes;
using MerchantsGuideGalaxy.Enum;
using MerchantsGuideGalaxy.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantsGuideGalaxy.Services
{
    public class InterpretadorService
    {       

        private const string ERROR = "I have no idea what you are talking about";
        private int _posicao = 0;
        private string[] _palavras;
        private List<Simbolo> _simbolos = new List<Simbolo>();
        private Dictionary<Simbolo, Simbolo> _simbolosDefinicao = new Dictionary<Simbolo, Simbolo>();

        public (bool resultado, string mensagem) IdentificarComando(string comando)
        {
            if (string.IsNullOrEmpty(comando))
            {
                return (false, ERROR);
            }

            _palavras = comando.Split(' ');

            if (_palavras.Length == 0)
            {
                return (false, ERROR);
            }

            List<Simbolo> simbolos = new List<Simbolo>();

            _palavras.ToList().ForEach(palavra =>
            {
                switch (palavra)
                {
                    case "is":
                        simbolos.Add(new Simbolo { Nome = palavra, Tipo = TipoValor.Operacao });
                        break;
                    case "?":
                        simbolos.Add(new Simbolo { Nome = palavra, Tipo = TipoValor.Questao });
                        break;
                    case "I":
                    case "V":
                    case "X":
                    case "L":
                    case "C":
                    case "D":
                    case "M":
                        simbolos.Add(new Simbolo { Nome = palavra, Tipo = TipoValor.Romano });
                        break;
                    case "how":
                        simbolos.Add(new Simbolo { Nome = palavra, Tipo = TipoValor.Quanto });
                        break;
                    case "many":
                    case "much":
                        simbolos.Add(new Simbolo { Nome = palavra, Tipo = TipoValor.QuantoValor });
                        break;
                    default:
                        simbolos.Add(InformarValor(palavra));
                        break;
                }

                _posicao++;

            });

            if (IsPergunta(simbolos))
            {
                return ExecutarPergunta(simbolos);
            }
            else if (IsClassificacao(simbolos))
            {
                return ExecutarClassificacao(simbolos);
            }
            else if (IsConstante(simbolos))
            {
                return ExecutarConstante(simbolos);
            }
            else
            {
                return (false, ERROR);
            }
        }

        public (bool resultado, string mensagem) ExecutarConstante(List<Simbolo> comando)
        {


            return (true, "");
        }

        public (bool resultado, string mensagem) ExecutarClassificacao(List<Simbolo> comando)
        {
            var definicao = comando.Single(s => s.Tipo == TipoValor.Definicao);
            var unidade = comando.Single(s => s.Tipo == TipoValor.Valor);
            var valor = comando.Single(x => x.Tipo == TipoValor.Definicao).ToDecimal();

            unidade.Valor = valor / RecuperarValor(comando.Where(x => x.Tipo == TipoValor.Constante).ToList());

            if (!_simbolos.Any(x => x.Equals(definicao)))
                _simbolosDefinicao.Add(definicao, new List<Simbolo>());

            if (!_simbolosDefinicao[definicao].Contains(unidade))
                _simbolosDefinicao[definicao].Add(unidade);
            else
                return (false, $"Valor já informado");

            return (true, $"Valor Informado: {declaration}");
        }

        public (bool resultado, string mensagem) ExecutarPergunta(List<Simbolo> comando)
        {
            var queryType = comando.Single(x => x.Tipo == TipoValor.QuantoValor).Nome;

            string mensagem;

            var constants = comando.Where(x => x.Tipo == TipoValor.Constante).ToList();
            var valor = RecuperarValor(constants);
            var constantes = string.Join(" ", constants.Select(c => c.ToString()));

            if (queryType == "much")
                mensagem = $"{constantes} is {valor}";

            else
            {
                var classifier = comando.Single(x => x.Tipo == TipoValor.Definicao);
                var unit = comando.Single(x => x.Tipo == TipoValor.Valor);

                var tipo = _simbolos.First(x => x.Tipo == TipoValor.Romano && x.Nome == unit.Nome);

                valor *= tipo.Valor;

                mensagem = $"{constantes} {classifier} is {valor} {unit}";
            }
                       
            return (true, mensagem);
        }

        private decimal RecuperarValor(List<Simbolo> valores)
        {
            var valoresRomanos = new StringBuilder();

            valores.Select(c => _simbolos.Where(x => x.Tipo == TipoValor.Romano).ToList())
                     .ToList().ForEach(r => valoresRomanos.Append(r));

            var romanNumber = valoresRomanos.ToString();

            return romanNumber.RomanToInt();
        }

        private bool IsPergunta(List<Simbolo> valores)
        {
            return valores.First().Tipo == TipoValor.Quanto
                   && valores[1].Tipo == TipoValor.QuantoValor
                   && valores.Any(s => s.Tipo == TipoValor.Constante)
                   && valores.Any(s => s.Tipo == TipoValor.Operacao)
                   && valores.Last().Tipo == TipoValor.Questao;
        }

        private bool IsClassificacao(List<Simbolo> valores)
        {
            return valores.First().Tipo == TipoValor.Constante
                   && valores.Any(s => s.Tipo == TipoValor.Tipo)
                   && valores.Any(s => s.Tipo == TipoValor.Operacao)
                   && valores.Any(s => s.Tipo == TipoValor.Definicao)
                   && valores.Last().Tipo == TipoValor.Valor;
        }

        private bool IsConstante(List<Simbolo> valores)
        {
            return valores.Count == 3
                              && valores.First().Tipo == TipoValor.Constante
                              && valores.Any(s => s.Tipo == TipoValor.Operacao)
                              && valores.Last().Tipo == TipoValor.Romano;
        }

        private Simbolo InformarValor(string comando)
        {


            if (double.TryParse(comando, out var doubleTest))
            {
                return new Simbolo { Nome = comando, Tipo = TipoValor.Definicao };
            }

            var anterior = _posicao == _palavras.Length - 1 ? null : _palavras[_posicao + 1];

            if (anterior == null)
            {
                return new Simbolo { Tipo = TipoValor.Constante, Nome = comando };
            }

            var proximo = _posicao == 0 ? null : _palavras[_posicao - 1];

            if (proximo == null || (IsQuanto(anterior) && IsOperacao(proximo)))
            {
                return new Simbolo { Tipo = TipoValor.Valor, Nome = comando };
            }

            if (!IsQuanto(anterior) && IsOperacao(proximo))
            {
                return new Simbolo { Tipo = TipoValor.Tipo, Nome = comando };
            }

            if (IsQueryQualifier(proximo) && IsMensurableCommand())
            {
                return new Simbolo { Tipo = TipoValor.Tipo, Nome = comando };
            }

            return new Simbolo { Tipo = TipoValor.Constante, Nome = comando };
        }

        private bool IsOperacao(string valor)
        {
            return valor == "is";
        }

        private bool IsQuanto(string valor)
        {
            return valor == "many" || valor == "much";
        }

        private bool IsQueryQualifier(string valor)
        {
            return valor == "?";
        }

        private bool IsMensurableCommand()
        {
            return _palavras.Contains("many");
        }
    }
}
