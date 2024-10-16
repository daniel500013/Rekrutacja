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

[assembly: Worker(typeof(CalculatorWorker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Template
{
    public sealed class CalculatorWorker
    {
        public class CalculatorWorkerParametry : ContextBase
        {
            [Caption("A")]
            [Priority(1)]
            public double VariableA { get; set; }

            [Caption("B")]
            [Priority(2)]
            public double VariableB { get; set; }

            [Caption("Data obliczeń")]
            [Priority(3)]
            public Date DataObliczen { get; set; }

            [Caption("Operacja")]
            [Priority(4)]
            public OperationEnum Operation { get; set; }

            public CalculatorWorkerParametry(Context context) : base(context)
            {
                this.DataObliczen = Date.Today;
            }
        }
        
        [Context]
        public Context Cx { get; set; }
        
        [Context]
        public CalculatorWorkerParametry Parametry { get; set; }

        [Action("Kalkulator",
           Description = "Prosty kalkulator ",
           Priority = 10,
           Mode = ActionMode.ReadOnlySession,
           Icon = ActionIcon.Accept,
           Target = ActionTarget.ToolbarWithText)]
        public dynamic WykonajAkcje()
        {
            //Większość z tych komunikatów informacyjnych (MessageBoxInformation) można usunąć, ponieważ te przypadki są już obsługiwane przez helpery. Jednak zostawiam je, ponieważ komunikaty te lepiej prezentują się w UI

            if (Parametry.Operation == OperationEnum.Division && Parametry.VariableB == 0)
                return new MessageBoxInformation("Błąd") { Text = "Nie da się dzielić przez 0" };

            DebuggerSession.MarkLineAsBreakPoint();

            Pracownik[] pracownicy = this.Cx[typeof(Pracownik[])] as Pracownik[];

            if (pracownicy.IsEmptyOrNull())
                return new MessageBoxInformation("Błąd") { Text = "Nie wybrano pracownika" };

            if (!Enum.IsDefined(typeof(OperationEnum), Parametry.Operation))
                return new MessageBoxInformation("Błąd") { Text = "Nie ma takiego operatora" };

            double result =
                CalculatorHelper.Calculate(Parametry.Operation, Parametry.VariableA, Parametry.VariableB);

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
    }
}