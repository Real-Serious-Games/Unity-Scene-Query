using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Parser.Tests
{
    public class QueryParserTests
    {
        MockTokenizer tokenizer;

        QueryParser testObject;

        public class MockToken
        {
            public QueryTokens Token;
            public string TokenString;
        }

        public class MockTokenizer : IQueryTokenizer
        {
            // Set to true when all mock tokens have been exhausted.
            public bool AtEnd { get; private set; }

            private IEnumerable<MockToken> mockTokens;
            private IEnumerator<MockToken> mockTokensEnumerator;

            public MockTokenizer(IEnumerable<MockToken> mockTokens)
            {
                Argument.NotNull(() => mockTokens);

                this.mockTokens = mockTokens;
                this.mockTokensEnumerator = mockTokens.GetEnumerator();
                AtEnd = !this.mockTokensEnumerator.MoveNext(); // Prime the first token.
                if (AtEnd)
                {
                    throw new ApplicationException("No tokens!");
                }
            }

            public void Start(string query)
            {
            }

            public string Expect(QueryTokens expectedQueryToken)
            {
                if (AtEnd)
                {
                    throw new ApplicationException("Unexpected end of tokens!");
                }
                var curToken = mockTokensEnumerator.Current.Token;
                if (curToken != expectedQueryToken)
                {
                    throw new ApplicationException("Unexpected token: " + curToken + ", expected: " + expectedQueryToken);
                }

                var curTokenString = mockTokensEnumerator.Current.TokenString;

                Advance();

                return curTokenString;
            }

            public QueryTokens Token
            {
                get
                {
                    if (AtEnd)
                    {
                        throw new ApplicationException("Unexpected end of tokens!");
                    }
                    return mockTokensEnumerator.Current.Token;
                }
            }

            public string TokenString
            {
                get
                {
                    if (AtEnd)
                    {
                        throw new ApplicationException("Unexpected end of tokens!");
                    }
                    return mockTokensEnumerator.Current.TokenString;
                }
            }

            public void Advance()
            {
                if (AtEnd)
                {
                    throw new ApplicationException("Unexpected end of tokens!");
                }
                AtEnd = !mockTokensEnumerator.MoveNext();
            }
        }

        void Init(params MockToken[] mockTokens)
        {
            tokenizer = new MockTokenizer(mockTokens);

            testObject = new QueryParser(tokenizer);
        }

        [Fact]
        public void can_parse_name()
        {
            var name = "SomeName";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            Assert.Equal(name, testObject.name());
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_quoted_name_with_spaces()
        {
            var name = "Some Name";

            Init(
                new MockToken
                {
                    Token = QueryTokens.QuotedString,
                    TokenString = name,
                }
            );

            Assert.Equal(name, testObject.name());
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_exact_name_matcher()
        {
            var name = "SomeName";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            var matcher = testObject.matcher();
            Assert.False(matcher.Partial);
            Assert.Equal(name, matcher.Name);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_partial_name_matcher()
        {
            var name = "SomeName";

            Init(
                new MockToken
                {
                    Token = QueryTokens.QuestionMark,
                    TokenString = "?",
                },
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            var matcher = testObject.matcher();
            Assert.True(matcher.Partial);
            Assert.Equal(name, matcher.Name);

            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void question_mark_by_itself_throws_exception()
        {
            Init(
                new MockToken
                {
                    Token = QueryTokens.QuestionMark,
                    TokenString = "?",
                }
            );

            Assert.Throws<ApplicationException>(() => testObject.matcher());
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_exact_name_selector()
        {
            var name = "SomeName";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            var query = testObject.selector();
            Assert.NotNull(query);
            Assert.IsType<NameQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_partial_name_selector()
        {
            var name = "SomeName";

            Init(
                new MockToken
                {
                    Token = QueryTokens.QuestionMark,
                    TokenString = "?",
                },
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            var query = testObject.selector();
            Assert.NotNull(query);
            Assert.IsType<RegexNameQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_unique_id_selector()
        {
            var uniqueId = "1234";

            Init(
                new MockToken
                {
                    Token = QueryTokens.UniqueID,
                    TokenString = uniqueId
                }
            );

            var query = testObject.selector();
            Assert.NotNull(query);
            Assert.IsType<UniqueIdQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_negated_selector()
        {
            var name = "foos";

            Init(
                new MockToken
                {
                    Token = QueryTokens.ExclamationMark,
                    TokenString = "!",
                },
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            var query = testObject.selector();
            Assert.NotNull(query);
            Assert.IsType<NotQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_exact_layer_selector()
        {
            var name = "foos";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Dot,
                    TokenString = ".",
                },
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            var query = testObject.selector();
            Assert.NotNull(query);
            Assert.IsType<LayerQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_partial_layer_selector()
        {
            var name = "foos";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Dot,
                    TokenString = ".",
                },
                new MockToken
                {
                    Token = QueryTokens.QuestionMark,
                    TokenString = "?",
                },
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            var query = testObject.selector();
            Assert.NotNull(query);
            Assert.IsType<RegexLayerQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_query_with_single_selector()
        {
            var name = "SomeName";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            var query = testObject.query();
            Assert.NotNull(query);
            Assert.IsType<NameQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_compound_selector()
        {
            var name1 = "name1";
            var name2 = "name2";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name1,
                },
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name2,
                }
            );

            var query = testObject.query();
            Assert.NotNull(query);
            Assert.IsType<AndQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_ancestor_query()
        {
            var name1 = "name1";
            var name2 = "name2";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name1,
                },
                new MockToken
                {
                    Token = QueryTokens.Separator,
                    TokenString = string.Empty,
                },
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name2,
                }
            );

            var query = testObject.query();
            Assert.NotNull(query);
            Assert.IsType<AndQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_parent_query()
        {
            var name1 = "name1";
            var name2 = "name2";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name1,
                },
                new MockToken
                {
                    Token = QueryTokens.Slash,
                    TokenString = "/",
                },
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name2,
                }
            );

            var query = testObject.query();
            Assert.NotNull(query);
            Assert.IsType<AndQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }

        [Fact]
        public void can_parse_root_query()
        {
            var name = "name";

            Init(
                new MockToken
                {
                    Token = QueryTokens.Slash,
                    TokenString = "/",
                },
                new MockToken
                {
                    Token = QueryTokens.Name,
                    TokenString = name,
                }
            );

            var query = testObject.query();
            Assert.NotNull(query);
            Assert.IsType<AndQuery>(query);
            Assert.True(tokenizer.AtEnd);
        }
    }
}
