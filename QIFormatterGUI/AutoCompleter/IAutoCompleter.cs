using QifApi.Transactions;

namespace QIFormatterGUI.AutoCompleter
{
    public interface IAutoCompleter
    {
        AttemptAutoCompleteResult AttemptAutoComplete(BasicTransaction trans);
        string GetAutoCompleteSummary();
    }
}