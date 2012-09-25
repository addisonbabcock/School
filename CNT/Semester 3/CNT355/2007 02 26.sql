use pubs
--SQL5 join theory 1


--Q1
select	employee.emp_id,		
		employee.lname + ', ' + employee.fname + ' ' + employee.minit + '.' as 'name',
		employee.hire_date,
		jobs.job_desc
from employee
		join jobs on (employee.job_id = jobs.job_id)
where year (employee.hire_date) = 1994 and len (employee.minit) > 0

union --accounting for a lack of middle initial

select	employee.emp_id,		
		employee.lname + ', ' + employee.fname as 'name',
		employee.hire_date,
		jobs.job_desc
from employee
		join jobs on (employee.job_id = jobs.job_id)
where year (employee.hire_date) = 1994 and len (employee.minit) = 0
order by employee.hire_date asc


--Q2
select	employee.emp_id,
		employee.pub_id,
		publishers.city,
		publishers.state
from employee
		join publishers on (employee.pub_id = publishers.pub_id)
order by publishers.state asc, publishers.city asc


--Q3
select	authors.au_fname + ' ' + authors.au_lname 'Author name',
		titleauthor.title_id 'Title id',
		titles.advance 'Advances',
		titles.ytd_sales 'YTD sales',
		titleauthor.royaltyper 'Royalty percent'
from authors
		join titleauthor on (authors.au_id = titleauthor.au_id)
				join titles on (titleauthor.title_id = titles.title_id)
where authors.state = 'CA'