

--q1
select datename (dw, 'Dec 25, 2010')

--q2
select datediff (dd, GetDate (), 'Dec 25, 2010')

--q3
select datediff (dd, GetDate (), 'Dec 25, ' + cast (year (GetDate ()) as varchar))

--Q4
use pubs

select	employee.fname + ' ' + employee.lname 'Employee',
		jobs.job_desc 'Job description',
		publishers.pub_name 'Publisher'
from	employee, publishers, jobs
where	employee.job_id = jobs.job_id and
		employee.pub_id = publishers.pub_id and
		datename (dw, employee.hire_date) in ('Saturday', 'Sunday')

--q5
select	titles.title 'Title',
		publishers.pub_name 'Publisher',
		cast (round (titles.ytd_sales * titles.price, 2) as decimal (8, 2)) 'Gross Sales'
from	titles, publishers
where	titles.pub_id = publishers.pub_id and
		cast (round (titles.ytd_sales * titles.price, 2) as decimal (8, 2)) >= 100000

--q6
select	distinct
		publishers.pub_name 'Publisher'
from	publishers, sales, titles
where	publishers.pub_id = titles.pub_id and
		sales.title_id = titles.title_id and
		datediff (yy, getdate (), sales.ord_date) < 8

--q7
select	authors.au_fname + ' ' + authors.au_lname 'Author name',
		ascii (upper (substring (au_lname, 1, 1)))
from	authors, titleauthor, titles
where	authors.au_id = titleauthor.au_id and
		titleauthor.title_id = titles.title_id and
		datediff (yy, getdate (), titles.pubdate) < 10

--q8
select	hire_date 'Day Hired',
		dateadd (dd, 15, hire_date) '15 Days after',
		lname 'Last name'
from	employee

--q9
select	day (getdate ()) 'Day',
		datename (mm, getdate ()) 'Month',
		year (getdate ()) 'Year'

--q10
select	au_lname 'Last name',
		len (au_lname) 'Length'
from authors

--q11
select	upper (left (au_fname, 1)) + substring (au_fname, 2, 4) 'Name',
		upper (left (au_lname, 1)) + '.' 'Last initial'
from	authors

--q12
use test

create table testvalues
(
	samplevalues	real	not null
)

insert testvalues
values (-10)

insert testvalues
values (10)

insert testvalues
values (3.66)

insert testvalues
values	(-3.66)

insert testvalues
values	(2.22)

insert testvalues
values	(-2.22)

select	samplevalues 'tests',
		ceiling (samplevalues) 'ceiling',
		floor (samplevalues) 'floor',
		round (samplevalues, 2) 'round',
		exp (samplevalues) 'exp',
		sign (samplevalues) 'sign',
		square (samplevalues) 'square',
		log (abs (samplevalues)) 'log',
		log10 (abs (samplevalues)) 'log10',
		sqrt (abs (samplevalues)) 'sqrt'
from testvalues