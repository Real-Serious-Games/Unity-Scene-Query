using RSG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
// The EBNF grammar for this parser can be found here:
//
// http://seriousserver:81/mediawiki/index.php/SceneLINQ_Grammar
//

namespace RSG.Scene.Query.Parser
{
    /// <summary>
    /// Parser for queries.
    /// </summary>
    public interface IQueryParser
    {
        /// <summary>
        /// Parse a query string and return a IQuery object that represents it.
        /// </summary>
        IQuery Parse(string query);
    }

    /// <summary>
    /// Parser for queries.
    /// </summary>
    public class QueryParser : IQueryParser
    {
        public IQueryTokenizer tokenizer;

        public QueryParser(IQueryTokenizer tokenizer)
        {
            Argument.NotNull(() => tokenizer);

            this.tokenizer = tokenizer;
        }

        /// <summary>
        /// Parse a name.
        /// </summary>
        public string name()
        {
            if (tokenizer.Token == QueryTokens.QuotedString)
            {
                var name = tokenizer.TokenString;
                tokenizer.Advance();
                return name;
            }

            return tokenizer.Expect(QueryTokens.Name);
        }

        /// <summary>
        /// Returns information parsed from a 'matcher'.
        /// </summary>
        public class Matcher
        {
            /// <summary>
            /// The name to match.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Set to true to only apply a partial match.
            /// </summary>
            public bool Partial { get; private set; }

            public Matcher(string name, bool partial)
            {
                Argument.StringNotNullOrEmpty(() => name);

                this.Name = name;
                this.Partial = partial;
            }
        }

        /// <summary>
        /// Parse a matcher.
        /// </summary>
        public Matcher matcher()
        {
            var partial = false;

            if (tokenizer.Token == QueryTokens.QuestionMark)
            {
                partial = true;
                
                tokenizer.Advance();
            }

            return new Matcher(name(), partial);
        }

        /// <summary>
        /// Parse a selector.
        /// </summary>
        public IQuery selector(ref bool nameSelectorAllowed)
        {
            if (tokenizer.Token == QueryTokens.Dot)
            {
                tokenizer.Advance();

                var layerMatcher = matcher();
                if (layerMatcher.Partial)
                {
                    return new RegexLayerQuery(layerMatcher.Name);
                }
                else
                {
                    return new LayerQuery(layerMatcher.Name);
                }
            }

            if (tokenizer.Token == QueryTokens.ExclamationMark)
            {
                tokenizer.Advance();

                return new NotQuery(selector(ref nameSelectorAllowed));
            }

            if (!nameSelectorAllowed)
            {
                throw new ApplicationException("Can't specify multiple names or ids in a single compound selector");
            }

            // Disallow other names in the same compound selector.
            nameSelectorAllowed = false;

            if (tokenizer.Token == QueryTokens.UniqueID)
            {
                var uniqueId = tokenizer.TokenString;

                tokenizer.Advance();

                return new UniqueIdQuery(Int32.Parse(uniqueId));
            }

            var nameMatcher = matcher();
            if (nameMatcher.Partial)
            {
                return new RegexNameQuery(nameMatcher.Name);
            }
            else
            {
                return new NameQuery(nameMatcher.Name);
            }
        }

        /// <summary>
        /// Parse a compound selector.
        /// </summary>
        public IQuery compound_selector()
        {
            var nameSelectorAllowed = true;
            var query = selector(ref nameSelectorAllowed);

            if (tokenizer.AtEnd || IsDescendentsSeparator(tokenizer.Token))
            {
                // There is only a single selector, no need to create a compound selector.
                return query;
            }

            do
            {
                var otherQuery = selector(ref nameSelectorAllowed);
                query = new AndQuery(otherQuery, query);

            } while (!tokenizer.AtEnd && 
                     !IsDescendentsSeparator(tokenizer.Token));

            return query;
        }

        /// <summary>
        /// Returns true if the token is a separator for stack filters.
        /// </summary>
        private bool IsDescendentsSeparator(QueryTokens queryToken)
        {
            return queryToken == QueryTokens.GreaterThan ||
                   queryToken == QueryTokens.Slash;
        }

        /// <summary>
        /// Parse a stack selector.
        /// </summary>
        public IQuery descendents_selector()
        {
            bool isRoot = tokenizer.Token == QueryTokens.Slash;
            if (isRoot)
            {
                tokenizer.Advance();
            }

            var query = compound_selector();

            if (isRoot)
            {
                var rootQuery = new RootQuery();
                query = new AndQuery(query, rootQuery);
            }

            if (tokenizer.AtEnd)
            {
                // Exhausted tokens, return single filter.
                return query;
            }

            do
            {
                if (tokenizer.Token == QueryTokens.Slash)
                {
                    query = new ParentQuery(query);
                }
                else if (tokenizer.Token == QueryTokens.GreaterThan)
                {
                    query = new AncestorQuery(query);
                }
                else 
                {
                    throw new ApplicationException("Unexpected token: " + tokenizer.TokenString);
                }

                tokenizer.Advance(); // Chew up the separator.

                var otherQuery = compound_selector();

                query = new AndQuery(otherQuery, query);

            } while (!tokenizer.AtEnd);

            return query;
        }

        /// <summary>
        /// Parse an entire query.
        /// </summary>
        public IQuery query()
        {
            return descendents_selector();
        }

        /// <summary>
        /// Parse a query string and return a IQuery object that represents it.
        /// </summary>
        public IQuery Parse(string queryStr)
        {
            Argument.StringNotNullOrEmpty(() => queryStr);

            tokenizer.Start(queryStr);
            return query();
        }
    }
}
