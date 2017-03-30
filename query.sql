create database Ngram
go
use Ngram

create table Unigram(
 gram nvarchar(20) primary key,
 syllIndex int,
 amount int
)

create table Bigram(
 firstSyllIndex int ,
 secondSyllIndex int,
 amount int,
 primary key (firstSyllIndex, secondSyllIndex)
)

create table Trigram(
firstSyllIndex int ,
 secondSyllIndex int,
 thirdSyllIndex int,
 amount int,
 primary key (firstSyllIndex, secondSyllIndex, thirdSyllIndex)
)
select * from Unigram

delete from unigram

select * from Bigram
select unigram.syllIndex, Unigram.amount from Unigram where Unigram.gram = N'Ngưỡng'
select bigram.amount from Bigram where Bigram.firstSyllIndex = 1 and Bigram.secondSyllIndex = 2

delete from Bigram