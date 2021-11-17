using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Caching.Memory;


namespace QuoteGeneratorAPI.Models {

    public class QuoteManager {

        private List<Quote> _quotes;
        private MySqlConnection conn;
        private MySqlCommand mysql;
        private MySqlDataReader reader;

        public Quote quote {get; set;}
        public int id {get; set;}

        public List<Quote> quotes {
            get {
                if (_quotes == null) {
                    _quotes = new List<Quote>();
                }
                return _quotes;
            }
            set {
                _quotes = value;
            }
        }


        public QuoteManager (IMemoryCache env){
            _quotes = new List<Quote>();
            quote = new Quote();

            List<Quote> tempQuotes;
            // Check if the quotes are in the cache
            MemoryCacheEntryOptions memOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(60));

            if (!env.TryGetValue("quotes", out tempQuotes)) {
                retrieveQuotes();
                env.Set("quotes", _quotes, memOptions);
            } else {
                quotes = tempQuotes;
            }
        }
        // Constructor for testing
        // Does not use the cache
        public QuoteManager () {
            _quotes = new List<Quote>();
            quote = new Quote();
            retrieveQuotes();
        }

        // Return all Quotes
        public List<Quote> GetQuotes () {
            return _quotes;
        }

        // Return N random Quotes
        public List<Quote> GetQuotes(int n) {
            // Set up a temporary list to hold the random quotes
            List<Quote> tempQuotes = new List<Quote>();
            Random rand = new Random();
            int index;

            // check if n is greater than the number of quotes
            if (n > _quotes.Count) {
                throw new Exception("Not enough quotes to return");
            }

            // populate tempQuotes
            while (tempQuotes.Count < n) {
                index = rand.Next(0, _quotes.Count);
                // check to see if quote is already in tempQuotes
                if (!tempQuotes.Contains(_quotes[index])) {
                    tempQuotes.Add(_quotes[index]);
                }
            }

            return tempQuotes;
        }



        // Get all quotes from DB
        private void retrieveQuotes(){
            try {
                conn = new MySqlConnection(Connection.CON_STRING);
                conn.Open();
                mysql = conn.CreateCommand();
                mysql.CommandText = "SELECT * FROM quotes";
                reader = mysql.ExecuteReader();

                while(reader.Read()) {
                    Quote quote = new Quote();
                    quote.id = reader.GetInt32(0);
                    quote.author = reader.GetString(1);
                    quote.content = reader.GetString(2);
                    quote.permaLink = reader.GetString(3);
                    quote.image = reader.GetString(4);
                    _quotes.Add(quote);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            } finally {
                conn.Close();
            }
        }
        
        // Adds a new quote
        public bool addQuote() {
            Console.WriteLine("Adding quote");
            Console.WriteLine("Permalink: " + quote.permaLink);

            try {
                conn = new MySqlConnection(Connection.CON_STRING);
                conn.Open();
                mysql = conn.CreateCommand();
                mysql.Parameters.AddWithValue("@author", quote.author);
                mysql.Parameters.AddWithValue("@content", quote.content);
                mysql.Parameters.AddWithValue("@permalink", quote.permaLink);
                mysql.Parameters.AddWithValue("@image", quote.image);

                mysql.CommandText = "INSERT INTO quotes (author, quote, permalink, image) VALUES (@author, @content, @permalink, @image)";
                mysql.ExecuteNonQuery();
                return true;
            } catch (Exception e) {
                Console.WriteLine("Error Occured During DB Update");
                Console.WriteLine(">> " + e);
                return false;
            } finally {
                conn.Close();
            }
        }
        // Deletes the selected quote
        public bool deleteQuote() {
            // If the quote ID is not set, return false
            Console.WriteLine("Deleting Quote: " + id);
            if (id == 0) {
                return false;
            }
            // Try to delete the quote
            try {
                conn = new MySqlConnection(Connection.CON_STRING);
                conn.Open();
                mysql = conn.CreateCommand();
                mysql.Parameters.AddWithValue("@id", id);
                mysql.CommandText = "DELETE FROM quotes WHERE id = @id";
                mysql.ExecuteNonQuery();
                return true;
            } catch (Exception e) {
                Console.WriteLine("Error Occured During DB Update");
                Console.WriteLine(">> " + e);
                return false;
            } finally {
                // Close the connection
                conn.Close();
            }
        }
    }
}
