using Rekrutacja.Extensions;
using Soneta.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.Kadry;
using Soneta.KadryPlace;
using Soneta.Types;
using Rekrutacja.Workers.Template;
using Soneta.Produkcja;
using Soneta.Tools;
using Rekrutacja.Workers.Enums;
using Rekrutacja.Workers.Helper;
using Soneta.Business.UI;

[assembly: Worker(typeof(ParserWorker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Template
{
    public sealed class ParserWorker
    {
        public class ParserWorkerParametry : ContextBase
        {
            [Caption("A")]
            [Priority(1)]
            public string VariableA { get; set; }

            [Caption("B")]
            [Priority(2)]
            public string VariableB { get; set; }

            [Caption("Data obliczeń")]
            [Priority(3)]
            public Date DataObliczen { get; set; }

            [Caption("Operacja")]
            [Priority(4)]
            public OperationEnum Operation { get; set; }

            public ParserWorkerParametry(Context context) : base(context)
            {
                this.DataObliczen = Date.Today;
            }
        }
        
        [Context]
        public Context Cx { get; set; }
        
        [Context]
        public ParserWorkerParametry Parametry { get; set; }

        [Action("Kalkulator (parser zadanie 3)",
           Description = "Prosty kalkulator który parsuje znaki na liczby ",
           Priority = 30,
           Mode = ActionMode.ReadOnlySession,
           Icon = ActionIcon.Accept,
           Target = ActionTarget.ToolbarWithText)]
        public dynamic WykonajAkcje()
        {
            //ContainsNonNumeric to moja własna funkcja która znajduje się w Extensions > StringExtensions
            if (Parametry.VariableA.ContainsNonNumeric() || Parametry.VariableB.ContainsNonNumeric())
                return new MessageBoxInformation("Błąd") { Text = "W pola muszą być wpisane liczby a nie znaki" };

            if (Parametry.Operation == OperationEnum.Division && StringToInt(Parametry.VariableB) == 0)
                return new MessageBoxInformation("Błąd") { Text = "Nie da się dzielić przez 0" };

            DebuggerSession.MarkLineAsBreakPoint();

            Pracownik[] pracownicy = null;

            if (this.Cx.Contains(typeof(Pracownik[])))
            {
                pracownicy = (Pracownik[])this.Cx[typeof(Pracownik[])];
            }

            if (pracownicy.IsEmptyOrNull())
                return new MessageBoxInformation("Błąd") { Text = "Nie wybrano pracownika" };

            if (!Enum.IsDefined(typeof(OperationEnum), Parametry.Operation))
                return new MessageBoxInformation("Błąd") { Text = "Nie ma takiego operatora" };

            double result =
                CalculatorHelper.Calculate(Parametry.Operation, StringToInt(Parametry.VariableA), StringToInt(Parametry.VariableB));

            using (Session nowaSesja = this.Cx.Login.CreateSession(false, false, "ModyfikacjaPracownika"))
            {
                using (ITransaction trans = nowaSesja.Logout(true))
                {
                    foreach (var pracownik in pracownicy)
                    {
                        var pracownikZSesja = nowaSesja.Get(pracownik);

                        pracownikZSesja.Features["DataObliczen"] = this.Parametry.DataObliczen;
                        pracownikZSesja.Features["Wynik"] = result;

                        trans.CommitUI();
                    }
                }

                nowaSesja.Save();
            }

            return null;
        }

        public int StringToInt(string str)
        {
            bool isNegative = false;

            if (str.StartsWith("-"))
            {
                isNegative = true;
                str = str.Substring(1);
            }

            int answer = 0;
            int factor = 1;

            for (int i = str.Length - 1; i >= 0; i--)
            {
                char c = str[i];

                int value = c - '0';
                answer += value * factor;

                factor *= 10;
            }

            if (isNegative)
            {
                answer *= -1;
            }

            return answer;
        }
    }
}