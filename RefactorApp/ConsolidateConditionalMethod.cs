using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RefactoringApp
{
    public class ConsolidateConditionalMethod : IRefactoringMethod
    {
        public string GetName()
        {
            return "Consolidate Duplicate Conditional Fragments";
        }

        public string Apply(string sourceCode)
        {
            if (string.IsNullOrEmpty(sourceCode))
                throw new ArgumentException("Source code cannot be null or empty.", nameof(sourceCode));

            bool[] prot = BuildProtectedMask(sourceCode);

            // Шукаємо if...else
            int searchFrom = 0;
            while (true)
            {
                int ifPos = FindKeyword(sourceCode, prot, "if", searchFrom);
                if (ifPos < 0) return sourceCode;

                int ifOpen = FindChar(sourceCode, prot, '{', ifPos);
                if (ifOpen < 0) return sourceCode;

                int ifClose = FindMatchingClose(sourceCode, prot, ifOpen);
                if (ifClose < 0) return sourceCode;

                int elsePos = FindElseImmediatelyAfter(sourceCode, prot, ifClose + 1);
                if (elsePos < 0)
                {
                    searchFrom = ifPos + 1;
                    continue;
                }

                int elOpen = FindChar(sourceCode, prot, '{', elsePos + 4);
                if (elOpen < 0) return sourceCode;

                int elClose = FindMatchingClose(sourceCode, prot, elOpen);
                if (elClose < 0) return sourceCode;

                return ProcessIfElse(sourceCode, prot, ifPos, ifOpen, ifClose, elsePos, elOpen, elClose);
            }
        }

        private string ProcessIfElse(
            string src, bool[] prot,
            int ifPos, int ifOpen, int ifClose,
            int elsePos, int elOpen, int elClose)
        {
            string ifBody = src.Substring(ifOpen + 1, ifClose - ifOpen - 1);
            string elBody = src.Substring(elOpen + 1, elClose - elOpen - 1);

            var ifStmts = ParseStatements(ifBody);
            var elStmts = ParseStatements(elBody);

            if (ifStmts.Count == 0 || elStmts.Count == 0) return src;

            // Спільні інструкції на початку
            int pre = 0;
            while (pre < ifStmts.Count && pre < elStmts.Count
                   && Norm(ifStmts[pre].text) == Norm(elStmts[pre].text))
                pre++;

            // Спільні інструкції в кінці
            int suf = 0;
            int ii = ifStmts.Count - 1, ei = elStmts.Count - 1;
            while (ii >= pre && ei >= pre
                   && Norm(ifStmts[ii].text) == Norm(elStmts[ei].text))
            { suf++; ii--; ei--; }

            // Обробка повного збігу
            if (pre > 0 && pre == ifStmts.Count && pre == elStmts.Count)
            {
                pre = 0;
                suf = ifStmts.Count;
                ii = -1; ei = -1;
            }

            if (pre == 0 && suf == 0) return src;

            var prefixTexts = new List<string>();
            for (int i = 0; i < pre; i++) prefixTexts.Add(ifStmts[i].text.Trim());

            var suffixTexts = new List<string>();
            for (int i = ifStmts.Count - suf; i < ifStmts.Count; i++) suffixTexts.Add(ifStmts[i].text.Trim());

            string newIfBody = SliceBody(ifBody, ifStmts, pre, ifStmts.Count - suf - 1);
            string newElBody = SliceBody(elBody, elStmts, pre, elStmts.Count - suf - 1);

            string before = src.Substring(0, ifPos);
            string ifCondStr = src.Substring(ifPos, ifOpen - ifPos);
            string between = src.Substring(ifClose + 1, elsePos - ifClose - 1);
            string elsePart = src.Substring(elsePos, elOpen - elsePos);
            string after = src.Substring(elClose + 1);

            var sb = new StringBuilder();

            if (pre > 0)
            {
                string bTrimmed = before.TrimEnd('\r', '\n');
                sb.Append(bTrimmed);
                if (bTrimmed.Length > 0) sb.Append("\n");
                sb.Append(string.Join("\n", prefixTexts)).Append("\n");
            }
            else
            {
                sb.Append(before);
            }

            sb.Append(ifCondStr).Append("{").Append(newIfBody).Append("}");
            sb.Append(between);
            sb.Append(elsePart).Append("{").Append(newElBody).Append("}");

            if (suf > 0)
            {
                sb.Append("\n").Append(string.Join("\n", suffixTexts));
            }

            if (after.Length > 0)
            {
                bool afterStartsWithNewline = after.Length > 0 && after[0] == '\n';
                string trimmedAfter = afterStartsWithNewline ? after.Substring(1) : after;
                if (trimmedAfter.Length > 0)
                    sb.Append(afterStartsWithNewline ? "\n" : "").Append(trimmedAfter);
            }

            return sb.ToString();
        }

        // Вирізаємо код, зберігаючи відступи
        private string RemoveFromBody(
            string body,
            List<(string text, int start, int end)> stmts,
            int prefixCount, int suffixCount)
        {
            int keepFrom = prefixCount;
            int keepTo = stmts.Count - suffixCount - 1;

            if (keepFrom > keepTo) return " ";

            int leftEdge = keepFrom == 0 ? 0 : stmts[keepFrom - 1].end + 1;
            int rightEdge = keepTo == stmts.Count - 1 ? body.Length : stmts[keepTo + 1].start;

            string slice = body.Substring(leftEdge, rightEdge - leftEdge);

            if (string.IsNullOrWhiteSpace(slice)) return " ";

            if (suffixCount > 0 && keepTo < stmts.Count - 1)
            {
                int trimEnd = slice.Length - 1;
                while (trimEnd >= 0 && (slice[trimEnd] == ' ' || slice[trimEnd] == '\t'))
                    trimEnd--;
                slice = slice.Substring(0, trimEnd + 1);
            }

            if (slice.Length > 0 && slice[slice.Length - 1] != ' ' && slice[slice.Length - 1] != '\n')
                slice = slice + " ";

            return slice;
        }

        private string SliceBody(
            string body,
            List<(string text, int start, int end)> stmts,
            int keepFrom, int keepTo)
        {
            int pre = keepFrom;
            int suf = stmts.Count - keepTo - 1;
            return RemoveFromBody(body, stmts, pre, suf);
        }

        private List<(string text, int start, int end)> ParseStatements(string body)
        {
            var result = new List<(string, int, int)>();
            int depth = 0;
            bool inStr = false;
            int stmtStart = -1;

            for (int i = 0; i < body.Length; i++)
            {
                char c = body[i];

                if (!inStr && c == '"') { inStr = true; }
                else if (inStr)
                {
                    if (c == '\\') i++;
                    else if (c == '"') inStr = false;
                    continue;
                }

                if (c == '{') depth++;
                else if (c == '}') depth--;

                if (stmtStart < 0 && !char.IsWhiteSpace(c)) stmtStart = i;

                if (depth == 0 && c == ';')
                {
                    if (stmtStart >= 0)
                        result.Add((body.Substring(stmtStart, i - stmtStart + 1), stmtStart, i));
                    stmtStart = -1;
                }
            }
            return result;
        }

        private string Norm(string s) => Regex.Replace(s.Trim(), @"\s+", " ");

        private bool[] BuildProtectedMask(string code)
        {
            var mask = new bool[code.Length];
            int i = 0;
            while (i < code.Length)
            {
                if (i + 1 < code.Length && code[i] == '/' && code[i + 1] == '*')
                {
                    int s = i; i += 2;
                    while (i + 1 < code.Length && !(code[i] == '*' && code[i + 1] == '/')) i++;
                    int e = Math.Min(i + 2, code.Length);
                    for (int j = s; j < e; j++) mask[j] = true;
                    i = e; continue;
                }
                if (code[i] == '"')
                {
                    int s = i; i++;
                    while (i < code.Length)
                    {
                        if (code[i] == '\\') { i += 2; continue; }
                        if (code[i] == '"') { i++; break; }
                        i++;
                    }
                    for (int j = s; j < i; j++) mask[j] = true;
                    continue;
                }
                i++;
            }
            return mask;
        }

        private int FindKeyword(string code, bool[] prot, string kw, int from)
        {
            for (int i = from; i <= code.Length - kw.Length; i++)
            {
                if (prot[i]) continue;
                if (code.Substring(i, kw.Length) != kw) continue;
                bool lb = i == 0 || !char.IsLetterOrDigit(code[i - 1]);
                bool rb = i + kw.Length >= code.Length || !char.IsLetterOrDigit(code[i + kw.Length]);
                if (lb && rb) return i;
            }
            return -1;
        }

        private int FindChar(string code, bool[] prot, char ch, int from)
        {
            for (int i = from; i < code.Length; i++)
                if (!prot[i] && code[i] == ch) return i;
            return -1;
        }

        private int FindMatchingClose(string code, bool[] prot, int openPos)
        {
            int depth = 0;
            for (int i = openPos; i < code.Length; i++)
            {
                if (prot[i]) continue;
                if (code[i] == '{') depth++;
                else if (code[i] == '}') { depth--; if (depth == 0) return i; }
            }
            return -1;
        }

        private int FindElseImmediatelyAfter(string code, bool[] prot, int from)
        {
            for (int i = from; i < code.Length; i++)
            {
                if (prot[i]) continue;
                if (char.IsWhiteSpace(code[i])) continue;
                if (i + 4 <= code.Length && code.Substring(i, 4) == "else")
                {
                    bool rightOk = i + 4 >= code.Length || !char.IsLetterOrDigit(code[i + 4]);
                    if (rightOk) return i;
                }
                return -1;
            }
            return -1;
        }
    }
}
