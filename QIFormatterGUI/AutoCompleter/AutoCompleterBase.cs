using System.Collections.Generic;
using System.Linq;
using QifApi.Transactions;

namespace QIFormatterGUI.AutoCompleter
{
    // Could be abstract, but it can be used as it is, just for the summary
    public class AutoCompleterBase : IAutoCompleter
    {
        protected List<AttemptAutoCompleteResult> AutoCompleteResults { get; }

        public AutoCompleterBase()
        {
            AutoCompleteResults = new List<AttemptAutoCompleteResult>();
        }

        protected virtual void DoAutoCompleteMagic(BasicTransaction trans, AttemptAutoCompleteResult result,
            string cleanMemo)
        {

        }

        public AttemptAutoCompleteResult AttemptAutoComplete(BasicTransaction trans)
        {
            var cleanMemo = trans.Memo.ToLower()
                .Replace('á', 'a')
                .Replace('é', 'e')
                .Replace('í', 'i')
                .Replace('ó', 'o')
                .Replace('ú', 'u');

            var result = new AttemptAutoCompleteResult(trans.Memo);
            AutoCompleteResults.Add(result);

            DoAutoCompleteMagic(trans, result, cleanMemo);

            return result;
        }

        public virtual string GetAutoCompleteSummary()
        {
            var conflictsCount = 0;
            var conflictsText = string.Empty;
            var nonMatchedCount = 0;
            var nonMatchedText = string.Empty;

            foreach (var result in AutoCompleteResults)
            {
                if (result.HasConflicts)
                {
                    conflictsCount++;
                    conflictsText += $"Memo: {result.Memo}\n";
                    foreach (var conflictTuple in result.Conflicts)
                    {
                        conflictsText += $"\t{conflictTuple.Payee}, {conflictTuple.Category}\n";
                    }
                }
                else if (!result.Matched)
                {
                    nonMatchedCount++;
                    nonMatchedText += $"Memo: {result.Memo}\n";
                }
            }

            var msgText =
                $"Saved: {AutoCompleteResults.Count}\n" +
                $"AutoCompleted: {AutoCompleteResults.Count(r => r.Matched)}\n\n" +
                $"NonMatched: {nonMatchedCount}\n" +
                (nonMatchedCount > 0 ? $"\n{nonMatchedText}\n" : string.Empty) +
                $"Conflicts: {conflictsCount}\n" +
                (conflictsCount > 0 ? $"\n{conflictsText}" : string.Empty);

            return msgText;
        }
    }
}