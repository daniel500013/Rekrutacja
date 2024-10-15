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

[assembly: Worker(typeof(TemplateWorker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Template
{
    public class TemplateWorker
    {
        public class TemplateWorkerParametry : ContextBase
        {
            [Caption("Data obliczeń")]
            public Date DataObliczen { get; set; }
            public TemplateWorkerParametry(Context context) : base(context)
            {
                this.DataObliczen = Date.Today;
            }
        }
        
        [Context]
        public Context Cx { get; set; }
        
        [Context]
        public TemplateWorkerParametry Parametry { get; set; }

        [Action("Kalkulator",
           Description = "Prosty kalkulator ",
           Priority = 10,
           Mode = ActionMode.ReadOnlySession,
           Icon = ActionIcon.Accept,
           Target = ActionTarget.ToolbarWithText)]
        public void WykonajAkcje()
        {
            DebuggerSession.MarkLineAsBreakPoint();

            Pracownik pracownik = null;
            if (this.Cx.Contains(typeof(Pracownik)))
            {
                pracownik = (Pracownik)this.Cx[typeof(Pracownik)];
            }

            using (Session nowaSesja = this.Cx.Login.CreateSession(false, false, "ModyfikacjaPracownika"))
            {
                using (ITransaction trans = nowaSesja.Logout(true))
                {
                    var pracownikZSesja = nowaSesja.Get(pracownik);

                    pracownikZSesja.Features["DataObliczen"] = this.Parametry.DataObliczen;

                    trans.CommitUI();
                }

                nowaSesja.Save();
            }
        }
    }
}