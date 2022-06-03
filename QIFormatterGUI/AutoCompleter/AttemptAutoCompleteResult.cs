using System.Collections.Generic;

namespace QIFormatterGUI.AutoCompleter
{
    public class AttemptAutoCompleteResult
    {
        public bool Matched { get; set; }
        public bool HasConflicts { get; set; }
        public string Memo { get; set; }
        public List<(string Payee, string Category)> Conflicts { get; set; }

        public AttemptAutoCompleteResult(string memo)
        {
            Memo = memo;
            Conflicts = new List<(string Payee, string Category)>();
        }
    }

}
