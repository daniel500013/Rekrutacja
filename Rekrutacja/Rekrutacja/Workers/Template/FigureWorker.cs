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
using Rekrutacja.Workers.Enums;
using Soneta.Tools;
using Soneta.Business.UI;
using Rekrutacja.Workers.Helper;

[assembly: Worker(typeof(FigureWorker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Template
{
    public class FigureWorker
    {
        public class FigureWorkerParametry : ContextBase
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
            public FigureEnum Operation { get; set; }

            public FigureWorkerParametry(Context context) : base(context)
            {
                this.DataObliczen = Date.Today;
            }
        }

        [Context]
        public Context Cx { get; set; }

        [Context]
        public FigureWorkerParametry Parametry { get; set; }

        [Action("Figury",
           Description = "Prosty kalkulator do obliczania pola figury ",
           Priority = 20,
           Mode = ActionMode.ReadOnlySession,
           Icon = ActionIcon.Accept,
           Target = ActionTarget.ToolbarWithText)]
        public dynamic WykonajAkcje()
        {
            DebuggerSession.MarkLineAsBreakPoint();

            Pracownik[] pracownicy = null;

            if (this.Cx.Contains(typeof(Pracownik[])))
            {
                pracownicy = (Pracownik[])this.Cx[typeof(Pracownik[])];
            }

            if (pracownicy.IsEmptyOrNull())
                return new MessageBoxInformation("Błąd") { Text = "Nie wybrano pracownika" };

            if (!Enum.IsDefined(typeof(FigureEnum), Parametry.Operation))
                return new MessageBoxInformation("Błąd") { Text = "Nie ma takiej figury" };

            if (Parametry.Operation == FigureEnum.Square && Parametry.VariableA != Parametry.VariableB)
                return new MessageBoxInformation("Błąd") { Text = "Boki kwadratu muszą być tej samej długości" };

            if (Parametry.VariableA.IsNegativeOrZero() || (Parametry.VariableB.IsNegativeOrZero() && Parametry.Operation != FigureEnum.Circle))
                return new MessageBoxInformation("Błąd") { Text = "Figura nie może mieć zerowej lub minusowej długości" };

            double result =
                FigureHelper.Calculate(Parametry.Operation, Parametry.VariableA, Parametry.VariableB);

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