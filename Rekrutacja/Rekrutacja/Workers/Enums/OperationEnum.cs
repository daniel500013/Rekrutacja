using Soneta.Types;

namespace Rekrutacja.Workers.Enums
{
    public enum OperationEnum
    {
        [Caption("+")]
        Add,

        [Caption("-")]
        Subtraction,

        [Caption("*")]
        Multiplication,

        [Caption("/")]
        Division,
    }
}
