using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CardapioWP7
{
    class ProcessHelper
    {
        public List<DateTime> Semana { get; private set; }

        public void ProcessInfo(string _info, string _periodo)
        {
            #region Exemplo de Entrada
            /*  Exemplo de entrada
             * 
             * <span class="style2">&raquo; Per&iacute;odo: </span><span class="style3">De 16/09 a 22/09</span><br>                            <br>
<strong>Segunda-feira<br></strong>(16/09/2013)<br>ALMO&Ccedil;O<br />
Maionese de repolho com abacaxi<br />
Frango assado<br />
Feijao Carioca<br />
Arroz Colorido<br />
Farofa Carioca<br />
Suco<br />
Sobremesa<br />
VEGETARIANA:<br />
Quibe<br />
Arroz Integral<br />
<br />
JANTAR:<br />
Sopa de feijao<br />
Cuscuz paulista<br />
Pao<br />
Cafe<br />
Fruta<br />
VEGETARIANA:<br />
Nhoque de jerimum a bolonhesa<br />
Arroz Integral<br><br><strong>Terça-feira<br></strong>(17/09/2013)<br>ALMO&Ccedil;O:<br />
Batata gratinada ao molho branco<br />
Carne chinesa<br />
Feijao preto<br />
Arroz refogado<br />
Farofa de alho<br />
Suco<br />
Sobremesa<br />
VEGETARIANA:<br />
Torta de aveia a pizzaiolo<br />
Arroz Integral<br />
<br />
JANTAR:<br />
Sopa de carne<br />
Lasanha de frango<br />
Pao<br />
Suco<br />
Fruta<br />
VEGETARIANA:<br />
Abobrinha recheada<br />
Arroz Integral<br><br><strong>Quarta-feira<br></strong>(18/09/2013)<br>ALMO&Ccedil;O<br />
Salada Crua<br />
Carne de sol acebolada<br />
Feijao Branco<br />
Arroz refogado<br />
Farofa d'agua<br />
Suco<br />
Sobremesa<br />
VEGETARIANA:<br />
Berinjela a parmegiana<br />
Arroz Integral<br />
<br />
JANTAR:<br />
Sopa de legumes<br />
Strogonoff de frango<br />
Macarrao ao alho e oleo<br />
Pao<br />
Suco<br />
Fruta<br />
VEGETARIANA:<br />
Gratinado divertido<br />
Arroz Integral<br><br><strong>Quinta-feira<br></strong>(19/09/2013)<br>ALMO&Ccedil;O<br />
Salada crua especial<br />
Cubos de frango ao molho de manga<br />
Feijao carioca<br />
Arroz refogado<br />
Farofa carioca<br />
Suco<br />
Sobremesa<br />
VEGETARIANA:<br />
Gratinado de abobrinha com tomate<br />
Arroz integral <br />
<br />
JANTAR<br />
Sopa de frango<br />
Picadinho nordestino com macaxeira<br />
Cuscuz com verduras<br />
Pao <br />
Cafe<br />
Fruta<br><br><strong>Sexta-feira<br></strong>(20/09/2013)<br>ALMO&Ccedil;O <br />
Couve com cenoura<br />
Feijoada completa<br />
Arroz refogado<br />
Farofa carioca<br />
Suco <br />
Sobremesa <br />
VEGETARIANA:<br />
Quibe assado de batata<br />
Arroz integral<br />
<br />
JANTAR<br />
Papa de maisena com chocolate<br />
Torta de carne<br />
Arroz refogado<br />
Pao <br />
Cafe<br />
Fruta<br><br><strong>Sábado<br></strong>(21/09/2013)<br>ALMO&Ccedil;O<br />
Macaxeira amanteigada <br />
Peixe empanado<br />
Feijao tropeiro<br />
Arroz com cenoura<br />
Farofa carioca<br />
Suco <br />
Sobremesa <br />
<br />
JANTAR<br />
Creme caipira<br />
Omelete de frango<br />
Arroz refogado<br />
Pao<br />
Suco <br />
Sobremesa<br><br><strong>Domingo<br></strong>(22/09/2013)<br>ALMO&Ccedil;O<br />
Salada de brocolis<br />
Iscas de frango ao molho agridoce<br />
Feijao preto<br />
Arroz refogado<br />
Farofa de alho<br />
Suco<br />
Sobremesa <br />
<br />
JANTAR<br />
Canja<br />
Strogonoff de carne<br />
Arroz refogado<br />
Pao <br />
Suco<br />
Sobremesa<br><br>
<!--                            <strong>Segunda-Feira<br> 
                            </strong>(03/01/2005)<br> 
                            Arroz, Feij&atilde;o, Macaxeira, bl&aacute; bl&aacute; bl&aacute; nonononon onnono nonon nonoonnoon nononno nononoon ononn onnoonnoon onon nonon nononon nononnono nonononno ono n no.<br>
                            <br>                            
                            <strong>Ter&ccedil;a-Feira<br>
                            </strong>(04/01/2005)<br>
Arroz, Feij&atilde;o, Macaxeira, bl&aacute; bl&aacute; bl&aacute; nonononon onnono nonon nonoonnoon nononno nononoon ononn onnoonnoon onon nonon nononon nononnono nonononno ono n no.<br>
<br>
<strong>Quarta-Feira<br>
</strong>(05/01/2005)<br>
Arroz, Feij&atilde;o, Macaxeira, bl&aacute; bl&aacute; bl&aacute; nonononon onnono nonon nonoonnoon nononno nononoon ononn onnoonnoon onon nonon nononon nononnono nonononno ono n no.<br>
<br>
<strong>Quinta-Feira<br>
</strong>(06/01/2005)<br>
Arroz, Feij&atilde;o, Macaxeira, bl&aacute; bl&aacute; bl&aacute; nonononon onnono nonon nonoonnoon nononno nononoon ononn onnoonnoon onon nonon nononon nononnono nonononno ono n no.<br>
<br>
<strong>Sexta-Feira<br>
</strong>(07/01/2005)<br>
Arroz, Feij&atilde;o, Macaxeira, bl&aacute; bl&aacute; bl&aacute; nonononon onnono nonon nonoonnoon nononno nononoon ononn onnoonnoon onon nonon nononon nononnono nonononno ono n no.<br>
-->
<br><br><br>
             * 
             */
            #endregion

            string Periodo = _periodo;

            Regex regex_datas = new Regex(@"\d+");
            var datas = regex_datas.Matches(Periodo);
            int dia_inicial = int.Parse(datas[0].ToString());
            int mes_inicial = int.Parse(datas[1].ToString());
            int dia_final = int.Parse(datas[2].ToString());
            int mes_final = int.Parse(datas[3].ToString());

            DateTime inicio = new DateTime(DateTime.Today.Year, mes_inicial, dia_inicial);
            DateTime fim = new DateTime(DateTime.Today.Year, mes_final, dia_final);

            for (int i = 0; true; i++)
            {
                DateTime dia = inicio.AddDays(i);
                Semana.Add(dia);
                if (dia.CompareTo(fim) == 0)
                    break;
            }
        }
    }
}
