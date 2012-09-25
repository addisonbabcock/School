--Lab 2 question 4 lecture notes
--Jan 29, 2007
--how to work with dates

use pubs

select fname, lname
from employee
where hire_date > 'Dec 31, 1990'

--date literals can be specified several ways
--for this course, use this format:
--first 3 chars of the month, space, day number, space, year

--'Jan 29, 2007'
--don't forget the apostrophes to delemit the data

	--date and time functions
		--see ms-help://MS.SQLCC.v9/MS.SQLSVR.v9.en/tsqlref9/html/83e378a2-6e89-4c80-bc4f-644958d9e0a9.htm

	--using a function to solve question 4
--try this...
select getdate () --this reads the real time clock from the motherboard
select year (getdate ()) --nested functions, returns the year only

--another solution for question 4:
select * from employee
where year (hire_date) > 1990

--experiment with datepart () function
select hire_date, datepart (yy , hire_date) as 'Year'
from employee

--year (hire_date) and datepart (yy, hire_date) are equivalent

--datepart is quite flexible 
--different parts can be extracted by using the first argument

select datepart (yy, 'jan 29, 2007')
select datepart (m, 'jan 29, 2007')

--using datename:
select datename (m, 'jan 29, 2007')

--Question 6
use northwind

select Companyname , address
from customers
where companyname like 'fr%' --our server is not set to be case-sensitive

/* for pattern matching, use like keyword, this is covered on the website under SQL2 */
/* like is generally used with strings but can also be used with dates */

--column aliases: there are several ways to create an alias
select Companyname 'Company Name', address 'Adr'
from customers
where companyname like 'fr%' 

--Question 7
select * 
from orders
--where orderdate >= 'jul 4, 1996' and orderdate <= 'sep 3, 1996'
--where orderdate between 'jul 4, 1996' and 'sep 3, 1996'

--both where parts are equivalent
--between is inclusive

--random notes
set nocount on
use pubs
select * from authors 
order by au_lname,au_fname desc
--descending first name within ascending last name
