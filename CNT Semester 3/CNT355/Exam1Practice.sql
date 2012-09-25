/* The following questions draw source data from Pubs */
use pubs

/* 1. Create an alphabetical list of titles for books where the author receives more than 
a 50% royalty.  Note that royalty percentage is stored in the royaltyper column
of the TitleAuthor table. */

select * from titles
where title_id in 
(
	select title_id from titleauthor
	where royaltyper > 50
)
order by type, title

--2. Provide a list of authors that reside in the same cities as publishers.

select * from authors
where city in
(
	select city from publishers
)

/* 3. Produce an alphabetical list of author last and first names that have a book 
published by Algodata Infosystems. */

select au_lname 'Last Name', au_fname 'First Name'
from authors
where au_id in
(
	select au_id from titleauthor
	where title_id in
	(
		select title_id from titles
		where pub_id = 1389
	)
)
order by 1

--4.List all publishers that have no published titles.

select pub_name from publishers
where pub_id not in 
(
	select pub_id from titles
)

--5. What publishers publish business books

select pub_name from publishers
where pub_id in
(
	select pub_id from titles
	where type = 'business'
)	
	
--6. Use SUBQUERIES ONLY (NO JOINs) to solve this one.  Source data comes from PUBS.
--Write a query to return the full name (formatted as Doe, John) of authors who
--write books with the word 'Computer' OR 'Computers' in the title.

select au_lname + ', ' + au_fname as 'Full Name'
from authors
where au_id in 
(
	select au_id from titleauthor
	where title_id in
	(
		select title_id from titles
		where title like '%Computer%' or title like '%Computers%'
	)
)

--7. List all publishers that publish a psychology book

select * from publishers
where pub_id in
(
	select pub_id from titles
	where type = 'psychology'
)

--8. What publishers have not paid any advances yet?

select * from publishers
where pub_id in
(
	select pub_id from titles
	where advance is null or advance = 0
)