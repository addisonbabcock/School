--CNT355 - SQL
--Assignment 3
--Addison Babcock
--Feb 01 2007

use pubs

--Question 1
select title 'Title'
from titles
where title_id in 
	(
	select title_id
	from titleauthor
	where royaltyper > 40
	)
order by title

--Question 2
select au_fname + ' ' + au_lname 'Name'
from authors
where city in 
	(
	select city
	from publishers
	)

--Question 3
select pub_name 'Publisher'
from publishers
where pub_id not in
	(
	select pub_id
	from titles
	)

--Question 4
select pub_name 'Publisher'
from publishers
where pub_id in
	(
	select pub_id
	from titles
	where type like '%business%'
	)

--Question 5
select pub_name 'Publisher'
from publishers
where pub_id in
	(
	select pub_id
	from titles
	where royalty = 0 or royalty is null
	)

--Question 6
select lname + ', ' + fname 'Name'
from employee
where datepart (yy, hire_date) = 1991

--Question 7
select fname + ' ' + lname 'Name',
	substring (emp_id, 9, 1) 'Gender'
from employee
order by 2, 1

--Question 8
select convert (char (20), fname + ' ' + lname) 'Name',
	substring (emp_id, 9, 1) 'Gender'
from employee
order by 2, 1

--Question 9
select *
from sales
where datepart (yy, ord_date) = 1993 and
	datepart (mm, ord_date) = 5
order by ord_date

--53/55