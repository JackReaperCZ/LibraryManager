CREATE DATABASE IF NOT EXISTS LibraryDB;
USE LibraryDB;

CREATE TABLE IF NOT EXISTS Books (
    BookID INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Price FLOAT NOT NULL,
    Available BOOL NOT NULL DEFAULT TRUE,
    Genre ENUM('Fiction','Non-Fiction','Science','History','Other') NOT NULL,
    PublishedDate DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS Authors (
    AuthorID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    BirthDate DATE
);

CREATE TABLE IF NOT EXISTS Members (
    MemberID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(150) UNIQUE NOT NULL,
    RegisteredDate DATE NOT NULL,
    IsActive BOOL NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS Loans (
    LoanID INT AUTO_INCREMENT PRIMARY KEY,
    BookID INT NOT NULL,
    MemberID INT NOT NULL,
    LoanDate DATE NOT NULL,
    ReturnDate DATE NULL,
    Returned BOOL NOT NULL DEFAULT FALSE,
    FOREIGN KEY (BookID) REFERENCES Books(BookID),
    FOREIGN KEY (MemberID) REFERENCES Members(MemberID)
);

CREATE TABLE IF NOT EXISTS BookAuthors (
    BookID INT NOT NULL,
    AuthorID INT NOT NULL,
    PRIMARY KEY (BookID, AuthorID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID),
    FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID)
);

CREATE OR REPLACE VIEW CurrentLoans AS
SELECT 
    l.LoanID,
    b.Title AS BookTitle,
    m.FirstName,
    m.LastName,
    l.LoanDate,
    l.ReturnDate
FROM Loans l
JOIN Books b ON l.BookID = b.BookID
JOIN Members m ON l.MemberID = m.MemberID
WHERE l.Returned = FALSE;

CREATE OR REPLACE VIEW OverdueLoans AS
SELECT 
    l.LoanID,
    b.Title AS BookTitle,
    m.FirstName,
    m.LastName,
    l.LoanDate,
    l.ReturnDate
FROM Loans l
JOIN Books b ON l.BookID = b.BookID
JOIN Members m ON l.MemberID = m.MemberID
WHERE l.Returned = FALSE AND (l.ReturnDate IS NOT NULL AND l.ReturnDate < CURDATE());
