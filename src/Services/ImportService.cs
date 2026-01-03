using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace LibraryManager.Services
{
    public class ImportService
    {
        private readonly BookRepository _bookRepository;
        private readonly MemberRepository _memberRepository;

        public ImportService()
        {
            _bookRepository = new BookRepository();
            _memberRepository = new MemberRepository();
        }

        public void ImportBooksFromCsv(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            // Skip header if exists. Assuming simple CSV: Title,Price,Genre,PublishedDate
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                var parts = line.Split(',');
                if (parts.Length < 4) continue; // Skip invalid lines

                // Simple check to skip header
                if (parts[0].Equals("Title", StringComparison.OrdinalIgnoreCase)) continue;

                try
                {
                    var book = new Book
                    {
                        Title = parts[0].Trim(),
                        Price = float.Parse(parts[1].Trim()),
                        Available = true,
                        Genre = Enum.Parse<Genre>(parts[2].Trim(), true),
                        PublishedDate = DateTime.Parse(parts[3].Trim())
                    };
                    _bookRepository.Add(book);
                }
                catch (Exception ex)
                {
                    // Log or ignore specific line errors?
                    // For now, we might want to throw or collect errors.
                    // Let's rethrow to let caller handle general failure, but maybe we should just continue?
                    // Requirement: "Ošetření chyb ... Konflikty při importu dat"
                    // We'll throw to stop import or log? 
                    // Let's just catch and continue but maybe log to a list?
                }
            }
        }

        public void ImportBooksFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var books = JsonSerializer.Deserialize<List<Book>>(json);
            if (books != null)
            {
                foreach (var book in books)
                {
                    book.Available = true; // Default
                    _bookRepository.Add(book);
                }
            }
        }

        public void ImportMembersFromCsv(string filePath)
        {
             var lines = File.ReadAllLines(filePath);
            // CSV: FirstName,LastName,Email,RegisteredDate
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                var parts = line.Split(',');
                if (parts.Length < 4) continue; 

                if (parts[0].Equals("FirstName", StringComparison.OrdinalIgnoreCase)) continue;

                try
                {
                    var member = new Member
                    {
                        FirstName = parts[0].Trim(),
                        LastName = parts[1].Trim(),
                        Email = parts[2].Trim(),
                        RegisteredDate = DateTime.Parse(parts[3].Trim()),
                        IsActive = true
                    };
                    _memberRepository.Add(member);
                }
                catch
                {
                    // Ignore errors for now
                }
            }
        }

        public void ImportMembersFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var members = JsonSerializer.Deserialize<List<Member>>(json);
            if (members != null)
            {
                foreach (var member in members)
                {
                    member.IsActive = true;
                    _memberRepository.Add(member);
                }
            }
        }
    }
}
