--CNT355 SQL
--Lab 3
--Feb 1, 2007
--Addison Babcock

use pubs

--Question 1
select Title 'Title', Price 'Price'
from titles
where Price between 10 and 12
--where price between $10.00 and $12.00

--Question 2
select pub_name 'Publisher Name', city 'Publisher City'
from publishers
where state in ('ma', 'ca')

--Question 3
select Title_Id 'Title ID', 
		type 'Type', 
		advance 'Advances',
		royalty 'Royalties',
		ytd_sales 'Year To Date Sales'
from titles
where ytd_sales < 1000 or ytd_sales > 10000
--where ytd_sales not between 1000 and 10000

--Question 4
--look for "Av."
select au_lname 'Author Last Name', 
		address 'Address',
		city 'City'
from authors
where address like '%Av.%'

--Question 5
--look for "f" in employee id
select lname + ' ' + fname 'Employee Name', 
		hire_date 'Hire Date'
from employee
where emp_id like '%f'
--where substring (emp_id, 9, 1) = 'F'

--Question 6
--look for "manag", use not like
select *
from jobs
where job_desc not like '%manag%'
--... or ....           '%[Mm]anag%'

--Question 7
select *
from employee
where year (hire_date) = 1994
--where datepart....
--where hire_date between 'jan 1, 1994' and 'dec 31 1994'

--Question 8
select Title, Type
from titles
where title like '% the %' or title like 'the %' -- minus 2
-- '%the%' might return these, theory... etc

--Question 9
select stor_id 'Store ID', 
		ord_num 'Order Number',
		ord_date 'Order Date'
from sales
where (year(ord_date) = 1992) or (qty > 12 and payterms like '%60%')

--Question 10
select title_id 'Title ID', 
		type 'Type',
		price 'Price'
from titles
where advance > 4000 and
		price > 12 and
		year (pubdate) <> 1991

-- 41 / 43