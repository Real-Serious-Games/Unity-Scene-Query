using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Parser.Tests
{
    public class QueryTokenizerTests
    {
        [Fact]
        public void empty_query_string_throws_exception()
        {
            Assert.Throws<ArgumentException>(() => new QueryTokenizer().Start(string.Empty));
        }

        [Fact]
        public void query_string_with_only_whitespace_throws_exception()
        {
            Assert.Throws<ArgumentException>(() => new QueryTokenizer().Start("    "));
        }

        [Fact]
        public void tokenizer_is_not_at_end_with_valid_query_string()
        {
            var testObject = new QueryTokenizer();

            testObject.Start("blah");

            Assert.False(testObject.AtEnd);
        }

        [Fact]
        public void advancing_with_a_single_token_ends_tokenizer()
        {
            var testObject = new QueryTokenizer();

            testObject.Start("blah");

            testObject.Advance();

            Assert.True(testObject.AtEnd);
            Assert.Equal(QueryTokens.End, testObject.Token);
            Assert.Equal(string.Empty, testObject.TokenString);
        }

        [Fact]
        public void advancing_too_far_with_1_token_throws_exception()
        {
            var testObject = new QueryTokenizer();

            testObject.Start("blah");

            testObject.Advance();

            Assert.Throws<ApplicationException>(() => testObject.Advance());
        }

        [Fact]
        public void advanced_to_end_with_two_tokens_and_separator()
        {
            var testObject = new QueryTokenizer();

            testObject.Start("blah blah");

            testObject.Advance();
            testObject.Advance();
            testObject.Advance();

            Assert.True(testObject.AtEnd);
            Assert.Equal(QueryTokens.End, testObject.Token);
            Assert.Equal(string.Empty, testObject.TokenString);
        }

        [Fact]
        public void advancing_too_far_with_2_tokens_throws_exception()
        {
            var testObject = new QueryTokenizer();

            testObject.Start("blah blah");

            testObject.Advance();
            testObject.Advance();
            testObject.Advance();

            Assert.Throws<ApplicationException>(() => testObject.Advance());
        }

        [Fact]
        public void tokenizer_recognizes_name()
        {
            var testObject = new QueryTokenizer();

            var name = "blah";
            testObject.Start(name);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_name_with_hypens()
        {
            var testObject = new QueryTokenizer();

            var name = "blah-t-blah";
            testObject.Start(name);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_name_with_underscores()
        {
            var testObject = new QueryTokenizer();

            var name = "blah_t_blah";
            testObject.Start(name);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_name_starting_with_digits()
        {
            var testObject = new QueryTokenizer();

            var name = "12blah";
            testObject.Start(name);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_name_starting_with_underscore()
        {
            var testObject = new QueryTokenizer();

            var name = "_blah";
            testObject.Start(name);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_name_with_hypen()
        {
            var testObject = new QueryTokenizer();

            var name = "blah-blah";
            testObject.Start(name);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_name_starting_with_hypen()
        {
            var testObject = new QueryTokenizer();

            var name = "-blah";
            testObject.Start(name);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_name_with_preceding_whitespace()
        {
            var testObject = new QueryTokenizer();

            var name = "blah";
            testObject.Start("  " + name);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_name_with_trailing_whitespace()
        {
            var testObject = new QueryTokenizer();

            var name = "blah";
            testObject.Start(name + "  ");

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_two_names_with_a_separator()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + " " + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_two_names_and_handles_multiple_whitespace()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + "               " + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_three_names()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            var name3 = "name3";
            testObject.Start(name1 + " " + name2 + " " + name3);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name3);
        }


        [Fact]
        public void first_token_broken_unique_id_causes_exception()
        {
            Assert.Throws<ApplicationException>(() => new QueryTokenizer().Start("#"));
        }

        [Fact]
        public void advancing_to_broken_unique_id_causes_exception()
        {
            var testObject = new QueryTokenizer();

            testObject.Start("name #");

            testObject.Advance(); // Chew separator.

            Assert.Throws<ApplicationException>(() => testObject.Advance());

            Assert.Equal(QueryTokens.End, testObject.Token);
            Assert.Equal(string.Empty, testObject.TokenString);
            Assert.True(testObject.AtEnd);
        }

        [Fact]
        public void tokenizer_recognizes_unique_id()
        {
            var testObject = new QueryTokenizer();

            var name = "blah";
            testObject.Start("#" + name);

            Assert.True(testObject.Token == QueryTokens.UniqueID);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_two_unique_ids()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start("#" + name1 + " #" + name2);

            Assert.True(testObject.Token == QueryTokens.UniqueID);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.UniqueID);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_two_unique_ids_with_no_space_between()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start("#" + name1 + "#" + name2);

            Assert.True(testObject.Token == QueryTokens.UniqueID);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.UniqueID);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_name_then_unique_id()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + " #" + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.UniqueID);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_name_then_unique_id_with_no_spaces_between()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + "#" + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.UniqueID);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_unique_id_then_name()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start("#" + name1 + " " + name2);

            Assert.True(testObject.Token == QueryTokens.UniqueID);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_question_mark()
        {
            var testObject = new QueryTokenizer();

            var name = "name";
            testObject.Start("?" + name);

            Assert.True(testObject.Token == QueryTokens.QuestionMark);
            Assert.True(testObject.TokenString == "?");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_question_mark_and_name_with_separator()
        {
            var testObject = new QueryTokenizer();

            var name = "name";
            testObject.Start("?   " + name);

            Assert.True(testObject.Token == QueryTokens.QuestionMark);
            Assert.True(testObject.TokenString == "?");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void question_mark_can_separate_names()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + "?" + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.QuestionMark);
            Assert.True(testObject.TokenString == "?");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_exclamation_mark()
        {
            var testObject = new QueryTokenizer();

            var name = "name";
            testObject.Start("!" + name);

            Assert.True(testObject.Token == QueryTokens.ExclamationMark);
            Assert.True(testObject.TokenString == "!");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_exclamation_mark_and_name_with_separator()
        {
            var testObject = new QueryTokenizer();

            var name = "name";
            testObject.Start("!   " + name);

            Assert.True(testObject.Token == QueryTokens.ExclamationMark);
            Assert.True(testObject.TokenString == "!");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void exclamation_mark_can_separate_names()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + "!" + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.ExclamationMark);
            Assert.True(testObject.TokenString == "!");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_dot()
        {
            var testObject = new QueryTokenizer();

            var name = "name";
            testObject.Start("." + name);

            Assert.True(testObject.Token == QueryTokens.Dot);
            Assert.True(testObject.TokenString == ".");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_dot_and_name_with_separator()
        {
            var testObject = new QueryTokenizer();

            var name = "name";
            testObject.Start(".   " + name);

            Assert.True(testObject.Token == QueryTokens.Dot);
            Assert.True(testObject.TokenString == ".");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);
            
            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void dot_can_separate_names()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + "." + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Dot);
            Assert.True(testObject.TokenString == ".");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_colon()
        {
            var testObject = new QueryTokenizer();

            var name = "name";
            testObject.Start(":" + name);

            Assert.True(testObject.Token == QueryTokens.Colon);
            Assert.True(testObject.TokenString == ":");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void tokenizer_recognizes_colon_and_name_with_separator()
        {
            var testObject = new QueryTokenizer();

            var name = "name";
            testObject.Start(":   " + name);

            Assert.True(testObject.Token == QueryTokens.Colon);
            Assert.True(testObject.TokenString == ":");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Separator);
            Assert.True(testObject.TokenString == string.Empty);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void colon_can_separate_names()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + ":" + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Colon);
            Assert.True(testObject.TokenString == ":");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_semi_colon()
        {
            var testObject = new QueryTokenizer();

            testObject.Start(";");

            Assert.True(testObject.Token == QueryTokens.SemiColon);
            Assert.True(testObject.TokenString == ";");
        }

        [Fact]
        public void semi_colon_can_separate_names()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + ";" + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.SemiColon);
            Assert.True(testObject.TokenString == ";");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_slash()
        {
            var testObject = new QueryTokenizer();

            var name = "name";
            testObject.Start("/" + name);

            Assert.True(testObject.Token == QueryTokens.Slash);
            Assert.True(testObject.TokenString == "/");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
        }

        [Fact]
        public void slash_can_separate_names()
        {
            var testObject = new QueryTokenizer();

            var name1 = "name1";
            var name2 = "name2";
            testObject.Start(name1 + "/" + name2);

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name1);

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Slash);
            Assert.True(testObject.TokenString == "/");

            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name2);
        }

        [Fact]
        public void tokenizer_recognizes_quoted_string()
        {
            var testObject = new QueryTokenizer();

            var str = "hello there";
            testObject.Start("\"" + str + "\"");

            Assert.True(testObject.Token == QueryTokens.QuotedString);
            Assert.True(testObject.TokenString == str);
        }

        [Fact]
        public void tokenizer_recognizes_empty_quoted_string()
        {
            var testObject = new QueryTokenizer();

            var str = string.Empty;
            testObject.Start("\"" + str + "\"");

            Assert.True(testObject.Token == QueryTokens.QuotedString);
            Assert.True(testObject.TokenString == str);
        }

        [Fact]
        public void unterminated_quoted_string_throws()
        {
            Assert.Throws<ApplicationException>(() =>
                new QueryTokenizer().Start("\"whatever")
            );
        }

        [Fact]
        public void advancing_to_unterminated_quoted_string_throws()
        {
            var testObject = new QueryTokenizer();

            testObject.Start("name \"whatever");

            Assert.Throws<ApplicationException>(() =>
            {
                testObject.Advance(); // Go past name
                testObject.Advance(); // Go poast whitespace.
            });
        }

        [Fact]
        public void tokenizer_recognizes_quoted_string_alt()
        {
            var testObject = new QueryTokenizer();

            var str = "hello there";
            testObject.Start("'" + str + "'");

            Assert.True(testObject.Token == QueryTokens.QuotedString);
            Assert.True(testObject.TokenString == str);
        }

        [Fact]
        public void tokenizer_recognizes_empty_quoted_string_alt()
        {
            var testObject = new QueryTokenizer();

            var str = string.Empty;
            testObject.Start("'" + str + "'");

            Assert.True(testObject.Token == QueryTokens.QuotedString);
            Assert.True(testObject.TokenString == str);
        }

        [Fact]
        public void unterminated_quoted_string_throws_alt()
        {
            Assert.Throws<ApplicationException>(() =>
                new QueryTokenizer().Start("'whatever")
            );
        }

        [Fact]
        public void advancing_to_unterminated_quoted_string_throws_alt()
        {
            var testObject = new QueryTokenizer();
            
            testObject.Start("name 'whatever");

            Assert.Throws<ApplicationException>(() =>
            {
                testObject.Advance(); // Go past name
                testObject.Advance(); // Go poast whitespace.
            });
        }

        [Fact]
        public void tokenizer_quoted_string_ends_name_token()
        {
            var testObject = new QueryTokenizer();

            var name = "foo";
            var str = "hello there";
            testObject.Start(name + "\"" + str + "\"");

            Assert.True(testObject.Token == QueryTokens.Name);
            Assert.True(testObject.TokenString == name);
            
            testObject.Advance();

            Assert.True(testObject.Token == QueryTokens.QuotedString);
            Assert.True(testObject.TokenString == str);
        }

        [Fact]
        public void failed_expectation_throws_exception()
        {
            var testObject = new QueryTokenizer();

            testObject.Start("blah");

            Assert.Throws<ApplicationException>(() => testObject.Expect(QueryTokens.UniqueID));

            Assert.True(testObject.AtEnd);
        }

        [Fact]
        public void succeeded_expectation_advances()
        {
            var testObject = new QueryTokenizer();

            testObject.Start("blah#foo");

            testObject.Expect(QueryTokens.Name);

            Assert.Equal(QueryTokens.UniqueID, testObject.Token);
        }
    }
}
