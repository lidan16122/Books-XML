using server.Models.Constants;
using server.Models.Entities;
using server.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace server.BL.Services
{
    public class BooksService
    {
        private readonly BooksXML booksXML;
        public BooksService(BooksXML _booksXML)
        {
            booksXML = _booksXML;
        }
        public List<Book> GetAllBooks(int pageNumber, int pageSize)
        {
            List<Book> books = booksXML.Doc.Root.Elements("book")
                .Select(x => new Book
                {
                    Category = (string)x.Attribute("category"),
                    Cover = (string)x.Attribute("cover"),
                    ISBN = (string)x.Element("isbn"),
                    Title = (string)x.Element("title"),
                    Lang = (string)x.Element("title")?.Attribute("lang"),
                    Authors = x.Elements("author").Select(a => (string)a).ToList(),
                    Year = (int)x.Element("year"),
                    Price = (double)x.Element("price")
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return books;
        }
        public eStatusCodes AddNewBook(Book book)
        {
            try
            {
                XElement? existingBook = booksXML.Doc.Root
                   .Elements("book")
                   .FirstOrDefault(b => (string)b.Element("isbn") == book.ISBN);

                if (existingBook != null)
                {
                    return eStatusCodes.BadRequest;
                }
                if (!ValidateBook(book))
                {
                    return eStatusCodes.BadRequest;
                }

                XElement newBook = new XElement("book",
                    new XAttribute("category", book.Category ?? ""),
                    book.Cover != null ? new XAttribute("cover", book.Cover) : null,
                    new XElement("isbn", book.ISBN),
                    new XElement("title", new XAttribute("lang", book.Lang ?? "en"), book.Title),
                    book.Authors.Select(a => new XElement("author", a)),
                    new XElement("year", book.Year),
                    new XElement("price", book.Price)
                );

                booksXML.Doc.Root.Add(newBook);

                booksXML.SaveDoc();

                return eStatusCodes.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine($"error adding book: {e.Message}");
                return eStatusCodes.BadRequest;
            }
        }
        public eStatusCodes UpdateBook(Book book)
        {
            try
            {
                XElement? existingBook = booksXML.Doc.Root
                    .Elements("book")
                    .FirstOrDefault(b => (string)b.Element("isbn") == book.ISBN);

                if (existingBook == null)
                {
                    return eStatusCodes.NotFound;
                }
                if (!ValidateBook(book))
                {
                    return eStatusCodes.BadRequest;
                }

                existingBook.SetAttributeValue("category", book.Category ?? "");
                if (!string.IsNullOrEmpty(book.Cover))
                    existingBook.SetAttributeValue("cover", book.Cover);
                else
                    existingBook.Attribute("cover")?.Remove();

                existingBook.Element("title").Value = book.Title;
                existingBook.Element("title").SetAttributeValue("lang", book.Lang ?? "en");

                existingBook.Elements("author").Remove();
                foreach (string author in book.Authors)
                {
                    existingBook.Add(new XElement("author", author));
                }

                existingBook.Element("year").Value = book.Year.ToString();
                existingBook.Element("price").Value = book.Price.ToString();

                booksXML.SaveDoc();

                return eStatusCodes.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine($"error updating book: {e.Message}");
                return eStatusCodes.BadRequest;
            }
        }
        public eStatusCodes DeleteBook(string isbn)
        {
            try
            {
                XElement? bookToDelete = booksXML.Doc.Root
                    .Elements("book")
                    .FirstOrDefault(b => (string)b.Element("isbn") == isbn);

                if (bookToDelete == null)
                {
                    return eStatusCodes.NotFound;
                }

                bookToDelete.Remove();

                booksXML.SaveDoc();

                return eStatusCodes.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine($"error deleting book: {e.Message}");
                return eStatusCodes.BadRequest;
            }
        }
        private bool ValidateBook(Book book)
        {
            bool result = true;
            if(book.Price < 0) { 
                result = false;
            }
            if(book.Year > DateTime.Now.Year + 1)
            {
                result = false;
            }
            return result;
        }
    }
}
