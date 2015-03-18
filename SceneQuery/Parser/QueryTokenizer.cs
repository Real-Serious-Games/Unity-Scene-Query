using RSG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSG.Scene.Query.Parser
{
    /// <summary>
    /// Defines each type of token that is understood and returned by the tokenizer.
    /// </summary>
    public enum QueryTokens
    {
        End,
        Name,
        QuestionMark,
        ExclamationMark,
        Dot,
        UniqueID,
        QuotedString,
        GreaterThan,
        Slash,
        Colon,
        SemiColon
    }

    /// <summary>
    /// Tokenizes a query prior to parsing.
    /// </summary>
    public interface IQueryTokenizer
    {
        /// <summary>
        /// Start tokenization of query.
        /// </summary>
        void Start(string query);

        /// <summary>
        /// Validate that the currrent token is the same as what is expected.
        /// Throws an exception if it is not.
        /// Advances to the next token automatically.
        /// Returns the token string for further processing.
        /// </summary>
        string Expect(QueryTokens expectedQueryToken);

        /// <summary>
        /// Returns true when no more tokens are available.
        /// </summary>
        bool AtEnd { get; }

        /// <summary>
        /// Retieve the current token.
        /// </summary>
        QueryTokens Token { get; }

        /// <summary>
        /// Retrieve the string for the current token.
        /// </summary>
        string TokenString { get; }

        /// <summary>
        /// Advance to the next token.
        /// </summary>
        void Advance();
    }

    /// <summary>
    /// Tokenizes a query prior to parsing.
    /// </summary>
    public class QueryTokenizer : IQueryTokenizer
    {
        /// <summary>
        /// The query being tokenized.
        /// </summary>
        private string query;

        /// <summary>
        /// The current position in the query.
        /// </summary>
        private int position = 0;

        /// <summary>
        /// Start tokenization of query.
        /// </summary>
        public void Start(string query)
        {
            Argument.StringNotNullOrEmpty(() => query);

            this.query = query.Trim();
            this.position = 0;

            if (this.query == string.Empty)
            {
                throw new ArgumentException("Query contains only whitespace.");
            }

            ExtractToken();
        }

        /// <summary>
        /// Extract the net token from the query string.
        /// </summary>
        private void ExtractToken()
        {
            if (position >= query.Length)
            {
                TokenizerEnded();
                return;
            }

            SkipwhiteSpace();

            if (query[position] == '?')
            {
                Token = QueryTokens.QuestionMark;
                TokenString = "?";
                ++position;
                return;
            }

            if (query[position] == '>')
            {
                Token = QueryTokens.GreaterThan;
                TokenString = ">";
                ++position;
                return;
            }

            if (query[position] == '!')
            {
                Token = QueryTokens.ExclamationMark;
                TokenString = "!";
                ++position;
                return;
            }

            if (query[position] == '/')
            {
                Token = QueryTokens.Slash;
                TokenString = "/";
                ++position;
                return;
            }

            if (query[position] == '.')
            {
                Token = QueryTokens.Dot;
                TokenString = ".";
                ++position;
                return;
            }

            if (query[position] == ':')
            {
                Token = QueryTokens.Colon;
                TokenString = ":";
                ++position;
                return;
            }

            if (query[position] == ';')
            {
                Token = QueryTokens.SemiColon;
                TokenString = ";";
                ++position;
                return;
            }

            if (query[position] == '"')
            {
                ++position;

                Token = QueryTokens.QuotedString;
                TokenString = ExtractToEndQuote('"');
                return;
            }

            if (query[position] == '\'')
            {
                ++position;

                Token = QueryTokens.QuotedString;
                TokenString = ExtractToEndQuote('\'');
                return;
            }

            if (query[position] == '#')
            {
                Token = QueryTokens.UniqueID;

                ++position;

                if (position >= query.Length)
                {
                    // Bad unique id.
                    // Put tokenizer in end state and throw exception.
                    TokenizerEnded();

                    throw new ApplicationException("Invalid unique id, you specifid a # but you need to add the unique id after it.");
                }
            }
            else
            {
                Token = QueryTokens.Name;
            }

            TokenString = ExtractToSeparator();
        }

        /// <summary>
        /// Called to put the tokenizer in the 'end' state.
        /// </summary>
        private void TokenizerEnded()
        {
            Token = QueryTokens.End;
            TokenString = string.Empty;
        }

        /// <summary>
        /// Skip leading whitespace in the query.
        /// </summary>
        private void SkipwhiteSpace()
        {
            while (position < query.Length &&
                   IsWhitespace(query[position]))
            {
                ++position;
            }
        }

        /// <summary>
        /// Extract the current token until the next separator.
        /// </summary>
        private string ExtractToSeparator()
        {
            var start = position;

            ++position;

            while (position < query.Length)
            {
                if (IsWhitespace(query[position])||
                    IsSeperator(query[position]))
                {
                    break;
                }

                ++position;
            }

            var length = position - start;
            return query.Substring(start, length);
        }

        /// <summary>
        /// Extract the string until an end quote is found.
        /// </summary>
        private string ExtractToEndQuote(char quoteType)
        {
            var start = position;

            while (position < query.Length)
            {
                if (query[position] == quoteType)
                {
                    var length = position - start;
                    ++position;
                    return query.Substring(start, length);
                }

                ++position;
            }

            throw new ApplicationException("Unterminated quoted string.");
        }

        /// <summary>
        /// Returns true if the character is an separator for other tokens.
        /// </summary>
        private bool IsSeperator(char ch)
        {
            return ch == '#' ||
                   ch == '?' ||
                   ch == '!' ||
                   ch == '.' ||
                   ch == '/' ||
                   ch == '>' ||
                   ch == ':' ||
                   ch == ';' ||
                   ch == '"';
        }

        /// <summary>
        /// Returns true if the requested character is whitespace.
        /// </summary>
        private bool IsWhitespace(char ch)
        {
            return ch == ' ';
        }

        /// <summary>
        /// Validate that the currrent token is the same as what is expected.
        /// Throws an exception if it is not.
        /// Advances to the next token automatically.
        /// Returns the token string for further processing.
        /// </summary>
        public string Expect(QueryTokens expectedQueryToken)
        {
            var tokenString = TokenString;

            if (Token == expectedQueryToken)
            {

                // Everything ok, move along.
                Advance();

                return tokenString;
            }

            var curToken = Token;

            //
            // Expectation was not meet, throw exception.
            //
            TokenizerEnded();

            throw new ApplicationException("Expected token: " + expectedQueryToken + ", but encountered token " + curToken + " (" + tokenString + ")");
        }

        /// <summary>
        /// Returns true when no more tokens are available.
        /// </summary>
        public bool AtEnd 
        {
            get
            {
                return Token == QueryTokens.End;
            }
        }

        /// <summary>
        /// Retieve the current token.
        /// </summary>
        public QueryTokens Token
        {
            get;
            private set;
        }

        /// <summary>
        /// Retrieve the string for the current token.
        /// </summary>
        public string TokenString 
        {
            get;
            private set;
        }

        /// <summary>
        /// Advance to the next token.
        /// </summary>
        public void Advance()
        {
            if (AtEnd)
            {
                throw new ApplicationException("Advanced too far through token stream!");
            }

            ExtractToken();
        }
    }
}
